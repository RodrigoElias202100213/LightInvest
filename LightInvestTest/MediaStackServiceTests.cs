using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;
using LightInvest.Services;

public class MediaStackServiceTests
{
    [Fact]
    public async Task GetSolarPanelArticlesAsync_ShouldReturnArticles_WhenApiResponseIsSuccessful()
    {
       
        var expectedArticles = new List<NewsArticle>
        {
            new NewsArticle { Title = "Solar Panel Advances", Url = "http://example.com/solar1" },
            new NewsArticle { Title = "New Solar Energy Trends", Url = "http://example.com/solar2" }
        };

        var responseContent = JsonConvert.SerializeObject(new { data = expectedArticles });
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var mediaStackService = new MediaStackService(httpClient);

       
        var result = await mediaStackService.GetSolarPanelArticlesAsync();

       
        Assert.NotNull(result);
        Assert.Equal(expectedArticles.Count, result.Count);
        Assert.Equal(expectedArticles[0].Title, result[0].Title);
    }

    [Fact]
    public async Task GetSolarPanelArticlesAsync_ShouldReturnEmptyList_WhenApiResponseFails()
    {
        
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var mediaStackService = new MediaStackService(httpClient);

       
        var result = await mediaStackService.GetSolarPanelArticlesAsync();

        
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
