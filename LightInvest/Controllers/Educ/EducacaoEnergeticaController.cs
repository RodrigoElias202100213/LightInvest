using Microsoft.AspNetCore.Mvc;

namespace LightInvest.Controllers.Educ
{
    /// <summary>
    /// Controller responsible for handling Energy Education-related views and actions.
    /// </summary>
    public class EducacaoEnergeticaController : Controller
    {
        /// <summary>
        /// Displays the main page for Energy Education.
        /// </summary>
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = userName;
            ViewBag.IsAdmin = isAdmin == "True";

            return View();
        }
    }
}
