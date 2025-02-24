using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LightInvest.Models
{
	public class EmailService
	{
		public async Task SendEmailAsync(string to, string subject, string body)
		{
			var smtpClient = new SmtpClient("smtp.seuprovedor.com")
			{
				Port = 587,
				Credentials = new NetworkCredential("seuemail@dominio.com", "suaSenha"),
				EnableSsl = true
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress("seuemail@dominio.com"),
				Subject = subject,
				Body = body,
				IsBodyHtml = true
			};

			mailMessage.To.Add(to);

			await smtpClient.SendMailAsync(mailMessage);
		}
	}

}

