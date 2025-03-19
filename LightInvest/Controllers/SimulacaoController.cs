using LightInvest.Models;
using LightInvest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

		// Método para exibir a visão com os dados calculados
		[HttpGet("simulacao-completa")]
		public async Task<IActionResult> SimulacaoCompleta()
		{
			var model = await CarregarSimulacaoCompletaAsync();
			return View(model);
		}

		// Método privado para carregar os dados necessários para a simulação
		private async Task<SimulacaoCompletaViewModel> CarregarSimulacaoCompletaAsync()
		{
			var consumoEnergetico = await ObterConsumoEnergeticoAsync();
			var tarifa = await ObterTarifaAsync();
			var dadosInstalacao = await ObterDadosInstalacaoAsync();

			var precoInstalacao = CalcularPrecoInstalacao(dadosInstalacao);
			var potenciaPainel = CalcularPotenciaPainel(dadosInstalacao);

			return new SimulacaoCompletaViewModel
			{
				EnergyConsumptionViewModel = consumoEnergetico,
				ResultadoTarifaViewModel = tarifa,
				DadosInstalacao = dadosInstalacao,
				PrecoInstalacao = precoInstalacao,
				PotenciaPainel = potenciaPainel
			};
		}

		// Método para obter o consumo energético do usuário
		private async Task<EnergyConsumptionViewModel> ObterConsumoEnergeticoAsync()
		{
			// Implementar a lógica para obter os dados de consumo energético
			// Exemplo:
			var consumo = await _context.EnergyConsumptions
				.Where(e => e.UserEmail == "usuario@example.com")
				.FirstOrDefaultAsync();

			return new EnergyConsumptionViewModel
			{
				/*
				ConsumoMensal = consumo?.ConsumoMensal ?? 0,
				ConsumoAnual = consumo?.ConsumoAnual ?? 0
			*/
				};
		}

		// Método para obter a tarifa do usuário
		private async Task<TarifaViewModel> ObterTarifaAsync()
		{
			// Implementar a lógica para obter os dados de tarifa
			// Exemplo:
			var tarifa = await _context.Tarifas
				.Where(t => t.UserEmail == "usuario@example.com")
				.FirstOrDefaultAsync();

			return new TarifaViewModel
			{
				PrecoKWh = tarifa?.PrecoKWh ?? 0,
				TipoDeTarifa = tarifa?.Tipo ?? "Desconhecido"
			};
		}

		// Método para obter os dados de instalação do usuário
		private async Task<DadosInstalacao> ObterDadosInstalacaoAsync()
		{
			// Implementar a lógica para obter os dados de instalação
			// Exemplo:
			var instalacao = await _context.DadosInstalacao
				.Where(d => d.UserEmail == "usuario@example.com")
				.FirstOrDefaultAsync();

			return instalacao ?? new DadosInstalacao();
		}

		// Método para calcular o preço da instalação
		private decimal CalcularPrecoInstalacao(DadosInstalacao dadosInstalacao)
		{
			// Implementar a lógica para calcular o preço da instalação
			// Exemplo:
			return dadosInstalacao.NumeroPaineis * 1000; // Valor fictício
		}

		// Método para calcular a potência do painel
		private decimal CalcularPotenciaPainel(DadosInstalacao dadosInstalacao)
		{
			// Implementar a lógica para calcular a potência do painel
			// Exemplo:
			return dadosInstalacao.NumeroPaineis * 300; // Valor fictício
		}
	}
}
