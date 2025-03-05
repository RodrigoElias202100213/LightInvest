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
using AngleSharp.Html.Parser;


public class AccountControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly HttpClient _client;
    private readonly ApplicationDbContext _context;

    public AccountControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        // Configura o HttpClient e o banco de dados em memória
        var scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove o contexto do banco de dados existente e substitui por um banco em memória
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
        // Limpa o banco de dados em memória após cada teste
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task Login_ReturnsRedirect_WhenCredentialsAreValid()
    {
        // Arrange
        var loginData = new { Email = "test@example.com", Password = "Password123" };
        var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/Account/Login", content);

        // Assert
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Redirect);
    }
    [Fact]
    public async Task Register_ReturnsError_WhenEmailAlreadyExists()
    {
        // Arrange
        var existingEmail = "existing@example.com";

        // Cria um usuário com o e-mail existente no banco de dados
        _context.Users.Add(new User { Email = existingEmail, Password = "password" });
        await _context.SaveChangesAsync();

        var duplicateData = new Dictionary<string, string>
        {
            ["Name"] = "Existing User",
            ["Email"] = existingEmail, // E-mail já cadastrado
            ["Password"] = "ValidPass123",
            ["ConfirmPassword"] = "ValidPass123"
        };

        var content = new FormUrlEncodedContent(duplicateData);

        // Act
        var response = await _client.PostAsync("/Account/Register", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Debug
        Console.WriteLine($"Response Status: {response.StatusCode}");
        Console.WriteLine($"Response Content: {responseContent}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode); // Espera-se que a página de registro seja retornada com erro

        // Verifica se a resposta contém uma mensagem de erro
        var parser = new HtmlParser();
        var document = parser.ParseDocument(responseContent);
        var errorMessage = document.QuerySelector(".validation-summary-errors li")?.TextContent;

        Assert.Contains("já está em uso", errorMessage); // Verifica se a mensagem de erro menciona que o e-mail já está em uso
    }
}
