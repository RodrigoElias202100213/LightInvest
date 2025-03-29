using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Utilizador.Login
{
	/// <summary>
	/// Represents a user in the system with basic authentication information and roles.
	/// </summary>
	public class User
	{
		/// <summary>
		/// Gets or sets the unique identifier for the user.
		/// </summary>
		/// <value>
		/// The unique identifier for the user.
		/// </value>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		/// <example>John Doe</example>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the email address of the user.
		/// </summary>
		/// <value>
		/// The email address of the user.
		/// </value>
		/// <example>user@example.com</example>
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password of the user.
		/// </summary>
		/// <value>
		/// The password for the user account.
		/// </value>
		[Required]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user has administrative privileges.
		/// </summary>
		/// <value>
		/// <c>true</c> if the user is an administrator; otherwise, <c>false</c>.
		/// </value>
		public bool IsAdmin { get; set; } = false;
	}
}
