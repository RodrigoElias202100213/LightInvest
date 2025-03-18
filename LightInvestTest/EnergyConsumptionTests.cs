using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using LightInvest.Models;

namespace LightInvestTest
{
	public class EnergyConsumptionTests
	{
		[Fact]
		public void CalcularMediaSemana_DeveCalcularMediaCorretamente()
		{
			// Arrange
			var energyConsumption = new EnergyConsumption
			{
				ConsumoDiaSemana = new List<decimal> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 }
			};

			// Act
			energyConsumption.CalcularMediaSemana();

			// Assert
			Assert.Equal(11.5m, energyConsumption.MediaSemana);
		}

		[Fact]
		public void CalcularMediaFimSemana_DeveCalcularMediaCorretamente()
		{
			// Arrange
			var energyConsumption = new EnergyConsumption
			{ 
				ConsumoFimSemana = new List<decimal> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 }
			};

			// Act
			energyConsumption.CalcularMediaFimSemana();

			// Assert
			Assert.Equal(9.3m, energyConsumption.MediaFimSemana);
		}

		[Fact]
		public void CalcularConsumoMensal_DeveCalcularConsumoTotalCorretamente()
		{
			// Arrange
			var energyConsumption = new EnergyConsumption
			{
				ConsumoDiaSemana = new List<decimal> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 }, //11.5
				ConsumoFimSemana = new List<decimal> { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 }, //15.8
				MesesOcupacao = new List<string> { "Janeiro", "Fevereiro" }
			};

			// Act
			energyConsumption.CalcularConsumoMensal();

			// Assert
			Assert.Equal(801.9m, energyConsumption.ConsumoTotal);
		}

		[Fact]
		public void AplicarDesconto_SemanaDeveAplicarDescontoCorretamente()
		{
			// Arrange
			var energyConsumption = new EnergyConsumption();

			// Act
			var consumoComDesconto = energyConsumption.AplicarDesconto(6, 100, false); 

			// Assert
			Assert.Equal(70m, consumoComDesconto);
		}

		[Fact]
		public void AplicarDesconto_FimDeSemanaDeveAplicarDescontoCorretamente()
		{
			// Arrange
			var energyConsumption = new EnergyConsumption();

			// Act
			var consumoComDesconto = energyConsumption.AplicarDesconto(10, 100, true); 

			// Assert
			Assert.Equal(80m, consumoComDesconto); // 20% de desconto
		}

		[Fact]
		public void ObterNumeroDeSemanasNoMes_DeveRetornarNumeroCorretoDeSemanas()
		{
			// Arrange
			var energyConsumption = new EnergyConsumption();

			// Act
			var semanasJaneiro = energyConsumption.ObterNumeroDeSemanasNoMes("Janeiro");
			var semanasFevereiro = energyConsumption.ObterNumeroDeSemanasNoMes("Fevereiro");

			// Assert
			Assert.Equal(5, semanasJaneiro); 
			Assert.Equal(4, semanasFevereiro);
		}
	}
}
