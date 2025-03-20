using System;
using Xunit;
using LightInvest.Models;

namespace LightInvest.Tests
{
	public class TarifaTests
	{
		[Theory]
		[InlineData(0.5, TipoTarifa.Residencial, 0.6)]
		[InlineData(0.5, TipoTarifa.Comercial, 1.0)]
		[InlineData(0.5, TipoTarifa.Industrial, 1.4)]
		public void PrecoFinal_DeveRetornarValorCorreto(decimal precoKWh, TipoTarifa tipo, decimal precoEsperado)
		{
			// Arrange
			var tarifa = new Tarifa { PrecoKWh = precoKWh, Tipo = tipo };

			// Act
			decimal precoFinal = tarifa.PrecoFinal;

			// Assert
			Assert.Equal(precoEsperado, precoFinal);
		}

		[Fact]
		public void ObterValorExtra_DeveLancarExcecaoParaTipoInvalido()
		{
			// Arrange
			var tarifa = new Tarifa { PrecoKWh = 0.5m, Tipo = (TipoTarifa)99 }; // Tipo inválido

			// Act & Assert
			Assert.Throws<ArgumentException>(() => tarifa.PrecoFinal);
		}
	}
}