﻿namespace LightInvest.Models.Roi
{
	public class RetornoInvestimentoAno
	{
		public int Ano { get; set; }
		public List<RetornoInvestimentoMes> Meses { get; set; }

		public RetornoInvestimentoAno()
		{
			Meses = new List<RetornoInvestimentoMes>();
		}
	}
}