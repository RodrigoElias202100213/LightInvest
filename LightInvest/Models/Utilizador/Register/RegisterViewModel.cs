using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Utilizador.Register
{
	/// <summary>
	/// Represents the model used for user registration in the system.
	/// </summary>
	public class RegisterViewModel
	{
		/// <summary>
		/// Gets or sets the user's name.
		/// </summary>
		/// <value>
		/// A string representing the name of the user.
		/// </value>
		/// <exception cref="ValidationException">Thrown if the name is empty or exceeds 100 characters.</exception>
		[Required(ErrorMessage = "O nome é obrigatório.")]
		[StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres.")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the user's email address.
		/// </summary>
		/// <value>
		/// A string representing the email of the user.
		/// </value>
		/// <exception cref="ValidationException">Thrown if the email is empty or invalid.</exception>
		[Required(ErrorMessage = "O e-mail é obrigatório.")]
		[EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the user's password.
		/// </summary>
		/// <value>
		/// A string representing the user's password.
		/// </value>
		/// <exception cref="ValidationException">Thrown if the password is empty or doesn't meet the required criteria.</exception>
		[Required(ErrorMessage = "A palavra-passe é obrigatória.")]
		[ValidPassword(ErrorMessage = "A palavra-passe deve ter pelo menos 8 caracteres, 2 números e 1 letra maiúscula.")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the confirmation of the user's password.
		/// </summary>
		/// <value>
		/// A string representing the confirmation of the password.
		/// </value>
		/// <exception cref="ValidationException">Thrown if the confirmation password doesn't match the original password.</exception>
		[Required(ErrorMessage = "A confirmação da palavra-passe é obrigatória.")]
		[Compare("Password", ErrorMessage = "As palavras-passes não coincidem.")]
		public string ConfirmPassword { get; set; }
	}
}