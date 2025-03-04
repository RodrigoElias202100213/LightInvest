using System;
using Xunit;
using LightInvest.Models;

public class RoiCalculatorTests
{
	[Fact]
	public void CalcularROI_DeveRetornarValorCorreto()
	{
		// Arrange
		var roiCalculator = new RoiCalculator
		{
			CustoInstalacao = 50000,
			CustoManutencaoAnual = 200,
			ConsumoEnergeticoMedio = 1000,
			ConsumoEnergeticoRede = 2500,
			RetornoEconomia = 0.5m
		};

		// Act
		var resultado = roiCalculator.CalcularROI();

		// Assert
		Assert.True(resultado > 0);
	}

	[Fact]
	public void CalcularROI_DeveLancarExcecao_ParaValoresInvalidos()
	{
		var roiCalculator = new RoiCalculator
		{
			CustoInstalacao = 0,
			CustoManutencaoAnual = 200,
			ConsumoEnergeticoMedio = 1000,
			ConsumoEnergeticoRede = 1200,
			RetornoEconomia = 0.5m
		};

		Assert.Throws<ArgumentException>(() => roiCalculator.CalcularROI());
	}
}