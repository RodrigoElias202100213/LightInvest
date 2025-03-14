using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public class EnergyConsumptionViewModel
	{
		public List<decimal> ConsumoDiaSemana { get; set; } // Lista de consumo horário de segunda a sexta
		public List<decimal> ConsumoFimSemana { get; set; } // Lista de consumo horário de fim de semana
		public List<string> MesesOcupacao { get; set; } // Meses ocupados
		public decimal MediaSemana { get; set; } // Média de consumo durante a semana
		public decimal MediaFimSemana { get; set; } // Média de consumo no fim de semana
		public decimal MediaAnual { get; set; } // Média anual do consumo
	}

}