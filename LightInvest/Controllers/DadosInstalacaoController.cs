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

		// Exibe o formulário de dados de instalação
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
				ModelState.AddModelError(string.Empty, "Erro: Nenhum usuário autenticado.");
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

			// Criar a instância de DadosInstalacao
			var dadosInstalacao = new DadosInstalacao
			{
				UserEmail = user.Email,
				CidadeId = model.CidadeId,
				ModeloPainelId = model.ModeloPainelId,
				ModeloPainel = modeloPainel,
				PotenciaId = model.PotenciaId,
				NumeroPaineis = model.NumeroPaineis,
				Inclinacao = model.Inclinacao,
				Dificuldade = model.Dificuldade
			};

			// Calcula o preço antes de salvar
			dadosInstalacao.AtualizarPrecoInstalacao();

			// Salvar os dados no banco com o preço calculado
			await SalvarOuAtualizarDadosInstalacao(dadosInstalacao);

			// Passa o preço calculado para a página de confirmação
			TempData["PrecoFinal"] = dadosInstalacao.PrecoInstalacao.ToString("F2");
			return RedirectToAction("Confirmacao");
		}

		public IActionResult Confirmacao()
		{
			var precoFinal = TempData["PrecoFinal"] as string; // Recuperando o preço final armazenado no TempData
			if (precoFinal != null)
			{
				ViewBag.PrecoFinal = precoFinal;  // Atribuindo o preço final ao ViewBag
			}
			else
			{
				ViewBag.PrecoFinal = "Preço não calculado";  // Caso o preço não tenha sido calculado ainda
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
		public IActionResult CalcularPrecoInstalacao(DadosInstalacao dadosInstalacao)
		{
			var precoFinal = dadosInstalacao.CalcularPrecoInstalacao();  // Calculando o preço de instalação
			TempData["PrecoFinal"] = precoFinal.ToString("F2");  // Armazenando o preço no TempData
			return RedirectToAction("Confirmacao");  // Redirecionando para a página de confirmação
		}



		private async Task SalvarOuAtualizarDadosInstalacao(DadosInstalacao dadosInstalacao)
		{
			var dadosExistente = await _context.DadosInstalacao
				.FirstOrDefaultAsync(d => d.UserEmail == dadosInstalacao.UserEmail);

			if (dadosExistente != null)
			{
				// Atualiza os dados existentes
				dadosExistente.CidadeId = dadosInstalacao.CidadeId;
				dadosExistente.ModeloPainelId = dadosInstalacao.ModeloPainelId;
				dadosExistente.ModeloPainel = dadosInstalacao.ModeloPainel;
				dadosExistente.PotenciaId = dadosInstalacao.PotenciaId;
				dadosExistente.NumeroPaineis = dadosInstalacao.NumeroPaineis;
				dadosExistente.Inclinacao = dadosInstalacao.Inclinacao;
				dadosExistente.Dificuldade = dadosInstalacao.Dificuldade;

				_context.DadosInstalacao.Update(dadosExistente);
			}
			else
			{
				_context.DadosInstalacao.Add(dadosInstalacao);
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
