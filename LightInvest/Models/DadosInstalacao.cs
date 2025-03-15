using LightInvest.Models.b;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models
{
	public class DadosInstalacao
	{
		[Key]
		public int Id { get; set; } // Chave primária
		
		public string UserEmail { get; set; }

		public int CidadeId { get; set; } // Relacionamento com a tabela Cidade
		[ForeignKey("CidadeId")]
		public Cidade Cidade { get; set; } // Navegação para a entidade Cidade

		public int ModeloPainelId { get; set; } // Relacionamento com ModeloPainelSolar
		[ForeignKey("ModeloPainelId")]
		public ModeloPainelSolar ModeloPainel { get; set; } // Navegação para a entidade ModeloPainelSolar

		public int NumeroPaineis { get; set; } // Número de painéis solares
		public decimal ConsumoPainel { get; set; } // Consumo por painel (em kWh)
		public decimal Inclinacao { get; set; } // Inclinação da instalação (em graus)
		public string Dificuldade { get; set; } // Dificuldade da instalação (ex: "Fácil", "Média", "Difícil")

		// Método para calcular o preço baseado na inclinação
		public decimal CalcularPrecoPorInclinacao()
		{
			decimal fatorInclinacao = 1.0m;

			// Define o fator de acordo com a inclinação (entre 0 e 30 graus)
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

		// Método para calcular o preço baseado na dificuldade
		public decimal CalcularPrecoPorDificuldade()
		{
			decimal fatorDificuldade = 1.0m;

			// Define o fator de acordo com a dificuldade
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

		// Método para calcular o preço total da instalação
		public decimal CalcularPrecoInstalacao()
		{
			decimal precoBase = 5000.00m; // Valor base da instalação (exemplo)
			decimal precoInclinacao = CalcularPrecoPorInclinacao();
			decimal precoDificuldade = CalcularPrecoPorDificuldade();

			// O preço total é influenciado pela inclinação, dificuldade e número de painéis solares
			decimal precoFinal = precoBase * precoInclinacao * precoDificuldade * NumeroPaineis;

			return precoFinal;
		}
	}
}
