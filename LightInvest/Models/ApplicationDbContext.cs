using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LightInvest.Models;
using LightInvest.Models.b;

namespace LightInvest.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}
		public DbSet<RoiCalculator> ROICalculators { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

		public DbSet<EnergyConsumption> EnergyConsumptions { get; set; }
		public DbSet<Tarifa> Tarifas { get; set; }


		public DbSet<DadosInstalacao> DadosInstalacao { get; set; }

		public DbSet<Cidade> Cidades { get; set; }
		public DbSet<ModeloPainelSolar> ModelosDePaineisSolares { get; set; }
		public DbSet<PotenciaPainelSolar> PotenciasDePaineisSolares { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Relação entre ModeloPainelSolar e PotenciaPainelSolar
			modelBuilder.Entity<PotenciaPainelSolar>()
				.HasOne(p => p.PainelSolar)
				.WithMany(m => m.Potencias)
				.HasForeignKey(p => p.PainelSolarId)
				.OnDelete(DeleteBehavior.Cascade); // Define a ação ao deletar um modelo

			// Seed data para Cidade
			modelBuilder.Entity<Cidade>().HasData(
				new Cidade { Id = 1, Nome = "Lisboa" },
				new Cidade { Id = 2, Nome = "Porto" },
				new Cidade { Id = 3, Nome = "Coimbra" },
				new Cidade { Id = 4, Nome = "Funchal" }
			);

			// Seed data para ModeloPainelSolar
			modelBuilder.Entity<ModeloPainelSolar>().HasData(
				new ModeloPainelSolar { Id = 1, Modelo = "Modelo A" },
				new ModeloPainelSolar { Id = 2, Modelo = "Modelo B" },
				new ModeloPainelSolar { Id = 3, Modelo = "Modelo C" }
			);

			// Seed data para PotenciaPainelSolar
			modelBuilder.Entity<PotenciaPainelSolar>().HasData(
				// Potências para o Modelo A
				new PotenciaPainelSolar { Id = 1, Potencia = 250, PainelSolarId = 1 },
				new PotenciaPainelSolar { Id = 2, Potencia = 270, PainelSolarId = 1 },
				new PotenciaPainelSolar { Id = 3, Potencia = 300, PainelSolarId = 1 },

				// Potências para o Modelo B
				new PotenciaPainelSolar { Id = 4, Potencia = 300, PainelSolarId = 2 },
				new PotenciaPainelSolar { Id = 5, Potencia = 320, PainelSolarId = 2 },
				new PotenciaPainelSolar { Id = 6, Potencia = 350, PainelSolarId = 2 },

				// Potências para o Modelo C
				new PotenciaPainelSolar { Id = 7, Potencia = 350, PainelSolarId = 3 },
				new PotenciaPainelSolar { Id = 8, Potencia = 380, PainelSolarId = 3 },
				new PotenciaPainelSolar { Id = 9, Potencia = 400, PainelSolarId = 3 }
			);

			modelBuilder.Entity<RoiCalculator>(entity =>
			{
				entity.Property(e => e.CustoInstalacao).HasColumnType("decimal(18,2)");
				entity.Property(e => e.CustoManutencaoAnual).HasColumnType("decimal(18,2)");
				entity.Property(e => e.ConsumoEnergeticoMedio).HasColumnType("decimal(18,2)");
				entity.Property(e => e.ConsumoEnergeticoRede).HasColumnType("decimal(18,2)");
				entity.Property(e => e.RetornoEconomia).HasColumnType("decimal(18,2)");
				entity.Property(e => e.ROI).HasColumnType("decimal(18,2)");
			});
		}
	}

}