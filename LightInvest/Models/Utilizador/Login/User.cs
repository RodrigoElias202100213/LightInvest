using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Utilizador.Login
{
	public class User
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}