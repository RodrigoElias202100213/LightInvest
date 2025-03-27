using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class InittialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artigos",
                columns: table => new
                {
                    ArtigoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagemUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<int>(type: "int", nullable: false),
                    DescricaoCurta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DataPublicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArtigoId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artigos", x => x.ArtigoId);
                    table.ForeignKey(
                        name: "FK_Artigos_Artigos_ArtigoId1",
                        column: x => x.ArtigoId1,
                        principalTable: "Artigos",
                        principalColumn: "ArtigoId");
                });

            migrationBuilder.CreateTable(
                name: "Cidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnergyConsumptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumoDiaSemana = table.Column<string>(type: "nvarchar(max)", maxLength: 24, nullable: false),
                    ConsumoFimSemana = table.Column<string>(type: "nvarchar(max)", maxLength: 24, nullable: false),
                    MesesOcupacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaSemana = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MediaFimSemana = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MediaAnual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConsumoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyConsumptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelosDePaineisSolares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModeloNome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelosDePaineisSolares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ROICalculators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustoInstalacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustoManutencaoAnual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConsumoEnergeticoMedio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConsumoEnergeticoRede = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetornoEconomia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ROI = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCalculado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROICalculators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarifas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrecoKWh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PotenciasDePaineisSolares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Potencia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ModeloPainelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PotenciasDePaineisSolares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PotenciasDePaineisSolares_ModelosDePaineisSolares_ModeloPainelId",
                        column: x => x.ModeloPainelId,
                        principalTable: "ModelosDePaineisSolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DadosInstalacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CidadeId = table.Column<int>(type: "int", nullable: false),
                    ModeloPainelId = table.Column<int>(type: "int", nullable: false),
                    PotenciaId = table.Column<int>(type: "int", nullable: false),
                    NumeroPaineis = table.Column<int>(type: "int", nullable: false),
                    Inclinacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dificuldade = table.Column<int>(type: "int", nullable: false),
                    PrecoInstalacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosInstalacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DadosInstalacao_Cidades_CidadeId",
                        column: x => x.CidadeId,
                        principalTable: "Cidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DadosInstalacao_ModelosDePaineisSolares_ModeloPainelId",
                        column: x => x.ModeloPainelId,
                        principalTable: "ModelosDePaineisSolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DadosInstalacao_PotenciasDePaineisSolares_PotenciaId",
                        column: x => x.PotenciaId,
                        principalTable: "PotenciasDePaineisSolares",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Artigos",
                columns: new[] { "ArtigoId", "ArtigoId1", "Categoria", "Conteudo", "DataPublicacao", "DescricaoCurta", "ImagemUrl", "Titulo" },
                values: new object[,]
                {
                    { 1, null, 0, "\r\nA energia solar é uma fonte renovável e limpa que está se tornando cada vez mais popular devido aos seus benefícios econômicos e ambientais. Este artigo explora as vantagens de adotar a energia solar tanto para residências quanto para empresas.\r\n\r\n### Benefícios Econômicos\r\n- **Redução de Custos:** A principal vantagem da energia solar é a redução da conta de energia elétrica. Ao gerar sua própria eletricidade, você diminui a dependência da rede elétrica.\r\n- **Valorização do Imóvel:** Imóveis que possuem sistemas de energia solar são geralmente mais valorizados no mercado, uma vez que têm custos operacionais menores e atraem compradores interessados em soluções sustentáveis.\r\n- **Incentivos e Subsídios:** Em muitas regiões, o governo oferece incentivos fiscais e subsídios para a instalação de sistemas fotovoltaicos, tornando o investimento mais acessível.\r\n\r\n### Benefícios Ambientais\r\n- **Redução da Pegada de Carbono:** A energia solar não emite gases de efeito estufa, o que contribui significativamente para a redução da pegada de carbono.\r\n- **Fontes Renováveis:** Ao contrário das fontes de energia tradicionais, como carvão e gás natural, a energia solar é renovável e não esgota os recursos naturais do planeta.\r\n\r\n### Conclusão\r\nInvestir em energia solar é uma escolha inteligente tanto do ponto de vista econômico quanto ambiental. Ao reduzir os custos com eletricidade e contribuir para a preservação do meio ambiente, a energia solar se torna uma solução cada vez mais viável e atraente.", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Entenda os benefícios da energia solar para sua residência ou empresa.", "/images/artigos/energia-solar.jpg", "Benefícios da Energia Solar" },
                    { 2, null, 2, "\r\nCalcular o Retorno sobre o Investimento (ROI) em sistemas fotovoltaicos é essencial para avaliar a viabilidade financeira de um projeto. Este artigo explica como calcular o ROI e por que ele é importante para qualquer instalação de energia solar.\r\n\r\n### O que é o ROI?\r\nO ROI é uma métrica financeira usada para avaliar o desempenho de um investimento. Ele calcula o lucro ou perda relativa ao valor investido e é expresso como uma porcentagem.\r\n\r\n### Fórmula do ROI\r\nA fórmula básica para calcular o ROI é a seguinte:\r\nPara um sistema de energia solar, o retorno pode incluir a economia na conta de energia elétrica, o valor dos incentivos fiscais, e a possível valorização do imóvel. O custo do investimento inclui a instalação dos painéis solares, manutenção e outros custos operacionais.\r\n\r\n### Exemplo de Cálculo do ROI\r\nSuponhamos que você tenha investido ´20.000 € em um sistema de energia solar e, ao longo do tempo, tenha economizado 3.000 € anualmente na sua conta de energia elétrica. O cálculo do ROI seria: ROI (%) = (Retorno do Investimento / Custo do Investimento) x 100\r\n\r\n\r\nIsso significa que, em média, você terá um retorno de 15% do valor investido a cada ano.\r\n\r\n### Conclusão\r\nO cálculo do ROI ajuda a determinar se o investimento em energia solar vale a pena. Com os dados certos, você pode projetar a viabilidade financeira e o tempo de retorno do seu investimento em energia solar.", new DateTime(2022, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aprenda a calcular o ROI de um sistema fotovoltaico e entenda se o investimento vale a pena.", "/images/artigos/calcular-roi.jpg", "Como Calcular o Retorno sobre o Investimento em Energia Solar" },
                    { 3, null, 3, "\r\nPlanejar e instalar um sistema de energia solar requer um processo detalhado e bem coordenado. Este artigo apresenta um guia completo sobre como planejar e executar a instalação de um sistema fotovoltaico de forma eficiente.\r\n\r\n### Passos para o Planejamento\r\n1. **Análise de Viabilidade:** Antes de iniciar, é importante realizar uma análise detalhada do local, levando em consideração fatores como o consumo de energia, a localização e a inclinação do telhado.\r\n2. **Dimensionamento do Sistema:** A quantidade de energia que um sistema solar pode gerar depende do número de painéis e da capacidade de cada um. O dimensionamento correto do sistema é crucial para maximizar a eficiência.\r\n3. **Escolha dos Componentes:** Os componentes principais de um sistema solar são os painéis solares, o inversor e a estrutura de montagem. Escolher materiais de boa qualidade é essencial para garantir o bom funcionamento e a longevidade do sistema.\r\n\r\n### Processo de Instalação\r\n- **Instalação dos Painéis Solares:** Os painéis solares devem ser instalados de forma a otimizar a exposição solar, garantindo que eles recebam a maior quantidade de luz possível ao longo do dia.\r\n- **Conexão Elétrica:** A instalação elétrica envolve a ligação dos painéis solares ao inversor, que converte a energia gerada em energia utilizável para a residência ou empresa.\r\n- **Testes e Comissionamento:** Após a instalação, é necessário realizar testes para garantir que o sistema está funcionando corretamente e de forma segura.\r\n\r\n### Conclusão\r\nA instalação de sistemas solares é um processo técnico que exige planejamento cuidadoso. Um bom planejamento e a escolha de profissionais qualificados podem garantir que o sistema solar seja eficiente e tenha uma vida útil longa.", new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dicas essenciais para planejar e instalar um sistema de energia solar de forma eficiente.", "/images/artigos/planejamento-solar.jpg", "Planejamento e Instalação de Sistemas de Energia Solar" }
                });

            migrationBuilder.InsertData(
                table: "Cidades",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Albufeira" },
                    { 2, "Almada" },
                    { 3, "Amadora" },
                    { 4, "Aveiro" },
                    { 5, "Barcelos" },
                    { 6, "Beja" },
                    { 7, "Braga" },
                    { 8, "Bragança" },
                    { 9, "Caldas da Rainha" },
                    { 10, "Cascais" },
                    { 11, "Coimbra" },
                    { 12, "Évora" },
                    { 13, "Faro" },
                    { 14, "Figueira da Foz" },
                    { 15, "Funchal" },
                    { 16, "Guarda" },
                    { 17, "Guimarães" },
                    { 18, "Leiria" },
                    { 19, "Lisboa" },
                    { 20, "Matosinhos" },
                    { 21, "Montijo" },
                    { 22, "Odivelas" },
                    { 23, "Oeiras" },
                    { 24, "Portalegre" },
                    { 25, "Portimão" },
                    { 26, "Porto" },
                    { 27, "Póvoa de Varzim" },
                    { 28, "Santarem" },
                    { 29, "Setúbal" },
                    { 30, "Barreiro" },
                    { 31, "Sintra" },
                    { 32, "Tomar" },
                    { 33, "Torres Vedras" },
                    { 34, "Viana do castelo" },
                    { 35, "Vila do Conde" },
                    { 36, "Vila Nova de Gaia" },
                    { 37, "Viseu" }
                });

            migrationBuilder.InsertData(
                table: "ModelosDePaineisSolares",
                columns: new[] { "Id", "ModeloNome", "Preco" },
                values: new object[,]
                {
                    { 1, "Aiko - Comet 2U", 1250.00m },
                    { 2, "Maxeon 7", 1320.00m },
                    { 3, "Longi - HI-MO X6", 1280.00m },
                    { 4, "Huasun - Himalaya", 1300.00m },
                    { 5, "TW Solar", 1230.00m },
                    { 6, "JA Solar DeepBlue 4.0 Pro", 1270.00m },
                    { 7, "Astroenergy - Astro N5", 1260.00m },
                    { 8, "Grand Sunergy", 1240.00m },
                    { 9, "DMEGC - Infinity RT", 1290.00m },
                    { 10, "Spic", 910.00m }
                });

            migrationBuilder.InsertData(
                table: "PotenciasDePaineisSolares",
                columns: new[] { "Id", "ModeloPainelId", "Potencia" },
                values: new object[,]
                {
                    { 1, 1, 670m },
                    { 2, 1, 680m },
                    { 3, 1, 690m },
                    { 4, 1, 700m },
                    { 5, 2, 445m },
                    { 6, 2, 455m },
                    { 7, 2, 465m },
                    { 8, 2, 475m },
                    { 9, 3, 600m },
                    { 10, 3, 610m },
                    { 11, 3, 620m },
                    { 12, 3, 630m },
                    { 13, 4, 720m },
                    { 14, 4, 730m },
                    { 15, 4, 740m },
                    { 16, 4, 750m },
                    { 17, 5, 715m },
                    { 18, 5, 725m },
                    { 19, 5, 735m },
                    { 20, 5, 745m },
                    { 21, 5, 590m },
                    { 22, 5, 600m },
                    { 23, 5, 610m },
                    { 24, 5, 620m },
                    { 25, 6, 595m },
                    { 26, 6, 605m },
                    { 27, 6, 615m },
                    { 28, 6, 625m },
                    { 29, 7, 640m },
                    { 30, 7, 650m },
                    { 31, 7, 660m },
                    { 32, 7, 670m },
                    { 33, 8, 710m },
                    { 34, 8, 720m },
                    { 35, 8, 730m },
                    { 36, 8, 740m },
                    { 37, 9, 615m },
                    { 38, 9, 625m },
                    { 39, 9, 635m },
                    { 40, 9, 645m },
                    { 41, 10, 410m },
                    { 42, 10, 420m },
                    { 43, 10, 430m },
                    { 44, 10, 440m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artigos_ArtigoId1",
                table: "Artigos",
                column: "ArtigoId1");

            migrationBuilder.CreateIndex(
                name: "IX_DadosInstalacao_CidadeId",
                table: "DadosInstalacao",
                column: "CidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_DadosInstalacao_ModeloPainelId",
                table: "DadosInstalacao",
                column: "ModeloPainelId");

            migrationBuilder.CreateIndex(
                name: "IX_DadosInstalacao_PotenciaId",
                table: "DadosInstalacao",
                column: "PotenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_PotenciasDePaineisSolares_ModeloPainelId",
                table: "PotenciasDePaineisSolares",
                column: "ModeloPainelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artigos");

            migrationBuilder.DropTable(
                name: "DadosInstalacao");

            migrationBuilder.DropTable(
                name: "EnergyConsumptions");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "ROICalculators");

            migrationBuilder.DropTable(
                name: "Tarifas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cidades");

            migrationBuilder.DropTable(
                name: "PotenciasDePaineisSolares");

            migrationBuilder.DropTable(
                name: "ModelosDePaineisSolares");
        }
    }
}
