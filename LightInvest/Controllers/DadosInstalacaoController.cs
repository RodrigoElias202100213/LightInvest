using LightInvest.Models;
using LightInvest.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LightInvest.Controllers
{
	public class DadosInstalacaoController : Controller
	{
		private readonly ApplicationDbContext _context;

		public DadosInstalacaoController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("dados-instalacao")]
		public async Task<IActionResult> Create()
		{
			var model = await CarregarViewModelAsync();
			return View(model);
		}

		private async Task<DadosInstalacaoViewModel> CarregarViewModelAsync()
		{
			return new DadosInstalacaoViewModel
			{
				Cidades = await _context.Cidades.ToListAsync(),
				ModelosDePaineis = await _context.ModelosDePaineisSolares.ToListAsync(),
				Potencias = await _context.PotenciasDePaineisSolares.ToListAsync()
			};
		}

		[HttpPost("dados-instalacao")]
		public async Task<IActionResult> Create(DadosInstalacaoViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model = await CarregarViewModelAsync();
				return View(model);
			}

			var user = await ObterUsuarioLogadoAsync();
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "Erro: Nenhum utilizador autenticado.");
				model = await CarregarViewModelAsync();
				return View(model);
			}

			var modeloPainel = await _context.ModelosDePaineisSolares
				.FirstOrDefaultAsync(m => m.Id == model.ModeloPainelId);

			if (modeloPainel == null)
			{
				ModelState.AddModelError("", "Erro: Modelo de painel não encontrado.");
				return View(model);
			}

			var potenciaPainel = await _context.PotenciasDePaineisSolares
<<<<<<< HEAD
				.FirstOrDefaultAsync(p => p.Id == model.PotenciaId); 
=======
				.FirstOrDefaultAsync(p => p.Id == model.PotenciaId);
>>>>>>> backup

			if (potenciaPainel == null)
			{
				ModelState.AddModelError("", "Erro: Potência do painel não encontrada.");
				return View(model);
			}

			var dadosInstalacao = new DadosInstalacao
			{
				UserEmail = user.Email,
				CidadeId = model.CidadeId,
				ModeloPainelId = model.ModeloPainelId,
				ModeloPainel = modeloPainel,
				PotenciaId = model.PotenciaId,
				Potencia = potenciaPainel,  
				Potencia = potenciaPainel,
				NumeroPaineis = model.NumeroPaineis,
				Inclinacao = model.Inclinacao,
				Dificuldade = model.Dificuldade,
			};

<<<<<<< HEAD
			
=======

>>>>>>> backup
			dadosInstalacao.AtualizarPrecoInstalacao();

			await SalvarOuAtualizarDadosInstalacao(dadosInstalacao);

			TempData["PrecoFinal"] = dadosInstalacao.PrecoInstalacao.ToString("F2");

<<<<<<< HEAD
			return RedirectToAction("Simular","SimulacaoValores");
=======
			return RedirectToAction("Simular", "SimulacaoValores");
>>>>>>> backup
		}

		public IActionResult Confirmacao()
		{
<<<<<<< HEAD
			var precoFinal = TempData["PrecoFinal"] as string; 
			if (precoFinal != null)
			{
				ViewBag.PrecoFinal = precoFinal; 
			}
			else
			{
				ViewBag.PrecoFinal = "Preço não calculado"; 
=======
			var precoFinal = TempData["PrecoFinal"] as string;
			if (precoFinal != null)
			{
				ViewBag.PrecoFinal = precoFinal;
			}
			else
			{
				ViewBag.PrecoFinal = "Preço não calculado";
>>>>>>> backup
			}

			return View();
		}

		private async Task<User> ObterUsuarioLogadoAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			return string.IsNullOrEmpty(userEmail)
				? null
				: await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

		[HttpPost("dados-instalacao/calcular-preco")]
		public async Task<IActionResult> CalcularPrecoInstalacao(DadosInstalacaoViewModel model)
		{
			var modeloPainel = await _context.ModelosDePaineisSolares
				.FirstOrDefaultAsync(m => m.Id == model.ModeloPainelId);

			if (modeloPainel == null)
			{
				return BadRequest("Modelo de painel não encontrado.");
			}

			var dadosInstalacao = new DadosInstalacao
			{
				ModeloPainel = modeloPainel,
				NumeroPaineis = model.NumeroPaineis,
				Inclinacao = model.Inclinacao,
				Dificuldade = model.Dificuldade
			};

			var precoFinal = dadosInstalacao.CalcularPrecoInstalacao();
			return Json(new { preco = precoFinal.ToString("F2") });
		}


		private async Task SalvarOuAtualizarDadosInstalacao(DadosInstalacao dadosInstalacao)
		{
			var dadosExistente = await _context.DadosInstalacao
				.FirstOrDefaultAsync(d => d.UserEmail == dadosInstalacao.UserEmail);

			if (dadosExistente != null)
			{
				dadosExistente.CidadeId = dadosInstalacao.CidadeId;
				dadosExistente.ModeloPainelId = dadosInstalacao.ModeloPainelId;
				dadosExistente.ModeloPainel = dadosInstalacao.ModeloPainel;
				dadosExistente.PotenciaId = dadosInstalacao.PotenciaId;
				dadosExistente.NumeroPaineis = dadosInstalacao.NumeroPaineis;
				dadosExistente.Inclinacao = dadosInstalacao.Inclinacao;
				dadosExistente.Dificuldade = dadosInstalacao.Dificuldade;
				dadosExistente.PrecoInstalacao = dadosInstalacao.PrecoInstalacao;

				_context.DadosInstalacao.Update(dadosExistente);
			}
			else
			{
				await _context.DadosInstalacao.AddAsync(dadosInstalacao);
			}
			await _context.SaveChangesAsync();
		}

		[HttpGet]
		public async Task<IActionResult> GetConsumosPainel(int modeloPainelId)
		{
			var potencia = await _context.PotenciasDePaineisSolares
				.Where(p => p.ModeloPainelId == modeloPainelId)
				.Select(p => new { p.Id, p.Potencia })
				.ToListAsync();

			if (potencia == null || !potencia.Any())
			{
				return NotFound();
			}

			return Json(potencia);
		}
	}
}
