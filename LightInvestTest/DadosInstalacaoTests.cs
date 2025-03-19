using System;
using LightInvest.Models;
using LightInvest.Models.b;
using Xunit;

namespace LightInvest.Tests
{
	public class DadosInstalacaoTests
	{
		[Fact]
		public void CalcularPrecoInstalacao_DeveRetornarPrecoCorreto()
		{
			// Arrange
			var modeloPainel = new ModeloPainelSolar
			{
				Id = 1,
				ModeloNome = "Painel Solar X",
				Preco = 2000.00m
			};

			var dadosInstalacao = new DadosInstalacao
			{
				ModeloPainel = modeloPainel,
				NumeroPaineis = 10,
				Inclinacao = 30,
				Dificuldade = DificuldadeInstalacao.Media
			};

			// Act
			dadosInstalacao.AtualizarPrecoInstalacao();

			// Assert
			var precoEsperado = modeloPainel.Preco * 0.3m * 1.2m * dadosInstalacao.NumeroPaineis;
			Assert.Equal(precoEsperado, dadosInstalacao.PrecoInstalacao);
		}

		[Fact]
		public void CalcularPrecoPorInclinacao_DeveRetornarValorCorreto()
		{
			// Arrange
			var dadosInstalacao = new DadosInstalacao
			{
				Inclinacao = 30
			};

			// Act
			var precoPorInclinacao = dadosInstalacao.CalcularPrecoPorInclinacao();

			// Assert
			Assert.Equal(0.3m, precoPorInclinacao);
		}

		[Fact]
		public void CalcularPrecoPorDificuldade_DeveRetornarValorCorreto()
		{
			// Arrange
			var dadosInstalacao = new DadosInstalacao
			{
				Dificuldade = DificuldadeInstalacao.Media
			};

			// Act
			var precoPorDificuldade = dadosInstalacao.CalcularPrecoPorDificuldade();

			// Assert
			Assert.Equal(1.2m, precoPorDificuldade);
		}

		[Fact]
		public void AtualizarPrecoInstalacao_DeveAtualizarPrecoCorretamente()
		{
			// Arrange
			var modeloPainel = new ModeloPainelSolar
			{
				Id = 1,
				ModeloNome = "Painel Solar X",
				Preco = 1500.00m 
			};

			var dadosInstalacao = new DadosInstalacao
			{
				ModeloPainel = modeloPainel,
				NumeroPaineis = 5,
				Inclinacao = 45, 
				Dificuldade = DificuldadeInstalacao.Facil
			};

			// Act
			dadosInstalacao.AtualizarPrecoInstalacao();

			// Assert
			var precoEsperado = modeloPainel.Preco * 0.5m * 1.0m * dadosInstalacao.NumeroPaineis;
			Assert.Equal(precoEsperado, dadosInstalacao.PrecoInstalacao);
		}

		[Fact]
		public void CalcularPrecoInstalacao_DeveLancarExcecaoQuandoModeloPainelForNulo()
		{
			// Arrange
			var dadosInstalacao = new DadosInstalacao
			{
				ModeloPainel = null, // Modelo de painel nulo
				NumeroPaineis = 5,
				Inclinacao = 30,
				Dificuldade = DificuldadeInstalacao.Media
			};

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => dadosInstalacao.CalcularPrecoInstalacao());
		}

		[Theory]
		[InlineData(0, 0.3)]
		[InlineData(30, 0.3)]
		[InlineData(45, 0.5)]
		[InlineData(75, 0.7)]
		public void CalcularPrecoPorInclinacao_DeveRetornarValoresCorretosParaVariosAngulos(decimal inclinacao, decimal valorEsperado)
		{
			// Arrange
			var dadosInstalacao = new DadosInstalacao
			{
				Inclinacao = inclinacao
			};

			// Act
			var precoPorInclinacao = dadosInstalacao.CalcularPrecoPorInclinacao();

			// Assert
			Assert.Equal(valorEsperado, precoPorInclinacao);
		}
	}
}
