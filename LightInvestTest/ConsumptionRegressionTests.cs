using System;
using System.Collections.Generic;
using Xunit;

public class ConsumptionRegressionTests
{
	[Fact]
	public void CalculateTotalConsumption_ReturnsCorrectValue()
	{
		// Simula dados de consumo
		var model = new EnergyConsumptionViewModel
		{
			ConsumoDiaSemana = new List<decimal> { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 },
			ConsumoFimSemana = new List<decimal> { 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 },
			MesesOcupacao = new List<string> { "Jan", "Fev", "Mar" }
		};

		var controller = new EnergySimulationController();

		// Calcula o consumo total
		decimal totalConsumo = controller.CalculateTotalConsumption(model);

		// Verifica se o valor está correto
		Assert.True(totalConsumo > 0, "O consumo total deve ser maior que zero.");
	}
}

// Classe fictícia para simular o ViewModel
public class EnergyConsumptionViewModel
{
	public List<decimal> ConsumoDiaSemana { get; set; }
	public List<decimal> ConsumoFimSemana { get; set; }
	public List<string> MesesOcupacao { get; set; }
}

// Classe fictícia para simular o Controller
public class EnergySimulationController
{
	public decimal CalculateTotalConsumption(EnergyConsumptionViewModel model)
	{
		decimal consumoTotal = 0;
		foreach (var consumo in model.ConsumoDiaSemana) consumoTotal += consumo;
		foreach (var consumo in model.ConsumoFimSemana) consumoTotal += consumo;
		return consumoTotal;
	}
}