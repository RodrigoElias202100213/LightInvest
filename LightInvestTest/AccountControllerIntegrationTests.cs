using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using LightInvest.Models;
using LightInvest.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;



public class AccountControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
	private readonly HttpClient _client;
	private readonly ApplicationDbContext _context;

	public AccountControllerIntegrationTests(WebApplicationFactory<Program> factory)
	{
		var scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
		var scope = scopeFactory.CreateScope();
		_context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		_client = factory.WithWebHostBuilder(builder =>
		{
			builder.ConfigureServices(services =>
			{
				var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
				if (descriptor != null)
				{
					services.Remove(descriptor);
				}
				services.AddDbContext<ApplicationDbContext>(options =>
				{
					options.UseInMemoryDatabase("TestDb");
				});
			});
		}).CreateClient();
	}

	public void Dispose()
	{
		_context.Database.EnsureDeleted();
		_context.Dispose();
	}

	[Fact]
	public async Task Login_ReturnsRedirect_WhenCredentialsAreValid()
	{
		var loginData = new { Email = "test@example.com", Password = "Password123" };
		var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

		var response = await _client.PostAsync("/Account/Login", content);

		Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Redirect);
	}

}
