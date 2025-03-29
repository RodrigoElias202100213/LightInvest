using System;
using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Roi
{
	/// <summary>
	/// Represents a calculator for calculating the Return on Investment (ROI) for a specific user and energy consumption scenario.
	/// </summary>
	public class RoiCalculator
	{
		/// <summary>
		/// Gets or sets the unique identifier for the ROI calculation.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the email address of the user requesting the ROI calculation.
		/// </summary>
		/// <value>
		/// A string representing the user's email address.
		/// </value>
		[Required]
		public string UserEmail { get; set; }

		/// <summary>
		/// Gets or sets the installation cost for the energy solution.
		/// </summary>
		/// <value>
		/// A decimal representing the installation cost (in the local currency).
		/// </value>
		[Required]
		public decimal CustoInstalacao { get; set; }

		/// <summary>
		/// Gets or sets the annual maintenance cost for the energy solution.
		/// </summary>
		/// <value>
		/// A decimal representing the annual maintenance cost (in the local currency).
		/// </value>
		[Required]
		public decimal CustoManutencaoAnual { get; set; }

		/// <summary>
		/// Gets or sets the average energy consumption of the user.
		/// </summary>
		/// <value>
		/// A decimal representing the average energy consumption (in kWh or similar units).
		/// </value>
		[Required]
		public decimal ConsumoEnergeticoMedio { get; set; }

		/// <summary>
		/// Gets or sets the energy consumption from the grid (external electricity source).
		/// </summary>
		/// <value>
		/// A decimal representing the energy consumed from the grid (in kWh or similar units).
		/// </value>
		[Required]
		public decimal ConsumoEnergeticoRede { get; set; }

		/// <summary>
		/// Gets or sets the savings per unit of energy generated or saved.
		/// </summary>
		/// <value>
		/// A decimal representing the savings achieved per unit of energy (in local currency per kWh or similar units).
		/// </value>
		[Required]
		public decimal RetornoEconomia { get; set; }

		/// <summary>
		/// Gets or sets the Return on Investment (ROI) value, calculated from the provided inputs.
		/// </summary>
		/// <value>
		/// A decimal representing the ROI, calculated based on the inputs provided (ratio of installation cost to annual savings).
		/// </value>
		public decimal ROI { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the ROI calculation was performed.
		/// </summary>
		/// <value>
		/// A DateTime representing the date and time of the calculation.
		/// </value>
		public DateTime DataCalculado { get; set; }

		/// <summary>
		/// Calculates the Return on Investment (ROI) based on the provided parameters.
		/// </summary>
		/// <returns>
		/// A decimal representing the calculated ROI, which is the ratio of installation cost to annual savings.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Thrown if any of the required values (CustoInstalacao, RetornoEconomia, ConsumoEnergeticoMedio, or ConsumoEnergeticoRede) are less than or equal to zero.
		/// </exception>
		public decimal CalcularROI()
		{
			if (CustoInstalacao <= 0 || RetornoEconomia <= 0 || ConsumoEnergeticoMedio <= 0 || ConsumoEnergeticoRede <= 0)
				throw new ArgumentException("Todos os valores devem ser positivos e maiores que zero.");

			decimal economiaAnual = (ConsumoEnergeticoRede - ConsumoEnergeticoMedio) * RetornoEconomia - CustoManutencaoAnual;

			ROI = CustoInstalacao / economiaAnual;

			return ROI;
		}
	}
}
