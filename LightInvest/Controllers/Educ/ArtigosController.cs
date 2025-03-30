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

        /// <summary>
        /// Lists articles by category.
        /// </summary>
        public IActionResult ListarPorCategoria(string categoria)
        {
            var artigos = _context.Artigos.Where(a => a.Categoria == categoria).ToList();
            ViewBag.Categoria = categoria;
            return View(artigos);
        }

        /// <summary>
        /// Displays the details of a specific article, including comments and related articles.
        /// </summary>
        public async Task<IActionResult> Detalhes(int id)
        {
            var artigo = await _context.Artigos
                .Include(a => a.Comentarios)
                .Include(a => a.ArtigosRelacionados)
                .FirstOrDefaultAsync(a => a.ArtigoId == id);
            if (artigo == null)
            {
                return NotFound();
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
            var utilizadorLogado = await GetLoggedInUserAsync();
            ViewBag.UtilizadorLogadoId = utilizadorLogado?.Id;

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

        /// <summary>
        /// Adds a comment to an article.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarComentario(int artigoId, string texto)
        {
            var artigo = await _context.Artigos.FirstOrDefaultAsync(a => a.ArtigoId == artigoId);
            if (artigo == null)
            {
                return NotFound("Article not found.");
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
				return NotFound("Comment not found.");
			}

			var utilizadorLogado = await GetLoggedInUserAsync();
			var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";

			// Verifica se é admin OU se o usuário logado é o autor do comentário
			if (!isAdmin && comentario.UserId != utilizadorLogado?.Id)
			{
				return Unauthorized("You can only delete your own comments.");
			}

			_context.Comentario.Remove(comentario);
			await _context.SaveChangesAsync();

			return RedirectToAction("Detalhes", new { id = artigoId });
		}

		/// <summary>
		/// Renders the comment editing page.
		/// </summary>
		public async Task<IActionResult> EditarComentario(int comentarioId, int artigoId)
        {
            var comentario = await _context.Comentario.FirstOrDefaultAsync(c => c.Id == comentarioId);
            if (comentario == null)
            {
                return NotFound("Comment not found.");
            }

            var utilizadorLogado = await GetLoggedInUserAsync();
            if (comentario.UserId != utilizadorLogado?.Id)
            {
                return Unauthorized("You cannot edit this comment.");
            }

            return View(comentario);
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarComentario(int comentarioId, int artigoId, string texto)
        {
            var comentario = await _context.Comentario.FirstOrDefaultAsync(c => c.Id == comentarioId);
            if (comentario == null)
            {
                return NotFound("Comment not found.");
            }

            var utilizadorLogado = await GetLoggedInUserAsync();
            if (comentario.UserId != utilizadorLogado?.Id)
            {
                return Unauthorized("You cannot edit this comment.");
            }

            comentario.Texto = texto;
            comentario.DataCriacao = DateTime.UtcNow;

            _context.Comentario.Update(comentario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Detalhes", new { id = artigoId });
        }
    }
}