using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightInvest.Controllers
{
	public class SimulacaoController : Controller
	{
		private readonly ApplicationDbContext _context;

		public SimulacaoController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Método auxiliar para obter o usuário logado
		private async Task<User> GetLoggedInUserAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			return string.IsNullOrEmpty(userEmail)
				? null
				: await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

		// Método auxiliar para mapear dados de consumo de energia para o ViewModel
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

			// Buscar consumo de energia do usuário
			var energyConsumption = await _context.EnergyConsumptions
				.Where(ec => ec.UserEmail == user.Email)
				.FirstOrDefaultAsync();

			if (energyConsumption == null)
			{
				return NotFound("Nenhum dado de consumo encontrado.");
			}

			// Buscar tarifa do usuário
			var tarifa = await _context.Tarifas
				.Where(t => t.UserEmail == user.Email)
				.OrderByDescending(t => t.DataAlteracao)
				.FirstOrDefaultAsync();

			if (tarifa == null)
			{
				return NotFound("Nenhuma tarifa associada ao usuário.");
			}

			// Buscar dados de instalação do usuário, incluindo as entidades relacionadas (ModeloPainel, Cidade)
			var dadosInstalacao = await _context.DadosInstalacao
				.Include(di => di.ModeloPainel) // Incluir ModeloPainel se for uma entidade relacionada
				.Include(di => di.Cidade) // Incluir Cidade se for uma entidade relacionada
				.Where(di => di.UserEmail == user.Email)
				.FirstOrDefaultAsync();

			if (dadosInstalacao == null)
			{
				return NotFound("Nenhum dado de instalação encontrado.");
			}

			// Calcular custo do consumo
			energyConsumption.CalcularMedias();
			decimal custoAnual = energyConsumption.MediaAnual * tarifa.PrecoKwh;

			// Calcular o preço de instalação
			dadosInstalacao.AtualizarPrecoInstalacao();

			// Calcular o custo mensal considerando o número de meses ocupados
			var consumoMensal =
				energyConsumption.MediaAnual /
				energyConsumption.MesesOcupacao.Count; // Dividido pelo número de meses ocupados
			decimal custoMensal = consumoMensal * tarifa.PrecoKwh;

			// Preencher o ViewModel de Resultado de Tarifa
			var resultado = new ResultadoTarifaViewModel
			{
				ConsumoTotal = energyConsumption.MediaAnual, // Usando média anual de consumo
				ValorAnual = custoAnual,
				TarifaEscolhida = tarifa.Nome.ToString(), // Corrigido para usar o nome do enum
				PrecoKwh = tarifa.PrecoKwh,
				MesesOcupacao = energyConsumption.MesesOcupacao,
				ConsumoMensal = energyConsumption.MesesOcupacao
					.Select(mes => new MesConsumo
					{
						Mes = mes,
						Consumo = consumoMensal, // Agora usando o consumo mensal calculado
						Custo = custoMensal // Agora usando o custo mensal calculado
					})
					.ToList()
			};

			// Preencher o ViewModel de Consumo de Energia
			var energyConsumptionViewModel = new EnergyConsumptionViewModel
			{
				ConsumoDiaSemana = energyConsumption.ConsumoDiaSemana,
				ConsumoFimSemana = energyConsumption.ConsumoFimSemana,
				MesesOcupacao = energyConsumption.MesesOcupacao,
				MediaSemana = energyConsumption.MediaSemana,
				MediaFimSemana = energyConsumption.MediaFimSemana,
				MediaAnual = energyConsumption.MediaAnual
			};

			// Criar o ViewModel composto com os dados de instalação
			var viewModel = new SimulacaoCompletaViewModel
			{
				EnergyConsumptionViewModel = energyConsumptionViewModel,
				ResultadoTarifaViewModel = resultado,
				DadosInstalacao = dadosInstalacao,
				PrecoInstalacao = dadosInstalacao.PrecoInstalacao
			};

			return View("UserEnergyConsumption", viewModel);
		}
	}
}
