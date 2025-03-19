using System;
using System.Collections.Generic;
using System.Linq;
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

		// Módulo 2: Processa os dados da Tarifa e calcula o custo mensal e anual
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

		// Ação principal que integra os módulos e retorna a simulação completa
		public async Task<IActionResult> Simular()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return BadRequest("Usuário não autenticado.");

			// Processa o consumo de energia
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

			// Monta o view model completo para a simulação
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
				ResultadoTarifaViewModel = resultadoTarifa
			};

			return View("SimulacaoCompleta", simulacao);
		}
	}
}
