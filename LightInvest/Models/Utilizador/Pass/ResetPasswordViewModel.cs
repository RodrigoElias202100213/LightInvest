using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
	public string Email { get; set; }
	public string Token { get; set; }

	[Required]
	[DataType(DataType.Password)]
	[Display(Name = "Nova Palavra-Passe")]
	public string NewPassword { get; set; }

	[Required]
	[DataType(DataType.Password)]
	[Compare("NewPassword", ErrorMessage = "As palavras-passes não coincidem.")]
	[Display(Name = "Confirmar Palavra-Passe")]
	public string ConfirmPassword { get; set; }
}