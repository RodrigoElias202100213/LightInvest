using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "O nome é obrigatório.")]
		[StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "O e-mail é obrigatório.")]
		[EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "A palavra-passe é obrigatória.")]
		[ValidPassword(ErrorMessage = "A palavra-passe deve ter pelo menos 8 caracteres, 2 números e 1 letra maiúscula.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "A confirmação da palavra-passe é obrigatória.")]
		[Compare("Password", ErrorMessage = "As palavras-passes não coincidem.")]
		public string ConfirmPassword { get; set; }
	}
}