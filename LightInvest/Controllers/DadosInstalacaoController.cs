using LightInvest.Models;
using LightInvest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using LightInvest.Models.b;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LightInvest.Controllers
{
	public class DadosInstalacaoController : Controller
	{
		private readonly ApplicationDbContext _context;

		public DadosInstalacaoController(ApplicationDbContext context)
		{
			_context = context;
		}

		private async Task<User> GetLoggedInUserAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return null;

			return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var user = await GetLoggedInUserAsync();
			if (user == null)
			{
				ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
				return View();
			}

			var dadosInstalacao = await _context.DadosInstalacao
				.FirstOrDefaultAsync(d => d.UserEmail == user.Email);

			if (dadosInstalacao == null)
			{
				dadosInstalacao = new DadosInstalacao
				{
					UserEmail = user.Email,
					CidadeId = 0,
					ModeloPainelId = 0,
					NumeroPaineis = 0,
					ConsumoPainel = 0,
					Inclinacao = 0,
					Dificuldade = "fácil"
				};

				_context.DadosInstalacao.Add(dadosInstalacao);
				await _context.SaveChangesAsync();
			}

			ViewBag.Cidades = new SelectList(_context.Cidades, "Id", "Nome");
			ViewBag.Modelos = new SelectList(_context.ModelosDePaineisSolares, "Id", "Nome");

			return View(dadosInstalacao);
		}

		[HttpPost]
		public async Task<IActionResult> Create(DadosInstalacao model)
		{
			var user = await GetLoggedInUserAsync();
			if (user == null)
			{
				ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
				return View(model);
			}

			var dadosInstalacao = await _context.DadosInstalacao
				.FirstOrDefaultAsync(d => d.UserEmail == user.Email);

			if (dadosInstalacao == null)
			{
				dadosInstalacao = new DadosInstalacao
				{
					UserEmail = user.Email,
					CidadeId = model.CidadeId,
					ModeloPainelId = model.ModeloPainelId,
					NumeroPaineis = model.NumeroPaineis,
					ConsumoPainel = model.ConsumoPainel,
					Inclinacao = model.Inclinacao,
					Dificuldade = model.Dificuldade
				};

				_context.DadosInstalacao.Add(dadosInstalacao);
			}
			else
			{
				dadosInstalacao.CidadeId = model.CidadeId;
				dadosInstalacao.ModeloPainelId = model.ModeloPainelId;
				dadosInstalacao.NumeroPaineis = model.NumeroPaineis;
				dadosInstalacao.ConsumoPainel = model.ConsumoPainel;
				dadosInstalacao.Inclinacao = model.Inclinacao;
				dadosInstalacao.Dificuldade = model.Dificuldade;

				_context.DadosInstalacao.Update(dadosInstalacao);
			}

			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> GetPotenciasByModelo(int modeloId)
		{
			var potencias = await _context.PotenciasDePaineisSolares
				.Where(p => p.PainelSolarId == modeloId)
				.ToListAsync();

			return Json(potencias);
		}
	}
}
