using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LightInvest.Models
{
	public class User : IdentityUser
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
