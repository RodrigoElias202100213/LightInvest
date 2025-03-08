using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

public class SecurityTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SecurityTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    //  Teste de Autenticação - Usuário não autenticado tenta acessar área protegida
    [Fact]
    public async Task AcessoProtegido_DeveRetornar_UnauthorizedOuNotFound_SeNaoAutenticado()
    {
        var response = await _client.GetAsync("/Admin/Dashboard");
        // Aceita Unauthorized ou NotFound
        Assert.True(response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.NotFound);
    }


    //  Teste de Autorização - Usuário comum tenta acessar área de administrador
    [Fact]
    public async Task AcessoAdmin_DeveRetornar_ForbiddenOuNotFound_SeNaoAutorizado()
    {
        var loginData = new { Email = "user@teste.com", Password = "Senha123!" };
        var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

        var loginResponse = await _client.PostAsync("/Account/Login", content);
        Assert.True(loginResponse.IsSuccessStatusCode);

        // Simula um token inválido para indicar falta de autorização
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "token-falso");

        var response = await _client.GetAsync("/Admin/Dashboard");

        // Aceita Forbidden ou NotFound, dependendo da estratégia de segurança
        Assert.True(response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.NotFound);
    }


   


    //  Teste contra XSS - Evitar que scripts sejam armazenados no sistema
    [Fact]
    public async Task Cadastro_DeveFalhar_SeContiverXSS()
    {
        var xssPayload = new { Name = "<script>alert('Hacked!')</script>", Email = "xss@teste.com", Password = "Senha123!" };
        var content = new StringContent(JsonSerializer.Serialize(xssPayload), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/Account/Register", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // Teste de Segurança de Senha - Validar regras mínimas de senha
    [Fact]
    public async Task Registro_DeveFalhar_SeSenhaForFraca()
    {
        var weakPasswordPayload = new { Name = "Teste", Email = "user@teste.com", Password = "123" };
        var content = new StringContent(JsonSerializer.Serialize(weakPasswordPayload), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/Account/Register", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }


}
