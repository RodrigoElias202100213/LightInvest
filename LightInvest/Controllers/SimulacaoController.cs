using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
				return Unauthorized("Usuário não autenticado.");
			}

			// Fetch energy consumption data
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
				return NotFound("Nenhuma tarifa associada ao usuário.");
			}

			// Fetch installation data including panel models and available powers
			var dadosInstalacao = await _context.DadosInstalacao
				.Include(di => di.ModeloPainel)
				.ThenInclude(mp => mp.Potencias) // Includes the available powers
				.Include(di => di.Cidade)
				.Where(di => di.UserEmail == user.Email)
				.FirstOrDefaultAsync();

			// Ensure that installation data exists
			if (dadosInstalacao == null)
			{
				return NotFound("Nenhum dado de instalação encontrado.");
			}


			var potenciaPainel = dadosInstalacao?.ModeloPainel?.Potencias
				.FirstOrDefault(p => p.Id == dadosInstalacao.ConsumoPainel)?.Potencia ?? 0;

			if (potenciaPainel == 0)
			{
				return BadRequest("Nenhuma potência registrada para o modelo do painel solar.");
			}

			// Calculate energy consumption metrics
			energyConsumption.CalcularMedias();
			decimal custoAnual = energyConsumption.MediaAnual * tarifa.PrecoKwh;

			dadosInstalacao.AtualizarPrecoInstalacao();

			// Monthly consumption calculation
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



			// Crie um modelo de exibição que inclui a potência
			var viewModel = new SimulacaoCompletaViewModel
			{
				EnergyConsumptionViewModel = energyConsumptionViewModel,
				ResultadoTarifaViewModel = resultado,
				DadosInstalacao = dadosInstalacao,
				PrecoInstalacao = dadosInstalacao.PrecoInstalacao,
				PotenciaPainel = potenciaPainel // Adicionando a potência aqui
			};

			// Return the view with the model
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

			// Obter os dados da simulação completa
			var simulacao = await SimulacaoCompleta(); // Retorna o ViewResult diretamente
			if (simulacao is not ViewResult viewResult || viewResult.Model is not SimulacaoCompletaViewModel viewModel)
			{
				return NotFound("Erro ao obter dados da simulação.");
			}

			var roiCalculator = new RoiCalculator
			{
				UserEmail = user.Email,
				CustoInstalacao = viewModel.PrecoInstalacao,
				CustoManutencaoAnual = 1, // Exemplo fixo
				ConsumoEnergeticoMedio = viewModel.EnergyConsumptionViewModel.MediaAnual,
				ConsumoEnergeticoRede = viewModel.EnergyConsumptionViewModel.MediaAnual,
				RetornoEconomia = viewModel.ResultadoTarifaViewModel.ValorAnual,
				DataCalculado = DateTime.Now
			};

			if (roiCalculator.RetornoEconomia <= 0)
			{
				return BadRequest("A economia anual deve ser maior que zero para calcular o ROI.");
			}

			roiCalculator.ROI = roiCalculator.CalcularROI();
			_context.ROICalculators.Add(roiCalculator);
			await _context.SaveChangesAsync();

			// Criar a ViewModel de ROI e adicionar o ROI calculado
			var roiDashboardViewModel = new RoiCalculatorDashboardViewModel
			{
				CurrentRoi = roiCalculator,
				History = _context.ROICalculators.ToList() // Exemplo para carregar o histórico de ROI
			};

			// Criar o ViewModel de Simulação Completa e adicionar o ROI calculado
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
	}
}