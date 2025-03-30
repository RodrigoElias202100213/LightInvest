namespace LightInvest.Models.Simulacao.Tarifa
{
	/// <summary>
	/// Represents the view model for selecting a tariff in the energy simulation.
	/// </summary>
	public class SelecionarTarifaViewModel
	{
		/// <summary>
		/// Gets or sets the selected tariff type.
		/// </summary>
		/// <value>
		/// A string representing the chosen tariff type.
		/// </value>
		public string TarifaEscolhida { get; set; }

		/// <summary>
		/// Gets or sets the price per kilowatt-hour (kWh) for the selected tariff.
		/// </summary>
		/// <value>
		/// A decimal representing the price per kWh.
		/// </value>
		public decimal PrecoKwh { get; set; }

		/// <summary>
		/// Gets the list of available tariff types.
		/// </summary>
		/// <value>
		/// A list of strings representing the different tariff types available, derived from the <see cref="TipoTarifa"/> enum.
		/// </value>
		public List<string> TiposDeTarifa { get; set; } = Enum.GetNames(typeof(TipoTarifa)).ToList();
	}
}
