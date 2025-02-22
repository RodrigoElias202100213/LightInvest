namespace LightInvest.Models
{
	public class ROICalculator
	{
		public decimal CustoInstalacao { get; set; }
		public decimal CustoManutencaoAnual { get; set; }
		public decimal ConsumoEnergeticoMedio { get; set; }
		public decimal ConsumoEnergeticoRede { get; set; }
		public decimal RetornoEconomia { get; set; }

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