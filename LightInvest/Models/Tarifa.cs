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
		public TipoTarifa Nome { get; set; }  // Tipo de tarifa: Residencial, Comercial, Industrial
		public decimal PrecoKwh { get; set; }

		public decimal GetPrecoKwh()
		{
			var adicionais = new Dictionary<TipoTarifa, decimal>
			{
				{ TipoTarifa.Residencial, 0.0m },
				{ TipoTarifa.Comercial, 0.15m },
				{ TipoTarifa.Industrial, 0.12m }
			};

			return PrecoKwh + adicionais[Nome];
		}

	}
}
