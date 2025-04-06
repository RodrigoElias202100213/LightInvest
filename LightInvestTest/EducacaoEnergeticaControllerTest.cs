using LightInvest.Controllers.Educ;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace LightInvestTest
{
    public class EducacaoEnergeticaControllerTest
    {
		[Fact]
		public void Index_WithoutSession_RedirectsToLogin()
		{
			// Arrange
			var controller = new EducacaoEnergeticaController();

			// Mock ISession
			var sessionMock = new Mock<ISession>();

			byte[] userNameBytes;
			sessionMock.Setup(s => s.TryGetValue("UserName", out userNameBytes))
					   .Returns(false);

			var contextMock = new Mock<HttpContext>();
			contextMock.Setup(c => c.Session).Returns(sessionMock.Object);

			controller.ControllerContext = new ControllerContext
			{
				HttpContext = contextMock.Object
			};

			// Act
			var result = controller.Index();

			// Assert
			var redirect = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Login", redirect.ActionName);
			Assert.Equal("Account", redirect.ControllerName);
		}

		[Fact]
		public void Index_WithValidSession_ReturnsViewResult()
		{
			// Arrange
			var controller = new EducacaoEnergeticaController();

			var sessionMock = new Mock<ISession>();

			var userNameBytes = System.Text.Encoding.UTF8.GetBytes("TestUser");
			var isAdminBytes = System.Text.Encoding.UTF8.GetBytes("True");

			sessionMock.Setup(s => s.TryGetValue("UserName", out userNameBytes))
					   .Returns(true);
			sessionMock.Setup(s => s.TryGetValue("IsAdmin", out isAdminBytes))
					   .Returns(true);

			var contextMock = new Mock<HttpContext>();
			contextMock.Setup(c => c.Session).Returns(sessionMock.Object);

			controller.ControllerContext = new ControllerContext
			{
				HttpContext = contextMock.Object
			};

			// Act
			var result = controller.Index();

			// Assert
			var view = Assert.IsType<ViewResult>(result);
		}

	}
}
