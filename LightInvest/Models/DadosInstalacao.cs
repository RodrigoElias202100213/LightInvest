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

		public int PotenciaId { get; set; }

		[ForeignKey("PotenciaId")]
		public PotenciaPainelSolar Potencia { get; set; }

		public int NumeroPaineis { get; set; }

		// A potência do painel é calculada diretamente a partir da Potencia
		public decimal PotenciaDoPainel => Potencia?.Potencia ?? 0;

		public decimal Inclinacao { get; set; }

		[Required]
		public string Dificuldade { get; set; }

		private decimal _precoInstalacao;

		public decimal PrecoInstalacao
		{
			get => _precoInstalacao;
			set => _precoInstalacao = value;
		}

		public void AtualizarPrecoInstalacao()
		{
			_precoInstalacao = CalcularPrecoInstalacao();
		}

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
				_ => 1.0m
			};
		}

		public decimal CalcularPrecoInstalacao()
		{
			// Agora, o preço base vem do preço do modelo de painel solar
			decimal precoBase = ModeloPainel.Preco;
			return precoBase * CalcularPrecoPorInclinacao() * CalcularPrecoPorDificuldade() * NumeroPaineis;
		}
	}
}
