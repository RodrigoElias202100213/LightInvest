using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class HomeController : Controller
{
    public async Task<IActionResult> Splash()
    {
        await Task.Delay(3000); 
        return RedirectToAction("Login", "Account");
    }

    public IActionResult Index()
    {
        return View();
    }
}
