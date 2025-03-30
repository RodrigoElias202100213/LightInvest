using System.ComponentModel.DataAnnotations;

/// <summary>
/// Custom validation attribute that ensures a password meets the required criteria:
/// - At least 8 characters long
/// - Contains at least 2 digits
/// - Contains at least 1 uppercase letter
/// </summary>
public class ValidPasswordAttribute : ValidationAttribute
{
	/// <summary>
	/// Validates whether the password meets the specified criteria.
	/// </summary>
	/// <param name="value">The value to validate (password).</param>
	/// <returns>Returns true if the password meets all requirements, otherwise false.</returns>
	public override bool IsValid(object value)
	{
		if (value == null)
			return false;

		var password = value.ToString();

		if (password.Length < 8)
			return false;

		if (password.Count(char.IsDigit) < 2)
			return false;

		if (!password.Any(char.IsUpper))
			return false;

		return true;
	}

	/// <summary>
	/// Provides the error message that will be displayed when the password validation fails.
	/// </summary>
	/// <param name="name">The name of the property being validated (usually the password field).</param>
	/// <returns>A string containing the error message.</returns>
	public override string FormatErrorMessage(string name)
	{
		return "A palavra-passe deve ter pelo menos 8 caracteres, 2 números e 1 letra maiúscula.";
	}
}
