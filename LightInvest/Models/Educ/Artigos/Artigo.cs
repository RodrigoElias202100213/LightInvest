using System;
using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Educ.Artigos
{
	public class Artigo
	{
		[Key]
		public int ArtigoId { get; set; }

		[Required]
		[MaxLength(200)]
		public string Titulo { get; set; }

		[Required]
		public string Conteudo { get; set; }

		public string ImagemUrl { get; set; }

		public string Categoria { get; set; }
		[MaxLength(500)]
		public string DescricaoCurta { get; set; }

		public DateTime DataPublicacao { get; set; } = DateTime.UtcNow;
		
		public List<Artigo> ArtigosRelacionados { get; set; }
		
		public List<Comentario> Comentarios { get; set; } = new List<Comentario>();

	}
}
