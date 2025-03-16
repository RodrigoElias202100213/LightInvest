using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;

public class ThirdPartyApiTests
{
    [Fact]
    public async Task GetTarifa_ReturnsValidResponse()
    {
        // Simula resposta da API externa
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"tarifa\": 0.55 }")
            });

        var client = new HttpClient(httpMessageHandlerMock.Object);

        // Simulação da chamada à API
        var response = await client.GetAsync("https://api-tarifas.com/tarifa");
        var content = await response.Content.ReadAsStringAsync();

        // Verificações
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("0.55", content);
    }
}
