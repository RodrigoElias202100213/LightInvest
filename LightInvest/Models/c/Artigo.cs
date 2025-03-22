using System;
using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public class Artigo
	{
		[Key]
		public int Id { get; set; }

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
	}
}
