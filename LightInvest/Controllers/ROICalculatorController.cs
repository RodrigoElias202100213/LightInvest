using Microsoft.AspNetCore.Mvc;
using LightInvest.Models;
using Newtonsoft.Json;

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
				try
				{
					decimal resultadoROI = model.CalcularROI();
					ViewBag.ROI = resultadoROI;

					// Cálculo do tempo de retorno (em anos) e dos dados para o gráfico
					decimal tempoRetorno = model.CustoInstalacao /
						(model.ConsumoEnergeticoRede - model.ConsumoEnergeticoMedio) * model.RetornoEconomia;
					ViewBag.TempoRetorno = tempoRetorno;

					// Simulando dados para gráfico (Exemplo de valores aleatórios)
					var anos = new List<int> { 1, 2, 3, 4, 5 };
					var retorno = new List<decimal> { 10, 20, 30, 40, 50 };

					ViewBag.AnosJson = JsonConvert.SerializeObject(anos);
					ViewBag.RetornoJson = JsonConvert.SerializeObject(retorno);
				}
				catch (ArgumentException ex)
				{
					ViewBag.ROI = $"Erro: {ex.Message}";
				}
				catch (InvalidOperationException ex)
				{
					ViewBag.ROI = $"Erro: {ex.Message}";
				}
			}
			else
			{
				ViewBag.ROI = "Há erros de validação nos dados inseridos.";
			}

			return View("Index", model);
		}
	}
}