/*
 *  O HomeController gere as ações relacionadas aos utilizadores como login, exibição da página inicial,
 *  gerir administradores, listagem de utilizadores e manipulação de erros.
 * 
 */

using LightInvest.Models;
using LightInvest.Models.BD;
using LightInvest.Models.Error;
using LightInvest.Models.Utilizador.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LightInvest.Controllers.Auth
{
	/// <summary>
	/// The HomeController class manages the user-related actions such as login, privacy, error handling, and admin management.
	/// </summary>
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// Initializes the HomeController with context and logger dependencies.
		/// </summary>
		/// <param name="context">The application database context.</param>
		/// <param name="logger">The logger for logging information.</param>
		public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
		{
			_context = context;
			_logger = logger;
		}

		/// <summary>
		/// Displays the home page, checks if the user is logged in and if they are an admin.
		/// </summary>
		/// <returns>Returns the view for the home page.</returns>
		public IActionResult Index()
		{
			var userName = HttpContext.Session.GetString("UserName");
			var isAdmin = HttpContext.Session.GetString("IsAdmin");

			if (string.IsNullOrEmpty(userName))
			{
				return RedirectToAction("Login", "Account");
			}

			ViewBag.UserName = userName;
			ViewBag.IsAdmin = isAdmin == "True";

			return View();
		}

		/// <summary>
		/// Displays the privacy policy page.
		/// </summary>
		/// <returns>Returns the view for the privacy page.</returns>
		public IActionResult Privacy()
		{
			return View();
		}

		/// <summary>
		/// Handles error pages and provides an error view with the request ID.
		/// </summary>
		/// <returns>Returns the error view with a request ID.</returns>
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		/// <summary>
		/// Displays the list of users, only accessible by an admin user.
		/// </summary>
		/// <returns>Returns the view with the list of users.</returns>
		public async Task<IActionResult> UserList()
		{
			var isAdmin = HttpContext.Session.GetString("IsAdmin");

			if (isAdmin != "True")
			{
				return RedirectToAction("Index");
			}

			var users = await _context.Users.ToListAsync();
			return View(users);
		}

		/// <summary>
		/// Promotes a user to admin status by updating their 'IsAdmin' flag.
		/// </summary>
		/// <param name="id">The ID of the user to promote.</param>
		/// <returns>Redirects to the UserList action after promoting the user.</returns>
		[HttpGet]
		public async Task<IActionResult> MakeAdmin(int id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
			{
				return NotFound("Utilizador não encontrado.");
			}

			user.IsAdmin = true;
			_context.Users.Update(user);
			await _context.SaveChangesAsync();

			return RedirectToAction("UserList");
		}

		/// <summary>
		/// Displays the user edit page with the current user data to modify.
		/// </summary>
		/// <param name="id">The ID of the user to edit.</param>
		/// <returns>Returns the edit user view with the user data.</returns>
		[HttpGet]
		public async Task<IActionResult> EditUser(int id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
			{
				return NotFound("Utilizador não encontrado.");
			}

			var model = new EditUserViewModel
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email
			};

			return View(model);
		}

		/// <summary>
		/// Bans a user by removing them from the database.
		/// </summary>
		/// <param name="id">The ID of the user to ban.</param>
		/// <returns>Redirects to the UserList action after banning the user.</returns>
		[HttpGet]
		public async Task<IActionResult> BanUser(int id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
			{
				return NotFound("User not found.");
			}
				
			var comentarios = await _context.Comentario.Where(c => c.UserId == id).ToListAsync();

			if (comentarios.Any())
			{
				_context.Comentario.RemoveRange(comentarios);
			}
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return RedirectToAction("UserList");
		}

		/// <summary>
		/// Edits a user's data, such as their name.
		/// </summary>
		/// <param name="model">The model containing the new user data.</param>
		/// <returns>Redirects to the UserList action after updating the user data.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditUser(EditUserViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (model.Id == 0)
				{
					return NotFound("Invalid ID.");
				}

				var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
				if (user == null)
				{
					return NotFound("Utilizador não encontrado.");
				}

				user.Name = model.Name;

				_context.Users.Update(user);
				await _context.SaveChangesAsync();

				return RedirectToAction("UserList");
			}
			return View(model);
		}
	}
}
