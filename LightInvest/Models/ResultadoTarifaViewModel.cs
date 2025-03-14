namespace LightInvest.Models
{
	public enum Meses
	{
		Janeiro, Fevereiro, Marco, Abril, Maio, Junho,
		Julho, Agosto, Setembro, Outubro, Novembro, Dezembro
	}

	public class ResultadoTarifaViewModel
	{
		public decimal ConsumoTotal { get; set; }
		public List<MesConsumo> ConsumoMensal { get; set; }
		public decimal ValorAnual { get; set; }
		public string TarifaEscolhida { get; set; }
		public decimal PrecoKwh { get; set; }  // Esta é a propriedade que você deseja alterar

		// Adiciona MesesOcupacao
		public List<string> MesesOcupacao { get; set; }

		public ResultadoTarifaViewModel()
		{
			// Inicializa a lista de meses ocupados
			MesesOcupacao = new List<string>();
			ConsumoMensal = new List<MesConsumo>();
		}
	}

	public class MesConsumo
	{
		public string Mes { get; set; }
		public decimal Consumo { get; set; }
		public decimal Custo { get; set; }
	}
}