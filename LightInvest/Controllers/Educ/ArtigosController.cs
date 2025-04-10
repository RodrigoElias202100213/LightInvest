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

		/// <summary>
		/// Displays the default articles page with session information for logged-in users.
		/// </summary>
		/// <returns>
		/// Redirects to the login page if the user is not logged in; otherwise, returns the view displaying articles.
		/// </returns>
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

		/// <summary>
		/// Lists articles filtered by a specified category.
		/// </summary>
		/// <param name="categoria">The category name used to filter articles.</param>
		/// <returns>
		/// Returns a view displaying the list of articles in the specified category.
		/// </returns>
		public IActionResult ListarPorCategoria(string categoria)
		{
			var artigos = _context.Artigos.Where(a => a.Categoria == categoria).ToList();
			ViewBag.Categoria = categoria;
			return View(artigos);
		}

        /// <summary>
        /// Displays the details of a specific article, including its comments, likes/dislikes, and related articles.
        /// Also loads external content related to the article's topic through the MediaStack service.
        /// </summary>
        /// <param name="id">The ID of the article to be displayed.</param>
        /// <returns>
        /// Returns the view with the article details, including formatted content, comments with like/dislike states,
        /// and both internal and external related articles.
        /// If the article is not found, returns a 404 (Not Found) response.
        /// </returns>
        public async Task<IActionResult> Detalhes(int id)
        {
            var artigo = await _context.Artigos
              .Include(a => a.Comentarios)
              .ThenInclude(c => c.Likes)
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

            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";
            ViewBag.IsAdmin = isAdmin;
            ViewBag.UtilizadorLogadoId = utilizadorLogado?.Id;

            if (id == 1)
            {
                var noticiasRelacionadas = await _mediaStackService.GetRenewableEnergyArticlesAsync();
                ViewBag.NoticiasRelacionadas = noticiasRelacionadas;
            }
            else if (id == 2)
            {
                var noticiasRelacionadas = await _mediaStackService.GetROIArticlesAsync();
                ViewBag.NoticiasRelacionadas = noticiasRelacionadas;
            }

            else if (id == 3)
            {
                var noticiasRelacionadas = await _mediaStackService.GetSolarPanelArticlesAsync();
                ViewBag.NoticiasRelacionadas = noticiasRelacionadas;
            }

            else if (id == 4)
            {
                var noticiasRelacionadas = await _mediaStackService.GetEnergyEfficiencyArticleAsync();
                ViewBag.NoticiasRelacionadas = noticiasRelacionadas;
            }

            return View(artigo);
        }

        /// <summary>
        /// Retrieves the logged-in user based on the session's email.
        /// </summary>
        /// <returns>
        /// Returns the user object if logged in, otherwise returns null.
        /// </returns>
        private async Task<User> GetLoggedInUserAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			return string.IsNullOrEmpty(userEmail)
				? null
				: await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

		/// <summary>
		/// Adds a new comment to an article.
		/// </summary>
		/// <param name="artigoId">The ID of the article to which the comment is being added.</param>
		/// <param name="texto">The text content of the comment.</param>
		/// <returns>
		/// Redirects to the article details page after adding the comment. 
		/// If the user is not logged in, it redirects them to the login page.
		/// </returns>
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

		/// <summary>
		/// Removes a comment from an article.
		/// </summary>
		/// <param name="comentarioId">The ID of the comment to be removed.</param>
		/// <param name="artigoId">The ID of the article from which the comment is being removed.</param>
		/// <returns>
		/// Redirects to the article details page after removing the comment. 
		/// If the user does not have permission, it returns an unauthorized response.
		/// </returns>
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

		/// <summary>
		/// Displays the view for editing a comment.
		/// </summary>
		/// <param name="comentarioId">The ID of the comment to be edited.</param>
		/// <param name="artigoId">The ID of the article to which the comment belongs.</param>
		/// <returns>
		/// Returns the comment edit view if the user has permission to edit the comment.
		/// If the comment is not found, it returns a not found response.
		/// </returns>
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

		/// <summary>
		/// Edits an existing comment.
		/// </summary>
		/// <param name="comentarioId">The ID of the comment to be edited.</param>
		/// <param name="artigoId">The ID of the article to which the comment belongs.</param>
		/// <param name="texto">The updated text of the comment.</param>
		/// <returns>
		/// Redirects to the article details page after successfully updating the comment.
		/// If the comment is not found, it returns a not found response.
		/// </returns>
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

		/// <summary>
		/// Likes a specific comment.
		/// </summary>
		/// <param name="comentarioId">The ID of the comment to be liked.</param>
		/// <returns>
		/// Returns a JSON object with success status. 
		/// If the comment is not found, it returns a not found response.
		/// </returns>
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

			var utilizadorLogado = await GetLoggedInUserAsync();

			var existingLike = comentario.Likes.FirstOrDefault(l => l.UserId == utilizadorLogado.Id);

			if (existingLike != null)
			{
				_context.ComentarioLike.Remove(existingLike);
			}

			comentario.Likes.Add(new ComentarioLike
			{
				ComentarioId = comentarioId,
				UserId = utilizadorLogado.Id,
				IsLike = true
			});

			await _context.SaveChangesAsync();

			return Json(new { success = true });
		}

		/// <summary>
		/// Removes the like or dislike from a comment.
		/// </summary>
		/// <param name="comentarioId">The ID of the comment from which the like/dislike is being removed.</param>
		/// <returns>
		/// Returns a JSON object with success status. 
		/// If the comment is not found, it returns a not found response.
		/// </returns>
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

			var utilizadorLogado = await GetLoggedInUserAsync();

			var existingLike = comentario.Likes.FirstOrDefault(l => l.UserId == utilizadorLogado.Id);

			if (existingLike != null)
			{
				_context.ComentarioLike.Remove(existingLike);
			}

			comentario.Likes.Add(new ComentarioLike
			{
				ComentarioId = comentarioId,
				UserId = utilizadorLogado.Id,
				IsLike = false
			});

			await _context.SaveChangesAsync();

			return Json(new { success = true });
		}
	}
	}