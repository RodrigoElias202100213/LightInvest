namespace LightInvest.Services
{
    /// <summary>
    /// Represents a news article with relevant details.
    /// </summary>
    public class NewsArticle
    {
        /// <summary>
        /// Title of the news article.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Brief description of the news article.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL link to the full news article.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// URL of the image associated with the news article.
        /// </summary>
        public string Image { get; set; }
    }
}
