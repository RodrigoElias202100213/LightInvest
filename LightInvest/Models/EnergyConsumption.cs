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
		public decimal ConsumoTotal { get; set; }  // Armazenar o consumo total calculado

		// Construtor para inicializar as listas de consumo (opcional)
		public EnergyConsumption()
		{
			ConsumoDiaSemana = new List<decimal>(7); // Inicializa com 7 elementos (se necessário)
			ConsumoFimSemana = new List<decimal>(2); // Inicializa com 2 elementos (sábado e domingo)
			MesesOcupacao = new List<string>();
		}

		// Métodos para calcular as médias
		public void CalcularMediaSemana()
		{
			if (ConsumoDiaSemana.Count == 7) // Garantir que a lista tenha 7 valores
			{
				MediaSemana = ConsumoDiaSemana.Average();
			}
		}

		public void CalcularMediaFimSemana()
		{
			if (ConsumoFimSemana.Count == 2) // Garantir que a lista tenha 2 valores
			{
				MediaFimSemana = ConsumoFimSemana.Average();
			}
		}

		public void CalcularMediaAnual()
		{
			// A média anual pode ser calculada somando todas as semanas e dividindo por 12 ou 52, por exemplo
			var totalDias = ConsumoDiaSemana.Count * MesesOcupacao.Count;
			MediaAnual = totalDias > 0 ? (ConsumoDiaSemana.Sum() * MesesOcupacao.Count) / totalDias : 0;
		}
	}
}