using LightInvest.Data;
using LightInvest.Models;
using LightInvest.Models.b;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

public class DadosInstalacaoController : Controller
{
	private readonly ApplicationDbContext _context;

	public DadosInstalacaoController(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IActionResult> Instalacao()
	{
		// Obtenha a lista de cidades e modelos de painéis do banco de dados
		var cidades = await _context.Cidades.ToListAsync();
		ViewBag.Cidades = cidades.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
		{
			Value = c.Id.ToString(),  // O valor do item será o Id da cidade
			Text = c.Nome            // O texto exibido será o nome da cidade
		}).ToList();

		var modelosPaineis = await _context.ModelosDePaineisSolares.ToListAsync();
		ViewBag.ModelosPaineis = modelosPaineis.Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
		{
			Value = m.Id.ToString(),
			Text = m.Modelo
		}).ToList();

		var model = InitializeViewModel();
		LoadTempData(model);
		return View(model);
	}


	private DadosInstalacao InitializeViewModel()
	{
		return new DadosInstalacao()
		{
			//CidadeId = 1,
			//ModeloPainelId = 1,
			Inclinacao = 0,
			Dificuldade = "fácil",
			NumeroPaineis = 1,
			ConsumoPainel = 0, // TODO: AQUI

		};
	}

	private void LoadTempData(DadosInstalacao model)
	{
		if (TempData["PrecoInstalacao"] != null && decimal.TryParse(TempData["PrecoInstalacao"].ToString(), out decimal preco))
		{
			model.PrecoInstalacao = preco;
		}
	}


	private async Task<User> GetLoggedInUserAsync()
	{
		var userEmail = HttpContext.Session.GetString("UserEmail");
		return string.IsNullOrEmpty(userEmail) ? null : await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
	}

	[HttpPost]
	public async Task<IActionResult> Instalacao(DadosInstalacao model)
	{
		if (!ModelState.IsValid)
		{
			// Repopula os dropdowns com SelectListItem
			ViewBag.Cidades = (await _context.Cidades.ToListAsync()).Select(c => new SelectListItem
			{
				Value = c.Id.ToString(),
				Text = c.Nome
			}).ToList();

			ViewBag.ModelosPaineis = (await _context.ModelosDePaineisSolares.ToListAsync()).Select(m => new SelectListItem
			{
				Value = m.Id.ToString(),
				Text = m.Modelo
			}).ToList();

			return View(model);
		}

		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
			return View("Index", model);
		}

		decimal precoInstalacao = CalcularPrecoInstalacao(model);
		await SaveInstalacaoToDatabase(user.Email, model, precoInstalacao);
		StoreTempData(model);

		return View(model);
	}

	private decimal CalcularPrecoInstalacao(DadosInstalacao model)
	{
		// Se a lógica de cálculo de preço precisa de alguma alteração, pode ser ajustada aqui
		var dadosInstalacao = new DadosInstalacao
		{
			CidadeId = model.CidadeId,
			ModeloPainelId = model.ModeloPainelId,
			NumeroPaineis = model.NumeroPaineis,
			ConsumoPainel = model.ConsumoPainel,
			Inclinacao = model.Inclinacao,
			Dificuldade = model.Dificuldade
		};

		return dadosInstalacao.CalcularPrecoInstalacao();  // Método que já existe no modelo DadosInstalacao
	}

	private async Task SaveInstalacaoToDatabase(string userEmail, DadosInstalacao model, decimal precoInstalacao)
	{
		Console.WriteLine($"Salvando instalação para {userEmail}: Preço={precoInstalacao}");

		var instalacaoExistente = await _context.DadosInstalacao
			.FirstOrDefaultAsync(i => i.UserEmail == userEmail);

		if (instalacaoExistente != null)
		{
			instalacaoExistente.NumeroPaineis = model.NumeroPaineis;
			instalacaoExistente.ConsumoPainel = model.ConsumoPainel; // TODO: AQUI
			instalacaoExistente.Inclinacao = model.Inclinacao;
			instalacaoExistente.Dificuldade = model.Dificuldade;
			instalacaoExistente.PrecoInstalacao = precoInstalacao;
			instalacaoExistente.CidadeId = model.CidadeId;
			instalacaoExistente.ModeloPainelId = model.ModeloPainelId;

			_context.Entry(instalacaoExistente).State = EntityState.Modified;
		}
		else
		{
			var dadosInstalacao = new DadosInstalacao
			{
				UserEmail = userEmail,
				CidadeId = model.CidadeId,
				ModeloPainelId = model.ModeloPainelId,
				NumeroPaineis = model.NumeroPaineis,
				ConsumoPainel = model.ConsumoPainel,  // TODO: AQUI
				Inclinacao = model.Inclinacao,
				Dificuldade = model.Dificuldade,
				PrecoInstalacao = precoInstalacao
			};

			_context.DadosInstalacao.Add(dadosInstalacao);
		}

		await _context.SaveChangesAsync();
	}

	private void StoreTempData(DadosInstalacao model)
	{
		TempData["PrecoInstalacao"] = model.PrecoInstalacao.ToString("F2");
	}

	[HttpGet]
	public async Task<IActionResult> GetPotencias(int modeloId)
	{
		Console.WriteLine($"Recebido modeloId: {modeloId}");

		if (modeloId <= 0)
		{
			Console.WriteLine("Erro: Modelo inválido.");
			return Json(new { error = "Modelo inválido." });
		}

		var potencias = await _context.PotenciasDePaineisSolares
			.Where(p => p.PainelSolarId == modeloId)
			.Select(p => new { p.Id, p.Potencia })
			.ToListAsync();

		Console.WriteLine($"Potências encontradas: {potencias.Count}");

		if (!potencias.Any())
		{
			Console.WriteLine("Nenhuma potência encontrada.");
			return Json(new { message = "Nenhuma potência encontrada." });
		}

		return Json(potencias);
	}
}
