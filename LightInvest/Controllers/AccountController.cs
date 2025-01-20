using Microsoft.AspNetCore.Mvc;
using LightInvest.Models;
using LightInvest.Data; 
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
	private readonly ApplicationDbContext _context;

	public AccountController(ApplicationDbContext context)
	{
		_context = context;
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
                    HttpContext.Session.SetString("UserName", user.Name);
                    return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Palavra-passe incorreta.");
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

			TempData["SuccessMessage"] = "Registo bem-sucedido! Pode agora fazer login.";
			return RedirectToAction("Login", "Account");
		}

		return View(model);
	}

	public IActionResult ForgotPassword()
	{
		return RedirectToAction("Recover", "PasswordRecovery");
	}
}