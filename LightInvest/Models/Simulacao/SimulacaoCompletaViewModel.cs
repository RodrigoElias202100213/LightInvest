using LightInvest.Models.Roi;
using LightInvest.Models.Simulacao.Energ;
using LightInvest.Models.Simulacao.Tarifa;

namespace LightInvest.Models.Ener
{
	/// <summary>
	/// Represents the complete simulation model for energy consumption, tariff selection, and ROI (Return on Investment) calculation.
	/// </summary>
	public class SimulacaoCompletaViewModel
	{
		/// <summary>
		/// Gets or sets the energy consumption data view model.
		/// </summary>
		/// <value>
		/// The energy consumption data view model containing weekly, weekend consumption, and occupancy months.
		/// </value>
		public EnergyConsumptionViewModel EnergyConsumptionViewModel { get; set; }

		/// <summary>
		/// Gets or sets the tariff data view model.
		/// </summary>
		/// <value>
		/// The tariff data view model that contains tariff type and price per kWh.
		/// </value>
		public TarifaViewModel TarifaViewModel { get; set; }

		/// <summary>
		/// Gets or sets the result of the tariff calculation.
		/// </summary>
		/// <value>
		/// The result view model of the tariff calculation, including total consumption and monthly costs.
		/// </value>
		public ResultadoTarifaViewModel ResultadoTarifaViewModel { get; set; }

		/// <summary>
		/// Gets or sets the installation data view model.
		/// </summary>
		/// <value>
		/// The installation data view model, which contains panel installation information.
		/// </value>
		public DadosInstalacao DadosInstalacao { get; set; }

		/// <summary>
		/// Gets or sets the ROI calculator dashboard view model.
		/// </summary>
		/// <value>
		/// The view model that contains the Return on Investment calculation data.
		/// </value>
		public RoiCalculatorDashboardViewModel ROI { get; set; }

		/// <summary>
		/// Gets or sets the list of investment returns per year.
		/// </summary>
		/// <value>
		/// A list of investment returns calculated for each year of the simulation.
		/// </value>
		public List<RetornoInvestimentoAno> RetornoInvestimentoPorAno { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SimulacaoCompletaViewModel"/> class.
		/// </summary>
		public SimulacaoCompletaViewModel()
		{
			RetornoInvestimentoPorAno = new List<RetornoInvestimentoAno>();
		}
	}
}
