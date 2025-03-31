/*
 * O ManualTecnicoController é responsável por gerir a página do manual técnico.
 * Contém uma única ação que exibe a página correspondente.
 */

using Microsoft.AspNetCore.Mvc;

namespace LightInvest.Controllers.Educ
{
    /// <summary>
    /// Controlador responsável por exibir a página do manual técnico.
    /// </summary>
    public class ManualTecnicoController : Controller
    {
        /// <summary>
        /// Exibe a página do manual técnico.
        /// </summary>
        /// <returns>Retorna a view correspondente à página do manual técnico.</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}