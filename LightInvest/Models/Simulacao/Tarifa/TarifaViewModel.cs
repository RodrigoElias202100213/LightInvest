using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Simulacao.Tarifa
{
	/// <summary>
	/// Represents the view model for a tariff, containing the tariff type and price per kWh.
	/// </summary>
	public class TarifaViewModel
	{
		/// <summary>
		/// Gets or sets the selected tariff type.
		/// </summary>
		/// <value>
		/// The type of the tariff chosen by the user.
		/// </value>
		[Required(ErrorMessage = "Selecione um tipo de tarifa.")]
		public TipoTarifa? TipoDeTarifaEscolhida { get; set; }

		/// <summary>
		/// Gets or sets the price per kilowatt-hour (kWh).
		/// </summary>
		/// <value>
		/// The price per kWh.
		/// </value>
		[Required(ErrorMessage = "Informe o preço por kWh.")]
		[Range(0.01, double.MaxValue, ErrorMessage = "O preço por kWh deve ser maior que zero.")]
		public decimal PrecoKWh { get; set; }

		/// <summary>
		/// Gets a list of available tariff types.
		/// </summary>
		/// <value>
		/// A list of tariff types as string representations.
		/// </value>
		public List<string> TiposDeTarifa { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TarifaViewModel"/> class.
		/// </summary>
		public TarifaViewModel()
		{
			TiposDeTarifa = Enum.GetNames(typeof(TipoTarifa)).ToList();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TarifaViewModel"/> class with the specified tariff.
		/// </summary>
		/// <param name="tarifa">The tariff object to initialize the view model with.</param>
		public TarifaViewModel(Tarifa tarifa) : this()
		{
			TipoDeTarifaEscolhida = tarifa.Tipo;
			PrecoKWh = tarifa.PrecoKWh;
		}
	}
	/// <summary>
	/// Represents the result of a tariff calculation, including total consumption and monthly costs.
	/// </summary>
	public class ResultadoTarifaViewModel
	{
		/// <summary>
		/// Gets or sets the total energy consumption.
		/// </summary>
		/// <value>
		/// A decimal representing the total energy consumption.
		/// </value>
		public decimal ConsumoTotal { get; set; }

		/// <summary>
		/// Gets or sets the monthly energy consumption details.
		/// </summary>
		/// <value>
		/// A list of <see cref="MesConsumo"/> representing monthly energy consumption and costs.
		/// </value>
		public List<MesConsumo> ConsumoMensal { get; set; }

		/// <summary>
		/// Gets or sets the total annual cost based on the consumption.
		/// </summary>
		/// <value>
		/// A decimal representing the total annual cost.
		/// </value>
		public decimal ValorAnual { get; set; }

		/// <summary>
		/// Gets or sets the chosen tariff type as a string.
		/// </summary>
		/// <value>
		/// A string representing the selected tariff type.
		/// </value>
		public string TarifaEscolhida { get; set; }

		/// <summary>
		/// Gets or sets the price per kilowatt-hour (kWh) for the selected tariff.
		/// </summary>
		/// <value>
		/// The price per kWh.
		/// </value>
		public decimal PrecoKwh { get; set; }

		/// <summary>
		/// Gets or sets the list of months for which the user selected occupation.
		/// </summary>
		/// <value>
		/// A list of strings representing the selected months.
		/// </value>
		public List<string> MesesOcupacao { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ResultadoTarifaViewModel"/> class.
		/// </summary>
		public ResultadoTarifaViewModel()
		{
			MesesOcupacao = new List<string>();
			ConsumoMensal = new List<MesConsumo>();
		}

		/// <summary>
		/// Updates the price per kWh based on the selected tariff type and base price.
		/// </summary>
		/// <param name="tipoTarifa">The selected tariff type.</param>
		/// <param name="precoBase">The base price per kWh.</param>
		public void AtualizarPrecoKwh(TipoTarifa tipoTarifa, decimal precoBase)
		{
			var tarifa = new Tarifa { Tipo = tipoTarifa, PrecoKWh = precoBase };
			PrecoKwh = tarifa.PrecoKWh;
		}
	}
	/// <summary>
	/// Represents monthly consumption details, including the month, consumption, and cost.
	/// </summary>
	public class MesConsumo
	{
		/// <summary>
		/// Gets or sets the month name.
		/// </summary>
		/// <value>
		/// A string representing the month.
		/// </value>
		public string Mes { get; set; }

		/// <summary>
		/// Gets or sets the energy consumption for the month.
		/// </summary>
		/// <value>
		/// A decimal representing the energy consumption.
		/// </value>
		public decimal Consumo { get; set; }

		/// <summary>
		/// Gets or sets the total cost for the energy consumption in the month.
		/// </summary>
		/// <value>
		/// A decimal representing the cost of consumption.
		/// </value>
		public decimal Custo { get; set; }
	}

	/// <summary>
	/// Represents the months of the year.
	/// </summary>
	public enum Meses
	{
		/// <summary>
		/// January
		/// </summary>
		Janeiro,

		/// <summary>
		/// February
		/// </summary>
		Fevereiro,

		/// <summary>
		/// March
		/// </summary>
		Março,

		/// <summary>
		/// April
		/// </summary>
		Abril,

		/// <summary>
		/// May
		/// </summary>
		Maio,

		/// <summary>
		/// June
		/// </summary>
		Junho,

		/// <summary>
		/// July
		/// </summary>
		Julho,

		/// <summary>
		/// August
		/// </summary>
		Agosto,

		/// <summary>
		/// September
		/// </summary>
		Setembro,

		/// <summary>
		/// October
		/// </summary>
		Outubro,

		/// <summary>
		/// November
		/// </summary>
		Novembro,

		/// <summary>
		/// December
		/// </summary>
		Dezembro
	}
}