using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LightInvest.Models;
using LightInvest.Models.BD;

public class AccountControllerTests
{
	private readonly ApplicationDbContext _context;
	private readonly AccountController _controller;

	public AccountControllerTests()
	{
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "TestDb")
			.Options;

		_context = new ApplicationDbContext(options);

		_context.Users.Add(new User { Email = "existente@email.com", Name = "Utilizador Existente", Password = "Pass123" });

		_context.Users.Add(new User { Email = "teste@email.com", Name = "Teste User", Password = "Pass123" });

		_context.SaveChanges();

		_controller = new AccountController(_context, new EmailService(null));
	}


	[Fact]
	public async Task Register_DeveRetornarErro_QuandoEmailJaExiste()
	{
		var registerModel = new RegisterViewModel
		{
			Name = "Novo Utilizador",
			Email = "existente@email.com",
			Password = "Pass123",
			ConfirmPassword = "Pass123"
		};

		var result = await _controller.Register(registerModel) as ViewResult;

		Assert.NotNull(result);
		Assert.False(_controller.ModelState.IsValid);
		Assert.Contains(_controller.ModelState.Values, v => v.Errors.Any(e => e.ErrorMessage.Contains("Já existe")));
	}
	[Fact]
	public async Task Login_DeveRetornarErro_QuandoEmailNaoExiste()
	{
		var loginModel = new LoginViewModel
		{
			Email = "inexistente@email.com",
			Password = "Pass123"
		};

		var result = await _controller.Login(loginModel) as ViewResult;

		Assert.NotNull(result);
		Assert.False(_controller.ModelState.IsValid);
		Assert.Contains(_controller.ModelState.Values, v => v.Errors.Any(e => e.ErrorMessage.Contains("Email ou palavra-passe não encontrada.")));
	}


	[Fact]
	public void Login_ReturnsView()
	{
		var ac = new AccountController(null, null);
		var result = ac.Login();
		Assert.IsType<ViewResult>(result);
	}

	[Fact]
	public void Register_ReturnsViewWithRegisterViewModel()
	{
		var ac = new AccountController(null, null);
		var result = ac.Register();
		var viewResult = Assert.IsType<ViewResult>(result);
		Assert.IsType<RegisterViewModel>(viewResult.Model);
	}
}