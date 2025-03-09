using Moq;
using System.Threading.Tasks;
using Xunit;

public class ExternalApiServiceMock
{
    [Fact]
    public async Task Simular_ApiDeTerceiros()
    {
        var mock = new Mock<IExternalApiService>();

        mock.Setup(api => api.ObterTaxaCambioAsync("USD"))
            .ReturnsAsync(5.25m);

        var taxa = await mock.Object.ObterTaxaCambioAsync("USD");

        Assert.Equal(5.25m, taxa);
    }
}

public interface IExternalApiService
{
    Task<decimal> ObterTaxaCambioAsync(string moeda);
}
