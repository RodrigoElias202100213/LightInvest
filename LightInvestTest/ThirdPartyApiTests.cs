using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Moq.Protected;
using Xunit;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using LightInvest; 


public class TarifaService
{
	private readonly HttpClient _httpClient;

	public TarifaService(HttpClient httpClient)
	{
		if (httpClient.BaseAddress == null)
		{
			throw new InvalidOperationException("HttpClient precisa ter uma BaseAddress definida.");
		}

		_httpClient = httpClient;
	}

	public async Task<decimal> GetTarifaAsync()
	{
		var response = await _httpClient.GetAsync("tarifa"); // Apenas "tarifa", pois já temos a BaseAddress
		response.EnsureSuccessStatusCode();

		var content = await response.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<TarifaResponse>(content)
					 ?? throw new InvalidOperationException("Resposta inválida da API");

		return result.Tarifa;
	}

	public class TarifaResponse
	{
		public decimal Tarifa { get; set; }
	}

}

public class ThirdPartyApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public ThirdPartyApiIntegrationTests(WebApplicationFactory<Program> factory)
	{
		_client = factory.CreateClient();
	}

	[Fact]
	public async Task GetTarifaAsync_ReturnsValidResponse()
	{
		
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

		var httpClient = new HttpClient(httpMessageHandlerMock.Object)
		{
			BaseAddress = new Uri("https://api-tarifas.com/") // Definindo a BaseAddress corretamente
		};

		var service = new TarifaService(httpClient);

		
		var tarifa = await service.GetTarifaAsync();

		
		Assert.Equal(0.55m, tarifa);
	}
}
