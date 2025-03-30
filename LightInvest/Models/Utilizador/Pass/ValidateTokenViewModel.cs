using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the model used to validate a password reset token.
/// </summary>
public class ValidateTokenViewModel
{
	/// <summary>
	/// Gets or sets the email address of the user requesting token validation.
	/// </summary>
	/// <value>
	/// The email address of the user whose token is being validated.
	/// </value>
	/// <example>user@example.com</example>
	[Required]
	public string Email { get; set; }

	/// <summary>
	/// Gets or sets the token to validate the password reset request.
	/// </summary>
	/// <value>
	/// The token used for password reset validation.
	/// </value>
	/// <example>abcdef123456</example>
	[Required]
	public string Token { get; set; }
}
