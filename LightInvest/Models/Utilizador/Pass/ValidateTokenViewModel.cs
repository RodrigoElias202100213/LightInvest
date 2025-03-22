using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Utilizador.Pass
{
    public class ValidateTokenViewModel
	{
        [Required]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
