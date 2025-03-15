using LightInvest.Models;
using LightInvest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace LightInvest.Controllers
{
	public class DadosInstalacaoController : Controller
	{
		private readonly ApplicationDbContext _context;

		public DadosInstalacaoController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Método para obter o usuário logado
		private async Task<User> GetLoggedInUserAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return null;

			return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

		public async Task<IActionResult> Create()
		{
			var user = await GetLoggedInUserAsync();
			if (user == null)
			{
				ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
				return View();
			}

			// Buscar ou criar os dados de instalação para o usuário
			var dadosInstalacao = await _context.DadosInstalacao
				.Where(d => d.UserEmail == user.Email)
				.FirstOrDefaultAsync();

			if (dadosInstalacao == null)
			{
				// Caso não exista dados de instalação, cria um novo com valores padrão
				dadosInstalacao = new DadosInstalacao
				{
					UserEmail = user.Email,
					CidadeId = 1, // Defina um valor padrão (id de uma cidade no banco de dados)
					ModeloPainelId = 1, // Defina um modelo de painel solar padrão (id de um modelo no banco)
					NumeroPaineis = 1, // Defina um valor padrão para o número de painéis
					ConsumoPainel = 300, // Defina um valor padrão para o consumo de painel (em kWh)
					Inclinacao = 30, // Defina um valor padrão para a inclinação (em graus)
					Dificuldade = "Média", // Defina um valor padrão para a dificuldade da instalação
				};


				_context.DadosInstalacao.Add(dadosInstalacao);
				await _context.SaveChangesAsync();
			}

			// Repopula as listas de cidades e modelos de painéis solares
			ViewBag.Cidades = await _context.Cidades.ToListAsync();
			ViewBag.ModelosPainel = await _context.ModelosDePaineisSolares.ToListAsync();

			return View(dadosInstalacao);
		}

		// Método para buscar as potências associadas ao modelo de painel solar
		public async Task<IActionResult> GetPotenciasByModelo(int modeloId)
		{
			var potencias = await _context.PotenciasDePaineisSolares
				.Where(p => p.PainelSolarId == modeloId)
				.ToListAsync();

			return Json(potencias);
		}
	}
}
