namespace LightInvest.Models
{
	public class PasswordRecoveryToken
	{
		public int Id { get; set; } // ID único do token
		public string Email { get; set; } // E-mail do usuário que solicitou a recuperação
		public string ResetToken { get; set; } // Token gerado para a recuperação
		public DateTime TokenExpiration { get; set; } // Data e hora de expiração do token
	}

}