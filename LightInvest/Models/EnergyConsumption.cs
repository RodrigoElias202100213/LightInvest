using System.ComponentModel.DataAnnotations;
namespace LightInvest.Models
{
	public class EnergyConsumption
	{
		public int Id { get; set; }
		public string UserEmail { get; set; }

		[Required(ErrorMessage = "O consumo durante a semana é obrigatório.")]
		[MinLength(24, ErrorMessage = "É necessário informar os 24 valores de consumo diário.")]
		[MaxLength(24, ErrorMessage = "A lista não pode ter mais de 24 valores.")]
		public List<decimal> ConsumoDiaSemana { get; set; }

		[Required(ErrorMessage = "O consumo no fim de semana é obrigatório.")]
		[MinLength(24, ErrorMessage = "É necessário informar os 24 valores de consumo do fim de semana.")]
		[MaxLength(24, ErrorMessage = "A lista não pode ter mais de 24 valores.")]
		public List<decimal> ConsumoFimSemana { get; set; }

		[Required(ErrorMessage = "Selecione pelo menos um mês de ocupação.")]
		public List<string> MesesOcupacao { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "A média semanal deve ser um valor positivo.")]
		public decimal MediaSemana { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "A média do fim de semana deve ser um valor positivo.")]
		public decimal MediaFimSemana { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "A média anual deve ser um valor positivo.")]
		public decimal MediaAnual { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "O consumo total deve ser um valor positivo.")]
		public decimal ConsumoTotal { get; set; }

		public EnergyConsumption()
		{
			ConsumoDiaSemana = new List<decimal>(new decimal[24]);
			ConsumoFimSemana = new List<decimal>(new decimal[24]);
			MesesOcupacao = new List<string>();
		}

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

		public void CalcularMedias()
		{
			CalcularMediaSemana();
			CalcularMediaFimSemana();
		}
		public void CalcularMediaSemana()
		{
			var consumosCorrigidos = ConsumoDiaSemana.Select((c, hora) => AplicarDesconto(hora, c, false));
			MediaSemana = Math.Round(consumosCorrigidos.Average(), 1);
		}

		public void CalcularMediaFimSemana()
		{
			var consumosCorrigidos = ConsumoFimSemana.Select((c, hora) => AplicarDesconto(hora, c, true));
			MediaFimSemana = Math.Round(consumosCorrigidos.Average(), 1);
		}

		public void CalcularConsumoMensal()
		{
			CalcularMedias();

			if (MesesOcupacao.Any())
			{
				decimal consumoTotalMensal = 0;

				foreach (var mes in MesesOcupacao)
				{
					int semanasNoMes = ObterNumeroDeSemanasNoMes(mes);
					decimal consumoSemana = (MediaSemana * 5) + (MediaFimSemana * 2);
					decimal consumoMes = Math.Round(consumoSemana * semanasNoMes, 1);

					consumoTotalMensal += consumoMes;
				}

				ConsumoTotal = Math.Round(consumoTotalMensal, 1);

			}
		}

		public int ObterNumeroDeSemanasNoMes(string mes)
		{
			var dataInicio = new DateTime(DateTime.Now.Year, MesParaNumero(mes), 1);
			var dataFim = dataInicio.AddMonths(1).AddDays(-1);
			int diasNoMes = (dataFim - dataInicio).Days + 1;

			return (int)Math.Ceiling(diasNoMes / 7.0);
		}

		public int MesParaNumero(string mes)
		{
			switch (mes.ToLower())
			{
				case "janeiro": return 1;
				case "fevereiro": return 2;
				case "marco": return 3;
				case "abril": return 4;
				case "maio": return 5;
				case "junho": return 6;
				case "julho": return 7;
				case "agosto": return 8;
				case "setembro": return 9;
				case "outubro": return 10;
				case "novembro": return 11;
				case "dezembro": return 12;
				default: throw new ArgumentException("Mês inválido");
			}
		}

		public void CalcularMediaAnual()
		{
			if (MesesOcupacao.Any())
			{
				MediaAnual = ConsumoTotal / MesesOcupacao.Count;
			}
			else
			{
				MediaAnual = 0;
			}
		}

	}
}