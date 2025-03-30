using LightInvest.Models;
using LightInvest.Models.BD;
using LightInvest.Models.Educ.Artigos;
using LightInvest.Models.Utilizador.Login;
using LightInvest.Services;
using Markdig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LightInvest.Controllers.Educ
{
	public class ArtigosController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly MediaStackService _mediaStackService;

		public ArtigosController(ApplicationDbContext context, MediaStackService mediaStackService)
		{
			_context = context;
			_mediaStackService = mediaStackService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult ListarPorCategoria(string categoria)
		{
			var artigos = _context.Artigos.Where(a => a.Categoria == categoria).ToList();
			ViewBag.Categoria = categoria;
			return View(artigos);
		}

		public async Task<IActionResult> Detalhes(int id)
		{
			var artigo = await _context.Artigos
				.Include(a => a.Comentarios) // Carrega os comentários relacionados
				.Include(a => a.ArtigosRelacionados)
				.FirstOrDefaultAsync(a => a.ArtigoId == id);
			if (artigo == null)
			{
				return NotFound();
			}

			// Converte o conteúdo de Markdown para HTML
			var htmlConteudo = Markdown.ToHtml(artigo.Conteudo);
			ViewBag.ConteudoHtml = htmlConteudo;

			var artigosRelacionados = _context.Artigos
				.Where(a => a.Categoria == artigo.Categoria && a.ArtigoId != artigo.ArtigoId)
				.Take(3)
				.ToList();
			artigo.ArtigosRelacionados = artigosRelacionados ?? new List<Artigo>();
			var noticiasRelacionadas = await _mediaStackService.GetSolarPanelArticlesAsync();
			ViewBag.NoticiasRelacionadas = noticiasRelacionadas;

			// Verifica se o usuário é admin usando a sessão
			var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";
			ViewBag.IsAdmin = isAdmin;

			// Obtém o ID do usuário logado
			var usuarioLogado = await GetLoggedInUserAsync();
			ViewBag.UsuarioLogadoId = usuarioLogado?.Id; // Passa o ID do usuário logado para a View

			return View(artigo);
		}



		/// <summary>
		/// Retrieves the currently logged-in user based on the session data.
		/// </summary>
		/// <returns>The logged-in user if found; otherwise, null.</returns>
		private async Task<User> GetLoggedInUserAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			return string.IsNullOrEmpty(userEmail)
				? null
				: await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AdicionarComentario(int artigoId, string texto)
		{
			var artigo = await _context.Artigos.FirstOrDefaultAsync(a => a.ArtigoId == artigoId);
			if (artigo == null)
			{
				return NotFound("Artigo não encontrado.");
			}

			// Recuperar o usuário logado
			var usuarioLogado = await GetLoggedInUserAsync();
			if (usuarioLogado == null)
			{
				// Se o usuário não estiver logado, redireciona para a página de login ou exibe uma mensagem de erro
				return RedirectToAction("Login", "Account"); // Ajuste o nome da ação conforme necessário
			}

			// Criar o novo comentário
			var comentario = new Comentario
			{
				ArtigoId = artigoId,
				Texto = texto,
				Autor = usuarioLogado.Name, // Usar o nome do usuário logado
				DataCriacao = DateTime.UtcNow,
				UserId = usuarioLogado.Id // Associar o comentário ao usuário logado
			};

			_context.Comentario.Add(comentario);
			await _context.SaveChangesAsync();

			return RedirectToAction("Detalhes", new { id = artigoId });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoverComentario(int comentarioId, int artigoId)
		{
			// Verifica a role do usuário via sessão (ajuste conforme sua lógica de autenticação)
			var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";
			if (!isAdmin)
			{
				return Unauthorized("Somente administradores podem remover comentários.");
			}

			var comentario = await _context.Comentario.FirstOrDefaultAsync(c => c.Id == comentarioId);
			if (comentario == null)
			{
				return NotFound("Comentário não encontrado.");
			}

			_context.Comentario.Remove(comentario);
			await _context.SaveChangesAsync();

			return RedirectToAction("Detalhes", new { id = artigoId });
		}

		// GET: Artigos/EditarComentario/5
		public async Task<IActionResult> EditarComentario(int comentarioId, int artigoId)
		{
			var comentario = await _context.Comentario
				.FirstOrDefaultAsync(c => c.Id == comentarioId);

			if (comentario == null)
			{
				return NotFound("Comentário não encontrado.");
			}

			// Verifica se o usuário logado é o autor do comentário
			var usuarioLogado = await GetLoggedInUserAsync();
			if (comentario.UserId != usuarioLogado?.Id)
			{
				return Unauthorized("Você não pode editar este comentário.");
			}

			return View(comentario);
		}

		// POST: Artigos/EditarComentario/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditarComentario(int comentarioId, int artigoId, string texto)
		{
			var comentario = await _context.Comentario
				.FirstOrDefaultAsync(c => c.Id == comentarioId);

			if (comentario == null)
			{
				return NotFound("Comentário não encontrado.");
			}

			// Verifica se o usuário logado é o autor do comentário
			var usuarioLogado = await GetLoggedInUserAsync();
			if (comentario.UserId != usuarioLogado?.Id)
			{
				return Unauthorized("Você não pode editar este comentário.");
			}

			// Atualiza o conteúdo do comentário
			comentario.Texto = texto;
			comentario.DataCriacao = DateTime.UtcNow; // Atualiza a data de criação

			_context.Comentario.Update(comentario);
			await _context.SaveChangesAsync();

			return RedirectToAction("Detalhes", new { id = artigoId });
		}

	}
}
