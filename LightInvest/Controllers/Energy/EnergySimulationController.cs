/*
 * O EnergySimulationController lida com a simulação de consumo energético, incluindo a exibição do formulário,
 * a validação dos dados fornecidos pelo utilizador e o armazenamento dos dados da simulação na base de dados.
 * Ele também calcula a média anual de consumo e redireciona o utilizador para outras simulações, como a simulação de tarifas.
 * O controlador lida com os dados temporários, garantindo que as informações sejam persistidas entre as solicitações.
 */
using LightInvest.Models.BD;
using LightInvest.Models.Simulacao.Energ;
using LightInvest.Models.Utilizador.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightInvest.Controllers.Energy
{
	/// <summary>
	/// Handles the energy consumption simulation, including form submission, validation, and saving the simulation data.
	/// </summary>
	public class EnergySimulationController : Controller
	{
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// Initializes the EnergySimulationController with the provided database context.
		/// </summary>
		/// <param name="context">The database context for accessing application data.</param>
		public EnergySimulationController(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Displays the energy consumption simulation page.
		/// </summary>
		/// <returns>The view for energy simulation.</returns>
		[HttpGet("energy-simulation")]
		public async Task<IActionResult> Simulation()
		{
			var model = InitializeViewModel();
			LoadTempData(model);
			return View(model);
		}

		/// <summary>
		/// Initializes the view model for energy consumption simulation with default values.
		/// </summary>
		/// <returns>The initialized energy consumption view model.</returns>
		private EnergyConsumptionViewModel InitializeViewModel()
		{
			return new EnergyConsumptionViewModel
			{
				ConsumoDiaSemana = Enumerable.Repeat(0m, 24).ToList(),
				ConsumoFimSemana = Enumerable.Repeat(0m, 24).ToList(),
				MesesOcupacao = new List<string>()
			};
		}

		/// <summary>
		/// Loads data from TempData into the view model.
		/// </summary>
		/// <param name="model">The energy consumption view model to populate.</param>
		private void LoadTempData(EnergyConsumptionViewModel model)
		{
			if (TempData["ConsumoTotal"] != null)
			{
				model.MediaAnual = decimal.Parse(TempData["ConsumoTotal"].ToString());
			}
		}

		/// <summary>
		/// Retrieves the currently logged-in user based on the session data.
		/// </summary>
		/// <returns>The logged-in user if found; otherwise, null.</returns>
		private async Task<User> GetLoggedInUserAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			return string.IsNullOrEmpty(userEmail)
				? null
				: await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

		/// <summary>
		/// Validates the consumption data provided by the user in the view model.
		/// </summary>
		/// <param name="model">The energy consumption view model to validate.</param>
		/// <returns>True if the consumption data is valid; otherwise, false.</returns>
		private bool ValidarConsumo(EnergyConsumptionViewModel model)
		{
			if (model.ConsumoDiaSemana.All(c => c == 0) && model.ConsumoFimSemana.All(c => c == 0))
			{
				ModelState.AddModelError("Consumo", "Por favor preencha os campos de consumo.");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Handles the POST request to submit energy consumption data.
		/// </summary>
		/// <param name="model">The energy consumption view model containing the user's data.</param>
		/// <returns>A redirect to the tariff simulation page if valid; otherwise, re-renders the current view with errors.</returns>
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

		/// <summary>
		/// Redirects to the energy simulation view.
		/// </summary>
		/// <returns>A redirect to the energy simulation page.</returns>
		public IActionResult RedirectToEnergyView()
		{
			return RedirectToAction("Simulation", "EnergySimulation");
		}

		/// <summary>
		/// Ensures the data provided by the user is valid and initializes default values if necessary.
		/// </summary>
		/// <param name="model">The energy consumption view model to validate and modify.</param>
		private void EnsureValidData(EnergyConsumptionViewModel model)
		{
			model.ConsumoDiaSemana = model.ConsumoDiaSemana?.Take(24).ToList() ?? Enumerable.Repeat(0m, 24).ToList();
			model.ConsumoFimSemana = model.ConsumoFimSemana?.Take(24).ToList() ?? Enumerable.Repeat(0m, 24).ToList();
			model.MesesOcupacao ??= new List<string>();
		}

		/// <summary>
		/// Saves the energy consumption data to the database.
		/// </summary>
		/// <param name="userEmail">The email address of the user whose consumption data is being saved.</param>
		/// <param name="consumo">The energy consumption data to save.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
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

		/// <summary>
		/// Stores energy consumption data in TempData for use in subsequent requests.
		/// </summary>
		/// <param name="model">The energy consumption view model to store in TempData.</param>
		private void StoreTempData(EnergyConsumptionViewModel model)
		{
			TempData["ConsumoTotal"] = model.MediaAnual.ToString("F2");
			TempData["MesesOcupacao"] = model.MesesOcupacao;
		}
	}
}
