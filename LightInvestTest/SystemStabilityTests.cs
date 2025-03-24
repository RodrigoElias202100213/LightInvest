using Xunit;
using LightInvest.Controllers;
using LightInvest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using LightInvest.Models.BD;
using LightInvest.Models.Roi;
using LightInvest.Models.Utilizador.Login;

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
        // Arrange
        var user = new User
        {
            Email = "teste@lightinvest.com",
            Name = "Usuário Teste",
            Password = "SenhaSegura123"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Arrange
        var roiCalculator = new RoiCalculator
        {
            UserEmail = user.Email,
            CustoInstalacao = 10000m,
            CustoManutencaoAnual = 500m,
          
            ConsumoEnergeticoMedio = 1000m,
            ConsumoEnergeticoRede = 1001.25m,
            RetornoEconomia = 2000m,
            DataCalculado = DateTime.Now
        };

        _context.ROICalculators.Add(roiCalculator);
        await _context.SaveChangesAsync();

        // Act
        var resultado = roiCalculator.CalcularROI();

        // Assert
        Assert.True(resultado > 0, "O ROI deve ser maior que zero.");
        Assert.Equal(5m, resultado, precision: 2);
    }
}
