using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class DadosInstalacaoController : Controller
{
	private readonly ApplicationDbContext _context;

	public DadosInstalacaoController(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IActionResult> Create()
	{
		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
			return RedirectToAction("Index", "Home");

		}

		ViewBag.Cidades = await _context.Cidades.ToListAsync();
		ViewBag.ModelosPaineis = await _context.ModelosDePaineisSolares.ToListAsync();
		return View();
	}
	
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(DadosInstalacao dados)
	{
		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
			return RedirectToAction("Index", "Home");
		}

		var dadosExistentes = await _context.DadosInstalacao
			.Where(d => d.UserEmail == user.Email)
			.FirstOrDefaultAsync();

		if (dadosExistentes == null)
		{
			dados.UserEmail = user.Email;
			
			_context.DadosInstalacao.Add(dados);
			await _context.SaveChangesAsync();
			TempData["SuccessMessage"] = "Dados de instalação salvos com sucesso!";
		}
		else
		{
			dadosExistentes.CidadeId = dados.CidadeId;
			dadosExistentes.ModeloPainelId = dados.ModeloPainelId;
			dadosExistentes.NumeroPaineis = dados.NumeroPaineis;
			dadosExistentes.ConsumoPainel = dados.ConsumoPainel;
			dadosExistentes.Inclinacao = dados.Inclinacao;
			dadosExistentes.Dificuldade = dados.Dificuldade;
			dadosExistentes.PrecoInstalacao = dados.CalcularPrecoInstalacao();
			_context.DadosInstalacao.Update(dadosExistentes);
			await _context.SaveChangesAsync();
			TempData["SuccessMessage"] = "Dados de instalação atualizados com sucesso!";
		}

		return RedirectToAction("Index", "Home");
	}

	private async Task<User> GetLoggedInUserAsync()
	{
		var userEmail = HttpContext.Session.GetString("UserEmail");
		if (string.IsNullOrEmpty(userEmail))
			return null;

		return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
	}

	[HttpGet]
	public async Task<IActionResult> GetConsumosPainel(int modeloPainelId)
	{
		var consumos = await _context.PotenciasDePaineisSolares
			.Where(p => p.PainelSolarId == modeloPainelId)
			.Select(p => new { p.Id, p.Potencia })
			.ToListAsync();

		if (consumos == null || !consumos.Any())
		{
			return NotFound();
		}

		return Json(consumos);
	}
}
