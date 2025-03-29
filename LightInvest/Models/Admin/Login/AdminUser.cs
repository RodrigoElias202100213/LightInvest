using System.ComponentModel.DataAnnotations;


namespace LightInvest.Models.Admin.Login
{
    public class AdminUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A palavra-passe é obrigatória.")]
        public string Password { get; set; }
    }
}
