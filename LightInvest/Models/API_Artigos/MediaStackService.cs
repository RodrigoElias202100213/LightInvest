using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace LightInvest.Services
{
    /// <summary>
    /// Service responsible for fetching news articles from the MediaStack API.
    /// </summary>
    public class MediaStackService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "73261e3e3f837ec6c829b44371ae2ad7";
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
            var urls = new List<string>
    {
        $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=en&keywords=solar panels&limit=2",
        $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=en&keywords=solar incentives&limit=2",
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



        public async Task<List<NewsArticle>> GetRenewableEnergyArticlesAsync()
        {
            var url = $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=en&keywords=renewable energy&limit=5";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<NewsArticle>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MediaStackResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var articles = result?.Data ?? new List<NewsArticle>();


            var distinctArticles = articles
                .GroupBy(a => a.Title)
                .Select(g => g.First())
                .ToList();

            return distinctArticles;
        }

        public async Task<List<NewsArticle>> GetROIArticlesAsync()
        {
            var url = $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=en&keywords=return on investment calculation&roilimit=5";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<NewsArticle>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MediaStackResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var articles = result?.Data ?? new List<NewsArticle>();


            var distinctArticles = articles
                .GroupBy(a => a.Title)
                .Select(g => g.First())
                .ToList();

            return distinctArticles;
        }

        public async Task<List<NewsArticle>> GetEnergyEfficiencyArticleAsync()
        {
            var url = $"{BaseUrl}?access_key={ApiKey}&categories=general&languages=en&keywords=energy efficiency&limit=5";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<NewsArticle>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MediaStackResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var articles = result?.Data ?? new List<NewsArticle>();


            var distinctArticles = articles
                .GroupBy(a => a.Title)
                .Select(g => g.First())
                .ToList();

            return distinctArticles;
        }

    }
}
