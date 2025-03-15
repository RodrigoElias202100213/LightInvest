using LightInvest.Models;
using LightInvest.Models.b;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models
{
	public class DadosInstalacao
	{
		[Key]
		public int Id { get; set; }

		[Required]
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

		[Required]
		public string Dificuldade { get; set; }

		// Campo privado para armazenar o preço de instalação calculado
		private decimal _precoInstalacao;

		// Propriedade que retorna o preço de instalação calculado
		public decimal PrecoInstalacao
		{
			get => _precoInstalacao;
			set => _precoInstalacao = value; // Não vai mais gerar recursão
		}

		// Método para atualizar o preço de instalação com base nos dados
		public void AtualizarPrecoInstalacao()
		{
			_precoInstalacao = CalcularPrecoInstalacao();
		}

		// Método para calcular o preço com base nos dados da instalação
		public decimal CalcularPrecoPorInclinacao()
		{
			if (Inclinacao <= 35) return 0.3m;
			if (Inclinacao <= 60) return 0.5m;
			return 0.7m;
		}

		private decimal CalcularPrecoPorDificuldade()
		{
			return (Dificuldade?.ToLowerInvariant() ?? "") switch
			{
				"fácil" => 1.0m,
				"média" => 1.2m,
				"difícil" => 1.5m,
				_ => 1.0m // Caso a dificuldade seja inválida, mantém o preço base
			};
		}

		// Método que calcula o preço total de instalação
		public decimal CalcularPrecoInstalacao()
		{
			const decimal precoBase = 5000.00m; // preço base fixo
			return precoBase * CalcularPrecoPorInclinacao() * CalcularPrecoPorDificuldade() * NumeroPaineis;
		}
	}
}
