using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "O e-mail é obrigatório.")]
		[EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "A palavra-passe é obrigatória.")]
		public string Password { get; set; }
	}
}