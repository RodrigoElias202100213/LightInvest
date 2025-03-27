	using System;
	using System.Collections.Generic;
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

			[Required]
			public CategoriaArtigo Categoria { get; set; } // Agora usa o enum

			[MaxLength(500)]
			public string DescricaoCurta { get; set; }

			public DateTime DataPublicacao { get; set; } = DateTime.UtcNow;

			public List<Artigo> ArtigosRelacionados { get; set; }
		}

		public enum CategoriaArtigo
		{
			Energia_Renovavel,
			Principios_Basicos_painels,
			Calculo_ROI,
			Eficiencia_Energetica,
		}
	}
