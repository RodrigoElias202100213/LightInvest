using System;
using System.Linq;
using System.Threading.Tasks;
using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightInvest.Controllers
{
	public class TarifaController : Controller
	{
		private readonly ApplicationDbContext _context;

		public TarifaController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("tarifa-simulation")]
		public async Task<IActionResult> Simulation()
		{
			var model = InitializeViewModel();
			LoadTempData(model);
			return View(model);
		}

		private TarifaViewModel InitializeViewModel()
		{
			return new TarifaViewModel();
		}

		private void LoadTempData(TarifaViewModel model)
		{
			if (TempData["PrecoFinal"] != null)
			{
				ViewBag.PrecoFinal = TempData["PrecoFinal"].ToString();
			}
		}
		private async Task<User> GetLoggedInUserAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			return string.IsNullOrEmpty(userEmail)
				? null
				: await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

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
					Tipo = model.TipoDeTarifaEscolhida.Value // Agora garantimos que não será null
				};

				await SaveTarifaToDatabase(user.Email, tarifa);

				TempData["PrecoFinal"] = tarifa.PrecoFinal.ToString("F2");
			}

			return RedirectToAction("Create", "DadosInstalacao");
		}

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
