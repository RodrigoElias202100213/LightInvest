using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public class RoiCalculator
	{
		public int Id { get; set; }

		[Required] public int UserId { get; set; } // 🔥 O ID do usuário é obrigatório

		[Required] public virtual User? User { get; set; } // 🔥 O usuário logado deve estar associado

		[Required] public decimal CustoInstalacao { get; set; }

		[Required] public decimal CustoManutencaoAnual { get; set; }

		[Required] public decimal ConsumoEnergeticoMedio { get; set; }

		[Required] public decimal ConsumoEnergeticoRede { get; set; }

		[Required] public decimal RetornoEconomia { get; set; }

		public decimal ROI { get; set; }

		public DateTime DataCalculado { get; set; }

		public decimal CalcularROI()
		{
			if (CustoInstalacao <= 0 || RetornoEconomia <= 0 || ConsumoEnergeticoMedio <= 0 ||
			    ConsumoEnergeticoRede <= 0)
				throw new ArgumentException("Todos os valores devem ser positivos e maiores que zero.");

			decimal economiaAnual = (ConsumoEnergeticoRede - ConsumoEnergeticoMedio) * RetornoEconomia;
			decimal custoTotal = CustoInstalacao + CustoManutencaoAnual;

			if (economiaAnual <= 0)
				throw new InvalidOperationException("A economia anual deve ser maior que zero para calcular o ROI.");

			return (custoTotal / economiaAnual) * 100;
		}
	}
}