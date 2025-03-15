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

	// Exibe o formulário para inserir os dados de instalação
	public async Task<IActionResult> Create()
	{
		ViewBag.Cidades = await _context.Cidades.ToListAsync();
		ViewBag.ModelosPaineis = await _context.ModelosDePaineisSolares.ToListAsync();
		return View();
	}

	// Salva ou atualiza os dados preenchidos pelo usuário
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(DadosInstalacao dados)
	{
		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			TempData["ErrorMessage"] = "Usuário não autenticado.";
			return RedirectToAction("Login", "Account"); // Redireciona para login se não autenticado
		}

		if (ModelState.IsValid)
		{
			// Verifica se já existe um dado de instalação para o usuário
			var dadosExistentes = await _context.DadosInstalacao
				.Where(d => d.UserEmail == user.Email)
				.FirstOrDefaultAsync();

			if (dadosExistentes == null)
			{
				// Caso não exista, cria um novo dado de instalação
				dados.UserEmail = user.Email;
				dados.PrecoInstalacao =
					dados.CalcularPrecoInstalacao(); // Certifique-se de que este método calcula corretamente o preço
				_context.DadosInstalacao.Add(dados);
				await _context.SaveChangesAsync();
				TempData["SuccessMessage"] = "Dados de instalação salvos com sucesso!";
			}
			else
			{
				// Caso já exista, atualiza os dados de instalação
				dadosExistentes.CidadeId = dados.CidadeId;
				dadosExistentes.ModeloPainelId = dados.ModeloPainelId;
				dadosExistentes.NumeroPaineis = dados.NumeroPaineis;
				dadosExistentes.ConsumoPainel = dados.ConsumoPainel; // Atualiza o consumo do painel
				dadosExistentes.Inclinacao = dados.Inclinacao;
				dadosExistentes.Dificuldade = dados.Dificuldade;
				dadosExistentes.PrecoInstalacao = dados.CalcularPrecoInstalacao(); // Atualiza o preço de instalação

				_context.DadosInstalacao.Update(dadosExistentes);
				await _context.SaveChangesAsync();
				TempData["SuccessMessage"] = "Dados de instalação atualizados com sucesso!";
			}

			return RedirectToAction("Index", "Home"); // Redireciona após salvar ou atualizar
		}

		// Se o modelo não for válido, recarrega as opções de cidades e modelos de painéis
		ViewBag.Cidades = await _context.Cidades.ToListAsync();
		ViewBag.ModelosPaineis = await _context.ModelosDePaineisSolares.ToListAsync();
		return View(dados); // Retorna à view com os dados preenchidos
	}

	// Método para obter o usuário logado com base no e-mail armazenado na sessão
	private async Task<User> GetLoggedInUserAsync()
	{
		var userEmail = HttpContext.Session.GetString("UserEmail");
		if (string.IsNullOrEmpty(userEmail))
			return null;

		return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
	}

	// Função que retorna os consumos disponíveis para o painel solar selecionado
	[HttpGet]
	public async Task<IActionResult> GetConsumosPainel(int modeloPainelId)
	{
		// Buscando as potências (ou consumos) do painel solar selecionado
		var consumos = await _context.PotenciasDePaineisSolares
			.Where(p => p.PainelSolarId == modeloPainelId)
			.Select(p => new { p.Id, p.Potencia }) // Retorna a ID e a Potência do painel solar
			.ToListAsync();

		if (consumos == null || !consumos.Any())
		{
			return NotFound(); // Retorna erro caso não haja consumos
		}

		return Json(consumos); // Retorna os consumos no formato JSON
	}

}