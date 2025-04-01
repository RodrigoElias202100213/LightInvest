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

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
		/// </summary>
		/// <param name="options">The options to be used for the context.</param>
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}

		/// <summary>
		/// Gets or sets the <see cref="RoiCalculator"/> entities in the database.
		/// </summary>
		public DbSet<RoiCalculator> ROICalculators { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="User"/> entities in the database.
		/// </summary>
		public DbSet<User> Users { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="PasswordResetToken"/> entities in the database.
		/// </summary>
		public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="EnergyConsumption"/> entities in the database.
		/// </summary>
		public DbSet<EnergyConsumption> EnergyConsumptions { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Tarifa"/> entities in the database.
		/// </summary>
		public DbSet<Tarifa> Tarifas { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="DadosInstalacao"/> entities in the database.
		/// </summary>
		public DbSet<DadosInstalacao> DadosInstalacao { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Cidade"/> entities in the database.
		/// </summary>
		public DbSet<Cidade> Cidades { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="ModeloPainelSolar"/> entities in the database.
		/// </summary>
		public DbSet<ModeloPainelSolar> ModelosDePaineisSolares { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="PotenciaPainelSolar"/> entities in the database.
		/// </summary>
		public DbSet<PotenciaPainelSolar> PotenciasDePaineisSolares { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Artigo"/> entities in the database.
		/// </summary>
		public DbSet<Artigo> Artigos { get; set; }


		public DbSet<Comentario> Comentario { get; set; }

		/// <summary>
		/// Configures the model and relationships in the database context.
		/// </summary>
		/// <param name="modelBuilder">The model builder used to configure the model.</param>
		/// <remarks>This method is used to configure entity relationships and seed data.</remarks>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			/// <summary>
			/// Configures the relationship between <see cref="DadosInstalacao"/> and <see cref="PotenciaPainelSolar"/>.
			/// </summary>
			modelBuilder.Entity<DadosInstalacao>()
				.HasOne(d => d.Potencia)
				.WithMany()
				.HasForeignKey(d => d.PotenciaId)
				.OnDelete(DeleteBehavior.NoAction);

			/// <summary>
			/// Configures the relationship between <see cref="PotenciaPainelSolar"/> and <see cref="ModeloPainelSolar"/>.
			/// </summary>
			modelBuilder.Entity<PotenciaPainelSolar>()
				.HasOne(p => p.ModeloPainelSolar)
				.WithMany(m => m.Potencias)
				.HasForeignKey(p => p.ModeloPainelId)
				.OnDelete(DeleteBehavior.Restrict);

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

			modelBuilder.Entity<User>().HasData(new User
			{
				Id = 1,
				Name = "Rodrigo",
				Email = "rodrigo.elias2003@gmail.com",
				Password = "rodrigoR123",
				IsAdmin = true
			});



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

			modelBuilder.Entity<Artigo>().HasData(
		new Artigo
		{
			ArtigoId = 1,
			Titulo = "Benefícios da Energia Solar",
			Conteudo = @"
A energia solar é uma fonte renovável e limpa que se está a tornar cada vez mais popular devido aos seus benefícios econômicos e ambientais. Este artigo explora as vantagens de adotar a energia solar tanto para residências quanto para empresas.

### Benefícios Econômicos
- **Redução de Custos:** A principal vantagem da energia solar é a redução da conta de energia elétrica. Ao gerar sua própria eletricidade, diminui a dependência da rede elétrica.
- **Valorização do Imóvel:** Imóveis que possuem sistemas de energia solar são geralmente mais valorizados no mercado, uma vez que têm custos operacionais menores e atraem compradores interessados em soluções sustentáveis.
- **Incentivos e Subsídios:** Em muitas regiões, o governo oferece incentivos fiscais e subsídios para a instalação de sistemas fotovoltaicos, tornando o investimento mais acessível.

### Benefícios Ambientais
- **Redução da Pegada de Carbono:** A energia solar não emite gases de efeito estufa, o que contribui significativamente para a redução da pegada de carbono.
- **Fontes Renováveis:** Ao contrário das fontes de energia tradicionais, como o carvão e o gás natural, a energia solar é renovável e não esgota os recursos naturais do planeta.

### Conclusão
Investir em sistemas de energia solar é uma escolha inteligente tanto do ponto de vista econômico quanto ambiental. Ao reduzir os custos com eletricidade e contribuir para a preservação do meio ambiente, a energia solar torna-se uma solução cada vez mais viável e atraente.",

			ImagemUrl = "~/images/energia-solar.jpg",
			Categoria = "Energia Renovável",
			DescricaoCurta = "Entenda os benefícios da energia solar para a sua residência ou empresa.",
			DataPublicacao = new DateTime(2025, 2, 15)
		},
		new Artigo
		{
			ArtigoId = 2,
			Titulo = "Como Calcular o Retorno sobre o Investimento em Energia Solar",
			Conteudo = @"
Calcular o Retorno sobre o Investimento (ROI) em sistemas fotovoltaicos é essencial para avaliar a viabilidade financeira de um projeto. Este artigo explica como calcular o ROI e por que ele é importante para qualquer instalação de energia solar.

### O que é o ROI?
O ROI é uma métrica financeira usada para avaliar o desempenho de um investimento. Ele calcula o lucro ou perda relativa ao valor investido e é expresso como uma porcentagem.

### Fórmula do ROI
A fórmula básica para calcular o ROI é a seguinte:
Para um sistema de energia solar, o retorno pode incluir a economia na conta de energia elétrica, o valor dos incentivos fiscais, e a possível valorização do imóvel. O custo do investimento inclui a instalação dos painéis solares, manutenção e outros custos operacionais. Se tiver interesse nesta máteria na nossa plataforma consegues aceder à ferramenta do cálculo do ROI.

### Exemplo de Cálculo do ROI
Suponhamos que investiu ´20.000 € numm sistema de energia solar e, ao longo do tempo, economizou 3.000 € anualmente na sua conta de energia elétrica. O cálculo do ROI seria: ROI (%) = (Retorno do Investimento / Custo do Investimento) x 100


Isso significa que, em média, terá um retorno de 15% do valor investido a cada ano.

### Conclusão
O cálculo do ROI ajuda a determinar se o investimento em energia solar vale a pena. Com os dados certos, consegue analisar e avaliar a viabilidade financeira e o tempo de retorno do seu investimento em energia solar.",

			ImagemUrl = "/images/artigos/calcular-roi.jpg", // ainda não encontrei uma: por fazer
			Categoria = "ROI",
			DescricaoCurta = "Aprenda a calcular o ROI de um sistema fotovoltaico e entenda se o investimento vale a pena.",
			DataPublicacao = new DateTime(2022, 2, 15)
		},
		new Artigo
		{
			ArtigoId = 3,
			Titulo = "Planeamento e Instalação de Sistemas de Energia Solar",
			Conteudo = @"
Planear e instalar um sistema de energia solar requer um processo detalhado e bem coordenado. Este artigo apresenta um guia completo sobre como planear e executar a instalação de um sistema fotovoltaico de forma eficiente.

### Passos para o Planeamento
1. **Análise de Viabilidade:** Antes de iniciar, é importante realizar uma análise detalhada do local, levando em consideração fatores como o consumo de energia, a localização e a inclinação do telhado.
2. **Dimensionamento do Sistema:** A quantidade de energia que um sistema solar pode gerar depende do número de painéis e da capacidade de cada um. O dimensionamento correto do sistema é crucial para maximizar a eficiência.
3. **Escolha dos Componentes:** Os componentes principais de um sistema solar são os painéis solares, o inversor e a estrutura de montagem. Escolher materiais de boa qualidade é essencial para garantir o bom funcionamento e a longevidade do sistema.

### Processo de Instalação
- **Instalação dos Painéis Solares:** Os painéis solares devem ser instalados de forma a otimizar a exposição solar, garantindo que eles recebam a maior quantidade de luz possível ao longo do dia.
- **Conexão Elétrica:** A instalação elétrica envolve a ligação dos painéis solares ao inversor, que converte a energia gerada em energia utilizável para a residência ou empresa.
- **Testes e Comissionamento:** Após a instalação, é necessário realizar testes para garantir que o sistema está funcionando corretamente e de forma segura.

### Conclusão
A instalação de sistemas solares é um processo técnico que exige um planeamento cuidadoso. Um bom planeamento e a escolha de profissionais qualificados podem garantir que o sistema solar seja eficiente e tenha uma vida útil longa.",
			ImagemUrl = "/images/artigos/planeamento-solar.jpg", // ainda não encontrei uma: por fazer
            Categoria = "Painéis Solares",
			DescricaoCurta = "Dicas essenciais para planear e instalar um sistema de energia solar de forma eficiente.",
			DataPublicacao = new DateTime(2024, 2, 15)
		}
);
			modelBuilder.Entity<Comentario>().HasData(
	new Comentario
	{
		Id = 1,
		Autor = "Rodrigo",
		ArtigoId = 1, 
		Texto = "Ótimo artigo! Muito informativo.",
		DataCriacao = new DateTime(2025, 2, 16),
		UserId = 1  
					
	},
	new Comentario
	{
		Id = 2,
		Autor="Rodrigo",
		ArtigoId = 1,
		Texto = "Gostei bastante das explicações sobre os benefícios ambientais!",
		DataCriacao = new DateTime(2025, 2, 17),
		UserId = 1 
	},
	new Comentario
	{
		Id = 3,
		Autor = "Rodrigo",
		ArtigoId = 2, 
		Texto = "A ferramenta de cálculo do ROI seria muito útil! Vocês têm alguma recomendação?",
		DataCriacao = new DateTime(2025, 3, 1),
		UserId = 1 
	}
);


		}
	}

}