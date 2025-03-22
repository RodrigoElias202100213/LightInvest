using LightInvest.Models.BD;
using LightInvest.Models.Email;
using LightInvest.Models.Utilizador.Login;
using LightInvest.Models.Utilizador.Pass;
using LightInvest.Models.Utilizador.Register;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace LightInvest.Controllers.Auth
{

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
					ModelState.AddModelError("", "Email ou palavra-passe não encontrada.");
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
						ModelState.AddModelError("Email", "Já existe uma conta com este email.");
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
				return NotFound("Se este e-mail existir, um token será enviado para o email.");
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

			string subject = "Recuperação da  palavra-passe LightInvest";
			string body = $"Olá,\n\nO seu token para conseguir proceder à redefinição da palavra-passe é: {token}\n\nEste token é válido por 1 hora.";

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

			var body = $"Olá,\nO token para conseguires proceder à redefinição da palavra-passe é: {token}.\n Este Token tem a validade de 1 hora, depois disso deixa de ser válido. \n\n  LigthInvest";
			await Enviaremail(email, "Recuperação da palavra-passe", body);

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
					ModelState.AddModelError("", "A palavra-passe deve ter pelo menos 8 caracteres.");
					return View(model);
				}

				if (!model.NewPassword.Any(char.IsDigit) || !model.NewPassword.Any(char.IsUpper))
				{
					ModelState.AddModelError("", "A palavra-passe deve conter pelo menos 2 número e 1 letra maiúscula.");
					return View(model);
				}

				user.Password = model.NewPassword;

				_context.Users.Update(user);
				_context.PasswordResetTokens.Remove(tokenEntry);
				await _context.SaveChangesAsync();

				TempData["Message"] = "Palavra-passe redefinida com sucesso! Faça login com sua nova palavra-passe.";
				return RedirectToAction("Login", "Account");
			}

			return View(model);
		}

	}
}