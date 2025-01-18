using Microsoft.AspNetCore.Mvc;
using LightInvest.Models;

public class AccountController : Controller
{
	[HttpGet]
	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Login(LoginViewModel model)
	{
		if (ModelState.IsValid)
		{
			// Lógica de autenticação (exemplo simples)
			if (model.Email == "teste@gmail.com" && model.Password == "1234")
			{
				return RedirectToAction("Index", "Home");
			}
			//validar se o login e bem feito depois com a base de dados.
			ModelState.AddModelError("", "Credenciais inválidas.");
		}
		return View(model);
	}

	public IActionResult Register()
	{
		return View(new RegisterViewModel());
	}

	[HttpPost]
	public IActionResult Register(RegisterViewModel model)
	{
		if (ModelState.IsValid)
		{
			// Lógica fictícia de registro (substitua pela lógica real)
			// Exemplo: Salvar usuário no banco de dados
			TempData["SuccessMessage"] = "Conta criada com sucesso!";
			return RedirectToAction("Login");
		}

		// Se falhar a validação, retorne à view com as mensagens de erro
		return View(model);
	}

	// Exibe o formulário de recuperação de senha no PasswordRecoveryController
	public IActionResult ForgotPassword()
	{
		return RedirectToAction("Recover", "PasswordRecovery");
	}
}