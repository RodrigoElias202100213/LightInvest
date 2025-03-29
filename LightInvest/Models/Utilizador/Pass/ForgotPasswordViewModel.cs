using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Utilizador.Pass
{
	/// <summary>
	/// View model for handling the forgot password request.
	/// </summary>
	public class ForgotPasswordViewModel
	{
		/// <summary>
		/// Gets or sets the email address of the user requesting a password reset.
		/// </summary>
		/// <value>
		/// The email address associated with the user's account.
		/// </value>
		/// <example>user@example.com</example>
		[Required(ErrorMessage = "O e-mail é obrigatório.")]
		[EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
		public string Email { get; set; }
	}
}
