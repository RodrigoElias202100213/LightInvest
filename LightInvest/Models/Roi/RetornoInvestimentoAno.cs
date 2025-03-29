namespace LightInvest.Models.Roi
{
	/// <summary>
	/// Represents the return on investment (ROI) for a specific year, including the breakdown of each month.
	/// </summary>
	public class RetornoInvestimentoAno
	{
		/// <summary>
		/// Gets or sets the year of the return on investment data.
		/// </summary>
		/// <value>
		/// An integer representing the year for the ROI data.
		/// </value>
		public int Ano { get; set; }

		/// <summary>
		/// Gets or sets the list of monthly ROI data for the given year.
		/// </summary>
		/// <value>
		/// A list of <see cref="RetornoInvestimentoMes"/> objects, each representing the ROI for a specific month.
		/// </value>
		public List<RetornoInvestimentoMes> Meses { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="RetornoInvestimentoAno"/> class with an empty list of months.
		/// </summary>
		public RetornoInvestimentoAno()
		{
			Meses = new List<RetornoInvestimentoMes>();
		}
	}
}
