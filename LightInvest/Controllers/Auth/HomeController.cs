using LightInvest.Models;
using LightInvest.Models.Error;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LightInvest.Controllers.Auth
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			var userName = HttpContext.Session.GetString("UserName");

			if (string.IsNullOrEmpty(userName))
			{
				return RedirectToAction("Login", "Account");
			}

			ViewBag.UserName = userName;
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}