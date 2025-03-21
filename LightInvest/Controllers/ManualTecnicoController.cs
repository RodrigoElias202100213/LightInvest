using Microsoft.AspNetCore.Mvc;

namespace LightInvest.Controllers
{

	public class ManualTecnicoController : Controller
	{
		[HttpGet("manual-tecnico")]
		public IActionResult Index()
		{
			return View();
		}
	}
}