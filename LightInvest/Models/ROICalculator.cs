using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	public class ROICalculator
	{
		public int Id { get; set; }  // Identificador único do cálculo

		public int UserId { get; set; }  // Relacionamento com o usuário logado
		public virtual User User { get; set; }  // Propriedade de navegação para o User

		[Required]
		public decimal CustoInstalacao { get; set; }

		[Required]
		public decimal CustoManutencaoAnual { get; set; }

		[Required]
		public decimal ConsumoEnergeticoMedio { get; set; }

		[Required]
		public decimal ConsumoEnergeticoRede { get; set; }

		[Required]
		public decimal RetornoEconomia { get; set; }

		public decimal ROI { get; set; }  // Resultado do cálculo do ROI

		public DateTime DataCalculado { get; set; }  // Data do cálculo
		
		// Método para calcular o ROI
		public decimal CalcularROI()
		{
			// Verificação de entradas válidas
			if (CustoInstalacao <= 0 || RetornoEconomia <= 0 || ConsumoEnergeticoMedio <= 0 || ConsumoEnergeticoRede <= 0)
				throw new ArgumentException("Todos os valores devem ser positivos e maiores que zero.");

			decimal economiaAnual = (ConsumoEnergeticoRede - ConsumoEnergeticoMedio) * RetornoEconomia;
			decimal custoTotal = CustoInstalacao + CustoManutencaoAnual;

			if (economiaAnual <= 0)
				throw new InvalidOperationException("A economia anual deve ser maior que zero para calcular o ROI.");

			return (custoTotal / economiaAnual) * 100; // ROI em %
		}
	}
}