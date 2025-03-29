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
