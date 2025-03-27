using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LightInvest.Models.Educ.Artigos;

namespace LightInvest.Services
{
	public class MediastackService
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiKey;

		public MediastackService(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_apiKey = configuration["Mediastack:ApiKey"]; // Pegando a API Key do appsettings.json
		}

		public async Task<MediastackResponse> GetNewsAsync(string categoria)
		{
			string url = $"http://api.mediastack.com/v1/news?access_key={_apiKey}&categories={categoria}&languages=en";
			var response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();

			string responseContent = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<MediastackResponse>(responseContent);
		}
	}
}
