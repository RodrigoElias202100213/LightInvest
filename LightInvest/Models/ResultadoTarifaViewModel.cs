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
<<<<<<< HEAD
		public decimal PrecoKwh { get; set; }
=======
		public decimal PrecoKwh { get; set; } 
>>>>>>> 6f1280410c6fc0b5bc957e67d01c2a07fabd69f0
		public List<string> MesesOcupacao { get; set; }

		public ResultadoTarifaViewModel()
		{
			MesesOcupacao = new List<string>();
			ConsumoMensal = new List<MesConsumo>();
		}

		public void AtualizarPrecoKwh(TipoTarifa tipoTarifa, decimal precoBase)
		{
			var tarifa = new Tarifa { Nome = tipoTarifa, PrecoKwh = precoBase };
			PrecoKwh = tarifa.GetPrecoKwh();
<<<<<<< HEAD
=======

>>>>>>> 6f1280410c6fc0b5bc957e67d01c2a07fabd69f0
		}
	}


	public class MesConsumo
	{
		public string Mes { get; set; }
		public decimal Consumo { get; set; }
		public decimal Custo { get; set; }
	}
}