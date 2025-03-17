using System.ComponentModel.DataAnnotations;
namespace LightInvest.Models
{
	public class EnergyConsumption
	{
		public int Id { get; set; }
		public string UserEmail { get; set; }

		[Required]
		public List<decimal> ConsumoDiaSemana { get; set; }
		[Required]
		public List<decimal> ConsumoFimSemana { get; set; }
		[Required]
		public List<string> MesesOcupacao { get; set; }

		public decimal MediaSemana { get; set; }
		public decimal MediaFimSemana { get; set; }
		public decimal MediaAnual { get; set; }
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
					return consumo * 0.8m; // 20% de desconto
				else
					return consumo * 0.6m; // 40% de desconto
			}
			else
			{
				if (hora >= 22 || hora <= 7)
					return consumo * 0.7m; // 30% de desconto
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
			// Certificar-se de que as médias foram calculadas
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

					Console.WriteLine($"Mês: {mes}, Número de Semanas: {semanasNoMes}, Consumo: {consumoMes}");
				}

				ConsumoTotal = Math.Round(consumoTotalMensal, 1);
			}
		}


		// Função para calcular o número de semanas em um mês
		public int ObterNumeroDeSemanasNoMes(string mes)
		{
			var dataInicio = new DateTime(DateTime.Now.Year, MesParaNumero(mes), 1);
			var dataFim = dataInicio.AddMonths(1).AddDays(-1); // Último dia do mês
			int diasNoMes = (dataFim - dataInicio).Days + 1; // Conta os dias no mês

			// Retorna o número de semanas arredondando para cima
			return (int)Math.Ceiling(diasNoMes / 7.0);
		}

		// Função para mapear o nome do mês para seu número
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