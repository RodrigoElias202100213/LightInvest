using Microsoft.AspNetCore.Mvc;

namespace LightInvest.Controllers.Educ
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