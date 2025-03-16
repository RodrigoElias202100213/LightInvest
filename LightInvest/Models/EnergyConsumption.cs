using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public class EnergyConsumption
	{
		public int Id { get; set; }
		public string UserEmail { get; set; }

		[Required]
		public List<decimal> ConsumoDiaSemana { get; set; } = new List<decimal>();

		[Required]
		public List<decimal> ConsumoFimSemana { get; set; } = new List<decimal>();

		[Required]
		public List<string> MesesOcupacao { get; set; } = new List<string>();
		

		public decimal MediaSemana { get; set; }
		public decimal MediaFimSemana { get; set; }
		public decimal MediaAnual { get; set; }
		public decimal ConsumoTotal { get; set; }

		public EnergyConsumption()
		{
			ConsumoDiaSemana = new List<decimal>(7); // Inicializa com 7 elementos (se necessário)
			ConsumoFimSemana = new List<decimal>(2); // Inicializa com 2 elementos (sábado e domingo)
			MesesOcupacao = new List<string>();
		}

		public void CalcularMedias()
		{
			CalcularMediaSemana();
			CalcularMediaFimSemana();
			CalcularMediaAnual();
		}

		public void CalcularMediaSemana()
		{
			if (ConsumoDiaSemana.Count == 7)
			{
				MediaSemana = ConsumoDiaSemana.Average();
			}
		}

		public void CalcularMediaFimSemana()
		{
			if (ConsumoFimSemana.Count == 2)
			{
				MediaFimSemana = ConsumoFimSemana.Average();
			}
		}

		public void CalcularConsumoMensal()
		{
			if (MesesOcupacao.Any())
			{
				decimal consumoTotalAnual = MediaAnual;

				decimal consumoPorMes = consumoTotalAnual / MesesOcupacao.Count;

				foreach (var mes in MesesOcupacao)
				{
					Console.WriteLine($"Mês: {mes}, Consumo: {consumoPorMes}");
				}

				ConsumoTotal = consumoPorMes * MesesOcupacao.Count;
			}
		}

		public void CalcularMediaAnual()
		{
			var totalDias = ConsumoDiaSemana.Count * MesesOcupacao.Count;
			MediaAnual = totalDias > 0 ? (ConsumoDiaSemana.Sum() * MesesOcupacao.Count) / totalDias : 0;
		}
	}
}