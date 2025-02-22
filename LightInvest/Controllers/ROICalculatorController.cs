using Microsoft.AspNetCore.Mvc;

using LightInvest.Models; // Certifique-se de que este namespace está correto

namespace LightInvest.Controllers
{
    public class ROICalculatorController : Controller
    {
        // GET: ROICalculator
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Calcular(ROICalculator model)
        {
            if (ModelState.IsValid)
            {
                decimal resultadoROI = model.CalcularROI();
                ViewBag.Resultado = resultadoROI;
            }
            else
            {
                ViewBag.Resultado = "Valores inválidos!";
            }
            return View("Index", model);
        }
    }
}
