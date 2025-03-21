using System;
using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public class Artigo
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Titulo { get; set; }

		[Required]
		public string Conteudo { get; set; }

		public string ImagemUrl { get; set; }

		public DateTime DataPublicacao { get; set; }
	}
}
