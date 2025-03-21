using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LightInvest;
using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace LightInvest.Tests
{
	public class AccountControllerThirdPartyIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly WebApplicationFactory<Program> _factory;

		public AccountControllerThirdPartyIntegrationTests(WebApplicationFactory<Program> factory)
		{
			_factory = factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureServices(services =>
				{
					var dbContextDescriptor = services.SingleOrDefault(
						d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
					if (dbContextDescriptor != null)
					{
						services.Remove(dbContextDescriptor);
					}
					services.AddDbContext<ApplicationDbContext>(options =>
					{
						options.UseInMemoryDatabase("TestDbThirdParty");
					});

					var emailServiceDescriptor = services.SingleOrDefault(
						d => d.ServiceType == typeof(IEmailService));
					if (emailServiceDescriptor != null)
					{
						services.Remove(emailServiceDescriptor);
					}

					var emailServiceMock = new Mock<IEmailService>();
					emailServiceMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
						.ReturnsAsync(true);
					services.AddSingleton<IEmailService>(emailServiceMock.Object);
				});
			});
		}
		[Fact]
		public async Task Enviaremail_Success_ReturnsRedirectToHome()
		{
			var factoryWithSuccess = _factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureServices(services =>
				{
					var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmailService));
					if (descriptor != null)
					{
						services.Remove(descriptor);
					}
					var emailServiceMock = new Mock<IEmailService>();
					emailServiceMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
						.ReturnsAsync(true);
					services.AddSingleton<IEmailService>(emailServiceMock.Object);
				});
			});

			var client = factoryWithSuccess.CreateClient(new WebApplicationFactoryClientOptions
			{
				AllowAutoRedirect = false
			});

			string toAddress = "test@example.com";
			string subject = "Test Subject";
			string body = "Test Body";

			var response = await client.PostAsync($"/Account/Enviaremail?toAddress={toAddress}&subject={subject}&body={body}", null);

			Assert.Equal(System.Net.HttpStatusCode.Redirect, response.StatusCode);
			Assert.Contains("/Home/Index", response.Headers.Location.ToString());
		}


		[Fact]
		public async Task Enviaremail_Failure_ReturnsRedirectToHome()
		{
			var factoryWithFailure = _factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureServices(services =>
				{
					var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmailService));
					if (descriptor != null)
					{
						services.Remove(descriptor);
					}
					var emailServiceMock = new Mock<IEmailService>();
					emailServiceMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
						.ReturnsAsync(false);
					services.AddSingleton<IEmailService>(emailServiceMock.Object);
				});
			});

			var client = factoryWithFailure.CreateClient(new WebApplicationFactoryClientOptions
			{
				AllowAutoRedirect = false
			});

			string toAddress = "test@example.com";
			string subject = "Test Subject";
			string body = "Test Body";

			var response = await client.PostAsync($"/Account/Enviaremail?toAddress={toAddress}&subject={subject}&body={body}", null);

			Assert.Equal(System.Net.HttpStatusCode.Redirect, response.StatusCode);
			Assert.Contains("/Home/Index", response.Headers.Location.ToString());
		}

	}
}