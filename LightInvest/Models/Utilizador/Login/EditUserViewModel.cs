namespace LightInvest.Models.Utilizador.Login
{
	/// <summary>
	/// Represents the view model for editing user information.
	/// </summary>
	public class EditUserViewModel
	{
		/// <summary>
		/// Gets or sets the unique identifier for the user.
		/// </summary>
		/// <value>
		/// The identifier of the user.
		/// </value>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the email address of the user.
		/// </summary>
		/// <value>
		/// The email address of the user.
		/// </value>
		public string Email { get; set; }
	}
}
