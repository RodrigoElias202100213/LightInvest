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
		[StringLength(100, MinimumLength = 6, ErrorMessage = "A palavra-passe deve ter pelo menos 6 caracteres.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "A confirmação da palavra-passe é obrigatória.")]
		[Compare("Password", ErrorMessage = "As palavras-passe não coincidem.")]
		public string ConfirmPassword { get; set; }
	}
}