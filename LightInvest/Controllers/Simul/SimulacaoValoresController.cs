/*
 * O SimulacaoValoresController lida com o processo de simulação de consumo de energia, tarifas e cálculo de retorno sobre o investimento (ROI) para o utilizador.
 * Este controlador obtém os dados de consumo do utilizador, calcula os custos mensais e anuais de energia com base na tarifa escolhida, e também calcula o ROI de um sistema de painéis solares com base nos dados de instalação.
 * Além disso, oferece funcionalidades para exportar os dados da simulação em formato CSV ou PDF (a exportação para PDF ainda não está implementada).
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInvest.Models;
using LightInvest.Models.BD;
using LightInvest.Models.Ener;
using LightInvest.Models.Roi;
using LightInvest.Models.Simulacao.Energ;
using LightInvest.Models.Simulacao.Tarifa;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightInvest.Controllers.Simul
{
	public class SimulacaoValoresController : Controller
	{
		private readonly ApplicationDbContext _context;

		public SimulacaoValoresController(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Processes the user's energy consumption data based on their email.
		/// </summary>
		/// <param name="userEmail">The email address of the currently authenticated user.</param>
		/// <returns>
		/// An <see cref="EnergyConsumption"/> object containing processed consumption data.
		/// </returns>
		/// <remarks>
		/// This method calculates average energy consumption, monthly consumption, and annual consumption for the user.
		/// </remarks>
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

		/// <summary>
		/// Processes the user's energy tariff and calculates the monthly and annual costs based on their consumption.
		/// </summary>
		/// <param name="userEmail">The email address of the currently authenticated user.</param>
		/// <param name="consumo">The <see cref="EnergyConsumption"/> object that holds the user's consumption data.</param>
		/// <returns>
		/// A <see cref="ResultadoTarifaViewModel"/> object containing the tariff information and calculated costs for each month.
		/// </returns>
		/// <remarks>
		/// This method calculates the monthly and annual energy consumption cost based on the user's chosen tariff and consumption data.
		/// </remarks>
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
				decimal consumoSemana = consumo.MediaSemana * 5 + consumo.MediaFimSemana * 2;
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

		/// <summary>
		/// Simulates the energy consumption, tariff, and ROI calculations for the user, and returns the simulation result.
		/// </summary>
		/// <returns>
		/// A view with the simulation result displayed, including energy consumption data, tariff information, and ROI calculation.
		/// </returns>
		/// <remarks>
		/// This method aggregates the user’s energy consumption data, calculates the monthly and annual costs, and computes the return on investment (ROI) based on solar panel installation.
		/// </remarks>
		public async Task<IActionResult> Simular()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (!HttpContext.Session.TryGetValue("UserEmail", out _))
			{
				return Unauthorized("Utilizador não autenticado.");
			}

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
			decimal horasSolDiarias = 10m;
			decimal diasNoMes = 30m;
			decimal energiaGeradaMensal = potenciaPainel * numeroPaineis * horasSolDiarias * diasNoMes / 1000;
			decimal economiaMensal = energiaGeradaMensal * tarifaForVM.PrecoKWh;
			int mesesOcupados = consumo.MesesOcupacao.Count;
			decimal economiaAnual = economiaMensal * mesesOcupados;

			decimal custoInstalacao = dadosInstalacao.PrecoInstalacao;
			decimal custoManutencaoAnual = 50m;
			decimal roiValue = custoInstalacao / (economiaAnual - custoManutencaoAnual);


			var roiData = new RoiCalculator
			{
				UserEmail = userEmail,
				CustoInstalacao = custoInstalacao,
				CustoManutencaoAnual = custoManutencaoAnual,
				ConsumoEnergeticoMedio = consumo.ConsumoTotal / energiaGeradaMensal*12,
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

		/// <summary>
		/// Gets the name of the month from its numeric representation (1-12).
		/// </summary>
		/// <param name="numeroMes">The numeric value of the month (1 for January, 12 for December).</param>
		/// <returns>
		/// The name of the month as a string. For example, "January" for 1, "December" for 12.
		/// </returns>
		/// <remarks>
		/// This method converts the numeric month value to its string name, which is used for displaying month names in reports.
		/// </remarks>
		private string ObterNomeDoMes(int numeroMes)
		{
			string[] meses = { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
						"Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
			return numeroMes >= 1 && numeroMes <= 12 ? meses[numeroMes - 1] : "Mês Inválido";
		}

		/// <summary>
		/// Exports the simulation data as a PDF file (functionality not yet implemented).
		/// </summary>
		/// <returns>
		/// A message indicating that the PDF export functionality is not yet implemented.
		/// </returns>
		/// <remarks>
		/// This method currently only returns a message indicating that PDF export functionality has not been implemented.
		/// </remarks>
		public async Task<IActionResult> ExportPDF()
		{

			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return BadRequest("Utilizador não autenticado.");

			var viewModel = await GerarViewModelCompleto(userEmail);

			return Content("Funcionalidade de exportação para PDF não implementada.");
		}

		/// <summary>
		/// Exports the simulation data as a CSV file.
		/// </summary>
		/// <returns>
		/// A CSV file containing the energy consumption and tariff simulation data.
		/// </returns>
		/// <remarks>
		/// This method generates a CSV file with key simulation data, including weekly consumption averages, total consumption, and annual consumption costs.
		/// </remarks>
		[HttpGet]
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
			return Content(csv.ToString(), "text/csv", Encoding.UTF8);
		}


		public async Task<IActionResult> ExportExcel()
		{

			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return BadRequest("Utilizador não autenticado.");

			var viewModel = await GerarViewModelCompleto(userEmail);

			return Content("Funcionalidade de exportação para PDF não implementada.");
		}


		/// <summary>
		/// Generates the complete view model for the user, including energy consumption, tariff, and ROI data.
		/// </summary>
		/// <param name="userEmail">The email address of the currently authenticated user.</param>
		/// <returns>
		/// A <see cref="SimulacaoCompletaViewModel"/> object containing all necessary simulation data for the view.
		/// </returns>
		/// <remarks>
		/// This method aggregates all relevant data (energy consumption, tariff, ROI, etc.) and prepares it for display in the simulation view.
		/// </remarks>
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
					
				},
				TarifaViewModel = new TarifaViewModel(await _context.Tarifas.FirstOrDefaultAsync(t => t.UserEmail == userEmail)),
				ResultadoTarifaViewModel = resultadoTarifa,
				DadosInstalacao = dadosInstalacao,
				ROI = roiDashboard,

			};

			return simulacao;
		}
	}
}