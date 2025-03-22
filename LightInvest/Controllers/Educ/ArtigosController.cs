using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LightInvest.Models;
using LightInvest.Models.BD;

namespace LightInvest.Controllers.Educ
{
	public class ArtigosController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ArtigosController(ApplicationDbContext context)
		{
			_context = context;
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


		public IActionResult Detalhes(int id)
		{
			var artigo = _context.Artigos.FirstOrDefault(a => a.Id == id);
			if (artigo == null)
			{
				return NotFound();
			}
			return View(artigo);
		}

	}
}