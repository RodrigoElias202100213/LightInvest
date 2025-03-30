namespace LightInvest.Models.Roi
{
	/// <summary>
	/// Represents the view model for the ROI calculator dashboard, containing the current ROI calculation and the history of previous calculations.
	/// </summary>
	public class RoiCalculatorDashboardViewModel
	{
		/// <summary>
		/// Gets or sets the current ROI calculation.
		/// </summary>
		/// <value>
		/// An instance of the <see cref="RoiCalculator"/> class representing the current ROI calculation.
		/// </value>
		public RoiCalculator CurrentRoi { get; set; }

		/// <summary>
		/// Gets or sets the history of previous ROI calculations.
		/// </summary>
		/// <value>
		/// A list of <see cref="RoiCalculator"/> instances representing the history of ROI calculations made by the user.
		/// </value>
		public List<RoiCalculator> History { get; set; }
	}
}
