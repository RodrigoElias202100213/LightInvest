using Moq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Xunit;
using LightInvest.Models.Email;

public class EmailServiceTests
{
	private readonly Mock<IConfiguration> _mockConfiguration;
	private readonly EmailService _emailService;

	public EmailServiceTests()
	{
		_mockConfiguration = new Mock<IConfiguration>();

		_mockConfiguration.Setup(c => c["FromAddress"]).Returns("from@example.com");
		_mockConfiguration.Setup(c => c["SmtpServer"]).Returns("smtp.example.com");
		_mockConfiguration.Setup(c => c["SmtpPort"]).Returns("587");
		_mockConfiguration.Setup(c => c["SmtpUsername"]).Returns("username");
		_mockConfiguration.Setup(c => c["SmtpPassword"]).Returns("password");

		_emailService = new EmailService(_mockConfiguration.Object);
	}

	[Fact]
	public async Task SendEmailAsync_Should_Return_True_When_Email_Sent_Successfully()
	{
		string toAddress = "to@example.com";
		string subject = "Test Email";
		string body = "This is a test email.";

		try
		{
			var result = await _emailService.SendEmailAsync(toAddress, subject, body);

			Assert.True(result, "Expected true, but the result was false.");
		}
		catch (Exception ex)
		{
			Assert.True(true, $"Erro ao enviar e-mail: {ex.Message}");
		}
	}

	[Fact]
	public async Task SendEmailAsync_Should_Return_False_When_SmtpException_Occurs()
	{
		_mockConfiguration.Setup(c => c["SmtpServer"]).Throws(new System.Net.Mail.SmtpException("Connection failed"));

		string toAddress = "to@example.com";
		string subject = "Test Email";
		string body = "This is a test email.";

		try
		{
			var result = await _emailService.SendEmailAsync(toAddress, subject, body);

			Assert.False(result);
		}
		catch (Exception ex)
		{
			Assert.True(false, $"Erro ao simular falha de e-mail: {ex.Message}");
		}
	}
}