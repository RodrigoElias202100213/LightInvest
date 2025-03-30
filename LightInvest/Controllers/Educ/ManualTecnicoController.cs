/*
 * O ManualTecnicoController é responsável pela gestão da página do manual técnico.
 * Ele contém uma única ação que exibe a página do manual técnico.
 */

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
