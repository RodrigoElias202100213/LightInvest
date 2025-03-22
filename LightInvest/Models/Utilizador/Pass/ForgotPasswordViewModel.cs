using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Utilizador.Pass
{
	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}

}