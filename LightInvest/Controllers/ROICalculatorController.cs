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

	// GET: ROICalculator
	public async Task<ActionResult> Index()
	{
		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			ViewBag.Resultado = "Erro: Nenhum usuário autenticado!";
			return View();
		}

		// Buscar um cálculo de ROI existente ou criar um novo com valores iniciais "0"
		var roiCalculation = await _context.ROICalculators
			.Where(r => r.UserEmail == user.Email)
			.FirstOrDefaultAsync();

		if (roiCalculation == null)
		{
			// Caso não exista cálculo anterior, cria-se um novo
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

		// Passar o cálculo existente ou o novo para a View
		return View(roiCalculation);
	}

	// Método para obter o usuário autenticado a partir da sessão
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
			ViewBag.Resultado = "Erro: Nenhum usuário autenticado!";
			return View("Index", model);
		}

		if (model.RetornoEconomia <= 0)
		{
			ViewBag.Resultado = "Erro: A economia total deve ser maior que zero!";
			return View("Index", model);
		}

		// Buscar o cálculo de ROI existente
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

		// Calcular a economia anual (valor que reduz a dívida a cada ano)
		decimal economiaAnual = (roiCalculation.ConsumoEnergeticoRede - roiCalculation.ConsumoEnergeticoMedio)
								 * roiCalculation.RetornoEconomia - roiCalculation.CustoManutencaoAnual;

		// Determinar o número total de anos (arredondando para cima)
		int totalAnos = (int)Math.Ceiling(resultadoROI);

		// Gerar listas com os anos e o valor da dívida remanescente para cada ano
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

		// Passa os arrays para a view via ViewBag
		ViewBag.Years = anos;
		ViewBag.Debts = dividas;

		// Opcional: Buscar histórico de cálculos se desejar mostrar junto
		var history = await _context.ROICalculators
			.Where(r => r.UserEmail == user.Email)
			.OrderBy(r => r.DataCalculado)
			.ToListAsync();

		// Cria o ViewModel do Dashboard (já criado anteriormente)
		var dashboardViewModel = new RoiCalculatorDashboardViewModel
		{
			CurrentRoi = roiCalculation,
			History = history
		};

		ViewBag.Resultado = $"ROI Calculado: {resultadoROI:F2} anos"; // Limitando para 2 casas decimais

		return View("Dashboard", dashboardViewModel);
	}
	
	
	[HttpGet]
	public async Task<IActionResult> Grafico()
	{
		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			TempData["Resultado"] = "Erro: Nenhum usuário autenticado!";
			return RedirectToAction("Index");
		}

		// Recupera todos os registros de ROI do usuário, ordenados pela data
		var roiRecords = await _context.ROICalculators
			.Where(r => r.UserEmail == user.Email)
			.OrderBy(r => r.DataCalculado)
			.ToListAsync();

		return View(roiRecords);
	}

}
