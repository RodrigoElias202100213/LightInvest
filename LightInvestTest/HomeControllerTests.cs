using LightInvest.Controllers.Auth;
using LightInvest.Models;
using LightInvest.Models.BD;
using LightInvest.Models.Utilizador.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class HomeControllerTests
{
	private readonly ApplicationDbContext _context;
	private readonly Mock<ILogger<HomeController>> _mockLogger;
	private readonly Mock<HttpContext> _mockHttpContext;
	private readonly Mock<ISession> _mockSession;
	private readonly HomeController _controller;
	private readonly Dictionary<string, byte[]> _sessionStorage;

	public HomeControllerTests()
	{
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "TestDb")
			.Options;
		_context = new ApplicationDbContext(options);
		_context.Database.EnsureDeleted(); // Limpa os dados antes de cada teste
		_context.Database.EnsureCreated();

		_mockLogger = new Mock<ILogger<HomeController>>();
		_mockHttpContext = new Mock<HttpContext>();
		_mockSession = new Mock<ISession>();
		_sessionStorage = new Dictionary<string, byte[]>();

		_mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
			.Callback<string, byte[]>((key, value) => _sessionStorage[key] = value);
		_mockSession.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
			.Returns((string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value));

		_mockHttpContext.Setup(x => x.Session).Returns(_mockSession.Object);

		_controller = new HomeController(_context, _mockLogger.Object);
		_controller.ControllerContext = new ControllerContext { HttpContext = _mockHttpContext.Object };
	}

	[Fact]
	public async Task UserList_AdminAccess_ReturnsViewWithUsers()
	{
		// Arrange
		_sessionStorage["IsAdmin"] = Encoding.UTF8.GetBytes("True");

		_context.Users.Add(new User { Name = "Admin", Email = "admin@test.com", Password = "password" });
		await _context.SaveChangesAsync();

		// Act
		var result = await _controller.UserList();

		// Assert
		var viewResult = Assert.IsType<ViewResult>(result);
		Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);
	}

	[Fact]
	public async Task UserList_NonAdminAccess_RedirectsToIndex()
	{
		// Arrange
		_sessionStorage["IsAdmin"] = Encoding.UTF8.GetBytes("False");

		// Act
		var result = await _controller.UserList();

		// Assert
		var redirectResult = Assert.IsType<RedirectToActionResult>(result);
		Assert.Equal("Index", redirectResult.ActionName);
	}

	[Fact]
	public async Task MakeAdmin_ValidUserId_UserBecomesAdmin()
	{
		// Arrange
		var user = new User { Name = "User", Email = "user@test.com", Password = "password", IsAdmin = false };
		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		// Act
		var result = await _controller.MakeAdmin(user.Id);

		// Assert
		var updatedUser = await _context.Users.FindAsync(user.Id);
		Assert.True(updatedUser.IsAdmin);
		var redirectResult = Assert.IsType<RedirectToActionResult>(result);
		Assert.Equal("UserList", redirectResult.ActionName);
	}
}