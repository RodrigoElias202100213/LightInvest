using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LightInvest.Services
{
    /// <summary>
    /// Service responsible for fetching news articles from the MediaStack API.
    /// </summary>
    public class MediaStackService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "d8b5055bff8f9fdea12220314947ccfa";
        private const string BaseUrl = "http://api.mediastack.com/v1/news";

        public MediaStackService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Fetches news articles related to solar panels from the MediaStack API.
        /// </summary>
        /// <returns>A list of news articles.</returns>
        public async Task<List<NewsArticle>> GetSolarPanelArticlesAsync()
        {
            var url = $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=en&keywords=solar panels&limit=5";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<NewsArticle>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MediaStackResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Data ?? new List<NewsArticle>();
        }
    }
}
