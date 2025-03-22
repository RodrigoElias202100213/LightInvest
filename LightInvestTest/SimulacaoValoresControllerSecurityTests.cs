using System;
using System.Threading.Tasks;
using LightInvest.Controllers.Simul;
using LightInvest.Models.BD;
using LightInvest.Models.Simulacao.Tarifa;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LightInvestTest
{
	public class SimulacaoValoresControllerSecurityTests
	{
		private readonly ApplicationDbContext _context;
		private readonly SimulacaoValoresController _controller;

		public SimulacaoValoresControllerSecurityTests()
		{
			// Configura o banco de dados em memória
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDb")
				.Options;

			_context = new ApplicationDbContext(options);

			// Inicializa o controlador com o contexto real
			_controller = new SimulacaoValoresController(_context);

			// Configura o HttpContext (simula um usuário não autenticado)
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};
		}

		[Fact]
		public async Task Simular_DadosInstalacaoNaoEncontrado_RetornaBadRequest()
		{
			// Arrange
			var userEmail = "test@example.com";

			// Adiciona um usuário válido
			_context.Users.Add(new User
			{
				Email = userEmail,
				Name = "Test User",
				Password = "Senha123"
			});

			// Adiciona uma tarifa válida (Residencial/Comercial/Industrial)
			_context.Tarifas.Add(new Tarifa
			{
				UserEmail = userEmail,
				Tipo = TipoTarifa.Residencial, // Tipo correto do enum
				PrecoKWh = 0.15m
			});

			await _context.SaveChangesAsync();

			// Configura a sessão mockada
			var httpContext = new DefaultHttpContext();
			httpContext.Session = new MockHttpSession();
			httpContext.Session.SetString("UserEmail", userEmail);

			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = httpContext
			};

			// Act
			var result = await _controller.Simular();

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.Equal("Nenhum dado de instalação encontrado para este utilizador.", badRequestResult.Value);
		}
	}
}