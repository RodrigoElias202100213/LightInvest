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

		// Buscar o cálculo de ROI existente
		var roiCalculation = await _context.ROICalculators
			.Where(r => r.UserEmail == user.Email)
			.FirstOrDefaultAsync();

		if (roiCalculation == null)
		{
			// Caso não exista cálculo anterior, criamos um novo
			roiCalculation = new RoiCalculator
			{
				UserEmail = user.Email,
				CustoInstalacao = model.CustoInstalacao,
				CustoManutencaoAnual = model.CustoManutencaoAnual,
				ConsumoEnergeticoMedio = model.ConsumoEnergeticoMedio,
				ConsumoEnergeticoRede = model.ConsumoEnergeticoRede,
				RetornoEconomia = model.RetornoEconomia,
				ROI = 0, // Inicialmente 0, pois ainda não foi calculado
				DataCalculado = DateTime.Now
			};

			_context.ROICalculators.Add(roiCalculation);
			await _context.SaveChangesAsync();
		}
		else
		{
			// Atualizar os valores no banco de dados
			roiCalculation.CustoInstalacao = model.CustoInstalacao;
			roiCalculation.CustoManutencaoAnual = model.CustoManutencaoAnual;
			roiCalculation.ConsumoEnergeticoMedio = model.ConsumoEnergeticoMedio;
			roiCalculation.ConsumoEnergeticoRede = model.ConsumoEnergeticoRede;
			roiCalculation.RetornoEconomia = model.RetornoEconomia;
			roiCalculation.DataCalculado = DateTime.Now;

			_context.ROICalculators.Update(roiCalculation);
			await _context.SaveChangesAsync();
		}

		// Validação: Garantir que a Economia Total seja maior que zero
		if (roiCalculation.RetornoEconomia <= 0)
		{
			ViewBag.Resultado = "Erro: A economia total deve ser maior que zero!";
			return View("Index", model); // Retorna à página com o erro
		}

		// Agora, após a atualização dos valores, o ROI será calculado
		decimal resultadoROI = 0;
		try
		{
			// Calcular o ROI
			resultadoROI = roiCalculation.CalcularROI();
			roiCalculation.ROI = resultadoROI;

			// Atualiza o ROI calculado no banco de dados
			_context.ROICalculators.Update(roiCalculation);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			ViewBag.Resultado = "Erro ao calcular o ROI: " + ex.Message;
			return View("Index", model);
		}

		// Exibir o resultado do cálculo do ROI na view
		// Exibindo o resultado como anos e não como porcentagem
		ViewBag.Resultado = $"ROI Calculado: {resultadoROI:F2} anos"; // Limitando para 2 casas decimais
		return View("Index", model);
	}
}
