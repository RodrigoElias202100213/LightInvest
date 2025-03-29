/*
 * 
 * Este controller é responsável por mostrar o splash screen no inicio da utilização da plataforma
 * 
 */

using Microsoft.AspNetCore.Mvc;

namespace YourNamespace
{
	/// <summary>
	/// The SplashScreenController handles the splash screen view for the application.
	/// </summary>
	public class SplashScreenController : Controller
	{
		/// <summary>
		/// Displays the splash screen view.
		/// </summary>
		/// <returns>Returns the view for the splash screen.</returns>
		public IActionResult SplashScreen()
		{
			return View();
		}
	}
}
