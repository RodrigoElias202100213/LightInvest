using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LightInvest.Controllers.Educ;
using LightInvest.Models;
using LightInvest.Models.BD;
using LightInvest.Models.Educ.Artigos;
using LightInvest.Models.Utilizador.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LightInvestTest
{
    
    public class FakeSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new();

        public IEnumerable<string> Keys => _sessionStorage.Keys;
        public string Id { get; } = Guid.NewGuid().ToString();
        public bool IsAvailable => true;

        public void Clear() => _sessionStorage.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _sessionStorage.Remove(key);

        public void Set(string key, byte[] value) => _sessionStorage[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);
    }

    
    public class ComentarioSecurityTest : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ArtigosController _controller;

        public ComentarioSecurityTest()
        {
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

           
            _context.Users.AddRange(
                new User { Id = 1, Email = "owner@example.com", Name = "Dono", Password = "senha123" },
                new User { Id = 2, Email = "other@example.com", Name = "Outro", Password = "senha123" }
            );

            _context.Artigos.Add(new Artigo
            {
                ArtigoId = 1,
                Titulo = "Artigo de Teste",
                Conteudo = "Conteúdo do artigo para teste",
                Categoria = "Teste",
                DescricaoCurta = "Descrição curta de teste",
                DataPublicacao = DateTime.UtcNow,
                ArtigosRelacionados = new List<Artigo>(),
                ImagemUrl = "https://example.com/image.jpg"
            });

            _context.Comentario.Add(new Comentario
            {
                Id = 1,
                ArtigoId = 1,
                Texto = "Comentário original",
                UserId = 1,
                Autor = "Dono",
                DataCriacao = DateTime.UtcNow
            });
            _context.SaveChanges();

            
            _controller = new ArtigosController(_context, null); 
            var httpContext = new DefaultHttpContext { Session = new FakeSession() };
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        }

        
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

       
        [Fact]
        public async Task AdicionarComentario_DeveRedirecionarParaLogin_SeNaoAutenticado()
        {
            var result = await _controller.AdicionarComentario(1, "Teste de comentário");
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
            Assert.Equal("Account", redirectResult.ControllerName);
        }

        
        [Fact]
        public async Task RemoverComentario_DeveRetornarUnauthorized_SeNaoForProprietarioNemAdmin()
        {
            
            _controller.HttpContext.Session.Set("UserEmail", Encoding.UTF8.GetBytes("other@example.com"));
            _controller.HttpContext.Session.Set("IsAdmin", Encoding.UTF8.GetBytes("False"));

            var result = await _controller.RemoverComentario(1, 1);
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Só consegue eliminar os seus comentários.", unauthorizedResult.Value);
        }

       
        [Fact]
        public async Task EditarComentario_DeveRetornarUnauthorized_SeNaoForProprietario()
        {
            
            _controller.HttpContext.Session.Set("UserEmail", Encoding.UTF8.GetBytes("other@example.com"));

            var result = await _controller.EditarComentario(1, 1);
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Não pode editar este comentário.", unauthorizedResult.Value);
        }
    }
}
