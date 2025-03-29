/*
 * O ArtigosController é responsável pela gestão das funcionalidades relacionadas aos artigos na aplicação.
 * Ele inclui ações para:
 * 1. Exibir a página principal de artigos.
 * 2. Listar artigos por categoria.
 * 3. Mostrar detalhes de um artigo específico.
 * 4. Recuperar e exibir artigos relacionados com base na categoria do artigo atual.
 * 
 * Além disso, o controlador integra um serviço externo, o MediaStackService, para ir buscar notícias relacionadas. 
 * 
 */


using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightInvest.Models;
using LightInvest.Models.BD;
using Markdig;
using LightInvest.Models.Educ.Artigos;
using LightInvest.Services;

namespace LightInvest.Controllers.Educ
{
    /// <summary>
    /// The ArtigosController handles article-related actions, including listing articles by category, 
    /// displaying article details, and retrieving related articles.
    /// </summary>
    public class ArtigosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly MediaStackService _mediaStackService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtigosController"/> class with dependencies.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="mediaStackService">The service for retrieving external news articles.</param>
        public ArtigosController(ApplicationDbContext context, MediaStackService mediaStackService)
        {
            _context = context;
            _mediaStackService = mediaStackService;
        }

        /// <summary>
        /// Displays the main articles page.
        /// </summary>
        /// <returns>The view for the articles index page.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lists articles filtered by a given category.
        /// </summary>
        /// <param name="categoria">The category of the articles.</param>
        /// <returns>The view displaying articles within the specified category.</returns>
        public IActionResult ListarPorCategoria(string categoria)
        {
            var artigos = _context.Artigos.Where(a => a.Categoria == categoria).ToList();
            ViewBag.Categoria = categoria;
            return View(artigos);
        }

        /// <summary>
        /// Displays the details of a specific article.
        /// </summary>
        /// <param name="id">The ID of the article to display.</param>
        /// <returns>The view showing the article details. If the article is not found, returns a 404 error.</returns>
        public async Task<IActionResult> Detalhes(int id)
        {
            var artigo = _context.Artigos.FirstOrDefault(a => a.ArtigoId == id);
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

            return View(artigo);
        }
    }
}
