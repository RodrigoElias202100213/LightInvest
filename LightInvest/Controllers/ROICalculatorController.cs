using Microsoft.AspNetCore.Mvc;
using LightInvest.Models;
using Microsoft.AspNetCore.Identity;
using System;
using LightInvest.Data;
using Microsoft.EntityFrameworkCore;

namespace LightInvest.Controllers
{
	public class ROICalculatorController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userManager;

		public ROICalculatorController(ApplicationDbContext context, UserManager<User> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: ROICalculator
		public ActionResult Index()
		{
			return View();
		}

		private async Task<User> GetLoggedInUser()
		{
			var userEmail = User.Identity.Name;  // Obtém o email do usuário autenticado
			if (string.IsNullOrEmpty(userEmail))
				return null;

			return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}


		[HttpPost]
		public async Task<IActionResult> Calcular(RoiCalculator model)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();
				ViewBag.Resultado = "Erro ao processar os valores: " + string.Join(", ", errors);
				return View("Index", model);
			}

			// 🔥 Buscar o usuário autenticado corretamente
			var user = await GetLoggedInUser();
			if (user == null)
			{
				ViewBag.Resultado = "Erro: Nenhum usuário autenticado!";
				return View("Index", model);
			}

			decimal resultadoROI;
			try
			{
				resultadoROI = model.CalcularROI();
			}
			catch (Exception ex)
			{
				ViewBag.Resultado = "Erro ao calcular o ROI: " + ex.Message;
				return View("Index", model);
			}

			// 🔥 Criar um novo objeto associando corretamente ao usuário autenticado
			var roiCalculation = new RoiCalculator()
			{
				UserId = user.Id,  // Associa ao ID do usuário autenticado
				User = user,        // Associa o próprio usuário autenticado
				CustoInstalacao = model.CustoInstalacao,
				CustoManutencaoAnual = model.CustoManutencaoAnual,
				ConsumoEnergeticoMedio = model.ConsumoEnergeticoMedio,
				ConsumoEnergeticoRede = model.ConsumoEnergeticoRede,
				RetornoEconomia = model.RetornoEconomia,
				ROI = resultadoROI,
				DataCalculado = DateTime.Now
			};

			_context.ROICalculators.Add(roiCalculation);
			await _context.SaveChangesAsync();

			ViewBag.Resultado = $"ROI Calculado: {resultadoROI}%";
			return View("Index", model);
		}

	}
}
