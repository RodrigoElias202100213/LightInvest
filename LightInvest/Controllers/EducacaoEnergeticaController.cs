using LightInvest.Models;
using LightInvest.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LightInvest.Controllers
{
	public class EducacaoEnergeticaController : Controller
	{
		private readonly ApplicationDbContext _context;

		public EducacaoEnergeticaController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("educacao-energetica")]
		public async Task<IActionResult> Index()
		{
			var model = await CarregarViewModelAsync();
			return View(model);
		}

		private async Task<EducacaoEnergeticaViewModel> CarregarViewModelAsync()
		{
			return new EducacaoEnergeticaViewModel
			{
				Artigos = await _context.Artigos.ToListAsync()
			};
		}

		[HttpGet("educacao-energetica/artigo/{id}")]
		public async Task<IActionResult> VerArtigo(int id)
		{
			var artigo = await _context.Artigos
				.FirstOrDefaultAsync(a => a.Id == id);

			if (artigo == null)
			{
				return NotFound();
			}

			return View(artigo);
		}
	}
}
