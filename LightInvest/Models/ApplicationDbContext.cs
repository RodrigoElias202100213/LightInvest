﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LightInvest.Models;

namespace LightInvest.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{ }

		public DbSet<RoiCalculator> ROICalculators { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

		public DbSet<EnergyConsumption> EnergyConsumptions { get; set; }
		public DbSet<Tarifa> Tarifas { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<RoiCalculator>(entity =>
			{
				entity.Property(e => e.CustoInstalacao)
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.CustoManutencaoAnual)
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.ConsumoEnergeticoMedio)
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.ConsumoEnergeticoRede)
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.RetornoEconomia)
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.ROI)
					.HasColumnType("decimal(18,2)");
			});
		}
	}

}