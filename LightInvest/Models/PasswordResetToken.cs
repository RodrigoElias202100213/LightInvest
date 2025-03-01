using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models
{
	public class PasswordResetToken
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Token { get; set; }

		public DateTime Expiration { get; set; }
	}
}