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

		public async Task<IActionResult> Create()
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

			DadosInstalacao dadosInstalacao = await _context.DadosInstalacao
				.FirstOrDefaultAsync(d => d.UserEmail == user.Email);

			if (dadosInstalacao == null)
			{
				dadosInstalacao = new DadosInstalacao
				{
					UserEmail = user.Email,
					CidadeId = 1,  // A cidade será definida aqui como "1" (padrão)
					ModeloPainelId = 1,
					NumeroPaineis = 0,
					ConsumoPainel = 0,
					Inclinacao = 0,
					Dificuldade = "fácil"
				};

				if (!_context.Cidades.Any(c => c.Id == 1))
				{
					ModelState.AddModelError("CidadeId", "Cidade não encontrada.");
					return View(dadosInstalacao);
				}

				_context.DadosInstalacao.Add(dadosInstalacao);
				await _context.SaveChangesAsync();
			}

			if (_context.Cidades == null || !_context.Cidades.Any())
			{
				ViewBag.Cidades = new SelectList(new List<Cidade>());
			}
			else
			{
				ViewBag.Cidades = new SelectList(_context.Cidades, "Id", "Nome");
			}

			return View(dadosInstalacao);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(DadosInstalacao dadosInstalacao)
		{
			if (ModelState.IsValid)
			{
				if (!_context.Cidades.Any(c => c.Id == dadosInstalacao.CidadeId))
				{
					ModelState.AddModelError("CidadeId", "Cidade não encontrada.");
					return View(dadosInstalacao); // retorna com erro
				}

				if (dadosInstalacao.Id == 0)
				{
					_context.DadosInstalacao.Add(dadosInstalacao);
				}
				else
				{
					_context.DadosInstalacao.Update(dadosInstalacao);
				}

				await _context.SaveChangesAsync();
				return RedirectToAction("Index");

			}

			ViewBag.Cidades = new SelectList(_context.Cidades, "Id", "Nome", dadosInstalacao.CidadeId);

			return View(dadosInstalacao);
		}
	}




}
