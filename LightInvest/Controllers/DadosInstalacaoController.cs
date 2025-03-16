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
		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
			return RedirectToAction("Privacy", "Home"); // Redireciona se não houver usuário logado 111111111111111111111111
		}

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
			ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
			return RedirectToAction("Index", "Home");
		}

		// Verifica se os dados de instalação já existem para o usuário logado
		var dadosExistentes = await _context.DadosInstalacao
			.Where(d => d.UserEmail == user.Email)
			.FirstOrDefaultAsync();

		if (dadosExistentes == null)
		{
			dados.UserEmail = user.Email;
			dados.AtualizarPrecoInstalacao(); // Atualiza o preço antes de salvar
			_context.DadosInstalacao.Add(dados);
			await _context.SaveChangesAsync();
		}
		else
		{
			dadosExistentes.CidadeId = dados.CidadeId;
			dadosExistentes.ModeloPainelId = dados.ModeloPainelId;
			dadosExistentes.NumeroPaineis = dados.NumeroPaineis;
			dadosExistentes.ConsumoPainel = dados.ConsumoPainel;
			dadosExistentes.Inclinacao = dados.Inclinacao;
			dadosExistentes.Dificuldade = dados.Dificuldade;
			dadosExistentes.AtualizarPrecoInstalacao(); // Atualiza o preço antes de salvar
			_context.DadosInstalacao.Update(dadosExistentes);
			await _context.SaveChangesAsync();
		}

		return RedirectToAction("SimulacaoCompleta", "Simulacao"); // Redireciona após salvar ou atualizar
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