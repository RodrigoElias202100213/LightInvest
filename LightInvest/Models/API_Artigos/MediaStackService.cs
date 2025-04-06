using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace LightInvest.Services
{
    /// <summary>
    /// Service responsible for retrieving news articles from the MediaStack API.
    /// </summary>
    public class MediaStackService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "3a596ceb3cb2f2c7a1b17a1771f4ce9e";
        private const string BaseUrl = "http://api.mediastack.com/v1/news";

        /// <summary>
        /// Constructor for the MediaStackService.
        /// </summary>
        /// <param name="httpClient">HttpClient instance used to make HTTP requests.</param>
        public MediaStackService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves articles about solar panels from the MediaStack API.
        /// </summary>
        /// <returns>List of news articles about solar panels.</returns>
        public async Task<List<NewsArticle>> GetSolarPanelArticlesAsync()
        {
            var urls = new List<string>
            {
               $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=en&keywords=solar panels,install solar panels&limit=3",
            };

            var articles = new List<NewsArticle>();

            foreach (var url in urls)
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    continue;
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<MediaStackResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result?.Data != null)
                {
                    articles.AddRange(result.Data);
                }
            }

            var distinctArticles = articles
                .GroupBy(a => a.Title)
                .Select(g => g.First())
                .ToList();

            return distinctArticles;
        }

        /// <summary>
        /// Retrieves articles about renewable energy from the MediaStack API.
        /// </summary>
        /// <returns>List of news articles about renewable energy.</returns>
        public async Task<List<NewsArticle>> GetRenewableEnergyArticlesAsync()
        {
            var urls = new List<string>
            {
                $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=pt&keywords=Incentivos à Energia Renovável&limit=4",
                $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=pt&keywords=Instalação de Painéis Solares&limit=4",
            };

            var articles = new List<NewsArticle>();

            foreach (var url in urls)
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    continue;
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<MediaStackResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result?.Data != null)
                {
                    articles.AddRange(result.Data);
                }
            }

            var distinctArticles = articles
                .GroupBy(a => a.Title)
                .Select(g => g.First())
                .ToList();

            return distinctArticles;
        }

        /// <summary>
        /// Retrieves articles about return on investment (ROI) calculation from the MediaStack API.
        /// </summary>
        /// <returns>List of news articles about ROI calculation.</returns>
        public async Task<List<NewsArticle>> GetROIArticlesAsync()
        {
            var urls = new List<string>
            {
                $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=pt&keywords=Viabilidade financeira energia renovável&limit=3",
                $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=pt&keywords=Investimento em energia limpa&limit=3",
            };

            var articles = new List<NewsArticle>();

            foreach (var url in urls)
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    continue;
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<MediaStackResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result?.Data != null)
                {
                    articles.AddRange(result.Data);
                }
            }

            var distinctArticles = articles
                .GroupBy(a => a.Title)
                .Select(g => g.First())
                .ToList();

            return distinctArticles;
        }

        /// <summary>
        /// Retrieves articles about energy efficiency from the MediaStack API.
        /// </summary>
        /// <returns>List of news articles about energy efficiency.</returns>
        public async Task<List<NewsArticle>> GetEnergyEfficiencyArticleAsync()
        {
            var urls = new List<string>
            {
                $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=en&keywords=energy efficiency&limit=2",
            };

            var articles = new List<NewsArticle>();

            foreach (var url in urls)
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    continue;
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<MediaStackResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result?.Data != null)
                {
                    articles.AddRange(result.Data);
                }
            }

            var distinctArticles = articles
                .GroupBy(a => a.Title)
                .Select(g => g.First())
                .ToList();

            return distinctArticles;
        }
    }
}
