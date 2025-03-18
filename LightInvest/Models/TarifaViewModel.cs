namespace LightInvest.Models
{
	public class TarifaViewModel
	{
		public TipoTarifa TipoDeTarifaEscolhida { get; set; }

		public decimal PrecoKWh { get; set; }

		public List<string> TiposDeTarifa { get; set; } = Enum.GetNames(typeof(TipoTarifa)).ToList();

		public TarifaViewModel()
		{
			TiposDeTarifa = Enum.GetNames(typeof(TipoTarifa)).ToList();
		}

		public TarifaViewModel(Tarifa tarifa)
		{
			TipoDeTarifaEscolhida = tarifa.Tipo;
			PrecoKWh = tarifa.PrecoKWh;
			TiposDeTarifa = Enum.GetNames(typeof(TipoTarifa)).ToList();
		}
	}
}