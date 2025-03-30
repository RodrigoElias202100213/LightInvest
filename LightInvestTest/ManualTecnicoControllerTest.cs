using LightInvest.Controllers.Educ;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LightInvestTest
{
    public class ManualTecnicoControllerTest
    {
        private readonly ManualTecnicoController _controller;

        public ManualTecnicoControllerTest()
        {
            _controller = new ManualTecnicoController();
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var result = _controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}
