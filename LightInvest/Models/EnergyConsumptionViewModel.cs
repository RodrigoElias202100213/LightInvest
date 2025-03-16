using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public class EnergyConsumptionViewModel
	{
		public List<decimal> ConsumoDiaSemana { get; set; } 
		public List<decimal> ConsumoFimSemana { get; set; } 
		public List<string> MesesOcupacao { get; set; } 
		public decimal MediaSemana { get; set; } 
		public decimal MediaFimSemana { get; set; } 
		public decimal MediaAnual { get; set; } 

	}

}