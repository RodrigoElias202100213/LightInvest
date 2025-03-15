using LightInvest.Models.b;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models
{
	public class DadosInstalacao
	{
		[Key]
		public int Id { get; set; }
		public string UserEmail { get; set; }

		public int CidadeId { get; set; }
		[ForeignKey("CidadeId")]
		public Cidade Cidade { get; set; } 
		public int ModeloPainelId { get; set; } 
		[ForeignKey("ModeloPainelId")]
		public ModeloPainelSolar ModeloPainel { get; set; } 
		
		public int NumeroPaineis { get; set; } 
		
		public decimal ConsumoPainel { get; set; }
		public decimal Inclinacao { get; set; } 
		public string Dificuldade { get; set; } 
		
		public decimal CalcularPrecoPorInclinacao()
		{
			decimal fatorInclinacao = 1.0m;

			if (Inclinacao >= 0 && Inclinacao <= 30)
			{
				fatorInclinacao = 0.3m;
			}
			else if (Inclinacao > 30 && Inclinacao <= 60)
			{
				fatorInclinacao = 0.5m;
			}
			else if (Inclinacao > 60)
			{
				fatorInclinacao = 0.7m;
			}

			return fatorInclinacao;
		}

		public decimal CalcularPrecoPorDificuldade()
		{
			decimal fatorDificuldade = 1.0m;

			switch (Dificuldade.ToLower())
			{
				case "fácil":
					fatorDificuldade = 1.0m; // Preço base
					break;
				case "média":
					fatorDificuldade = 1.2m; // Preço com aumento de 20%
					break;
				case "difícil":
					fatorDificuldade = 1.5m; // Preço com aumento de 50%
					break;
				default:
					throw new ArgumentException("Dificuldade inválida");
			}

			return fatorDificuldade;
		}

		public decimal CalcularPrecoInstalacao()
		{
			decimal precoBase = 5000.00m; // Valor base da instalação (exemplo)
			decimal precoInclinacao = CalcularPrecoPorInclinacao();
			decimal precoDificuldade = CalcularPrecoPorDificuldade();

			decimal precoFinal = precoBase * precoInclinacao * precoDificuldade * NumeroPaineis;

			return precoFinal;
		}
	}
}
