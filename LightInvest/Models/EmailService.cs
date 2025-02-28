namespace LightInvest.Models
{
	using System.Net;
	using System.Net.Mail;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Configuration;  // Adicionar para ler as configurações

	public class EmailService
	{
		private readonly IConfiguration _configuration;

		// Injeta a configuração via construtor
		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		// Método assíncrono para enviar e-mail
		public async Task<bool> SendEmailAsync(string toAddress, string subject, string body)
		{
			try
			{
				// Obtém as configurações do arquivo appsettings.json
				string fromAddress = _configuration["FromAddress"];
				string smtpServer = _configuration["SmtpServer"];
				int smtpPort = int.Parse(_configuration["SmtpPort"]);
				string smtpUsername = _configuration["SmtpUsername"];
				string smtpPassword = _configuration["SmtpPassword"];

				MailAddress to = new MailAddress(toAddress);
				MailAddress from = new MailAddress(fromAddress);

				MailMessage email = new MailMessage(from, to)
				{
					Subject = subject,
					Body = body
				};

				using (SmtpClient smtp = new SmtpClient())
				{
					smtp.Host = smtpServer; // Usando o servidor SMTP da configuração
					smtp.Port = smtpPort;   // Usando a porta SMTP da configuração
					smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword); // Credenciais SMTP
					smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtp.EnableSsl = true;  // SSL habilitado

					// Envia o e-mail de forma assíncrona
					await smtp.SendMailAsync(email);
					Console.WriteLine("E-mail enviado com sucesso!");

					return true; // E-mail enviado com sucesso
				}
			}
			catch (SmtpException ex)
			{
				// Exceção em caso de falha no envio
				Console.WriteLine("Erro ao enviar e-mail: " + ex.Message);
				return false; // Erro ao enviar o e-mail
			}
		}
	}
}
