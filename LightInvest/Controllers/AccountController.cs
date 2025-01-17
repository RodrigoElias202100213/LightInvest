using Microsoft.AspNetCore.Mvc;
using LightInvest.Models;

namespace LightInvest.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register model)
        {
            // Lógica para registrar o usuário
            return View();
        }

        // Exibir o formulário de login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Processar o login (a lógica será implementada mais tarde)
        [HttpPost]
        public IActionResult Login(Login model)
        {
            // Lógica de login
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPassword model)
        {
            // Lógica para enviar o e-mail de recuperação
            return View();
        }


    }
}
