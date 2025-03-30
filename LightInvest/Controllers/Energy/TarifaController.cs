/*
 * O TarifaController lida com a simulação de tarifas de energia, permitindo ao utilizador escolher o tipo de tarifa
 * e calcular o preço final com base nas suas escolhas. O controlador salva as informações de tarifa na base de dados
 * e armazena o preço final temporariamente para uso posterior.
 * Após a simulação, o utilizador é redirecionado para o processo de criação de dados de instalação.
 */



using System;
using System.Linq;
using System.Threading.Tasks;
using LightInvest.Models.BD;
using LightInvest.Models.Simulacao.Tarifa;
using LightInvest.Models.Utilizador.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightInvest.Controllers.Energy
{
	public class TarifaController : Controller
	{
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// Initializes the TarifaController with the provided database context.
		/// </summary>
		/// <param name="context">The database context for accessing application data.</param>
		public TarifaController(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Displays the tariff simulation page, initializing the view model and loading any necessary data.
		/// </summary>
		/// <returns>The tariff simulation view with the initialized model.</returns>
		[HttpGet("tarifa-simulation")]
		public async Task<IActionResult> Simulation()
		{
			var model = InitializeViewModel();
			LoadTempData(model);
			return View(model);
		}

		/// <summary>
		/// Initializes the view model for tariff simulation.
		/// </summary>
		/// <returns>A new instance of the TarifaViewModel.</returns>
		private TarifaViewModel InitializeViewModel()
		{
			return new TarifaViewModel();
		}

		/// <summary>
		/// Loads temporary data (such as final price) from TempData into the view model.
		/// </summary>
		/// <param name="model">The TarifaViewModel to load data into.</param>
		private void LoadTempData(TarifaViewModel model)
		{
			if (TempData["PrecoFinal"] != null)
			{
				ViewBag.PrecoFinal = TempData["PrecoFinal"].ToString();
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
		/// Handles the form submission for the tariff simulation and calculates the final price based on user input.
		/// The resulting tariff information is saved to the database and the final price is stored in TempData.
		/// </summary>
		/// <param name="model">The TarifaViewModel containing the user input for the tariff simulation.</param>
		/// <returns>The redirect to the next action (Create in DadosInstalacao).</returns>
		[HttpPost("tarifa-simulation")]
		public async Task<IActionResult> Simulation(TarifaViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await GetLoggedInUserAsync();
			if (user == null)
			{
				ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
				return View(model);
			}

			if (model.TipoDeTarifaEscolhida != null)
			{
				var tarifa = new Tarifa
				{
					UserEmail = user.Email,
					PrecoKWh = model.PrecoKWh,
					Tipo = model.TipoDeTarifaEscolhida.Value
				};

				await SaveTarifaToDatabase(user.Email, tarifa);
				TempData["PrecoFinal"] = tarifa.PrecoFinal.ToString("F2");
			}

			return RedirectToAction("Create", "DadosInstalacao");
		}

		/// <summary>
		/// Saves the tariff data to the database. If a tariff already exists for the user, it updates the existing record.
		/// Otherwise, a new tariff record is added.
		/// </summary>
		/// <param name="userEmail">The email address of the user.</param>
		/// <param name="tarifa">The Tarifa object containing the tariff data to be saved.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		private async Task SaveTarifaToDatabase(string userEmail, Tarifa tarifa)
		{
			var tarifaExistente = await _context.Tarifas.FirstOrDefaultAsync(t => t.UserEmail == userEmail);

			if (tarifaExistente != null)
			{
				tarifaExistente.PrecoKWh = tarifa.PrecoKWh;
				tarifaExistente.Tipo = tarifa.Tipo;
				typeof(Tarifa).GetProperty("DataAlteracao")?.SetValue(tarifaExistente, DateTime.Now);

				_context.Tarifas.Update(tarifaExistente);
			}
			else
			{
				_context.Tarifas.Add(tarifa);
			}

			await _context.SaveChangesAsync();
		}
	}
}
