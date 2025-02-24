using Microsoft.AspNetCore.Mvc;
using LightInvest.Models;
using LightInvest.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using LightInvest.Controllers;
using LightInvest.Models;

public class PasswordRecoveryController : Controller
{
	private readonly ApplicationDbContext _context;
	private readonly IEmailService _emailService;

	public PasswordRecoveryController(ApplicationDbContext context, IEmailService emailService)
	{
		_context = context;
		_emailService = emailService;
	}

	// Exibe o formulário de recuperação
	[HttpGet]
	public IActionResult Recover()
	{
		return View(new ForgotPasswordViewModel());
	}

	// Processa a solicitação de recuperação
	[HttpPost]
	public async Task<IActionResult> Recover(ForgotPasswordViewModel model)
	{
		if (ModelState.IsValid)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "Email não encontrado.");
				return View(model);
			}

			// Gerar um token seguro e definir a expiração
			var token = Guid.NewGuid().ToString();
			var expiration = DateTime.UtcNow.AddHours(1);

			var recoveryToken = new PasswordRecoveryToken
			{
				Email = model.Email,
				ResetToken = token,
				TokenExpiration = expiration
			};

			_context.PasswordRecoveryTokens.Add(recoveryToken);
			await _context.SaveChangesAsync();

			// Criar o link de recuperação
			var resetLink = Url.Action("ResetPassword", "PasswordRecovery", new { token }, Request.Scheme);

			// Enviar o e-mail de recuperação
			string emailBody = $"<p>Olá,</p>" +
				"<p>Você solicitou a redefinição de senha. Clique no link abaixo para redefinir:</p>" +
				$"<p><a href='{resetLink}'>Redefinir Senha</a></p>" +
				"<p>Se não foi você, ignore este e-mail.</p>";

			await _emailService.SendEmailAsync(model.Email, "Recuperação de Senha - LightInvest", emailBody);

			TempData["Message"] = "Um link de recuperação foi enviado para o seu e-mail.";
			return RedirectToAction("Login", "Account");
		}

		return View(model);
	}

	// Exibe o formulário de redefinição de senha
	[HttpGet]
	public IActionResult ResetPassword(string token)
	{
		if (string.IsNullOrEmpty(token))
		{
			return NotFound();
		}

		var model = new ResetPasswordViewModel { Token = token };
		Console.WriteLine($"Token sendo passado para a View: {token}");
		return View(model);

	}

	// Processa a redefinição de senha
	[HttpPost]
	public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		var tokenRecord = await _context.PasswordRecoveryTokens
			.FirstOrDefaultAsync(t => t.ResetToken == model.Token && t.TokenExpiration > DateTime.UtcNow);

		if (tokenRecord == null)
		{
			ModelState.AddModelError("", "Token inválido ou expirado.");
			return View(model);
		}

		var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == tokenRecord.Email);
		if (user == null)
		{
			ModelState.AddModelError("", "Usuário não encontrado.");
			return View(model);
		}

		// Atualizar a senha do usuário
		user.Password = model.NewPassword; // 🔴 Use hash em produção!
		_context.Users.Update(user);

		// Remover token usado
		_context.PasswordRecoveryTokens.Remove(tokenRecord);
		await _context.SaveChangesAsync();

		TempData["SuccessMessage"] = "Senha redefinida com sucesso! Faça login com sua nova senha.";
		return RedirectToAction("Login", "Account");
	}
}
