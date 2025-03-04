using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using LightInvest;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class ROICalculatorIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ROICalculatorIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Calcular_RetornaROI_ComSucesso()
    {
        var requestBody = new
        {
            CustoInstalacao = 50000,
            CustoManutencaoAnual = 1000,
            ConsumoEnergeticoMedio = 200,
            ConsumoEnergeticoRede = 500,
            RetornoEconomia = 1000
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/ROICalculator/Calcular", jsonContent);

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Contains("ROI Calculado", responseString);
    }

    [Fact]
    public async Task Calcular_DeveRetornarErro_QuandoEconomiaZero()
    {
        var requestBody = new
        {
            CustoInstalacao = 50000,
            CustoManutencaoAnual = 1000,
            ConsumoEnergeticoMedio = 200,
            ConsumoEnergeticoRede = 500,
            RetornoEconomia = 0 // Economia inválida
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/ROICalculator/Calcular", jsonContent);

        var responseString = await response.Content.ReadAsStringAsync();

        Assert.Contains("Erro: A economia total deve ser maior que zero!", responseString);
    }
}

