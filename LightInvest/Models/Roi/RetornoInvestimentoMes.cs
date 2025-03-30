namespace LightInvest.Models.Roi
{
	/// <summary>
	/// Represents the return on investment (ROI) data for a specific month, including generated energy, monthly savings, and remaining balance.
	/// </summary>
	public class RetornoInvestimentoMes
	{
		/// <summary>
		/// Gets or sets the name of the month (e.g., "January", "February").
		/// </summary>
		/// <value>
		/// A string representing the month of the ROI data.
		/// </value>
		public string Mes { get; set; }

		/// <summary>
		/// Gets or sets the total energy generated during the month.
		/// </summary>
		/// <value>
		/// A decimal representing the amount of energy generated (in kWh, for example) during the month.
		/// </value>
		public decimal EnergiaGerada { get; set; }

		/// <summary>
		/// Gets or sets the monthly savings achieved through the investment.
		/// </summary>
		/// <value>
		/// A decimal representing the savings for the month (in the local currency).
		/// </value>
		public decimal EconomiaMensal { get; set; }

		/// <summary>
		/// Gets or sets the remaining balance after the monthly savings have been accounted for.
		/// </summary>
		/// <value>
		/// A decimal representing the remaining balance after considering the energy savings and other factors (e.g., investment cost).
		/// </value>
		public decimal SaldoRestante { get; set; }
	}
}
