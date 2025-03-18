using System;
using System.Linq;
using LightInvest.Data;
using LightInvest.Models;
using LightInvest.Models.b;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LightInvest.Tests
{
	public class DadosInstalacaoTests
	{
		private DbContextOptions<ApplicationDbContext> _options;

		public DadosInstalacaoTests()
		{
			_options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "LightInvestTest")
				.Options;
		}

		[Fact]
		public void CalcularPrecoInstalacao_DeveCalcularPrecoCorretamente()
		{
			// Arrange
			using (var context = new ApplicationDbContext(_options))
			{
				var cidade = new Cidade { Nome = "Cidade Exemplo" };

				var modeloPainel = new ModeloPainelSolar
				{
					Modelo = "Modelo X",
					Preco = 1000.00m
				};

				var potencia = new PotenciaPainelSolar
				{
					Potencia = 250.00m,
					PainelSolar = modeloPainel
				};

				var dadosInstalacao = new DadosInstalacao
				{
					UserEmail = "usuario@exemplo.com",
					Cidade = cidade,
					ModeloPainel = modeloPainel,
					Potencia = potencia,
					NumeroPaineis = 10,
					Inclinacao = 45,
					Dificuldade = "média"
				};

				context.Cidades.Add(cidade);
				context.ModelosDePaineisSolares.Add(modeloPainel);
				context.PotenciasDePaineisSolares.Add(potencia);
				context.DadosInstalacao.Add(dadosInstalacao);
				context.SaveChanges();

				// Act
				dadosInstalacao.AtualizarPrecoInstalacao();

				// Assert
				Assert.Equal(1000.00m * 0.5m * 1.2m * 10, dadosInstalacao.PrecoInstalacao);
			}
		}

		[Fact]
		public void AtualizarPrecoInstalacao_DeveAtualizarPrecoCorretamente()
		{
			// Arrange
			using (var context = new ApplicationDbContext(_options))
			{
				var cidade = new Cidade { Nome = "Cidade Teste" };
				var modeloPainel = new ModeloPainelSolar { Modelo = "Painel A", Preco = 1500.00m };
				var potencia = new PotenciaPainelSolar { Potencia = 300.00m, PainelSolar = modeloPainel };

				var dadosInstalacao = new DadosInstalacao
				{
					UserEmail = "test@teste.com",
					Cidade = cidade,
					ModeloPainel = modeloPainel,
					Potencia = potencia,
					NumeroPaineis = 5,
					Inclinacao = 60,
					Dificuldade = "difícil"
				};

				context.Cidades.Add(cidade);
				context.ModelosDePaineisSolares.Add(modeloPainel);
				context.PotenciasDePaineisSolares.Add(potencia);
				context.DadosInstalacao.Add(dadosInstalacao);
				context.SaveChanges();

				// Act
				dadosInstalacao.AtualizarPrecoInstalacao();

				// Assert
				Assert.Equal(5625.00m, dadosInstalacao.PrecoInstalacao);
			}
		}

		[Fact]
		public void PotenciaDoPainel_DeveRetornarPotenciaCorretamente()
		{
			// Arrange
			using (var context = new ApplicationDbContext(_options))
			{
				var cidade = new Cidade { Nome = "Cidade Teste" };
				var modeloPainel = new ModeloPainelSolar { Modelo = "Painel X", Preco = 1200.00m };
				var potencia = new PotenciaPainelSolar { Potencia = 200.00m, PainelSolar = modeloPainel };

				var dadosInstalacao = new DadosInstalacao
				{
					UserEmail = "user@teste.com",
					Cidade = cidade,
					ModeloPainel = modeloPainel,
					Potencia = potencia,
					NumeroPaineis = 5,
					Inclinacao = 40,
					Dificuldade = "fácil"
				};

				context.Cidades.Add(cidade);
				context.ModelosDePaineisSolares.Add(modeloPainel);
				context.PotenciasDePaineisSolares.Add(potencia);
				context.DadosInstalacao.Add(dadosInstalacao);
				context.SaveChanges();

				// Act
				var potenciaPainel = dadosInstalacao.PotenciaDoPainel;

				// Assert
				Assert.Equal(200.00m, potenciaPainel);
			}
		}
	}
}
