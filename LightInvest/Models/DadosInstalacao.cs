using LightInvest.Models.b;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models
{
	public enum DificuldadeInstalacao
	{
		Facil,
		Media,
		Dificil
	}

	public class DadosInstalacao
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[EmailAddress] 
		public string UserEmail { get; set; }

		[Required]
		public int CidadeId { get; set; }

		[ForeignKey(nameof(CidadeId))]
		public virtual Cidade Cidade { get; set; }

		[Required]
		public int ModeloPainelId { get; set; }

		[ForeignKey(nameof(ModeloPainelId))]
		public virtual ModeloPainelSolar ModeloPainel { get; set; }

		[Required]
		public int PotenciaId { get; set; }

		[ForeignKey(nameof(PotenciaId))]
		public virtual PotenciaPainelSolar Potencia { get; set; }

		[Range(1, 1000)]
		public int NumeroPaineis { get; set; }

		[Range(0, 90)] 
		public decimal Inclinacao { get; set; }

		[Required]
		public DificuldadeInstalacao Dificuldade { get; set; }


		public decimal PrecoInstalacao { get; set; }

		public void AtualizarPrecoInstalacao()
		{
			PrecoInstalacao = CalcularPrecoInstalacao();
		}

		public decimal CalcularPrecoPorInclinacao()
		{
			if (Inclinacao <= 35)
				return 0.3m;
			if (Inclinacao <= 60)
				return 0.5m;
			return 0.7m;
		}

		public decimal CalcularPrecoPorDificuldade()
		{
			return Dificuldade switch
			{
				DificuldadeInstalacao.Facil => 1.0m,
				DificuldadeInstalacao.Media => 1.2m,
				DificuldadeInstalacao.Dificil => 1.5m,
				_ => 1.0m
			};
		}

		public decimal CalcularPrecoInstalacao()
		{
			if (ModeloPainel == null)
			{
				throw new InvalidOperationException("O modelo do painel não foi carregado corretamente.");
			}

			decimal precoBase = ModeloPainel.Preco;
			decimal fatorInclinacao = CalcularPrecoPorInclinacao();
			decimal fatorDificuldade = CalcularPrecoPorDificuldade();

			return precoBase * fatorInclinacao * fatorDificuldade * NumeroPaineis;
		}

	}
}
