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
			
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDb")
				.Options;

			_context = new ApplicationDbContext(options);

	
			_controller = new SimulacaoValoresController(_context);

			
			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext()
			};
		}

		[Fact]
		public async Task Simular_DadosInstalacaoNaoEncontrado_RetornaBadRequest()
		{
			
			var userEmail = "test@example.com";

			
			_context.Users.Add(new User
			{
				Email = userEmail,
				Name = "Test User",
				Password = "Senha123"
			});

			
			_context.Tarifas.Add(new Tarifa
			{
				UserEmail = userEmail,
				Tipo = TipoTarifa.Residencial, 
				PrecoKWh = 0.15m
			});

			await _context.SaveChangesAsync();

			
			var httpContext = new DefaultHttpContext();
			httpContext.Session = new MockHttpSession();
			httpContext.Session.SetString("UserEmail", userEmail);

			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = httpContext
			};

		
			var result = await _controller.Simular();

		
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.Equal("Nenhum dado de instalação encontrado para este utilizador.", badRequestResult.Value);
		}
	}
}
