using LightInvest.Models;

namespace LightInvestTest;

public class RoiCalculatorTests
{
	[Fact]
	public void CalcularROI_DeveRetornarValorCorreto()
	{
		var roiCalculator = new RoiCalculator
		{
			CustoInstalacao = 50000,
			CustoManutencaoAnual = 200,
			ConsumoEnergeticoMedio = 1000,
			ConsumoEnergeticoRede = 2500,
			RetornoEconomia = 0.5m
		};

		var resultado = roiCalculator.CalcularROI();

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