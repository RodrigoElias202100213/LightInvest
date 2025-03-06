using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class ROICalculatorRegressionTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ROICalculatorRegressionTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Calcular_DeveManterOMesmoResultado()
    {
        var formData = new Dictionary<string, string>
        {
            { "CustoInstalacao", "10000" },
            { "ConsumoEnergeticoMedio", "250" },
            { "ConsumoEnergeticoRede", "3000" },
            { "CustoManutencaoAnual", "200" },
            { "RetornoEconomia", "0.8" }
        };
        var content = new FormUrlEncodedContent(formData);

        var response = await _client.PostAsync("/ROICalculator/Calcular", content);
        var responseString = await response.Content.ReadAsStringAsync();
        System.Diagnostics.Debug.WriteLine(responseString);


        Assert.True(response.IsSuccessStatusCode, "A resposta do servidor não foi bem-sucedida.");
       

    }
}

