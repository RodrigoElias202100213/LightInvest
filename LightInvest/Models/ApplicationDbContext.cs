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

			modelBuilder.Entity<PotenciaPainelSolar>()
				.HasOne(p => p.PainelSolar)
				.WithMany(m => m.Potencias)
				.HasForeignKey(p => p.PainelSolarId)
				.OnDelete(DeleteBehavior.Cascade);

			// Cidades
			modelBuilder.Entity<Cidade>().HasData(
				new Cidade { Id = 1, Nome = "Albufeira" },
				new Cidade { Id = 2, Nome = "Almada" },
				new Cidade { Id = 3, Nome = "Amadora" },
				new Cidade { Id = 4, Nome = "Aveiro" },
				new Cidade { Id = 30, Nome = "Barreiro" },
				new Cidade { Id = 5, Nome = "Barcelos" },
				new Cidade { Id = 6, Nome = "Beja" },
				new Cidade { Id = 7, Nome = "Braga" },
				new Cidade { Id = 8, Nome = "Bragança" },
				new Cidade { Id = 9, Nome = "Caldas da Rainha" },
				new Cidade { Id = 10, Nome = "Cascais" },
				new Cidade { Id = 11, Nome = "Coimbra" },
				new Cidade { Id = 12, Nome = "Évora" },
				new Cidade { Id = 13, Nome = "Faro" },
				new Cidade { Id = 14, Nome = "Figueira da Foz" },
				new Cidade { Id = 15, Nome = "Funchal" },
				new Cidade { Id = 16, Nome = "Guarda" },
				new Cidade { Id = 17, Nome = "Guimarães" },
				new Cidade { Id = 18, Nome = "Leiria" },
				new Cidade { Id = 19, Nome = "Lisboa" },
				new Cidade { Id = 20, Nome = "Matosinhos" },
				new Cidade { Id = 21, Nome = "Montijo" },
				new Cidade { Id = 22, Nome = "Odivelas" },
				new Cidade { Id = 23, Nome = "Oeiras" },
				new Cidade { Id = 24, Nome = "Portalegre" },
				new Cidade { Id = 25, Nome = "Portimão" },
				new Cidade { Id = 26, Nome = "Porto" },
				new Cidade { Id = 27, Nome = "Póvoa de Varzim" },
				new Cidade { Id = 28, Nome = "Santarem" },
				new Cidade { Id = 29, Nome = "Setúbal" },
				new Cidade { Id = 31, Nome = "Sintra" },
				new Cidade { Id = 32, Nome = "Tomar" },
				new Cidade { Id = 33, Nome = "Torres Vedras" },
				new Cidade { Id = 34, Nome = "Viana do castelo" },
				new Cidade { Id = 35, Nome = "Vila do Conde" },
				new Cidade { Id = 36, Nome = "Vila Nova de Gaia" },
				new Cidade { Id = 37, Nome = "Viseu" }

			);

			// Modelos
			modelBuilder.Entity<ModeloPainelSolar>().HasData(
				new ModeloPainelSolar { Id = 1, Modelo = "Aiko - Comet 2U" },
				new ModeloPainelSolar { Id = 2, Modelo = "Maxeon 7" },
				new ModeloPainelSolar { Id = 3, Modelo = "Longi - HI-MO X6" },
				new ModeloPainelSolar { Id = 4, Modelo = "Huasun - Himalaya" },
				new ModeloPainelSolar { Id = 5, Modelo = "TW Solar" },
				new ModeloPainelSolar { Id = 6, Modelo = "JA Solar DeepBlue 4.0 Pro" },
				new ModeloPainelSolar { Id = 7, Modelo = "Astroenergy - Astro N5" },
				new ModeloPainelSolar { Id = 8, Modelo = "Grand Sunergy" },
				new ModeloPainelSolar { Id = 9, Modelo = "DMEGC - Infinity RT" },
				new ModeloPainelSolar { Id = 10, Modelo = "Spic" }
			);

			//Potencia para cada painel solar
			modelBuilder.Entity<PotenciaPainelSolar>().HasData(
				// Para o Modelo Aiko - Comet 2U
				new PotenciaPainelSolar { Id = 1, Potencia = 670, PainelSolarId = 1 },

				//Para o Modelo Maxeon 7
				new PotenciaPainelSolar { Id = 2, Potencia = 445, PainelSolarId = 2 },

				//Para o Modelo Longi - HI-MO X6
				new PotenciaPainelSolar { Id = 3, Potencia = 600, PainelSolarId = 3 },

				//Para o Modelo Huasun - Himalaya
				new PotenciaPainelSolar { Id = 4, Potencia = 720, PainelSolarId = 4 },

				//Para o Modelo TW Solar
				new PotenciaPainelSolar { Id = 5, Potencia = 715, PainelSolarId = 5 },
				new PotenciaPainelSolar { Id = 6, Potencia = 590, PainelSolarId = 5 },

				//Para o Modelo JA Solar DeepBlue 4.0 Pro
				new PotenciaPainelSolar { Id = 7, Potencia = 595, PainelSolarId = 6 },

				//Para o Modelo Astroenergy - Astro N5
				new PotenciaPainelSolar { Id = 8, Potencia = 640, PainelSolarId = 7 },

				//Para o Modelo Grand Sunergy
				new PotenciaPainelSolar { Id = 9, Potencia = 710, PainelSolarId = 8 },

				//Para o Modelo DMEGC - Infinity RT
				new PotenciaPainelSolar { Id = 10, Potencia = 615, PainelSolarId = 9 },

				//Para o Modelo Spic
				new PotenciaPainelSolar { Id = 11, Potencia = 410, PainelSolarId = 10 }

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