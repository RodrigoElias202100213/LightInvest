using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Utilizador.Pass
{
	/// <summary>
	/// Represents a token for resetting a user's password.
	/// </summary>
	public class PasswordResetToken
	{
		/// <summary>
		/// Gets or sets the unique identifier for the password reset token.
		/// </summary>
		/// <value>
		/// The identifier of the password reset token.
		/// </value>
		/// <example>1</example>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the email address associated with the user requesting the password reset.
		/// </summary>
		/// <value>
		/// The email address of the user.
		/// </value>
		/// <example>user@example.com</example>
		[Required]
		[EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the token used to verify the password reset request.
		/// </summary>
		/// <value>
		/// The unique token string.
		/// </value>
		/// <example>abcdef123456</example>
		[Required]
		public string Token { get; set; }

		/// <summary>
		/// Gets or sets the expiration date and time of the reset token.
		/// </summary>
		/// <value>
		/// The date and time when the token expires.
		/// </value>
		/// <example>2025-03-29T12:00:00</example>
		public DateTime Expiration { get; set; }
	}
}
