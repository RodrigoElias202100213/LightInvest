using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Simulacao.Energ
{
	/// <summary>
	/// Represents the view model for energy consumption data input, including daily, weekly, and monthly consumption.
	/// </summary>
	public class EnergyConsumptionViewModel
	{
		/// <summary>
		/// Gets or sets the daily energy consumption during the week (Monday to Friday).
		/// </summary>
		/// <value>
		/// A list of 24 decimal values representing the energy consumption per hour of the day during the weekdays.
		/// </value>
		[Required(ErrorMessage = "Os valores de consumo durante a semana são obrigatórios.")]
		[MinLength(24, ErrorMessage = "Deve conter 24 valores para cada hora do dia.")]
		public List<decimal> ConsumoDiaSemana { get; set; }

		/// <summary>
		/// Gets or sets the daily energy consumption during the weekend (Saturday and Sunday).
		/// </summary>
		/// <value>
		/// A list of 24 decimal values representing the energy consumption per hour of the day during the weekends.
		/// </value>
		[Required(ErrorMessage = "Os valores de consumo no fim de semana são obrigatórios.")]
		[MinLength(24, ErrorMessage = "Deve conter 24 valores para cada hora do dia.")]
		public List<decimal> ConsumoFimSemana { get; set; }

		/// <summary>
		/// Gets or sets the list of months the user occupies the premises.
		/// </summary>
		/// <value>
		/// A list of strings representing the months of the year in which the premises are occupied.
		/// </value>
		[Required(ErrorMessage = "Selecione ao menos um mês de ocupação.")]
		public List<string> MesesOcupacao { get; set; }

		/// <summary>
		/// Gets or sets the average weekly energy consumption after applying discounts.
		/// </summary>
		/// <value>
		/// A decimal representing the average weekly energy consumption.
		/// </value>
		public decimal MediaSemana { get; set; }

		/// <summary>
		/// Gets or sets the average weekend energy consumption after applying discounts.
		/// </summary>
		/// <value>
		/// A decimal representing the average weekend energy consumption.
		/// </value>
		public decimal MediaFimSemana { get; set; }

		/// <summary>
		/// Gets or sets the average annual energy consumption.
		/// </summary>
		/// <value>
		/// A decimal representing the average annual energy consumption.
		/// </value>
		public decimal MediaAnual { get; set; }

		/// <summary>
		/// Gets or sets the total energy consumption for the selected months.
		/// </summary>
		/// <value>
		/// A decimal representing the total energy consumption for the selected months.
		/// </value>
		public decimal ConsumoTotal { get; set; }
	}
}
