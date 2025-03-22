using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Simulacao.Energ
{
	public class EnergyConsumptionViewModel
	{
		[Required(ErrorMessage = "Os valores de consumo durante a semana são obrigatórios.")]
		[MinLength(24, ErrorMessage = "Deve conter 24 valores para cada hora do dia.")]
		public List<decimal> ConsumoDiaSemana { get; set; }

		[Required(ErrorMessage = "Os valores de consumo no fim de semana são obrigatórios.")]
		[MinLength(24, ErrorMessage = "Deve conter 24 valores para cada hora do dia.")]
		public List<decimal> ConsumoFimSemana { get; set; }

		[Required(ErrorMessage = "Selecione ao menos um mês de ocupação.")]
		public List<string> MesesOcupacao { get; set; }

		public decimal MediaSemana { get; set; }
		public decimal MediaFimSemana { get; set; }
		public decimal MediaAnual { get; set; }
		public decimal ConsumoTotal { get; set; }

	}

}