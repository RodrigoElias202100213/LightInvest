using Microsoft.EntityFrameworkCore;
using LightInvest.Models.b;
using LightInvest.Models.Ener;
using LightInvest.Models.Roi;
using LightInvest.Models.Educ.Artigos;
using LightInvest.Models.Simulacao.Energ;
using LightInvest.Models.Simulacao.Tarifa;
using LightInvest.Models.Utilizador.Login;
using LightInvest.Models.Utilizador.Pass;

namespace LightInvest.Models.BD
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


		public DbSet<Artigo> Artigos { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<DadosInstalacao>()
				.HasOne(d => d.Potencia)
				.WithMany()
				.HasForeignKey(d => d.PotenciaId)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<PotenciaPainelSolar>()
				.HasOne(p => p.ModeloPainelSolar)
				.WithMany(m => m.Potencias)
				.HasForeignKey(p => p.ModeloPainelId)
				.OnDelete(DeleteBehavior.Restrict);

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

			modelBuilder.Entity<ModeloPainelSolar>().HasData(
				new ModeloPainelSolar { Id = 1, ModeloNome = "Aiko - Comet 2U", Preco = 1250.00m },
				new ModeloPainelSolar { Id = 2, ModeloNome = "Maxeon 7", Preco = 1320.00m },
				new ModeloPainelSolar { Id = 3, ModeloNome = "Longi - HI-MO X6", Preco = 1280.00m },
				new ModeloPainelSolar { Id = 4, ModeloNome = "Huasun - Himalaya", Preco = 1300.00m },
				new ModeloPainelSolar { Id = 5, ModeloNome = "TW Solar", Preco = 1230.00m },
				new ModeloPainelSolar { Id = 6, ModeloNome = "JA Solar DeepBlue 4.0 Pro", Preco = 1270.00m },
				new ModeloPainelSolar { Id = 7, ModeloNome = "Astroenergy - Astro N5", Preco = 1260.00m },
				new ModeloPainelSolar { Id = 8, ModeloNome = "Grand Sunergy", Preco = 1240.00m },
				new ModeloPainelSolar { Id = 9, ModeloNome = "DMEGC - Infinity RT", Preco = 1290.00m },
				new ModeloPainelSolar { Id = 10, ModeloNome = "Spic", Preco = 910.00m }
			);


			// Potencia para cada painel solar
			modelBuilder.Entity<PotenciaPainelSolar>().HasData(
				// Para o ModeloNome Aiko - Comet 2U
				new PotenciaPainelSolar { Id = 1, Potencia = 670, ModeloPainelId = 1 },
				new PotenciaPainelSolar { Id = 2, Potencia = 680, ModeloPainelId = 1 },
				new PotenciaPainelSolar { Id = 3, Potencia = 690, ModeloPainelId = 1 },
				new PotenciaPainelSolar { Id = 4, Potencia = 700, ModeloPainelId = 1 },

				// Para o ModeloNome Maxeon 7
				new PotenciaPainelSolar { Id = 5, Potencia = 445, ModeloPainelId = 2 },
				new PotenciaPainelSolar { Id = 6, Potencia = 455, ModeloPainelId = 2 },
				new PotenciaPainelSolar { Id = 7, Potencia = 465, ModeloPainelId = 2 },
				new PotenciaPainelSolar { Id = 8, Potencia = 475, ModeloPainelId = 2 },

				// Para o ModeloNome Longi - HI-MO X6
				new PotenciaPainelSolar { Id = 9, Potencia = 600, ModeloPainelId = 3 },
				new PotenciaPainelSolar { Id = 10, Potencia = 610, ModeloPainelId = 3 },
				new PotenciaPainelSolar { Id = 11, Potencia = 620, ModeloPainelId = 3 },
				new PotenciaPainelSolar { Id = 12, Potencia = 630, ModeloPainelId = 3 },

				// Para o ModeloNome Huasun - Himalaya
				new PotenciaPainelSolar { Id = 13, Potencia = 720, ModeloPainelId = 4 },
				new PotenciaPainelSolar { Id = 14, Potencia = 730, ModeloPainelId = 4 },
				new PotenciaPainelSolar { Id = 15, Potencia = 740, ModeloPainelId = 4 },
				new PotenciaPainelSolar { Id = 16, Potencia = 750, ModeloPainelId = 4 },

				// Para o ModeloNome TW Solar
				new PotenciaPainelSolar { Id = 17, Potencia = 715, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 18, Potencia = 725, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 19, Potencia = 735, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 20, Potencia = 745, ModeloPainelId = 5 },

				new PotenciaPainelSolar { Id = 21, Potencia = 590, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 22, Potencia = 600, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 23, Potencia = 610, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 24, Potencia = 620, ModeloPainelId = 5 },

				// Para o ModeloNome JA Solar DeepBlue 4.0 Pro
				new PotenciaPainelSolar { Id = 25, Potencia = 595, ModeloPainelId = 6 },
				new PotenciaPainelSolar { Id = 26, Potencia = 605, ModeloPainelId = 6 },
				new PotenciaPainelSolar { Id = 27, Potencia = 615, ModeloPainelId = 6 },
				new PotenciaPainelSolar { Id = 28, Potencia = 625, ModeloPainelId = 6 },

				// Para o ModeloNome Astroenergy - Astro N5
				new PotenciaPainelSolar { Id = 29, Potencia = 640, ModeloPainelId = 7 },
				new PotenciaPainelSolar { Id = 30, Potencia = 650, ModeloPainelId = 7 },
				new PotenciaPainelSolar { Id = 31, Potencia = 660, ModeloPainelId = 7 },
				new PotenciaPainelSolar { Id = 32, Potencia = 670, ModeloPainelId = 7 },

				// Para o ModeloNome Grand Sunergy
				new PotenciaPainelSolar { Id = 33, Potencia = 710, ModeloPainelId = 8 },
				new PotenciaPainelSolar { Id = 34, Potencia = 720, ModeloPainelId = 8 },
				new PotenciaPainelSolar { Id = 35, Potencia = 730, ModeloPainelId = 8 },
				new PotenciaPainelSolar { Id = 36, Potencia = 740, ModeloPainelId = 8 },

				// Para o ModeloNome DMEGC - Infinity RT
				new PotenciaPainelSolar { Id = 37, Potencia = 615, ModeloPainelId = 9 },
				new PotenciaPainelSolar { Id = 38, Potencia = 625, ModeloPainelId = 9 },
				new PotenciaPainelSolar { Id = 39, Potencia = 635, ModeloPainelId = 9 },
				new PotenciaPainelSolar { Id = 40, Potencia = 645, ModeloPainelId = 9 },

				// Para o ModeloNome Spic
				new PotenciaPainelSolar { Id = 41, Potencia = 410, ModeloPainelId = 10 },
				new PotenciaPainelSolar { Id = 42, Potencia = 420, ModeloPainelId = 10 },
				new PotenciaPainelSolar { Id = 43, Potencia = 430, ModeloPainelId = 10 },
				new PotenciaPainelSolar { Id = 44, Potencia = 440, ModeloPainelId = 10 }
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