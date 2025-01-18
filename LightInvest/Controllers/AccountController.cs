using Microsoft.AspNetCore.Mvc;
using LightInvest.Models;

public class AccountController : Controller
{
	// Exibir a página de login
	public IActionResult Login()
	{
		return View(new LoginViewModel());
	}
	// Processar o login
	[HttpPost]
	public IActionResult Login(LoginViewModel model)
	{
		if (ModelState.IsValid)
		{
			// Lógica de autenticação (exemplo simples)
			if (model.Email == "teste@exemplo.com" && model.Password == "1234")
			{
				return RedirectToAction("Index", "Home");
			}

			//validar se o login e bem feito depois com a base de dados.

			// Se as credenciais forem inválidas
			ModelState.AddModelError("", "Credenciais inválidas.");
		}

		return View(model);
	}

	public IActionResult Register()
	{
		// Passando uma instância do ViewModel para a View
		return View(new RegisterViewModel());
	}

	// Processar o registro
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