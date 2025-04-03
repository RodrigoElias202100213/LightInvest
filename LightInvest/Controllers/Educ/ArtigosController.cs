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
			var userName = HttpContext.Session.GetString("UserName");
			var isAdmin = HttpContext.Session.GetString("IsAdmin");

			if (string.IsNullOrEmpty(userName))
			{
				return RedirectToAction("Login", "Account");
			}

			ViewBag.UserName = userName;
			ViewBag.IsAdmin = isAdmin == "True";

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
				.Include(a => a.Comentarios)
				.ThenInclude(c => c.Likes) // Inclui os Likes relacionados ao comentário
				.Include(a => a.ArtigosRelacionados)
				.FirstOrDefaultAsync(a => a.ArtigoId == id);

			if (artigo == null)
			{
				return NotFound();
			}

			var utilizadorLogado = await GetLoggedInUserAsync();
			if (utilizadorLogado != null)
			{
				foreach (var comentario in artigo.Comentarios)
				{
					// Verificar se o usuário logado curtiu ou descurtiu algum comentário
					var like = comentario.Likes.FirstOrDefault(l => l.UserId == utilizadorLogado.Id && l.IsLike);
					var dislike = comentario.Likes.FirstOrDefault(l => l.UserId == utilizadorLogado.Id && !l.IsLike);

					comentario.liked = like != null;
					comentario.disliked = dislike != null;
				}
			}

			var htmlConteudo = Markdown.ToHtml(artigo.Conteudo);
			ViewBag.ConteudoHtml = htmlConteudo;

			var artigosRelacionados = _context.Artigos
				.Where(a => a.Categoria == artigo.Categoria && a.ArtigoId != artigo.ArtigoId)
				.Take(3)
				.ToList();
			artigo.ArtigosRelacionados = artigosRelacionados ?? new List<Artigo>();

			var noticiasRelacionadas = await _mediaStackService.GetSolarPanelArticlesAsync();
			ViewBag.NoticiasRelacionadas = noticiasRelacionadas;

			var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";
			ViewBag.IsAdmin = isAdmin;
			ViewBag.UtilizadorLogadoId = utilizadorLogado?.Id;

			return View(artigo);
		}

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
			var utilizadorLogado = await GetLoggedInUserAsync();
			if (utilizadorLogado == null)
			{
				return RedirectToAction("Login", "Account");
			}

			var comentario = new Comentario
			{
				ArtigoId = artigoId,
				Texto = texto,
				Autor = utilizadorLogado.Name,
				DataCriacao = DateTime.UtcNow,
				UserId = utilizadorLogado.Id
			};

			_context.Comentario.Add(comentario);
			await _context.SaveChangesAsync();

			return RedirectToAction("Detalhes", new { id = artigoId });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RemoverComentario(int comentarioId, int artigoId)
		{
			var comentario = await _context.Comentario.FirstOrDefaultAsync(c => c.Id == comentarioId);
			if (comentario == null)
			{
				return NotFound("Comentário não encontrado.");
			}

			var utilizadorLogado = await GetLoggedInUserAsync();
			var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";

			if (!isAdmin && comentario.UserId != utilizadorLogado?.Id)
			{
				return Unauthorized("Só consegue eliminar os seus comentários.");
			}

			_context.Comentario.Remove(comentario);
			await _context.SaveChangesAsync();

			return RedirectToAction("Detalhes", new { id = artigoId });
		}

		public async Task<IActionResult> EditarComentario(int comentarioId, int artigoId)
		{
			var comentario = await _context.Comentario.FirstOrDefaultAsync(c => c.Id == comentarioId);
			if (comentario == null)
			{
				return NotFound("Comentário não encontrado.");
			}

			var utilizadorLogado = await GetLoggedInUserAsync();
			if (comentario.UserId != utilizadorLogado?.Id)
			{
				return Unauthorized("Não pode editar este comentário.");
			}

			return View(comentario);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditarComentario(int comentarioId, int artigoId, string texto)
		{
			var comentario = await _context.Comentario.FirstOrDefaultAsync(c => c.Id == comentarioId);
			if (comentario == null)
			{
				return NotFound("Comentário não encontrado.");
			}

			var utilizadorLogado = await GetLoggedInUserAsync();
			if (comentario.UserId != utilizadorLogado?.Id)
			{
				return Unauthorized("Não pode editar este comentário.");
			}

			comentario.Texto = texto;
			comentario.DataCriacao = DateTime.UtcNow;

			_context.Comentario.Update(comentario);
			await _context.SaveChangesAsync();

			return RedirectToAction("Detalhes", new { id = artigoId });
		}

		[HttpPost]
		public async Task<IActionResult> CurtirComentario(int comentarioId)
		{
			var comentario = await _context.Comentario
				.Include(c => c.Likes)
				.FirstOrDefaultAsync(c => c.Id == comentarioId);

			if (comentario == null)
			{
				return NotFound();
			}

			// Obter o usuário logado
			var utilizadorLogado = await GetLoggedInUserAsync();

			// Verificar se o usuário já tem um "like" ou "dislike" para este comentário
			var existingLike = comentario.Likes.FirstOrDefault(l => l.UserId == utilizadorLogado.Id);

			// Se o usuário já deu "dislike", removemos o "dislike" e damos o "like"
			if (existingLike != null)
			{
				// Se o "like" foi dado previamente, remove ele.
				_context.ComentarioLike.Remove(existingLike);
			}

			// Adicionar o "like" (IsLike = true)
			comentario.Likes.Add(new ComentarioLike
			{
				ComentarioId = comentarioId,
				UserId = utilizadorLogado.Id,
				IsLike = true  // Marcar como "like"
			});

			await _context.SaveChangesAsync();

			return Json(new { success = true });
		}

		[HttpPost]
		public async Task<IActionResult> RemoverCurtirComentario(int comentarioId)
		{
			var comentario = await _context.Comentario
				.Include(c => c.Likes)
				.FirstOrDefaultAsync(c => c.Id == comentarioId);

			if (comentario == null)
			{
				return NotFound();
			}

			// Obter o usuário logado
			var utilizadorLogado = await GetLoggedInUserAsync();

			// Verificar se o usuário já deu "like" ou "dislike" para este comentário
			var existingLike = comentario.Likes.FirstOrDefault(l => l.UserId == utilizadorLogado.Id);

			if (existingLike != null)
			{
				// Se o "like" já foi dado, removemos ele
				_context.ComentarioLike.Remove(existingLike);
			}

			// Adicionar o "dislike" (IsLike = false)
			comentario.Likes.Add(new ComentarioLike
			{
				ComentarioId = comentarioId,
				UserId = utilizadorLogado.Id,
				IsLike = false  // Marcar como "dislike"
			});

			await _context.SaveChangesAsync();

			return Json(new { success = true });
		}
	}
	}