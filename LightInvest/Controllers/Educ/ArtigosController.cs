using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LightInvest.Models;
using LightInvest.Models.BD;
using Markdig;
using LightInvest.Models.Educ.Artigos;
using Newtonsoft.Json;


namespace LightInvest.Controllers.Educ
{
	public class ArtigosController : Controller
	{
		private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _mediastackApiKey = "73261e3e3f837ec6c829b44371ae2ad7";

        public ArtigosController(ApplicationDbContext context, HttpClient httpClient)
		{
			_context = context;
            _httpClient = httpClient;
        }

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult ListarPorCategoria(string categoria)
		{
			var artigos = _context.Artigos.Where(a => a.Categoria == categoria).ToList();
			ViewBag.Categoria = categoria;
			return View(artigos);
		}

		public async Task<IActionResult> Detalhes(int id)
		{
			var artigo = _context.Artigos.FirstOrDefault(a => a.ArtigoId == id);

			// Obter artigos relacionados pela categoria do artigo atual
			var artigosRelacionados = await ObterArtigosRelacionadosDaApi(artigo.Categoria);

			// Passa os artigos relacionados para a view
			artigo.ArtigosRelacionados = artigosRelacionados;

			return View(artigo);
		}


        private async Task<List<Artigo>> ObterArtigosRelacionadosDaApi(string categoria)
        {

			string url = $"http://api.mediastack.com/v1/news?access_key=73261e3e3f837ec6c829b44371ae2ad7&categories=sustentabilidade&languages=pt";


			var response = await _httpClient.GetStringAsync(url);
            var artigos = JsonConvert.DeserializeObject<MediastackResponse>(response);

          
            return artigos?.Data?.Select(a => new Artigo
            {
                Titulo = a.Title,
                DescricaoCurta = a.Description,
                ImagemUrl = a.ImageUrl, 
                Categoria = categoria,  
            }).ToList();
        }

    }

}
