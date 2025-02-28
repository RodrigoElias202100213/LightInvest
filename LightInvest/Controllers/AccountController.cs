using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
	private readonly ApplicationDbContext _context;
	private readonly EmailService _emailService;

	// Modificando o construtor para injetar o EmailService
	public AccountController(ApplicationDbContext context, EmailService emailService)
	{
		_context = context;
		_emailService = emailService; // Agora o serviço de email é injetado
	}

	[HttpGet]
	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Login(LoginViewModel model)
	{
		if (ModelState.IsValid)
		{
			var user = await _context.Users
				.FirstOrDefaultAsync(u => u.Email == model.Email);

			if (user != null)
			{
				if (user.Password == model.Password)
				{
					HttpContext.Session.SetString("UserEmail", user.Email);
					HttpContext.Session.SetString("UserName", user.Name);

					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", " Email ou palavra-passe incorreta.");
				}
			}
			else
			{
				ModelState.AddModelError("", "Email não encontrado.");
			}
		}
		return View(model);
	}

	public IActionResult Register()
	{
		return View(new RegisterViewModel());
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Register(RegisterViewModel model)
	{
		if (ModelState.IsValid)
		{
			var existingUser = await _context.Users
				.FirstOrDefaultAsync(u => u.Email == model.Email);

			if (existingUser != null)
			{
				ModelState.AddModelError("Email", "Este email já está registado.");
				return View(model);
			}

			var user = new User()
			{
				Name = model.Name,
				Email = model.Email,
				Password = model.Password
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return RedirectToAction("Login", "Account");
		}

		return View(model);
	}

	public IActionResult Logout()
	{
		HttpContext.Session.Clear();
		return RedirectToAction("Login", "Account");
	}

	[HttpGet]
	public IActionResult Enviaremail()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Enviaremail(string toAddress, string subject, string body)
	{
		// Chama o serviço de envio de e-mail
		bool emailSent = await _emailService.SendEmailAsync(toAddress, subject, body);

		// Exibe uma mensagem após o envio do e-mail
		if (emailSent)
		{
			TempData["Message"] = "E-mail enviado com sucesso!";
		}
		else
		{
			TempData["Message"] = "Erro ao enviar o e-mail. Tente novamente!";
		}

		return RedirectToAction("Index", "Home");
	}
	
}