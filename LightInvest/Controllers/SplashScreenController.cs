using Microsoft.AspNetCore.Mvc;

namespace LightInvest.Controllers
{
    public class SplashScreenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
