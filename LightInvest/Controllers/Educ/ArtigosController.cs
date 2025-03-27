using Microsoft.AspNetCore.Mvc;
using LightInvest.Models.BD;
using LightInvest.Models.Educ.Artigos;
using Newtonsoft.Json;
using Markdig;

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
			if (!Enum.TryParse<CategoriaArtigo>(categoria, out var categoriaEnum))
			{
				return NotFound(); // Retorna erro 404 se a categoria não existir
			}

			var artigos = _context.Artigos
				.Where(a => a.Categoria == categoriaEnum) // Corrigido
				.ToList();

			ViewBag.Categoria = categoriaEnum;
			return View(artigos);
		}

		public async Task<IActionResult> Detalhes(int id)
		{
			var artigo = _context.Artigos.FirstOrDefault(a => a.ArtigoId == id);

			if (artigo == null)
			{
				return NotFound();
			}

			try
			{
				// Converte o conteúdo Markdown para HTML
				var htmlConteudo = Markdown.ToHtml(artigo.Conteudo);

				// Passa o conteúdo HTML para a View
				ViewBag.ConteudoHtml = htmlConteudo;

				// Obter artigos relacionados pela categoria
				var artigosRelacionados = await ObterArtigosRelacionadosDaApi(artigo.Categoria, artigo.DescricaoCurta);
				artigo.ArtigosRelacionados = artigosRelacionados;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Erro ao obter artigos relacionados: {ex.Message}");
				artigo.ArtigosRelacionados = new List<Artigo>();
			}

			return View(artigo);
		}
		private async Task<List<Artigo>> ObterArtigosRelacionadosDaApi(CategoriaArtigo categoria, string descricaoCurta)
		{
			try
			{
				// Converte o enum da categoria para string minúscula para compatibilidade com a API
				string categoriaStr = categoria.ToString().ToLower();

				// Cria uma string de palavras-chave a partir da descrição curta do artigo
				string palavrasChave = GetPalavrasChave(descricaoCurta);

				// Construa a URL para a API, incluindo a categoria e as palavras-chave (descrição curta)
				string url = $"http://api.mediastack.com/v1/news?access_key={_mediastackApiKey}&categories={categoriaStr}&keywords={palavrasChave}&languages=pt";

				var response = await _httpClient.GetAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Erro na API: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
				}

				var responseBody = await response.Content.ReadAsStringAsync();
				var artigos = JsonConvert.DeserializeObject<MediastackResponse>(responseBody);

				// Verifica se há artigos na resposta e retorna os resultados
				if (artigos?.Data == null || !artigos.Data.Any())
				{
					Console.WriteLine($"Nenhum artigo relacionado encontrado para a categoria: {categoriaStr} e palavras-chave: {palavrasChave}");
					return new List<Artigo>();
				}

				return artigos.Data.Select(a => new Artigo
				{
					Titulo = a.Title,
					DescricaoCurta = a.Description,
					Categoria = categoria,
				}).ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Erro ao buscar artigos relacionados: {ex.Message}");
				return new List<Artigo>(); // Retorna uma lista vazia para evitar falhas
			}
		}

		// Função para extrair palavras-chave da descrição curta (se necessário, pode ser aprimorada)
		private string GetPalavrasChave(string descricaoCurta)
		{
			if (string.IsNullOrEmpty(descricaoCurta))
			{
				return string.Empty;
			}

			// Extraímos as primeiras palavras da descrição, isso pode ser personalizado
			var palavras = descricaoCurta.Split(' '); // Divide a descrição em palavras
			var palavrasChave = string.Join(",", palavras.Take(5)); // Usa as primeiras 5 palavras

			return palavrasChave;
		}
	}
}