using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightInvest.Controllers
{
	public class EnergySimulationController : Controller
	{
		private readonly ApplicationDbContext _context;
		public EnergySimulationController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("energy-simulation")]
		public async Task<IActionResult> Simulation()
		{
			var model = InitializeViewModel();
			LoadTempData(model);
			return View(model);
		}

		private EnergyConsumptionViewModel InitializeViewModel()
		{
			return new EnergyConsumptionViewModel
			{
				ConsumoDiaSemana = Enumerable.Repeat(0m, 24).ToList(),
				ConsumoFimSemana = Enumerable.Repeat(0m, 24).ToList(),
				MesesOcupacao = new List<string>()
			};
		}

		private void LoadTempData(EnergyConsumptionViewModel model)
		{
			if (TempData["ConsumoTotal"] != null)
			{
				model.MediaAnual = decimal.Parse(TempData["ConsumoTotal"].ToString());
			}
		}

		private async Task<User> GetLoggedInUserAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			return string.IsNullOrEmpty(userEmail)
				? null
				: await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

		private bool ValidarConsumo(EnergyConsumptionViewModel model)
		{
			if (model.ConsumoDiaSemana.All(c => c == 0) && model.ConsumoFimSemana.All(c => c == 0))
			{
				ModelState.AddModelError("Consumo", "Por favor, preencha os campos de consumo.");
				return false;
			}

			return true;
		}


		[HttpPost("energy-simulation")]
		public async Task<IActionResult> Simulation(EnergyConsumptionViewModel model)
		{
			EnsureValidData(model);
			if (!ValidarConsumo(model))
			{
				return View(model);
			}


			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await GetLoggedInUserAsync();
			if (user == null)
			{
				ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
				return View("Error", model);
			}


			var consumo = new EnergyConsumption
			{
				UserEmail = user.Email,
				ConsumoDiaSemana = model.ConsumoDiaSemana,
				ConsumoFimSemana = model.ConsumoFimSemana,
				MesesOcupacao = model.MesesOcupacao
			};

			consumo.CalcularMedias();
			consumo.CalcularConsumoMensal();
			consumo.CalcularMediaAnual();

			model.MediaSemana = consumo.MediaSemana;
			model.MediaFimSemana = consumo.MediaFimSemana;
			model.MediaAnual = consumo.MediaAnual;
			model.ConsumoTotal = consumo.ConsumoTotal;

			await SaveConsumptionToDatabase(user.Email, consumo);

			StoreTempData(model);
			return RedirectToAction("Simulation", "Tarifa");
		}

		public IActionResult RedirectToEnergyView()
		{
			return RedirectToAction("Simulation", "EnergySimulation");
		}

		private void EnsureValidData(EnergyConsumptionViewModel model)
		{
			// Garantir que a lista tenha exatamente 24 elementos
			model.ConsumoDiaSemana = model.ConsumoDiaSemana?.Take(24).ToList() ?? Enumerable.Repeat(0m, 24).ToList();
			model.ConsumoFimSemana = model.ConsumoFimSemana?.Take(24).ToList() ?? Enumerable.Repeat(0m, 24).ToList();
			model.MesesOcupacao ??= new List<string>();
		}



		private async Task SaveConsumptionToDatabase(string userEmail, EnergyConsumption consumo)
		{
			var consumoExistente = await _context.EnergyConsumptions
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.UserEmail == userEmail);

			if (consumoExistente != null)
			{
				consumoExistente.ConsumoDiaSemana = consumo.ConsumoDiaSemana;
				consumoExistente.ConsumoFimSemana = consumo.ConsumoFimSemana;
				consumoExistente.MesesOcupacao = consumo.MesesOcupacao;
				consumoExistente.MediaSemana = consumo.MediaSemana;
				consumoExistente.MediaFimSemana = consumo.MediaFimSemana;
				consumoExistente.MediaAnual = consumo.MediaAnual;
				consumoExistente.ConsumoTotal = consumo.ConsumoTotal;

				_context.Entry(consumoExistente).State = EntityState.Modified;
			}
			else
			{
				_context.EnergyConsumptions.Add(consumo);
			}

			await _context.SaveChangesAsync();
		}

		private void StoreTempData(EnergyConsumptionViewModel model)
		{
			TempData["ConsumoTotal"] = model.MediaAnual.ToString("F2");
			TempData["MesesOcupacao"] = model.MesesOcupacao;
		}

	}
}