using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the model used to reset a user's password.
/// </summary>
public class ResetPasswordViewModel
{
	/// <summary>
	/// Gets or sets the email address of the user requesting the password reset.
	/// </summary>
	/// <value>
	/// The email address of the user.
	/// </value>
	/// <example>user@example.com</example>
	public string Email { get; set; }

	/// <summary>
	/// Gets or sets the token used to verify the password reset request.
	/// </summary>
	/// <value>
	/// The token string for the password reset.
	/// </value>
	/// <example>abcdef123456</example>
	public string Token { get; set; }

	/// <summary>
	/// Gets or sets the new password that the user wishes to set.
	/// </summary>
	/// <value>
	/// The new password entered by the user.
	/// </value>
	/// <example>newSecurePassword123</example>
	[Required]
	[DataType(DataType.Password)]
	[Display(Name = "Nova Palavra-Passe")]
	public string NewPassword { get; set; }

	/// <summary>
	/// Gets or sets the confirmation password to ensure the new password is correct.
	/// </summary>
	/// <value>
	/// The confirmation password entered by the user.
	/// </value>
	/// <example>newSecurePassword123</example>
	[Required]
	[DataType(DataType.Password)]
	[Compare("NewPassword", ErrorMessage = "As palavras-passes não coincidem.")]
	[Display(Name = "Confirmar Palavra-Passe")]
	public string ConfirmPassword { get; set; }
}
