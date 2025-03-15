using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public enum TipoTarifa
	{
		Residencial,
		Comercial,
		Industrial
	}

	public class Tarifa
	{
		[Key]
		public int Id { get; set; }
		public string UserEmail { get; set; }
		public DateTime DataAlteracao { get; set; }  // Data da alteração
		public TipoTarifa Nome { get; set; } // Tipo de tarifa: Residencial, Comercial, Industrial
		public decimal PrecoKwh { get; set; }

		private static readonly Dictionary<TipoTarifa, decimal> Adicionais = new()
		{
			{ TipoTarifa.Residencial, 0.0m },
			{ TipoTarifa.Comercial, 0.15m },
			{ TipoTarifa.Industrial, 0.12m }
		};

		public decimal GetPrecoKwh() => PrecoKwh + (Adicionais.TryGetValue(Nome, out var adicional) ? adicional : 0);
	}
}