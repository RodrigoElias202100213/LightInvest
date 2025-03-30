using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.Simulacao.Energ
{
	/// <summary>
	/// Represents energy consumption data, including daily, weekly, and monthly consumption with the ability to apply discounts and calculate averages.
	/// </summary>
	public class EnergyConsumption
	{
		/// <summary>
		/// Gets or sets the unique identifier for the energy consumption record.
		/// </summary>
		/// <value>
		/// An integer representing the unique identifier.
		/// </value>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the user's email associated with the energy consumption data.
		/// </summary>
		/// <value>
		/// A string representing the user's email.
		/// </value>
		public string UserEmail { get; set; }

		/// <summary>
		/// Gets or sets the daily energy consumption during the week.
		/// </summary>
		/// <value>
		/// A list of 24 decimal values representing the energy consumption per hour during the weekdays (Monday to Friday).
		/// </value>
		[Required(ErrorMessage = "O consumo durante a semana é obrigatório.")]
		[MinLength(24, ErrorMessage = "A lista deve conter exatamente 24 valores.")]
		[MaxLength(24, ErrorMessage = "A lista não pode ter mais de 24 valores.")]
		[Column(TypeName = "nvarchar(max)")]
		public List<decimal> ConsumoDiaSemana { get; set; }

		/// <summary>
		/// Gets or sets the daily energy consumption during the weekend.
		/// </summary>
		/// <value>
		/// A list of 24 decimal values representing the energy consumption per hour during the weekends (Saturday and Sunday).
		/// </value>
		[Required(ErrorMessage = "O consumo no fim de semana é obrigatório.")]
		[MinLength(24, ErrorMessage = "A lista deve conter exatamente 24 valores.")]
		[MaxLength(24, ErrorMessage = "A lista não pode ter mais de 24 valores.")]
		[Column(TypeName = "nvarchar(max)")]
		public List<decimal> ConsumoFimSemana { get; set; }

		/// <summary>
		/// Gets or sets the list of months the user occupies the premises.
		/// </summary>
		/// <value>
		/// A list of strings representing the months of the year in which the premises are occupied.
		/// </value>
		[Required(ErrorMessage = "Selecione pelo menos um mês de ocupação.")]
		public List<string> MesesOcupacao { get; set; }

		/// <summary>
		/// Gets or sets the average weekly energy consumption after applying discounts.
		/// </summary>
		/// <value>
		/// A decimal representing the average weekly energy consumption.
		/// </value>
		[Range(0, double.MaxValue, ErrorMessage = "A média semanal deve ser um valor positivo.")]
		public decimal MediaSemana { get; set; }

		/// <summary>
		/// Gets or sets the average weekend energy consumption after applying discounts.
		/// </summary>
		/// <value>
		/// A decimal representing the average weekend energy consumption.
		/// </value>
		[Range(0, double.MaxValue, ErrorMessage = "A média do fim de semana deve ser um valor positivo.")]
		public decimal MediaFimSemana { get; set; }

		/// <summary>
		/// Gets or sets the average annual energy consumption.
		/// </summary>
		/// <value>
		/// A decimal representing the average annual energy consumption.
		/// </value>
		[Range(0, double.MaxValue, ErrorMessage = "A média anual deve ser um valor positivo.")]
		public decimal MediaAnual { get; set; }

		/// <summary>
		/// Gets or sets the total energy consumption for the selected months.
		/// </summary>
		/// <value>
		/// A decimal representing the total energy consumption for the selected months.
		/// </value>
		[Range(0, double.MaxValue, ErrorMessage = "O consumo total deve ser um valor positivo.")]
		public decimal ConsumoTotal { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="EnergyConsumption"/> class.
		/// </summary>
		public EnergyConsumption()
		{
			ConsumoDiaSemana = Enumerable.Repeat(0m, 24).ToList();
			ConsumoFimSemana = Enumerable.Repeat(0m, 24).ToList();
			MesesOcupacao = new List<string>();
		}

		/// <summary>
		/// Applies a discount to the energy consumption based on the hour of the day and whether it's the weekend.
		/// </summary>
		/// <param name="hora">The hour of the day (0 to 23).</param>
		/// <param name="consumo">The original energy consumption for that hour.</param>
		/// <param name="fimDeSemana">A boolean indicating if it is the weekend.</param>
		/// <returns>
		/// A decimal value representing the adjusted energy consumption after applying the discount.
		/// </returns>
		public decimal AplicarDesconto(int hora, decimal consumo, bool fimDeSemana)
		{
			if (fimDeSemana)
			{
				if (hora >= 8 && hora <= 21)
					return consumo * 0.8m;
				else
					return consumo * 0.6m;
			}
			else
			{
				if (hora >= 22 || hora <= 7)
					return consumo * 0.7m;
				else
					return consumo;
			}
		}

		/// <summary>
		/// Calculates the average energy consumption for weekdays and weekends.
		/// </summary>
		public void CalcularMedias()
		{
			CalcularMediaSemana();
			CalcularMediaFimSemana();
		}

		/// <summary>
		/// Calculates the average energy consumption during weekdays (Monday to Friday) after applying discounts.
		/// </summary>
		public void CalcularMediaSemana()
		{
			var consumosCorrigidos = ConsumoDiaSemana.Select((c, hora) => AplicarDesconto(hora, c, false));
			MediaSemana = Math.Round(consumosCorrigidos.Average(), 1);
		}

		/// <summary>
		/// Calculates the average energy consumption during weekends (Saturday and Sunday) after applying discounts.
		/// </summary>
		public void CalcularMediaFimSemana()
		{
			var consumosCorrigidos = ConsumoFimSemana.Select((c, hora) => AplicarDesconto(hora, c, true));
			MediaFimSemana = Math.Round(consumosCorrigidos.Average(), 1);
		}

		/// <summary>
		/// Calculates the total monthly energy consumption based on selected months and averages for weekdays and weekends.
		/// </summary>
		public void CalcularConsumoMensal()
		{
			CalcularMedias();

			if (MesesOcupacao.Any())
			{
				decimal consumoTotalMensal = 0;

				foreach (var mes in MesesOcupacao)
				{
					int semanasNoMes = ObterNumeroDeSemanasNoMes(mes);
					decimal consumoSemana = MediaSemana * 5 + MediaFimSemana * 2;
					decimal consumoMes = Math.Round(consumoSemana * semanasNoMes, 1);

					consumoTotalMensal += consumoMes;
				}

				ConsumoTotal = Math.Round(consumoTotalMensal, 1);
			}
		}

		/// <summary>
		/// Calculates the number of weeks in a given month.
		/// </summary>
		/// <param name="mes">The month name in full (e.g., "January", "February").</param>
		/// <returns>
		/// An integer representing the number of weeks in the given month.
		/// </returns>
		public int ObterNumeroDeSemanasNoMes(string mes)
		{
			var dataInicio = new DateTime(DateTime.Now.Year, MesParaNumero(mes), 1);
			var dataFim = dataInicio.AddMonths(1).AddDays(-1);
			int diasNoMes = (dataFim - dataInicio).Days + 1;

			return (int)Math.Ceiling(diasNoMes / 7.0);
		}

		/// <summary>
		/// Converts a month name (e.g., "January") to its corresponding month number (e.g., 1 for January).
		/// </summary>
		/// <param name="mes">The name of the month in full.</param>
		/// <returns>
		/// An integer representing the month number (1 for January, 2 for February, etc.).
		/// </returns>
		public int MesParaNumero(string mes)
		{
			return DateTime.ParseExact(mes, "MMMM", new System.Globalization.CultureInfo("pt-PT")).Month;
		}

		/// <summary>
		/// Calculates the average annual energy consumption based on the total consumption and the number of months of occupation.
		/// </summary>
		public void CalcularMediaAnual()
		{
			MediaAnual = MesesOcupacao.Count > 0 ? Math.Round(ConsumoTotal / MesesOcupacao.Count, 1) : 0m;
		}
	}
}
