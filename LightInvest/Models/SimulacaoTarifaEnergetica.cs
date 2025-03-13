using System;
using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
    public class SimulacaoTarifaEnergetica
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  // Identificador do usuário que realizou a simulação

        [Required]
        [Display(Name = "Consumo Médio Mensal (kWh)")]
        public decimal ConsumoMedioMensal { get; set; }

        [Required]
        [Display(Name = "Tarifa de Energia Atual (€/kWh)")]
        public decimal TarifaEnergiaAtual { get; set; }

        [Required]
        [Display(Name = "Tarifa de Energia Simulada (€/kWh)")]
        public decimal TarifaEnergiaSimulada { get; set; }  // tarifa que será aplicada na simulação

        [Required]
        [Display(Name = "Custo de Instalação do Sistema (€/unidade)")]
        public decimal CustoInstalacaoSistema { get; set; }

        [Required]
        [Display(Name = "Potência do Sistema (kWp)")]
        public decimal PotenciaSistema { get; set; }

        [Required]
        [Display(Name = "Vida Útil do Sistema (anos)")]
        public int VidaUtilSistema { get; set; }

        // Cálculos derivados
        [Display(Name = "Economia Anual Estimada (€)")]
        public decimal EconomiaAnualEstimativa { get; set; }

        [Display(Name = "Retorno sobre Investimento (ROI %)")]
        public decimal ROI { get; set; }

        public DateTime DataSimulacao { get; set; } = DateTime.Now;

        /// <summary>
        /// Calcula a economia anual e o ROI.
        /// A economia anual é definida pela diferença entre a tarifa atual e a tarifa simulada, multiplicada pelo consumo anual.
        /// O ROI é calculado como (Custo de Instalação / Economia Anual) * 100.
        /// </summary>
        public void CalcularSimulacao()
        {
            // Consumo anual em kWh
            decimal consumoAnual = ConsumoMedioMensal * 12;

            // Economia anual: diferença entre a tarifa atual e a simulada multiplicada pelo consumo anual.
            EconomiaAnualEstimativa = consumoAnual * (TarifaEnergiaAtual - TarifaEnergiaSimulada);

            // Se os dados forem válidos, calcula o ROI
            if (CustoInstalacaoSistema > 0 && EconomiaAnualEstimativa > 0)
            {
                ROI = (CustoInstalacaoSistema / EconomiaAnualEstimativa) * 100;
            }
            else
            {
                ROI = 0;
            }
        }
    }
}
