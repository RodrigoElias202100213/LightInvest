using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LightInvest.Models;

[Route("PasswordRecovery")]
public class PasswordRecoveryController : Controller
{
    private readonly UserManager<User> _userManager;

    public PasswordRecoveryController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("Recover")]
    public IActionResult Recover()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Recover(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            ModelState.AddModelError("", "Por favor, insira um e-mail válido.");
            return View();
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            TempData["SuccessMessage"] = "Se este e-mail estiver registrado, um código de redefinição foi enviado.";
            return RedirectToAction("Recover");
        }

        // Gerar o token de redefinição de senha
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // Para testes: exibe o token na view (remova em produção)
        TempData["TokenForTesting"] = token;
        TempData["SuccessMessage"] = $"Token gerado (para teste): {token}";

        // Em produção, você enviaria o token por e-mail aqui
        // bool emailEnviado = await EnviarEmailComToken(user.Email, token);
        // TempData["SuccessMessage"] = emailEnviado ? "Código enviado para o e-mail." : "Erro ao enviar o e-mail.";

        return RedirectToAction("ValidateCode");

    }

    [HttpGet("ValidateCode")]
    public IActionResult ValidateCode()
    {
        return View(new ValidateCodeViewModel());
    }

    [HttpPost("ValidateCode")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ValidateCode(ValidateCodeViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "E-mail inválido.");
            return View(model);
        }

        var isValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", model.Code);
        if (!isValid)
        {
            ModelState.AddModelError("", "Código inválido.");
            return View(model);
        }

        return RedirectToAction("ResetPassword", new { email = model.Email, token = model.Code });
    }

    [HttpGet("ResetPassword")]
    public IActionResult ResetPassword(string email, string token)
    {
        return View(new ResetPasswordViewModel { Email = email, Token = token });
    }

    [HttpPost("ResetPassword")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "E-mail inválido.");
            return View(model);
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Erro ao redefinir a senha.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Senha redefinida com sucesso! Você pode fazer login agora.";
        return RedirectToAction("Login", "Account");
    }
}



