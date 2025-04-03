/*
 * 
 * Controlador responsável por gerir as ações relacionadas ao login, registo, recuperação da password.
 * Este controlador uso o serviço de email para envio de notificações de recuperação da password e de outros eventos.
 * 
 */
using LightInvest.Models.BD;
using LightInvest.Models.Email;
using LightInvest.Models.Utilizador.Login;
using LightInvest.Models.Utilizador.Pass;
using LightInvest.Models.Utilizador.Register;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using BCrypt.Net;


namespace LightInvest.Controllers.Auth
{

	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	public class AccountController : Controller
	{
		/// <summary>
		/// The context
		/// </summary>
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// The email service
		/// </summary>
		private readonly EmailService _emailService;

		/// <summary>
		/// Constructor of the controller, responsible for injecting necessary dependencies.
		/// </summary>
		/// <param name="context">Application context used for interacting with the database.</param>
		/// <param name="emailService">Service used for sending emails.</param>
		public AccountController(ApplicationDbContext context, EmailService emailService)
		{
			_context = context;
			_emailService = emailService;
		}

		/// <summary>
		/// Action responsible for displaying the login page (GET).
		/// </summary>
		/// <returns>
		/// Login view.
		/// </returns>
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		/// <summary>
		/// Action responsible for processing user login (POST).
		/// </summary>
		/// <param name="model">The login model containing the user's email and password.</param>
		/// <returns>
		/// Redirects to the home page on success, or shows error messages on failure.
		/// </returns>
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _context.Users
					.FirstOrDefaultAsync(u => u.Email == model.Email);

				if (user != null)
				{
					if (!user.Password.StartsWith("$2a$") &&
						!user.Password.StartsWith("$2b$") &&
						!user.Password.StartsWith("$2y$"))
					{
						user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
						_context.Users.Update(user);
						await _context.SaveChangesAsync();
					}

					if (BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
					{
						HttpContext.Session.SetString("UserEmail", user.Email);
						HttpContext.Session.SetString("UserName", user.Name);
						HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

						return RedirectToAction("Index", "Home");
					}
					else
					{
						ModelState.AddModelError("", "Email ou palavra-passe incorreta.");
					}
				}
				else
				{
					ModelState.AddModelError("", "Utilizador não encontrado.");
				}
			}
			return View(model);
		}


		/// <summary>
		/// Action responsible for displaying the user registration page (GET).
		/// </summary>
		/// <returns>
		/// Registration view.
		/// </returns>
		public IActionResult Register()
		{
			return View(new RegisterViewModel());
		}


		/// <summary>
		/// Action responsible for processing user registration (POST).
		/// </summary>
		/// <param name="model">The registration model containing the user's name, email, and password.</param>
		/// <returns>
		/// Redirects to the login page on successful registration, or shows error messages on failure.
		/// </returns>
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

				string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

				var user = new User()
				{
					Name = model.Name,
					Email = model.Email,
					Password = hashedPassword,
					IsAdmin = false
				};

				_context.Users.Add(user);
				await _context.SaveChangesAsync();

				return RedirectToAction("Login", "Account");
			}

			return View(model);
		}
		/// <summary>
		/// Action responsible for logging out the user and clearing the session.
		/// </summary>
		/// <returns>
		/// Redirects to the login page.
		/// </returns>
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login", "Account");
		}

		/// <summary>
		/// Action responsible for displaying the email sending page (GET).
		/// </summary>
		/// <returns>
		/// Email sending view.
		/// </returns>
		[HttpGet]
		public IActionResult Enviaremail()
		{
			return View();
		}

		/// <summary>
		/// Action responsible for sending an email with specified recipient, subject, and body.
		/// </summary>
		/// <param name="toAddress">The recipient's email address.</param>
		/// <param name="subject">The subject of the email.</param>
		/// <param name="body">The body content of the email.</param>
		/// <returns>
		/// Redirects to the home page with a success or error message.
		/// </returns>
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


		/// <summary>
		/// Action that generates a password reset token and sends it by email (POST).
		/// </summary>
		/// <param name="email">The user's email address for password recovery.</param>
		/// <returns>
		/// Redirects to the "GeneratePasswordResetTokenAndSendEmail" view with a success or failure message.
		/// </returns>
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

			string subject = "Recuperação da palavra-passe LightInvest";
			string body = $"Olá,\n\nO seu token para conseguir proceder à redefinição da palavra-passe é: \n\n {token} \n\n Este token é válido por 1 hora.";

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


		/// <summary>
		/// Action responsible for displaying the password recovery page (GET).
		/// </summary>
		/// <returns>
		/// Password recovery view.
		/// </returns>
		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		/// <summary>
		/// Action that sends a password reset token email after a recovery request (POST).
		/// </summary>
		/// <param name="email">The user's email address for password recovery.</param>
		/// <returns>
		/// Redirects to the "ValidateToken" view.
		/// </returns>
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

		/// <summary>
		/// Action responsible for validating the reset token (GET).
		/// </summary>
		/// <param name="email">The email associated with the reset token.</param>
		/// <returns>
		/// Validation view to input the token.
		/// </returns>
		[HttpGet]
		public IActionResult ValidateToken(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return RedirectToAction("ForgotPassword");
			}

			return View(new ValidateTokenViewModel { Email = email });
		}

		/// <summary>
		/// Action that validates the password reset token (POST).
		/// </summary>
		/// <param name="model">The model containing the token and email for validation.</param>
		/// <returns>
		/// Redirects to the reset password page on success, or shows error messages on failure.
		/// </returns>
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

		/// <summary>
		/// Action responsible for displaying the password reset form (GET).
		/// </summary>
		/// <param name="email">The user's email address for password reset.</param>
		/// <param name="token">The reset token for validating the request.</param>
		/// <returns>
		/// Password reset form view.
		/// </returns>
		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
			{
				return NotFound();
			}

			return View(new ResetPasswordViewModel { Email = email, Token = token });
		}

		/// <summary>
		/// Action responsible for resetting the password (POST).
		/// </summary>
		/// <param name="model">The model containing the new password details.</param>
		/// <returns>
		/// Redirects to the login page on successful password reset, or shows error messages on failure.
		/// </returns>
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
					ModelState.AddModelError("", "A palavra-passe deve conter pelo menos 2 números e 1 letra maiúscula.");
					return View(model);
				}

				user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

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
