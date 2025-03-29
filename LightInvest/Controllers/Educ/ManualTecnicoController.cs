using Microsoft.AspNetCore.Mvc;

namespace LightInvest.Controllers.Educ
{
	/// <summary>
	/// The ManualTecnicoController handles the technical manual page.
	/// </summary>
	public class ManualTecnicoController : Controller
	{
		/// <summary>
		/// Displays the technical manual page.
		/// </summary>
		/// <returns>Returns the view for the technical manual page.</returns>
		[HttpGet("manual-tecnico")]
		public IActionResult Index()
		{
			return View();
		}
	}
}
