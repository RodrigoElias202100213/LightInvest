using Microsoft.AspNetCore.Mvc;
using LightInvest.Models;
using Microsoft.AspNetCore.Identity;
using System;
using LightInvest.Data;

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

		[HttpPost]
		public async Task<ActionResult> Calcular(ROICalculator model)
		{
			if (ModelState.IsValid)
			{
				// Calcular o ROI
				decimal resultadoROI = model.CalcularROI();

				// Obter o id do usuário logado
				var user = await _userManager.GetUserAsync(User);
				var userId = user?.Id;

				if (userId != null)
				{
					// Criar a nova entrada no banco de dados
					var roiCalculation = new ROICalculator()
					{
						UserId = userId.Value,
						CustoInstalacao = model.CustoInstalacao,
						CustoManutencaoAnual = model.CustoManutencaoAnual,
						ConsumoEnergeticoMedio = model.ConsumoEnergeticoMedio,
						ConsumoEnergeticoRede = model.ConsumoEnergeticoRede,
						RetornoEconomia = model.RetornoEconomia,
						ROI = resultadoROI,
						DataCalculado = DateTime.Now
					};

					// Salvar no banco de dados
					_context.ROICalculators.Add(roiCalculation);
					await _context.SaveChangesAsync();
				}

				// Exibir o resultado na view
				ViewBag.Resultado = resultadoROI;
			}
			else
			{
				ViewBag.Resultado = "Valores inválidos!";
			}

			return View("Index", model);
		}
	}
}
