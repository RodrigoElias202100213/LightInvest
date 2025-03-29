using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Utilizador.Login
{
	/// <summary>
	/// Represents the view model for user login, containing the user's email and password.
	/// </summary>
	public class LoginViewModel
	{
		/// <summary>
		/// Gets or sets the email address of the user.
		/// </summary>
		/// <value>
		/// The email address of the user.
		/// </value>
		/// <example>user@example.com</example>
		[Required(ErrorMessage = "O e-mail é obrigatório.")]
		[EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password of the user.
		/// </summary>
		/// <value>
		/// The password for the user account.
		/// </value>
		[Required(ErrorMessage = "A palavra-passe é obrigatória.")]
		public string Password { get; set; }
	}
}
