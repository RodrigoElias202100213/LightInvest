using System.Collections.Generic;

namespace LightInvest.Services
{
    /// <summary>
    /// Represents the response structure from the MediaStack API.
    /// </summary>
    public class MediaStackResponse
    {
        /// <summary>
        /// List of news articles retrieved from the API.
        /// </summary>
        public List<NewsArticle> Data { get; set; }
    }
}
