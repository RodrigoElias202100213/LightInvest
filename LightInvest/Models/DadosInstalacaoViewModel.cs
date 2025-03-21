using LightInvest.Models.b;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public class DadosInstalacaoViewModel
	{
		[Required(ErrorMessage = "A cidade é obrigatória.")]
		public int CidadeId { get; set; }

		[Required(ErrorMessage = "O modelo do painel solar é obrigatório.")]
		public int ModeloPainelId { get; set; }

		[Required(ErrorMessage = "A potência do painel solar é obrigatória.")]
<<<<<<< HEAD
		public int PotenciaId { get; set; } 
=======
		public int PotenciaId { get; set; }
>>>>>>> backup
		[Required]
		[Range(1, 1000, ErrorMessage = "O número de painéis deve estar entre 1 e 1000.")]
		public int NumeroPaineis { get; set; }

		[Required]
		[Range(0, 90, ErrorMessage = "A inclinação deve estar entre 0 e 90 graus.")]
		public decimal Inclinacao { get; set; }

		[Required(ErrorMessage = "A dificuldade de instalação é obrigatória.")]
		public DificuldadeInstalacao? Dificuldade { get; set; } = null;
<<<<<<< HEAD
		
=======

>>>>>>> backup
		public decimal PrecoInstalacao { get; set; }

		public List<Cidade> Cidades { get; set; } = new();
		public List<ModeloPainelSolar> ModelosDePaineis { get; set; } = new();
		public List<PotenciaPainelSolar> Potencias { get; set; } = new();
	}
}