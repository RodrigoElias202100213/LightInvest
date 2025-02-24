using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}