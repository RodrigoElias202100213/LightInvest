using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LightInvest.Models.Educ.Artigos;
using Microsoft.Extensions.Configuration;

namespace LightInvest.Services
{
	public class MediastackService
	{
		public HttpClient HttpClient { get; }
		public string ApiKey { get; }
		private readonly string _baseUrl;
		private readonly string _defaultLanguage;

		// Construtor que injeta HttpClient e configura a API
		public MediastackService(HttpClient httpClient, IConfiguration configuration)
		{
			HttpClient = httpClient;
			ApiKey = configuration["Mediastack:ApiKey"]; // Obtém a chave da API do arquivo de configurações
			_baseUrl = configuration["Mediastack:BaseUrl"]; // Obtém a URL base da API
			_defaultLanguage = configuration["Mediastack:DefaultLanguage"]; // Obtém o idioma padrão
		}

		public async Task<List<MediastackArticle>> GetNewsAsync(string categoria)
		{
			string url = $"{_baseUrl}?access_key={ApiKey}&categories={categoria}&languages={_defaultLanguage}";

			var response = await HttpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			var data = JsonSerializer.Deserialize<MediastackResponse>(responseContent);
			return data?.Data ?? new List<MediastackArticle>();
		}
	}
}
