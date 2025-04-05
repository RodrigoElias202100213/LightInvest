using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LightInvest.Controllers.Educ;
using LightInvest.Models.BD;
using LightInvest.Models.Educ.Artigos;
using LightInvest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LightInvestTest
{
    public class ArtigosUnitTesting
    {
        private readonly ArtigosController _controller;
        private readonly Mock<ApplicationDbContext> _contextMock;
        private readonly Mock<MediaStackService> _mediaStackServiceMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<ISession> _sessionMock;

        public ArtigosUnitTesting()
        {
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "LightInvestTestDB")
                .Options;
            var context = new ApplicationDbContext(options);
            _contextMock = new Mock<ApplicationDbContext>(options);

            _mediaStackServiceMock = new Mock<MediaStackService>(new HttpClient());

            _httpContextMock = new Mock<HttpContext>();
            _sessionMock = new Mock<ISession>();

            _httpContextMock.Setup(ctx => ctx.Session).Returns(_sessionMock.Object);

           
            _controller = new ArtigosController(context, _mediaStackServiceMock.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };
        }

        private static byte[] SerializeToBytes(string value)
        {
            return value == null ? null : System.Text.Encoding.UTF8.GetBytes(value);
        }

        [Fact]
        public void Index_SemUtilizadorRegistado_DeveRedirecionarParaLogin()
        {
            var sessionMock = new Mock<ISession>();

            byte[] value = null;
            sessionMock.Setup(s => s.TryGetValue("UserName", out value)).Returns(false);

            _httpContextMock.Setup(ctx => ctx.Session).Returns(sessionMock.Object);

            var result = _controller.Index();
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Login", redirectResult.ActionName);
            Assert.Equal("Account", redirectResult.ControllerName);
        }


        [Fact]
        public async Task Detalhes_ArtigoNaoExiste_DeveRetornarNotFound()
        {
            var result = await _controller.Detalhes(999);
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task AdicionarComentario_SemUtilizadorRegistado_DeveRedirecionarParaLogin()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            var artigo = new Artigo
            {
                ArtigoId = 1,
                Titulo = "Artigo Teste",
                Conteudo = "Conteúdo de teste",
                Categoria = "Categoria Teste",
                DescricaoCurta = "Descrição curta",
                DataPublicacao = DateTime.UtcNow,
                ImagemUrl = "https://example.com/imagem.jpg"
            };
            context.Artigos.Add(artigo);
            await context.SaveChangesAsync(); 
            var mediaStackServiceMock = new Mock<MediaStackService>(new HttpClient());

            var controller = new ArtigosController(context, mediaStackServiceMock.Object);

            var httpContextMock = new DefaultHttpContext();
            httpContextMock.Session = new FakeSession();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock
            };

            var result = await controller.AdicionarComentario(1, "Comentário de teste");

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
            Assert.Equal("Account", redirectResult.ControllerName);
        }

    }
}
