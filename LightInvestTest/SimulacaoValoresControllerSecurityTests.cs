using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using LightInvest.Models.BD;
using LightInvest.Controllers.Simul;
using LightInvest.Models.Utilizador.Login;
using LightInvest.Models.Simulacao.Tarifa;

namespace LightInvestTest
{
	public class SimulacaoValoresControllerSecurityTests
	{
		private readonly ApplicationDbContext _context;
		private readonly SimulacaoValoresController _controller;
		private readonly Mock<ISession> _mockSession;
		private readonly Mock<HttpContext> _mockHttpContext;

		public SimulacaoValoresControllerSecurityTests()
		{
			
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDb")
				.Options;
			_context = new ApplicationDbContext(options);
			SeedDatabase();

		
			_controller = new SimulacaoValoresController(_context);

			
			_mockSession = new Mock<ISession>();
			
			byte[] dummy = null;
			_mockSession.Setup(s => s.TryGetValue("UserEmail", out dummy)).Returns(false);

			_mockHttpContext = new Mock<HttpContext>();
			_mockHttpContext.Setup(ctx => ctx.Session).Returns(_mockSession.Object);

			_controller.ControllerContext = new ControllerContext { HttpContext = _mockHttpContext.Object };
		}

		private void SeedDatabase()
		{
			
			_context.Users.RemoveRange(_context.Users);
			_context.Tarifas.RemoveRange(_context.Tarifas);
			_context.DadosInstalacao.RemoveRange(_context.DadosInstalacao);
			_context.SaveChanges();

			_context.Users.Add(new User
			{
				Email = "test@example.com",
				Name = "Test User",
				Password = "Senha123"
			});
			_context.Tarifas.Add(new Tarifa
			{
				UserEmail = "test@example.com",
				Tipo = TipoTarifa.Residencial,
				PrecoKWh = 0.15m
			});
			_context.SaveChanges();
		}

		[Fact]
		public async Task Simular_ReturnsBadRequest_WhenUserNotAuthenticated()
		{
			var result = await _controller.Simular();

			var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
			Assert.Equal("Utilizador não autenticado.", unauthorizedResult.Value);

		}
	}
}
