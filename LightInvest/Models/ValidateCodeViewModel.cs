using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
    public class ValidateCodeViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
