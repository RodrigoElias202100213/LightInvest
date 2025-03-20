/*
using LightInvest.Data;
using LightInvest.Controllers;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class ApiDependencyTests
{
    private readonly ApplicationDbContext _context;
    private readonly SimulacaoController _controller;

    public ApiDependencyTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new Mock<ISession>().Object;

        var controllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _controller = new SimulacaoController(_context)
        {
            ControllerContext = controllerContext
        };
    }



[Fact]
    public async Task SimulacaoCompleta_ReturnsUnauthorized_IfUserNotLoggedIn()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new Mock<ISession>().Object;
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

        var result = await _controller.SimulacaoCompleta();

        Assert.IsType<UnauthorizedObjectResult>(result);
    }

}
*/