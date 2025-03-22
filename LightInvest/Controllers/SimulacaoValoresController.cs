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
				throw new Exception("Tarifa não encontrada para o utilizador.");

			var resultado = new ResultadoTarifaViewModel
			{
				TarifaEscolhida = tarifa.Tipo.ToString(),
				PrecoKwh = tarifa.PrecoFinal,
				MesesOcupacao = consumo.MesesOcupacao
			};

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
				return BadRequest("Utilizador não autenticado.");


			var consumo = await ProcessarEnergyConsumptionAsync(userEmail);

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

			var dadosInstalacao = await _context.DadosInstalacao
				.Include(d => d.Cidade)
				.Include(d => d.ModeloPainel)
				.Include(d => d.Potencia)
				.FirstOrDefaultAsync(d => d.UserEmail == userEmail);

			if (dadosInstalacao == null)
				return BadRequest("Nenhum dado de instalação encontrado para este utilizador.");

			decimal potenciaPainel = dadosInstalacao.Potencia.Potencia;
			int numeroPaineis = dadosInstalacao.NumeroPaineis;
			decimal horasSolDiarias = 5m;
			decimal diasNoMes = 30m;
			decimal energiaGeradaMensal = (potenciaPainel * numeroPaineis * horasSolDiarias * diasNoMes) / 1000;
			decimal economiaMensal = energiaGeradaMensal * tarifaForVM.PrecoKWh;
			int mesesOcupados = consumo.MesesOcupacao.Count;
			decimal economiaAnual = economiaMensal * mesesOcupados;

			decimal custoInstalacao = dadosInstalacao.PrecoInstalacao;
			decimal custoManutencaoAnual = 500m;
			decimal roiValue = custoInstalacao / (economiaAnual - custoManutencaoAnual);


			var roiData = new RoiCalculator
			{
				UserEmail = userEmail,
				CustoInstalacao = custoInstalacao,
				CustoManutencaoAnual = custoManutencaoAnual,
				ConsumoEnergeticoMedio = consumo.ConsumoTotal / 12,
				ConsumoEnergeticoRede = consumo.ConsumoTotal,
				RetornoEconomia = economiaAnual,
				ROI = roiValue,
				DataCalculado = DateTime.Now
			};

			_context.ROICalculators.Add(roiData);
			await _context.SaveChangesAsync();

			int anosSimulados = Math.Max(1, (int)Math.Ceiling(roiValue));

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
						Mes = ObterNomeDoMes(mes),
						EnergiaGerada = energiaGeradaMensal,
						EconomiaMensal = economiaMensal,
						SaldoRestante = saldoAcumulado
					});
				}
				retornoInvestimentoPorAno.Add(investimentoAno);
			}

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
		private string ObterNomeDoMes(int numeroMes)
		{
			string[] meses = { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
						"Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
			return (numeroMes >= 1 && numeroMes <= 12) ? meses[numeroMes - 1] : "Mês Inválido";
		}

		public async Task<IActionResult> ExportPDF()
		{

			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return BadRequest("Utilizador não autenticado.");

			var viewModel = await GerarViewModelCompleto(userEmail);

			return Content("Funcionalidade de exportação para PDF não implementada.");
		}

		public async Task<IActionResult> ExportCSV()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return BadRequest("Utilizador não autenticado.");

			var viewModel = await GerarViewModelCompleto(userEmail);

			var csv = new StringBuilder();
			csv.AppendLine("Seção,Valor");
			csv.AppendLine($"Média Semana,{viewModel.EnergyConsumptionViewModel.MediaSemana}");
			csv.AppendLine($"Média Fim de Semana,{viewModel.EnergyConsumptionViewModel.MediaFimSemana}");
			csv.AppendLine($"Média Anual,{viewModel.EnergyConsumptionViewModel.MediaAnual}");
			csv.AppendLine($"Consumo Total,{viewModel.EnergyConsumptionViewModel.ConsumoTotal}");

			byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());
			return File(buffer, "text/csv", "Simulacao.csv");
		}
/*
		public async Task<IActionResult> ExportExcel()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return BadRequest("Utilizador não autenticado.");

			var viewModel = await GerarViewModelCompleto(userEmail);

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
*/
		private async Task<SimulacaoCompletaViewModel> GerarViewModelCompleto(string userEmail)
		{
			var consumo = await ProcessarEnergyConsumptionAsync(userEmail);
			ResultadoTarifaViewModel resultadoTarifa = await ProcessarTarifaAsync(userEmail, consumo);
			var dadosInstalacao = await _context.DadosInstalacao
				.Include(d => d.Cidade)
				.Include(d => d.ModeloPainel)
				.Include(d => d.Potencia)
				.FirstOrDefaultAsync(d => d.UserEmail == userEmail);

			var roiData = await _context.ROICalculators.FirstOrDefaultAsync(r => r.UserEmail == userEmail);
			var roiDashboard = new RoiCalculatorDashboardViewModel
			{
				CurrentRoi = roiData,
				History = await _context.ROICalculators
					.Where(r => r.UserEmail == userEmail)
					.OrderByDescending(r => r.DataCalculado)
					.ToListAsync()
			};


			var simulacao = new SimulacaoCompletaViewModel
			{
				EnergyConsumptionViewModel = new EnergyConsumptionViewModel
				{
					// Preencher com os dados
				},
				TarifaViewModel = new TarifaViewModel(await _context.Tarifas.FirstOrDefaultAsync(t => t.UserEmail == userEmail)),
				ResultadoTarifaViewModel = resultadoTarifa,
				DadosInstalacao = dadosInstalacao,
				ROI = roiDashboard,

			};

			return simulacao;
		}
		public IActionResult MostrarImagem()
		{
			var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "file.png");

			if (!System.IO.File.Exists(imagePath))
			{
				return NotFound("Imagem não encontrada.");
			}

			var imageBytes = System.IO.File.ReadAllBytes(imagePath);
			return File(imageBytes, "image/png");
		}

	}
}