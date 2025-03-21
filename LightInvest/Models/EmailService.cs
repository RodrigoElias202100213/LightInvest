namespace LightInvest.Models
{
	using System.Net;
	using System.Net.Mail;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Configuration;
	public class EmailService
	{
		private readonly IConfiguration _configuration;

		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<bool> SendEmailAsync(string toAddress, string subject, string body)
		{
			try
			{
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
					smtp.Host = smtpServer;
					smtp.Port = smtpPort;
					smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
					smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtp.EnableSsl = true;

					await smtp.SendMailAsync(email);
					Console.WriteLine("E-mail enviado com sucesso!");

					return true;
				}
			}
			catch (SmtpException ex)
			{
				Console.WriteLine("Erro ao enviar e-mail: " + ex.Message);
				return false;
			}
		}
	}
}