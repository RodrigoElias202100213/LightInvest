using LightInvest.Controllers.Educ;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LightInvestTest
{
    public class EducacaoEnergeticaControllerTest
    {
        private readonly EducacaoEnergeticaController _controller;

        public EducacaoEnergeticaControllerTest()
        {
            _controller = new EducacaoEnergeticaController();
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var result = _controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}
