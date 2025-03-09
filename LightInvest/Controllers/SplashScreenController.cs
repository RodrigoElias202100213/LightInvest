using Microsoft.AspNetCore.Mvc;

public class SplashScreenController : Controller
{
    public IActionResult SplashScreen()
    {
        return View();
    }
}