namespace LightInvest.Models.Email
{
	using System.Net;
	using System.Net.Mail;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Configuration;

	/// <summary>
	/// Provides methods for sending email messages asynchronously.
	/// </summary>
	public class EmailService
	{
		private readonly IConfiguration _configuration;


		/// <summary>
		/// Initializes a new instance of the <see cref="EmailService"/> class.
		/// </summary>
		/// <param name="configuration">The configuration object used to retrieve SMTP settings.</param>
		public EmailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		/// <summary>
		/// Sends an email asynchronously to a specified address with a subject and body.
		/// </summary>
		/// <param name="toAddress">The recipient's email address.</param>
		/// <param name="subject">The subject of the email.</param>
		/// <param name="body">The body content of the email.</param>
		/// <returns>A task that represents the asynchronous operation. The task result is a <see cref="bool"/> indicating whether the email was sent successfully.</returns>
		/// <remarks>
		/// This method uses SMTP configuration settings from the application's configuration file.
		/// </remarks>
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