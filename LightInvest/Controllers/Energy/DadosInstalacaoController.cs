/*
 * O DadosInstalacaoController gere os dados de instalação, incluindo a criação, cálculo de preços e visualização de informações relacionadas a painéis solares.
 * Ele permite a criação de dados de instalação, cálculo de preço de instalação, confirmação do preço, e a visualização dos dados da instalação.
 * Também lida com a autenticação do utilizador e com salvar e atualizar os dados na base de dados.
 */
using LightInvest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LightInvest.Models.Ener;
using LightInvest.Models.BD;
using LightInvest.Models.Utilizador.Login;

namespace LightInvest.Controllers.Energy
{
	/// <summary>
	/// Handles the creation and management of installation data, including pricing and panel configurations.
	/// </summary>
	public class DadosInstalacaoController : Controller
	{
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// Initializes the DadosInstalacaoController with the provided database context.
		/// </summary>
		/// <param name="context">The database context for accessing the application data.</param>
		public DadosInstalacaoController(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Displays the page for creating installation data.
		/// </summary>
		/// <returns>The view for creating installation data.</returns>
		[HttpGet("dados-instalacao")]
		public async Task<IActionResult> Create()
		{
			var model = await CarregarViewModelAsync();
			return View(model);
		}

		/// <summary>
		/// Loads the data for the installation creation view model.
		/// </summary>
		/// <returns>The populated view model with available cities, panel models, and panel power ratings.</returns>
		private async Task<DadosInstalacaoViewModel> CarregarViewModelAsync()
		{
			return new DadosInstalacaoViewModel
			{
				Cidades = await _context.Cidades.ToListAsync(),
				ModelosDePaineis = await _context.ModelosDePaineisSolares.ToListAsync(),
				Potencias = await _context.PotenciasDePaineisSolares.ToListAsync()
			};
		}

		/// <summary>
		/// Handles the POST request to create installation data.
		/// </summary>
		/// <param name="model">The installation data model.</param>
		/// <returns>A redirect to the simulation page or the current view if validation fails.</returns>
		[HttpPost("dados-instalacao")]
		public async Task<IActionResult> Create(DadosInstalacaoViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model = await CarregarViewModelAsync();
				return View(model);
			}

			var user = await ObterUtilizadorLogadoAsync();
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "Erro: Utilizador não autenticado.");
				model = await CarregarViewModelAsync();
				return View(model);
			}

			var modeloPainel = await _context.ModelosDePaineisSolares
				.FirstOrDefaultAsync(m => m.Id == model.ModeloPainelId);

			if (modeloPainel == null)
			{
				ModelState.AddModelError("", "Erro: Painel não encontrado.");
				return View(model);
			}

			var potenciaPainel = await _context.PotenciasDePaineisSolares
				.FirstOrDefaultAsync(p => p.Id == model.PotenciaId);

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
				NumeroPaineis = model.NumeroPaineis,
				Inclinacao = model.Inclinacao,
				Dificuldade = model.Dificuldade,
			};

			dadosInstalacao.AtualizarPrecoInstalacao();

			await SalvarOuAtualizarDadosInstalacao(dadosInstalacao);

			TempData["PrecoFinal"] = dadosInstalacao.PrecoInstalacao.ToString("F2");

			return RedirectToAction("Simular", "SimulacaoValores");
		}

		/// <summary>
		/// Displays a confirmation page after calculating the installation price.
		/// </summary>
		/// <returns>The view for the confirmation page with the calculated price.</returns>
		public IActionResult Confirmacao()
		{
			var precoFinal = TempData["PrecoFinal"] as string;
			if (precoFinal != null)
			{
				ViewBag.PrecoFinal = precoFinal;
			}
			else
			{
				ViewBag.PrecoFinal = "Preço não calculado";
			}

			return View();
		}

		/// <summary>
		/// Retrieves the currently authenticated user based on session data.
		/// </summary>
		/// <returns>The user object if found; otherwise, null.</returns>
		private async Task<User> ObterUtilizadorLogadoAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			return string.IsNullOrEmpty(userEmail)
				? null
				: await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

		/// <summary>
		/// Handles the POST request to calculate the installation price based on user inputs.
		/// </summary>
		/// <param name="model">The installation data model containing user inputs.</param>
		/// <returns>A JSON object with the calculated price.</returns>
		[HttpPost("dados-instalacao/calcular-preco")]
		public async Task<IActionResult> CalcularPrecoInstalacao(DadosInstalacaoViewModel model)
		{
			var modeloPainel = await _context.ModelosDePaineisSolares
				.FirstOrDefaultAsync(m => m.Id == model.ModeloPainelId);

			if (modeloPainel == null)
			{
				return BadRequest("Modelo do painel não encontrado.");
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

		/// <summary>
		/// Saves or updates the installation data in the database.
		/// </summary>
		/// <param name="dadosInstalacao">The installation data to save or update.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
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

		/// <summary>
		/// Retrieves the power consumption data for a specific panel model.
		/// </summary>
		/// <param name="modeloPainelId">The ID of the panel model.</param>
		/// <returns>A JSON object containing the power data for the given panel model.</returns>
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
