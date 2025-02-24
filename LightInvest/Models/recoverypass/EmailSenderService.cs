using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LightInvest.Controllers;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace LightInvest.Models.recovrypass
{
	public class EmailSenderService:IEmailService
	{
		private readonly EmailSettings _emailSettings;

		public EmailSenderService(IOptions<EmailSettings> emailSettings)
		{
			_emailSettings = emailSettings.Value;
		}

		public async Task SendEmailAsync(string to, string subject, string body)
		{
			using var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
			{
				Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword),
				EnableSsl = _emailSettings.EnableSsl
			};

			var mail = new MailMessage(_emailSettings.SenderEmail, to, subject, body)
			{
				IsBodyHtml = true
			};

			await smtp.SendMailAsync(mail);
		}
	}
}