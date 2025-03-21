using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ROICalculatorController : Controller
{
	private readonly ApplicationDbContext _context;

	public ROICalculatorController(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<ActionResult> Index()
	{
		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
			return View();
		}

		var roiCalculation = await _context.ROICalculators
			.Where(r => r.UserEmail == user.Email)
			.FirstOrDefaultAsync();

		if (roiCalculation == null)
		{
			roiCalculation = new RoiCalculator
			{
				UserEmail = user.Email,
				CustoInstalacao = 0,
				CustoManutencaoAnual = 0,
				ConsumoEnergeticoMedio = 0,
				ConsumoEnergeticoRede = 0,
				RetornoEconomia = 0,
				ROI = 0,
				DataCalculado = DateTime.Now
			};

			_context.ROICalculators.Add(roiCalculation);
			await _context.SaveChangesAsync();
		}

		return View(roiCalculation);
	}

	private async Task<User> GetLoggedInUserAsync()
	{
		var userEmail = HttpContext.Session.GetString("UserEmail");
		if (string.IsNullOrEmpty(userEmail))
			return null;

		return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
	}
	[HttpPost]
	public async Task<IActionResult> Calcular(RoiCalculator model)
	{
		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
			return View("Index", model);
		}
		/*
		if (model.RetornoEconomia <= 0)
		{
			ViewBag.Resultado = "Erro: A economia total deve ser maior que zero!";
			return View("Index", model);
		}
		*/
		var roiCalculation = await _context.ROICalculators
			.Where(r => r.UserEmail == user.Email)
			.FirstOrDefaultAsync();

		if (roiCalculation == null)
		{
			roiCalculation = new RoiCalculator
			{
				UserEmail = user.Email,
				CustoInstalacao = model.CustoInstalacao,
				CustoManutencaoAnual = model.CustoManutencaoAnual,
				ConsumoEnergeticoMedio = model.ConsumoEnergeticoMedio,
				ConsumoEnergeticoRede = model.ConsumoEnergeticoRede,
				RetornoEconomia = model.RetornoEconomia,
				ROI = 0,
				DataCalculado = DateTime.Now
			};

			_context.ROICalculators.Add(roiCalculation);
			await _context.SaveChangesAsync();
		}
		else
		{
			roiCalculation.CustoInstalacao = model.CustoInstalacao;
			roiCalculation.CustoManutencaoAnual = model.CustoManutencaoAnual;
			roiCalculation.ConsumoEnergeticoMedio = model.ConsumoEnergeticoMedio;
			roiCalculation.ConsumoEnergeticoRede = model.ConsumoEnergeticoRede;
			roiCalculation.RetornoEconomia = model.RetornoEconomia;
			roiCalculation.DataCalculado = DateTime.Now;

			_context.ROICalculators.Update(roiCalculation);
			await _context.SaveChangesAsync();
		}

		decimal resultadoROI = 0;
		try
		{
			resultadoROI = roiCalculation.CalcularROI();
			roiCalculation.ROI = resultadoROI;
			_context.ROICalculators.Update(roiCalculation);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			ViewBag.Resultado = "Erro ao calcular o ROI: " + ex.Message;
			return View("Index", model);
		}

		decimal economiaAnual = (roiCalculation.ConsumoEnergeticoRede - roiCalculation.ConsumoEnergeticoMedio)
								 * roiCalculation.RetornoEconomia - roiCalculation.CustoManutencaoAnual;

		int totalAnos = (int)Math.Ceiling(resultadoROI);

		var anos = new List<int>();
		var dividas = new List<decimal>();
		for (int ano = 0; ano <= totalAnos; ano++)
		{
			anos.Add(ano);
			decimal dividaRestante = roiCalculation.CustoInstalacao - (economiaAnual * ano);
			if (dividaRestante < 0)
				dividaRestante = 0;
			dividas.Add(dividaRestante);
		}

		ViewBag.Years = anos;
		ViewBag.Debts = dividas;

		var history = await _context.ROICalculators
			.Where(r => r.UserEmail == user.Email)
			.OrderBy(r => r.DataCalculado)
			.ToListAsync();

		var dashboardViewModel = new RoiCalculatorDashboardViewModel
		{
			CurrentRoi = roiCalculation,
			History = history
		};

<<<<<<< HEAD
		ViewBag.Resultado = $"{resultadoROI:F2} anos"; 
=======
		ViewBag.Resultado = $"{resultadoROI:F2} anos";
>>>>>>> backup

		return View("Dashboard", dashboardViewModel);
	}


	[HttpGet]
	public async Task<IActionResult> Grafico()
	{
		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			TempData["Resultado"] = "Erro: Nenhum utilizador autenticado!";
			return RedirectToAction("Index");
		}
		var roiRecords = await _context.ROICalculators
			.Where(r => r.UserEmail == user.Email)
			.OrderBy(r => r.DataCalculado)
			.ToListAsync();

		return View(roiRecords);
	}

}