using ClosedXML.Excel;
using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Rotativa;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightInvest.Controllers
{
	public class SimulacaoController : Controller
	{
		private readonly ApplicationDbContext _context;

		public SimulacaoController(ApplicationDbContext context)
		{
			_context = context;
		}

		private async Task<User> GetLoggedInUserAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			return string.IsNullOrEmpty(userEmail)
				? null
				: await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

		private EnergyConsumptionViewModel MapToViewModel(EnergyConsumption energyConsumption)
		{
			return new EnergyConsumptionViewModel
			{
				ConsumoDiaSemana = energyConsumption.ConsumoDiaSemana,
				ConsumoFimSemana = energyConsumption.ConsumoFimSemana,
				MesesOcupacao = energyConsumption.MesesOcupacao,
				MediaSemana = energyConsumption.MediaSemana,
				MediaFimSemana = energyConsumption.MediaFimSemana,
				MediaAnual = energyConsumption.MediaAnual
			};
		}

		[HttpGet("simulacao-completa")]
		public async Task<IActionResult> SimulacaoCompleta()
		{
			var user = await GetLoggedInUserAsync();
			if (user == null)
			{
				return Unauthorized("Utilizador não autenticado.");
			}

			var energyConsumption = await _context.EnergyConsumptions
				.Where(ec => ec.UserEmail == user.Email)
				.FirstOrDefaultAsync();

			if (energyConsumption == null)
			{
				return NotFound("Nenhum dado de consumo encontrado.");
			}

			// Fetch latest tariff data
			var tarifa = await _context.Tarifas
				.Where(t => t.UserEmail == user.Email)
				.OrderByDescending(t => t.DataAlteracao)
				.FirstOrDefaultAsync();

			if (tarifa == null)
			{
				return NotFound("Nenhuma tarifa associada ao utilizador.");
			}

			var dadosInstalacao = await _context.DadosInstalacao
				.Include(di => di.ModeloPainel)
				.ThenInclude(mp => mp.Potencias) 
				.Include(di => di.Cidade)
				.Where(di => di.UserEmail == user.Email)
				.FirstOrDefaultAsync();

			if (dadosInstalacao == null)
			{
				return NotFound("Nenhum dado de instalação encontrado.");
			}


			var potenciaPainel = dadosInstalacao?.ModeloPainel?.Potencias
				.FirstOrDefault(p => p.Id == dadosInstalacao.ConsumoPainel)?.Potencia ?? 0;

			if (potenciaPainel == 0)
			{
				return BadRequest("Nenhuma potência registada para o modelo do painel solar.");
			}

			energyConsumption.CalcularMedias();
			decimal custoAnual = energyConsumption.MediaAnual * tarifa.PrecoKwh;

			dadosInstalacao.AtualizarPrecoInstalacao();

			var consumoMensal = energyConsumption.MediaAnual / energyConsumption.MesesOcupacao.Count;

			decimal custoMensal = consumoMensal * tarifa.PrecoKwh;

			var resultado = new ResultadoTarifaViewModel
			{
				ConsumoTotal = energyConsumption.MediaAnual,
				ValorAnual = custoAnual,
				TarifaEscolhida = tarifa.Nome.ToString(),
				PrecoKwh = tarifa.PrecoKwh,
				MesesOcupacao = energyConsumption.MesesOcupacao,
				ConsumoMensal = energyConsumption.MesesOcupacao
					.Select(mes => new MesConsumo { Mes = mes, Consumo = consumoMensal, Custo = custoMensal })
					.ToList()
			};

			var energyConsumptionViewModel = new EnergyConsumptionViewModel
			{
				ConsumoDiaSemana = energyConsumption.ConsumoDiaSemana,
				ConsumoFimSemana = energyConsumption.ConsumoFimSemana,
				MesesOcupacao = energyConsumption.MesesOcupacao,
				MediaSemana = energyConsumption.MediaSemana,
				MediaFimSemana = energyConsumption.MediaFimSemana,
				MediaAnual = energyConsumption.MediaAnual
			};



			var viewModel = new SimulacaoCompletaViewModel
			{
				EnergyConsumptionViewModel = energyConsumptionViewModel,
				ResultadoTarifaViewModel = resultado,
				DadosInstalacao = dadosInstalacao,
				PrecoInstalacao = dadosInstalacao.PrecoInstalacao,
				PotenciaPainel = potenciaPainel 
			};

			
			return View("UserEnergyConsumption", viewModel);
		}

		[HttpGet("calcular-roi")]
		public async Task<IActionResult> CalcularROI()
		{
			var user = await GetLoggedInUserAsync();
			if (user == null)
			{
				return Unauthorized("Usuário não autenticado.");
			}

			var simulacao = await SimulacaoCompleta(); 
			if (simulacao is not ViewResult viewResult || viewResult.Model is not SimulacaoCompletaViewModel viewModel)
			{
				return NotFound("Erro ao obter dados da simulação.");
			}

			var roiCalculator = new RoiCalculator
			{
				UserEmail = user.Email,
				CustoInstalacao = viewModel.PrecoInstalacao,
				CustoManutencaoAnual = 1, 
				ConsumoEnergeticoMedio = viewModel.EnergyConsumptionViewModel.MediaAnual,
				ConsumoEnergeticoRede = viewModel.EnergyConsumptionViewModel.MediaAnual,
				RetornoEconomia = viewModel.ResultadoTarifaViewModel.ValorAnual,
				DataCalculado = DateTime.Now
			};
			/*
			if (roiCalculator.RetornoEconomia <= 0)
			{
				return BadRequest("A economia anual deve ser maior que zero para calcular o ROI.");
			}
			*/
			roiCalculator.ROI = roiCalculator.CalcularROI();
			_context.ROICalculators.Add(roiCalculator);
			await _context.SaveChangesAsync();

			var roiDashboardViewModel = new RoiCalculatorDashboardViewModel
			{
				CurrentRoi = roiCalculator,
				History = _context.ROICalculators.ToList() 
			};

			var simulacaoCompletaViewModel = new SimulacaoCompletaViewModel
			{
				EnergyConsumptionViewModel = viewModel.EnergyConsumptionViewModel,
				ResultadoTarifaViewModel = viewModel.ResultadoTarifaViewModel,
				DadosInstalacao = viewModel.DadosInstalacao,
				PrecoInstalacao = viewModel.PrecoInstalacao,
				PotenciaPainel = viewModel.PotenciaPainel,
				ROI = roiDashboardViewModel
			};

			return View("ROIResult", simulacaoCompletaViewModel);
		}


		// Função para exportar os dados da simulação (CSV, Excel ou PDF)
		[HttpGet("exportar-simulacao")]
		public async Task<IActionResult> ExportarSimulacao(string tipo)
		{
			// Obtém o usuário logado e os dados da simulação (duplicando a lógica da action SimulacaoCompleta)
			var user = await GetLoggedInUserAsync();
			if (user == null)
			{
				return Unauthorized("Utilizador não autenticado.");
			}

			var energyConsumption = await _context.EnergyConsumptions
				.Where(ec => ec.UserEmail == user.Email)
				.FirstOrDefaultAsync();
			if (energyConsumption == null)
			{
				return NotFound("Nenhum dado de consumo encontrado.");
			}

			var tarifa = await _context.Tarifas
				.Where(t => t.UserEmail == user.Email)
				.OrderByDescending(t => t.DataAlteracao)
				.FirstOrDefaultAsync();
			if (tarifa == null)
			{
				return NotFound("Nenhuma tarifa associada ao utilizador.");
			}

			var dadosInstalacao = await _context.DadosInstalacao
				.Include(di => di.ModeloPainel)
					.ThenInclude(mp => mp.Potencias)
				.Include(di => di.Cidade)
				.Where(di => di.UserEmail == user.Email)
				.FirstOrDefaultAsync();
			if (dadosInstalacao == null)
			{
				return NotFound("Nenhum dado de instalação encontrado.");
			}

			var potenciaPainel = dadosInstalacao?.ModeloPainel?.Potencias
				.FirstOrDefault(p => p.Id == dadosInstalacao.ConsumoPainel)?.Potencia ?? 0;
			if (potenciaPainel == 0)
			{
				return BadRequest("Nenhuma potência registada para o modelo do painel solar.");
			}

			energyConsumption.CalcularMedias();
			decimal custoAnual = energyConsumption.MediaAnual * tarifa.PrecoKwh;
			dadosInstalacao.AtualizarPrecoInstalacao();
			var consumoMensal = energyConsumption.MediaAnual / energyConsumption.MesesOcupacao.Count;
			decimal custoMensal = consumoMensal * tarifa.PrecoKwh;

			var resultado = new ResultadoTarifaViewModel
			{
				ConsumoTotal = energyConsumption.MediaAnual,
				ValorAnual = custoAnual,
				TarifaEscolhida = tarifa.Nome.ToString(),
				PrecoKwh = tarifa.PrecoKwh,
				MesesOcupacao = energyConsumption.MesesOcupacao,
				ConsumoMensal = energyConsumption.MesesOcupacao
					.Select(mes => new MesConsumo { Mes = mes, Consumo = consumoMensal, Custo = custoMensal })
					.ToList()
			};

			var energyConsumptionViewModel = new EnergyConsumptionViewModel
			{
				ConsumoDiaSemana = energyConsumption.ConsumoDiaSemana,
				ConsumoFimSemana = energyConsumption.ConsumoFimSemana,
				MesesOcupacao = energyConsumption.MesesOcupacao,
				MediaSemana = energyConsumption.MediaSemana,
				MediaFimSemana = energyConsumption.MediaFimSemana,
				MediaAnual = energyConsumption.MediaAnual
			};

			// Recupera o ROI (caso exista)
			var roiCalculator = await _context.ROICalculators
				.Where(r => r.UserEmail == user.Email)
				.OrderByDescending(r => r.DataCalculado)
				.FirstOrDefaultAsync();

			var roiDashboardViewModel = new RoiCalculatorDashboardViewModel
			{
				CurrentRoi = roiCalculator,
				History = _context.ROICalculators.Where(r => r.UserEmail == user.Email).ToList()
			};

			var viewModel = new SimulacaoCompletaViewModel
			{
				EnergyConsumptionViewModel = energyConsumptionViewModel,
				ResultadoTarifaViewModel = resultado,
				DadosInstalacao = dadosInstalacao,
				PrecoInstalacao = dadosInstalacao.PrecoInstalacao,
				PotenciaPainel = potenciaPainel,
				ROI = roiDashboardViewModel
			};

			// Escolhe o formato de exportação conforme o parâmetro 'tipo'
			if (string.IsNullOrEmpty(tipo))
				return BadRequest("Tipo de exportação não informado.");

			switch (tipo.ToLower())
			{
				case "csv":
					{
						var csv = new StringBuilder();
						csv.AppendLine("Métrica,Valor");
						csv.AppendLine($"Média Semanal,{energyConsumptionViewModel.MediaSemana:F3} kWh");
						csv.AppendLine($"Média Fim de Semana,{energyConsumptionViewModel.MediaFimSemana:F3} kWh");
						csv.AppendLine($"Média Anual,{energyConsumptionViewModel.MediaAnual:F3} kWh");
						csv.AppendLine($"Tarifa Escolhida,{resultado.TarifaEscolhida}");
						csv.AppendLine($"Preço por kWh,{resultado.PrecoKwh:F3} €");
						csv.AppendLine($"Consumo Total,{resultado.ConsumoTotal:F3} kWh");
						csv.AppendLine($"Custo Anual,{resultado.ValorAnual:F3} €");

						if (roiDashboardViewModel.CurrentRoi != null)
						{
							csv.AppendLine($"Data do Cálculo,{roiDashboardViewModel.CurrentRoi.DataCalculado:dd/MM/yyyy}");
							csv.AppendLine($"ROI Atual,{roiDashboardViewModel.CurrentRoi.ROI}");
							csv.AppendLine($"Custo de Instalação,{roiDashboardViewModel.CurrentRoi.CustoInstalacao:F2}");
						}

						byte[] csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
						return File(csvBytes, "text/csv", "SimulacaoDados.csv");
					}
				case "excel":
					{
						using (var workbook = new XLWorkbook())
						{
							var worksheet = workbook.Worksheets.Add("Simulação");
							worksheet.Cell("A1").Value = "Métrica";
							worksheet.Cell("B1").Value = "Valor";
							int row = 2;
							worksheet.Cell(row, 1).Value = "Média Semanal";
							worksheet.Cell(row, 2).Value = energyConsumptionViewModel.MediaSemana;
							row++;
							worksheet.Cell(row, 1).Value = "Média Fim de Semana";
							worksheet.Cell(row, 2).Value = energyConsumptionViewModel.MediaFimSemana;
							row++;
							worksheet.Cell(row, 1).Value = "Média Anual";
							worksheet.Cell(row, 2).Value = energyConsumptionViewModel.MediaAnual;
							row++;
							worksheet.Cell(row, 1).Value = "Tarifa Escolhida";
							worksheet.Cell(row, 2).Value = resultado.TarifaEscolhida;
							row++;
							worksheet.Cell(row, 1).Value = "Preço por kWh";
							worksheet.Cell(row, 2).Value = resultado.PrecoKwh;
							row++;
							worksheet.Cell(row, 1).Value = "Consumo Total";
							worksheet.Cell(row, 2).Value = resultado.ConsumoTotal;
							row++;
							worksheet.Cell(row, 1).Value = "Custo Anual";
							worksheet.Cell(row, 2).Value = resultado.ValorAnual;
							row++;
							if (roiDashboardViewModel.CurrentRoi != null)
							{
								worksheet.Cell(row, 1).Value = "Data do Cálculo";
								worksheet.Cell(row, 2).Value = roiDashboardViewModel.CurrentRoi.DataCalculado.ToString("dd/MM/yyyy");
								row++;
								worksheet.Cell(row, 1).Value = "ROI Atual";
								worksheet.Cell(row, 2).Value = roiDashboardViewModel.CurrentRoi.ROI;
								row++;
								worksheet.Cell(row, 1).Value = "Custo de Instalação";
								worksheet.Cell(row, 2).Value = roiDashboardViewModel.CurrentRoi.CustoInstalacao;
								row++;
							}
							using (var stream = new MemoryStream())
							{
								workbook.SaveAs(stream);
								return File(stream.ToArray(),
									"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
									"SimulacaoDados.xlsx");
							}
						}
					}
				case "pdf":
				{
					// Verificar se o caminho do executável do wkhtmltopdf está configurado corretamente
					var pdfResult = new Rotativa.AspNetCore.ViewAsPdf("SimulacaoCompletaPDF", viewModel)
					{
						FileName = "SimulacaoDados.pdf",
						// Adicionando a configuração de caminho do wkhtmltopdf se necessário
						CustomSwitches = "--no-stop-slow-scripts --disable-smart-shrinking"
					};

					return pdfResult;
				}
				default:
					return BadRequest("Tipo de exportação inválido.");
			}
		}
	}
}