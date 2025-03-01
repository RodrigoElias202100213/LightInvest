using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
	public string Email { get; set; }
	public string Token { get; set; }

	[Required]
	[DataType(DataType.Password)]
	[Display(Name = "Nova Password")]
	public string NewPassword { get; set; }

	[Required]
	[DataType(DataType.Password)]
	[Compare("NewPassword", ErrorMessage = "As passwrods não coincidem.")]
	[Display(Name = "Confirmar passowrd")]
	public string ConfirmPassword { get; set; }
}