using Microsoft.EntityFrameworkCore;
using LightInvest.Models.b;
using LightInvest.Models.Ener;
using LightInvest.Models.Roi;
using LightInvest.Models.Educ.Artigos;
using LightInvest.Models.Simulacao.Energ;
using LightInvest.Models.Simulacao.Tarifa;
using LightInvest.Models.Utilizador.Login;
using LightInvest.Models.Utilizador.Pass;
using Microsoft.EntityFrameworkCore.Migrations;

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


		/// <summary>
		/// Gets or sets the <see cref="ComentarioLike"/> entities in the database.
		/// </summary>
		public DbSet<ComentarioLike> ComentarioLike { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Comentario"/> entities in the database.
		/// </summary>
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

			modelBuilder.Entity<User>().HasData(
			new User { Id = 1, Name = "LighInvestSupport", Email = "lightinvestsup2425@gmail.com", Password = "LightInvestSup123", IsAdmin = true },
			new User { Id = 3, Name = "LightInvest", Email = "ligthinvestuser@gmail.com", Password = "LightInvest123", IsAdmin = false },

			new User { Id = 24, Name = "Rodrigo Elias", Email = "rodrigo.elias2003@gmail.com", Password = "rodrigoR123", IsAdmin = true },
			new User { Id = 25, Name = "Vera Fernandes", Email = "veragfernandes04@gmail.com", Password = "veraF123", IsAdmin = true },



			new User { Id = 4, Name = "Tiago Silva", Email = "tiagosilva@gmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 5, Name = "Ana Costa", Email = "anacosta@hotmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 6, Name = "João Ferreira", Email = "joaoferreira@sapo.pt", Password = "Password123", IsAdmin = false },
			new User { Id = 7, Name = "Marta Santos", Email = "martasantos@gmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 8, Name = "Bruno Rocha", Email = "brunorocha@hotmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 9, Name = "Carla Mendes", Email = "carlamendes@sapo.pt", Password = "Password123", IsAdmin = false },
			new User { Id = 10, Name = "Diogo Gomes", Email = "diogogomes@gmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 11, Name = "Filipa Ribeiro", Email = "filiparibeiro@hotmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 12, Name = "André Sousa", Email = "andresousa@sapo.pt", Password = "Password123", IsAdmin = false },
			new User { Id = 13, Name = "Raquel Almeida", Email = "raquelalmeida@gmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 14, Name = "Pedro Martins", Email = "pedromartins@hotmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 15, Name = "Sofia Lopes", Email = "sofialopes@sapo.pt", Password = "Password123", IsAdmin = false },
			new User { Id = 16, Name = "Ricardo Pinto", Email = "ricardopinto@gmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 17, Name = "Patrícia Nunes", Email = "patricianunes@hotmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 18, Name = "Luís Carvalho", Email = "luiscarvalho@sapo.pt", Password = "Password123", IsAdmin = false },
			new User { Id = 19, Name = "Beatriz Fonseca", Email = "beatrizfonseca@gmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 20, Name = "Miguel Teixeira", Email = "miguelteixeira@hotmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 21, Name = "Cátia Barros", Email = "catiabarros@sapo.pt", Password = "Password123", IsAdmin = false },
			new User { Id = 22, Name = "Hugo Correia", Email = "hugocorreia@gmail.com", Password = "Password123", IsAdmin = false },
			new User { Id = 23, Name = "Daniela Faria", Email = "danielafaria@hotmail.com", Password = "Password123", IsAdmin = false }

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


			modelBuilder.Entity<PotenciaPainelSolar>().HasData(
				new PotenciaPainelSolar { Id = 1, Potencia = 670, ModeloPainelId = 1 },
				new PotenciaPainelSolar { Id = 2, Potencia = 680, ModeloPainelId = 1 },
				new PotenciaPainelSolar { Id = 3, Potencia = 690, ModeloPainelId = 1 },
				new PotenciaPainelSolar { Id = 4, Potencia = 700, ModeloPainelId = 1 },

				new PotenciaPainelSolar { Id = 5, Potencia = 445, ModeloPainelId = 2 },
				new PotenciaPainelSolar { Id = 6, Potencia = 455, ModeloPainelId = 2 },
				new PotenciaPainelSolar { Id = 7, Potencia = 465, ModeloPainelId = 2 },
				new PotenciaPainelSolar { Id = 8, Potencia = 475, ModeloPainelId = 2 },

				new PotenciaPainelSolar { Id = 9, Potencia = 600, ModeloPainelId = 3 },
				new PotenciaPainelSolar { Id = 10, Potencia = 610, ModeloPainelId = 3 },
				new PotenciaPainelSolar { Id = 11, Potencia = 620, ModeloPainelId = 3 },
				new PotenciaPainelSolar { Id = 12, Potencia = 630, ModeloPainelId = 3 },

				new PotenciaPainelSolar { Id = 13, Potencia = 720, ModeloPainelId = 4 },
				new PotenciaPainelSolar { Id = 14, Potencia = 730, ModeloPainelId = 4 },
				new PotenciaPainelSolar { Id = 15, Potencia = 740, ModeloPainelId = 4 },
				new PotenciaPainelSolar { Id = 16, Potencia = 750, ModeloPainelId = 4 },

				new PotenciaPainelSolar { Id = 17, Potencia = 715, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 18, Potencia = 725, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 19, Potencia = 735, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 20, Potencia = 745, ModeloPainelId = 5 },

				new PotenciaPainelSolar { Id = 21, Potencia = 590, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 22, Potencia = 600, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 23, Potencia = 610, ModeloPainelId = 5 },
				new PotenciaPainelSolar { Id = 24, Potencia = 620, ModeloPainelId = 5 },

				new PotenciaPainelSolar { Id = 25, Potencia = 595, ModeloPainelId = 6 },
				new PotenciaPainelSolar { Id = 26, Potencia = 605, ModeloPainelId = 6 },
				new PotenciaPainelSolar { Id = 27, Potencia = 615, ModeloPainelId = 6 },
				new PotenciaPainelSolar { Id = 28, Potencia = 625, ModeloPainelId = 6 },

				new PotenciaPainelSolar { Id = 29, Potencia = 640, ModeloPainelId = 7 },
				new PotenciaPainelSolar { Id = 30, Potencia = 650, ModeloPainelId = 7 },
				new PotenciaPainelSolar { Id = 31, Potencia = 660, ModeloPainelId = 7 },
				new PotenciaPainelSolar { Id = 32, Potencia = 670, ModeloPainelId = 7 },

				new PotenciaPainelSolar { Id = 33, Potencia = 710, ModeloPainelId = 8 },
				new PotenciaPainelSolar { Id = 34, Potencia = 720, ModeloPainelId = 8 },
				new PotenciaPainelSolar { Id = 35, Potencia = 730, ModeloPainelId = 8 },
				new PotenciaPainelSolar { Id = 36, Potencia = 740, ModeloPainelId = 8 },

				new PotenciaPainelSolar { Id = 37, Potencia = 615, ModeloPainelId = 9 },
				new PotenciaPainelSolar { Id = 38, Potencia = 625, ModeloPainelId = 9 },
				new PotenciaPainelSolar { Id = 39, Potencia = 635, ModeloPainelId = 9 },
				new PotenciaPainelSolar { Id = 40, Potencia = 645, ModeloPainelId = 9 },

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

			ImagemUrl = "/images/artigos/calcular-roi.jpg",
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
			ImagemUrl = "/images/artigos/planeamento-solar.jpg",
			Categoria = "Painéis Solares",
			DescricaoCurta = "Dicas essenciais para planear e instalar um sistema de energia solar de forma eficiente.",
			DataPublicacao = new DateTime(2024, 2, 15)
		},
		new Artigo
		{
			ArtigoId = 4,
			Titulo = "Como Reduzir a Fatura de Energia com Eficiência Energética",
			Conteudo = @"
A eficiência energética é uma das estratégias mais eficazes para reduzir a fatura de eletricidade. Este artigo apresenta algumas práticas simples e acessíveis que podem fazer a diferença no consumo de energia.

### Dicas de Eficiência Energética
1. **Iluminação LED:** Trocar lâmpadas incandescentes por LEDs pode reduzir significativamente o consumo de energia em casa ou na empresa.
2. **Equipamentos Eficientes:** Optar por eletrodomésticos com classificação energética A+++ garante menor consumo de energia.
3. **Isolamento Térmico:** Melhorar o isolamento térmico das divisões reduz o uso excessivo de aquecedores e ar-condicionado.

### Benefícios
- Redução imediata na fatura mensal;
- Menor impacto ambiental;
- Valorização do imóvel.

### Conclusão
Investir em eficiência energética é uma solução sustentável que permite poupar dinheiro e proteger o meio ambiente.",
			ImagemUrl = "/images/artigos/eficiencia-energetica.jpg",
			Categoria = "Energia Renovável",
			DescricaoCurta = "Descubra como poupar energia e reduzir custos com medidas simples e eficientes.",
			DataPublicacao = new DateTime(2024, 3, 10)
		},
new Artigo
{
	ArtigoId = 5,
	Titulo = "Vantagens de Investir em Energia Renovável em Portugal",
	Conteudo = @"
Portugal tem-se destacado como um dos países europeus com maior aposta em energias renováveis. Conhece as principais vantagens de investir nesta área.

### Benefícios das Renováveis
1. **Redução de Custos:** A longo prazo, os investimentos em energia solar ou eólica geram poupanças consideráveis na fatura de energia.
2. **Sustentabilidade Ambiental:** Reduzem a pegada de carbono e protegem os recursos naturais.
3. **Incentivos e Apoios:** Existem vários programas e incentivos estatais para quem pretende investir em energias renováveis.

### Perspetivas Futuras
Portugal continuará a expandir a sua produção de energia limpa, criando oportunidades de investimento e desenvolvimento tecnológico.

### Conclusão
Investir em energias renováveis em Portugal é uma decisão inteligente, com benefícios económicos e ambientais a curto e longo prazo.",
	ImagemUrl = "/images/artigos/renovaveis-portugal.jpg",
	Categoria = "Energia Renovável",
	DescricaoCurta = "Conheça as principais vantagens de apostar nas energias renováveis em Portugal.",
	DataPublicacao = new DateTime(2024, 4, 5)
},
new Artigo
{
	ArtigoId = 6,
	Titulo = "Manutenção Preventiva de Painéis Solares: Boas Práticas",
	Conteudo = @"
A manutenção preventiva dos painéis solares é fundamental para garantir o seu correto funcionamento e prolongar a sua vida útil. Neste artigo, partilhamos boas práticas essenciais.

### Cuidados a Ter
1. **Limpeza Regular:** A acumulação de poeira, folhas ou resíduos nos painéis pode reduzir a eficiência da produção de energia.
2. **Inspeção de Cablagens:** Verificar se os cabos e conexões estão em bom estado evita perdas de energia ou avarias.
3. **Monitorização de Desempenho:** Utilizar sistemas de monitorização permite detetar rapidamente qualquer anomalia.

### Benefícios da Manutenção
- Aumento da eficiência energética;
- Prevenção de danos graves;
- Maior rentabilidade do investimento.

### Conclusão
A manutenção preventiva dos painéis solares é um passo simples, mas essencial, para garantir o melhor desempenho do sistema a longo prazo.",
	ImagemUrl = "/images/artigos/manutencao-paineis.jpg",
	Categoria = "Painéis Solares",
	DescricaoCurta = "Aprenda boas práticas para manter os painéis solares sempre eficientes e seguros.",
	DataPublicacao = new DateTime(2024, 5, 2)
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
				UserId = 1,
			},
			new Comentario
			{
				Id = 2,
				Autor = "Rodrigo",
				ArtigoId = 1,
				Texto = "Gostei bastante das explicações sobre os benefícios ambientais!",
				DataCriacao = new DateTime(2025, 2, 17),
				UserId = 1
			},
			 new Comentario { Id = 3, Autor = "Rodrigo", ArtigoId = 2, Texto = "A ferramenta de cálculo do ROI seria muito útil! Vocês têm alguma recomendação?", DataCriacao = new DateTime(2025, 3, 1), UserId = 1 },
			new Comentario { Id = 4, Autor = "Tiago Silva", ArtigoId = 1, Texto = "Artigo muito esclarecedor, obrigado pela partilha!", DataCriacao = new DateTime(2025, 3, 2), UserId = 4 },
			new Comentario { Id = 5, Autor = "Ana Costa", ArtigoId = 1, Texto = "Gostava de ver mais exemplos práticos.", DataCriacao = new DateTime(2025, 3, 2), UserId = 5 },
			new Comentario { Id = 6, Autor = "João Ferreira", ArtigoId = 1, Texto = "Concordo plenamente com o que foi escrito.", DataCriacao = new DateTime(2025, 3, 3), UserId = 6 },
			new Comentario { Id = 7, Autor = "Marta Santos", ArtigoId = 1, Texto = "Já sigo o vosso site há algum tempo, excelente trabalho.", DataCriacao = new DateTime(2025, 3, 3), UserId = 7 },
			new Comentario { Id = 8, Autor = "Bruno Rocha", ArtigoId = 2, Texto = "Também tenho dúvidas em relação ao ROI, podiam fazer um artigo só sobre isso.", DataCriacao = new DateTime(2025, 3, 4), UserId = 8 },
			new Comentario { Id = 9, Autor = "Carla Mendes", ArtigoId = 2, Texto = "Muito bom conteúdo, continue assim!", DataCriacao = new DateTime(2025, 3, 4), UserId = 9 },
			new Comentario { Id = 10, Autor = "Diogo Gomes", ArtigoId = 2, Texto = "Conseguem partilhar fontes adicionais sobre este tema?", DataCriacao = new DateTime(2025, 3, 5), UserId = 10 },
			new Comentario { Id = 11, Autor = "Filipa Ribeiro", ArtigoId = 2, Texto = "Gostei muito deste artigo, bem explicado.", DataCriacao = new DateTime(2025, 3, 5), UserId = 11 },
			new Comentario { Id = 12, Autor = "André Sousa", ArtigoId = 2, Texto = "Tenho uma sugestão de tema: fundos de investimento.", DataCriacao = new DateTime(2025, 3, 6), UserId = 12 },
			new Comentario { Id = 13, Autor = "Raquel Almeida", ArtigoId = 3, Texto = "Excelente conteúdo, parabéns!", DataCriacao = new DateTime(2025, 3, 6), UserId = 13 },
			new Comentario { Id = 14, Autor = "Pedro Martins", ArtigoId = 3, Texto = "Muito completo e detalhado.", DataCriacao = new DateTime(2025, 3, 7), UserId = 14 },
			new Comentario { Id = 15, Autor = "Sofia Lopes", ArtigoId = 3, Texto = "Era bom ter uma versão em vídeo também.", DataCriacao = new DateTime(2025, 3, 7), UserId = 15 },
			new Comentario { Id = 16, Autor = "Ricardo Pinto", ArtigoId = 3, Texto = "Óptima explicação dos conceitos base.", DataCriacao = new DateTime(2025, 3, 8), UserId = 16 },
			new Comentario { Id = 17, Autor = "Patrícia Nunes", ArtigoId = 3, Texto = "A parte dos exemplos ajudou-me muito.", DataCriacao = new DateTime(2025, 3, 8), UserId = 17 },
			new Comentario { Id = 18, Autor = "Luís Carvalho", ArtigoId = 1, Texto = "Gostava de saber mais sobre análise técnica.", DataCriacao = new DateTime(2025, 3, 9), UserId = 18 },
			new Comentario { Id = 19, Autor = "Beatriz Fonseca", ArtigoId = 1, Texto = "Parabéns pelo artigo, muito bem escrito.", DataCriacao = new DateTime(2025, 3, 9), UserId = 19 },
			new Comentario { Id = 20, Autor = "Miguel Teixeira", ArtigoId = 1, Texto = "O conteúdo foi muito útil para mim, obrigado.", DataCriacao = new DateTime(2025, 3, 10), UserId = 20 },
			new Comentario { Id = 21, Autor = "Cátia Barros", ArtigoId = 2, Texto = "Seria interessante aprofundar sobre ETFs.", DataCriacao = new DateTime(2025, 3, 10), UserId = 21 },
			new Comentario { Id = 22, Autor = "Hugo Correia", ArtigoId = 2, Texto = "Já partilhei com amigos, muito bom!", DataCriacao = new DateTime(2025, 3, 11), UserId = 22 },
			new Comentario { Id = 23, Autor = "Daniela Faria", ArtigoId = 2, Texto = "Ajudou-me a perceber melhor o mercado.", DataCriacao = new DateTime(2025, 3, 11), UserId = 23 },
			new Comentario { Id = 24, Autor = "Tiago Silva", ArtigoId = 5, Texto = "Este artigo me ajudou a entender melhor a importância da análise de mercado. Parabéns!", DataCriacao = new DateTime(2025, 3, 12), UserId = 4 },
			new Comentario { Id = 25, Autor = "Ana Costa", ArtigoId = 5, Texto = "Muito interessante, mas eu gostaria de mais exemplos de ferramentas.", DataCriacao = new DateTime(2025, 3, 12), UserId = 5 },
			new Comentario { Id = 26, Autor = "João Ferreira", ArtigoId = 5, Texto = "Acho que poderiam adicionar mais estudos de caso sobre ROI. Isso ajudaria bastante.", DataCriacao = new DateTime(2025, 3, 13), UserId = 6 },
			new Comentario { Id = 27, Autor = "Marta Santos", ArtigoId = 5, Texto = "Adorei a forma como o conteúdo foi estruturado. Fácil de entender!", DataCriacao = new DateTime(2025, 3, 13), UserId = 7 },
			new Comentario { Id = 28, Autor = "Bruno Rocha", ArtigoId = 6, Texto = "Muito bom, já compartilhei com a minha rede de contatos. Espero ver mais artigos assim.", DataCriacao = new DateTime(2025, 3, 14), UserId = 8 },
			new Comentario { Id = 29, Autor = "Carla Mendes", ArtigoId = 6, Texto = "Preciso de mais informações sobre como aplicar isso em investimentos pessoais.", DataCriacao = new DateTime(2025, 3, 14), UserId = 9 },
			new Comentario { Id = 30, Autor = "Diogo Gomes", ArtigoId = 6, Texto = "Muito relevante para quem está iniciando no mercado financeiro. Obrigado pelo conteúdo.", DataCriacao = new DateTime(2025, 3, 15), UserId = 10 }
			);
		}

	}
}