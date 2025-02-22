

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
            if (CustoInstalacao <= 0 || RetornoEconomia <= 0)
                return 0;

            decimal economiaAnual = (ConsumoEnergeticoRede - ConsumoEnergeticoMedio) * RetornoEconomia;
            decimal custoTotal = CustoInstalacao + CustoManutencaoAnual;

            return custoTotal / economiaAnual;
        }
    }
}
