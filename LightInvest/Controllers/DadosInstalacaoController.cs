using LightInvest.Models;
using LightInvest.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using LightInvest.Models.b;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LightInvest.Controllers
{
	public class DadosInstalacaoController : Controller
	{
		private readonly ApplicationDbContext _context;

		public DadosInstalacaoController(ApplicationDbContext context)
		{
			_context = context;
		}

		private async Task<User> GetLoggedInUserAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			if (string.IsNullOrEmpty(userEmail))
				return null;

			return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var cidades = await _context.Cidades.ToListAsync() ?? new List<Cidade>();
			var modelos = await _context.ModelosDePaineisSolares.ToListAsync() ?? new List<ModeloPainelSolar>();

			ViewBag.Cidades = new SelectList(cidades, "Id", "Nome");
			ViewBag.ModelosPainel = new SelectList(modelos, "Id", "Nome");

			return View(new DadosInstalacao());
		}

		public async Task<IActionResult> GetPotenciasByModelo(int modeloId)
		{
			var potencias = await _context.PotenciasDePaineisSolares
				.Where(p => p.PainelSolarId == modeloId)
				.ToListAsync();

			return Json(potencias);
		}
	}
}
