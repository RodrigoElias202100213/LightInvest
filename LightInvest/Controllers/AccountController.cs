using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
{
	private readonly ApplicationDbContext _context;
	private readonly EmailService _emailService;

	public AccountController(ApplicationDbContext context, EmailService emailService)
	{
		_context = context;
		_emailService = emailService;
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
				.FirstOrDefaultAsync(u => u.Email == model.Email || u.Name == model.Name);

			if (existingUser != null)
			{
				if (existingUser.Email == model.Email)
				{
					ModelState.AddModelError("Email", "Este email já está registado.");
				}

				if (existingUser.Name == model.Name)
				{
					ModelState.AddModelError("Name", "Este nome de utilizador já existe, por favor escolha outro.");
				}

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
		bool emailSent = await _emailService.SendEmailAsync(toAddress, subject, body);

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

	
	[HttpPost]
	public async Task<IActionResult> GeneratePasswordResetTokenAndSendEmail(string email)
	{
		if (string.IsNullOrEmpty(email))
		{
			return BadRequest("O e-mail é obrigatório.");
		}

		var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
		if (user == null)
		{
			return NotFound("Se este e-mail estiver cadastrado, um link de recuperação será enviado.");
		}

		var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

		var resetToken = new PasswordResetToken
		{
			Email = email,
			Token = token,
			Expiration = DateTime.UtcNow.AddHours(1)
		};

		_context.PasswordResetTokens.Add(resetToken);
		await _context.SaveChangesAsync();

		string subject = "Recuperação de password - LightInvest";
		string body = $"Olá,\n\nO seu token de recuperação de password é: {token}\n\nEste token é válido por 1 hora. Utilize-o para redefinir sua password.";

		bool emailSent = await _emailService.SendEmailAsync(email, subject, body);

		if (emailSent)
		{
			TempData["Message"] = "O token de recuperação foi enviado para o seu e-mail.";
		}
		else
		{
			TempData["Message"] = "Erro ao enviar o e-mail. Tente novamente mais tarde.";
		}

		return RedirectToAction("GeneratePasswordResetTokenAndSendEmail");
	}



	[HttpGet]
	public IActionResult ForgotPassword()
	{
		return View();
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ForgotPassword(string email)
	{
		if (string.IsNullOrEmpty(email))
		{
			ModelState.AddModelError("", "O email é obrigatório.");
			return View();
		}

		var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
		if (user == null)
		{
			ModelState.AddModelError("", "Email não encontrado.");
			return View();
		}

		var token = Guid.NewGuid().ToString();

		var tokenEntry = new PasswordResetToken
		{
			Email = email,
			Token = token,
			Expiration = DateTime.UtcNow.AddHours(1)
		};

		_context.PasswordResetTokens.Add(tokenEntry);
		await _context.SaveChangesAsync();

		var body = $"Seu token de recuperação de password é: {token}";
		await Enviaremail(email, "Recuperação de password", body);

		return RedirectToAction("ValidateToken", new { email = email });
	}

	[HttpGet]
	public IActionResult ValidateToken(string email)
	{
		if (string.IsNullOrEmpty(email))
		{
			return RedirectToAction("ForgotPassword");
		}

		return View(new ValidateTokenViewModel { Email = email });
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ValidateToken(ValidateTokenViewModel model)
	{
		if (string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.Email))
		{
			ModelState.AddModelError("", "O token e o e-mail são obrigatórios.");
			return View(model);
		}

		var tokenEntry = await _context.PasswordResetTokens
			.FirstOrDefaultAsync(t => t.Token == model.Token && t.Email == model.Email && t.Expiration > DateTime.UtcNow);

		if (tokenEntry == null)
		{
			ModelState.AddModelError("", "Token inválido ou expirado.");
			return View(model);
		}

		return RedirectToAction("ResetPassword", new { email = model.Email, token = model.Token });
	}


	[HttpGet]
	public IActionResult ResetPassword(string email, string token)
	{
		if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
		{
			return NotFound();
		}

		return View(new ResetPasswordViewModel { Email = email, Token = token });
	}
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
	{
		if (ModelState.IsValid)
		{
			var tokenEntry = await _context.PasswordResetTokens
				.FirstOrDefaultAsync(t => t.Email == model.Email && t.Token == model.Token && t.Expiration > DateTime.UtcNow);

			if (tokenEntry == null)
			{
				ModelState.AddModelError("", "Token inválido ou expirado.");
				return View(model);
			}

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "Utilizador não encontrado.");
				return View(model);
			}

			if (model.NewPassword.Length < 8)
			{
				ModelState.AddModelError("", "A password deve ter pelo menos 8 caracteres.");
				return View(model);
			}

			if (!model.NewPassword.Any(char.IsDigit) || !model.NewPassword.Any(char.IsUpper))
			{
				ModelState.AddModelError("", "A password deve conter pelo menos 2 número e 1 letra maiúscula.");
				return View(model);
			}

			user.Password = model.NewPassword;

			_context.Users.Update(user);
			_context.PasswordResetTokens.Remove(tokenEntry);
			await _context.SaveChangesAsync();

			TempData["Message"] = "Passowrd redefinida com sucesso! Faça login com sua nova password.";
			return RedirectToAction("Login", "Account");
		}

		return View(model);
	}

}