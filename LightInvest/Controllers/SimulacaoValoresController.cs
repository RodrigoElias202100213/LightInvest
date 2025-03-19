using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightInvest.Controllers
{
	public class SimulacaoValoresController : Controller
	{
		private readonly ApplicationDbContext _context;

		public SimulacaoValoresController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Módulo 1: Processa os dados de EnergyConsumption
		private async Task<EnergyConsumption> ProcessarEnergyConsumptionAsync(string userEmail)
		{
			var consumo = await _context.EnergyConsumptions.FirstOrDefaultAsync(c => c.UserEmail == userEmail);
			if (consumo == null)
			{
				consumo = new EnergyConsumption
				{
					UserEmail = userEmail,
					ConsumoDiaSemana = Enumerable.Repeat(0m, 24).ToList(),
					ConsumoFimSemana = Enumerable.Repeat(0m, 24).ToList(),
					MesesOcupacao = new List<string>()
				};
			}

			consumo.CalcularMedias();
			consumo.CalcularConsumoMensal();
			consumo.CalcularMediaAnual();
			return consumo;
		}

		private async Task<ResultadoTarifaViewModel> ProcessarTarifaAsync(string userEmail, EnergyConsumption consumo)
		{
			var tarifa = await _context.Tarifas.FirstOrDefaultAsync(t => t.UserEmail == userEmail);
			if (tarifa == null)
				throw new Exception("Tarifa não encontrada para o usuário.");

			var resultado = new ResultadoTarifaViewModel
			{
				TarifaEscolhida = tarifa.Tipo.ToString(),
				PrecoKwh = tarifa.PrecoFinal, // Assume que a propriedade PrecoFinal já aplica o adicional
				MesesOcupacao = consumo.MesesOcupacao
			};

			// Calcula o consumo e custo para cada mês considerando o número de semanas
			foreach (var mes in consumo.MesesOcupacao)
			{
				int semanas = consumo.ObterNumeroDeSemanasNoMes(mes);
				decimal consumoSemana = (consumo.MediaSemana * 5) + (consumo.MediaFimSemana * 2);
				decimal consumoMes = Math.Round(consumoSemana * semanas, 1);
				decimal custoMes = Math.Round(consumoMes * tarifa.PrecoFinal, 2);

				resultado.ConsumoMensal.Add(new MesConsumo
				{
					Mes = mes,
					Consumo = consumoMes,
					Custo = custoMes
				});
			}

			resultado.ConsumoTotal = resultado.ConsumoMensal.Sum(m => m.Consumo);
			resultado.ValorAnual = resultado.ConsumoMensal.Sum(m => m.Custo);

			return resultado;
		}
		public async Task<IActionResult> Simular()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return BadRequest("Usuário não autenticado.");

			// Módulo 1: Processar o consumo de energia
			var consumo = await ProcessarEnergyConsumptionAsync(userEmail);

			// Módulo 2: Processar a tarifa e calcular custo mensal/anual
			ResultadoTarifaViewModel resultadoTarifa;
			Tarifa tarifaForVM;
			try
			{
				resultadoTarifa = await ProcessarTarifaAsync(userEmail, consumo);
				tarifaForVM = await _context.Tarifas.FirstOrDefaultAsync(t => t.UserEmail == userEmail);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			// Buscar os dados de instalação do usuário
			var dadosInstalacao = await _context.DadosInstalacao
				.Include(d => d.Cidade)
				.Include(d => d.ModeloPainel)
				.Include(d => d.Potencia)
				.FirstOrDefaultAsync(d => d.UserEmail == userEmail);

			if (dadosInstalacao == null)
				return BadRequest("Nenhum dado de instalação encontrado para este usuário.");

			// Cálculo da energia gerada mensalmente pelos painéis
			decimal potenciaPainel = dadosInstalacao.Potencia.Potencia; // em Watts
			int numeroPaineis = dadosInstalacao.NumeroPaineis;
			decimal horasSolDiarias = 5m; // Valor fixo (ajustável conforme região)
			decimal diasNoMes = 30m;

			// Energia gerada mensal (em kWh)
			decimal energiaGeradaMensal = (potenciaPainel * numeroPaineis * horasSolDiarias * diasNoMes) / 1000;
			// Economia mensal (em R$), usando o preço do kWh da tarifa
			decimal economiaMensal = energiaGeradaMensal * tarifaForVM.PrecoKWh;
			// Economia anual
			int mesesOcupados = consumo.MesesOcupacao.Count;
			decimal economiaAnual = economiaMensal * mesesOcupados;

			// Cálculo do ROI:
			// Fórmula: ROI = Custo da Instalação / (Economia Anual - Custo de Manutenção Anual)
			decimal custoInstalacao = dadosInstalacao.PrecoInstalacao;
			decimal custoManutencaoAnual = 500m; // Valor fixo de manutenção (pode ser parametrizado)
			decimal roiValue = custoInstalacao / (economiaAnual - custoManutencaoAnual);

			// Inserir os dados do ROI na base de dados
			var roiData = new RoiCalculator
			{
				UserEmail = userEmail,
				CustoInstalacao = custoInstalacao,
				CustoManutencaoAnual = custoManutencaoAnual,
				ConsumoEnergeticoMedio = consumo.ConsumoTotal / 12, // média mensal
				ConsumoEnergeticoRede = consumo.ConsumoTotal,
				RetornoEconomia = economiaAnual,
				ROI = roiValue,
				DataCalculado = DateTime.Now
			};

			_context.ROICalculators.Add(roiData);
			await _context.SaveChangesAsync();

			// Determinar quantos anos simular com base no ROI
			int anosSimulados = Math.Max(1, (int)Math.Ceiling(roiValue));
			// Opcional: acrescente um ano extra para visualização
			// anosSimulados++;

			// Agrupar a evolução do investimento por ano
			var retornoInvestimentoPorAno = new List<RetornoInvestimentoAno>();
			decimal saldoAcumulado = -custoInstalacao;
			for (int ano = 1; ano <= anosSimulados; ano++)
			{
				var investimentoAno = new RetornoInvestimentoAno { Ano = ano };
				for (int mes = 1; mes <= 12; mes++)
				{
					saldoAcumulado += economiaMensal;
					investimentoAno.Meses.Add(new RetornoInvestimentoMes
					{
						// Converte o número do mês para seu nome (ex.: 1 -> "Janeiro")
						Mes = ObterNomeDoMes(mes),
						EnergiaGerada = energiaGeradaMensal,
						EconomiaMensal = economiaMensal,
						SaldoRestante = saldoAcumulado
					});
				}
				retornoInvestimentoPorAno.Add(investimentoAno);
			}

			// Montar o ViewModel da simulação completa
			var simulacao = new SimulacaoCompletaViewModel
			{
				EnergyConsumptionViewModel = new EnergyConsumptionViewModel
				{
					ConsumoDiaSemana = consumo.ConsumoDiaSemana,
					ConsumoFimSemana = consumo.ConsumoFimSemana,
					MesesOcupacao = consumo.MesesOcupacao,
					MediaSemana = consumo.MediaSemana,
					MediaFimSemana = consumo.MediaFimSemana,
					MediaAnual = consumo.MediaAnual,
					ConsumoTotal = consumo.ConsumoTotal
				},
				TarifaViewModel = new TarifaViewModel(tarifaForVM),
				ResultadoTarifaViewModel = resultadoTarifa,
				DadosInstalacao = dadosInstalacao,
				ROI = new RoiCalculatorDashboardViewModel
				{
					CurrentRoi = roiData,
					History = await _context.ROICalculators
						.Where(r => r.UserEmail == userEmail)
						.OrderByDescending(r => r.DataCalculado)
						.ToListAsync()
				},
				RetornoInvestimentoPorAno = retornoInvestimentoPorAno
			};

			return View("SimulacaoCompleta", simulacao);
		}

		// Método auxiliar para converter o número do mês para o nome do mês
		private string ObterNomeDoMes(int numeroMes)
		{
			string[] meses = { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
						"Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
			return (numeroMes >= 1 && numeroMes <= 12) ? meses[numeroMes - 1] : "Mês Inválido";
		}

		public async Task<IActionResult> ExportPDF()
		{
			// Obter os dados que serão exportados (por exemplo, chamando o método Simular ou recuperando o ViewModel)
			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return BadRequest("Usuário não autenticado.");

			var viewModel = await GerarViewModelCompleto(userEmail); // implemente este método para compor o seu ViewModel

			// Aqui você pode utilizar uma biblioteca como o Rotativa, iTextSharp ou similar para gerar o PDF.
			// Exemplo com Rotativa:
			// return new ViewAsPdf("SimulacaoCompletaPDF", viewModel) { FileName = "Simulacao.pdf" };

			// Se não utilizar uma biblioteca, retorne um placeholder:
			return Content("Funcionalidade de exportação para PDF não implementada.");
		}
		// Exemplo: Exportar CSV
		public async Task<IActionResult> ExportCSV()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return BadRequest("Usuário não autenticado.");

			var viewModel = await GerarViewModelCompleto(userEmail);

			// Cria uma string CSV (exemplo simplificado – adapte conforme os dados que deseja exportar)
			var csv = new StringBuilder();
			csv.AppendLine("Seção,Valor");
			csv.AppendLine($"Média Semana,{viewModel.EnergyConsumptionViewModel.MediaSemana}");
			csv.AppendLine($"Média Fim de Semana,{viewModel.EnergyConsumptionViewModel.MediaFimSemana}");
			csv.AppendLine($"Média Anual,{viewModel.EnergyConsumptionViewModel.MediaAnual}");
			csv.AppendLine($"Consumo Total,{viewModel.EnergyConsumptionViewModel.ConsumoTotal}");
			// Acrescente os demais dados conforme necessário...

			byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());
			return File(buffer, "text/csv", "Simulacao.csv");
		}

		public async Task<IActionResult> ExportExcel()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return BadRequest("Usuário não autenticado.");

			var viewModel = await GerarViewModelCompleto(userEmail);

			// Para exportar para Excel, você pode usar bibliotecas como o EPPlus ou ClosedXML.
			// Exemplo simplificado usando ClosedXML:
			using (var workbook = new ClosedXML.Excel.XLWorkbook())
			{
				var ws = workbook.Worksheets.Add("Simulação Completa");
				ws.Cell(1, 1).Value = "Seção";
				ws.Cell(1, 2).Value = "Valor";

				ws.Cell(2, 1).Value = "Média Semana";
				ws.Cell(2, 2).Value = viewModel.EnergyConsumptionViewModel.MediaSemana;
				ws.Cell(3, 1).Value = "Média Fim de Semana";
				ws.Cell(3, 2).Value = viewModel.EnergyConsumptionViewModel.MediaFimSemana;
				ws.Cell(4, 1).Value = "Média Anual";
				ws.Cell(4, 2).Value = viewModel.EnergyConsumptionViewModel.MediaAnual;
				ws.Cell(5, 1).Value = "Consumo Total";
				ws.Cell(5, 2).Value = viewModel.EnergyConsumptionViewModel.ConsumoTotal;
				// Adicione os demais dados conforme necessário...

				using (var stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					stream.Seek(0, SeekOrigin.Begin);
					return File(stream.ToArray(),
						"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
						"Simulacao.xlsx");
				}
			}
		}
		
		
		// Método auxiliar para montar o ViewModel completo (pode ser o mesmo que você usa na ação Simular)
		private async Task<SimulacaoCompletaViewModel> GerarViewModelCompleto(string userEmail)
		{
			// Aqui você pode chamar os métodos ProcessarEnergyConsumptionAsync, ProcessarTarifaAsync, etc.
			// e montar o SimulacaoCompletaViewModel conforme já feito na ação Simular.
			// Exemplo:
			var consumo = await ProcessarEnergyConsumptionAsync(userEmail);
			ResultadoTarifaViewModel resultadoTarifa = await ProcessarTarifaAsync(userEmail, consumo);
			var dadosInstalacao = await _context.DadosInstalacao
				.Include(d => d.Cidade)
				.Include(d => d.ModeloPainel)
				.Include(d => d.Potencia)
				.FirstOrDefaultAsync(d => d.UserEmail == userEmail);

			// Suponha que você já tenha calculado ROI e RetornoInvestimentoPorAno anteriormente
			var roiData = await _context.ROICalculators.FirstOrDefaultAsync(r => r.UserEmail == userEmail);
			var roiDashboard = new RoiCalculatorDashboardViewModel
			{
				CurrentRoi = roiData,
				History = await _context.ROICalculators
					.Where(r => r.UserEmail == userEmail)
					.OrderByDescending(r => r.DataCalculado)
					.ToListAsync()
			};

			// Aqui você deverá recriar a lógica de cálculo da evolução do saldo acumulado e agrupar em anos...
			var simulacao = new SimulacaoCompletaViewModel
			{
				EnergyConsumptionViewModel = new EnergyConsumptionViewModel
				{
					// Preencha com os dados necessários
				},
				TarifaViewModel = new TarifaViewModel(await _context.Tarifas.FirstOrDefaultAsync(t => t.UserEmail == userEmail)),
				ResultadoTarifaViewModel = resultadoTarifa,
				DadosInstalacao = dadosInstalacao,
				ROI = roiDashboard,
				// Preencha a propriedade RetornoInvestimentoPorAno conforme sua lógica
			};

			return simulacao;
		}
	}
}
