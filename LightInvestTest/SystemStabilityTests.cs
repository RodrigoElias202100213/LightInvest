using Xunit;
using LightInvest.Controllers;
using LightInvest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using LightInvest.Models.BD;

public class SystemStabilityTests
{
    private readonly ApplicationDbContext _context;

    public SystemStabilityTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
        _context = new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CalcularROI_ReturnsCorrectROI_WhenDataIsValid()
    {
        // Arrange: Cria um usuário válido
        var user = new User
        {
            Email = "teste@lightinvest.com",
            Name = "Usuário Teste",
            Password = "SenhaSegura123"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Arrange: Configura o ROI com dados que garantem economiaAnual = 2000
        var roiCalculator = new RoiCalculator
        {
            UserEmail = user.Email,
            CustoInstalacao = 10000m,
            CustoManutencaoAnual = 500m,
            // Valores ajustados para obter (ConsumoEnergeticoRede – ConsumoEnergeticoMedio) = 1.25
            ConsumoEnergeticoMedio = 1000m,
            ConsumoEnergeticoRede = 1001.25m,
            RetornoEconomia = 2000m,
            DataCalculado = DateTime.Now
        };

        _context.ROICalculators.Add(roiCalculator);
        await _context.SaveChangesAsync();

        // Act: Executa o método de cálculo do ROI
        var resultado = roiCalculator.CalcularROI();

        // Debug: Exibe os valores no console para verificação
        Console.WriteLine($"ROI Calculado: {resultado}");

        // Assert: ROI deve ser maior que zero e igual a 5 (10000/2000)
        Assert.True(resultado > 0, "O ROI deve ser maior que zero.");
        Assert.Equal(5m, resultado, precision: 2);
    }
}
