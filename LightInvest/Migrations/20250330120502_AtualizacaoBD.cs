using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Artigos",
                keyColumn: "ArtigoId",
                keyValue: 1,
                column: "Conteudo",
                value: "\r\nA energia solar é uma fonte renovável e limpa que se está a tornar cada vez mais popular devido aos seus benefícios econômicos e ambientais. Este artigo explora as vantagens de adotar a energia solar tanto para residências quanto para empresas.\r\n\r\n### Benefícios Econômicos\r\n- **Redução de Custos:** A principal vantagem da energia solar é a redução da conta de energia elétrica. Ao gerar sua própria eletricidade, diminui a dependência da rede elétrica.\r\n- **Valorização do Imóvel:** Imóveis que possuem sistemas de energia solar são geralmente mais valorizados no mercado, uma vez que têm custos operacionais menores e atraem compradores interessados em soluções sustentáveis.\r\n- **Incentivos e Subsídios:** Em muitas regiões, o governo oferece incentivos fiscais e subsídios para a instalação de sistemas fotovoltaicos, tornando o investimento mais acessível.\r\n\r\n### Benefícios Ambientais\r\n- **Redução da Pegada de Carbono:** A energia solar não emite gases de efeito estufa, o que contribui significativamente para a redução da pegada de carbono.\r\n- **Fontes Renováveis:** Ao contrário das fontes de energia tradicionais, como o carvão e o gás natural, a energia solar é renovável e não esgota os recursos naturais do planeta.\r\n\r\n### Conclusão\r\nInvestir em sistemas de energia solar é uma escolha inteligente tanto do ponto de vista econômico quanto ambiental. Ao reduzir os custos com eletricidade e contribuir para a preservação do meio ambiente, a energia solar torna-se uma solução cada vez mais viável e atraente.");

            migrationBuilder.UpdateData(
                table: "Artigos",
                keyColumn: "ArtigoId",
                keyValue: 2,
                column: "Conteudo",
                value: "\r\nCalcular o Retorno sobre o Investimento (ROI) em sistemas fotovoltaicos é essencial para avaliar a viabilidade financeira de um projeto. Este artigo explica como calcular o ROI e por que ele é importante para qualquer instalação de energia solar.\r\n\r\n### O que é o ROI?\r\nO ROI é uma métrica financeira usada para avaliar o desempenho de um investimento. Ele calcula o lucro ou perda relativa ao valor investido e é expresso como uma porcentagem.\r\n\r\n### Fórmula do ROI\r\nA fórmula básica para calcular o ROI é a seguinte:\r\nPara um sistema de energia solar, o retorno pode incluir a economia na conta de energia elétrica, o valor dos incentivos fiscais, e a possível valorização do imóvel. O custo do investimento inclui a instalação dos painéis solares, manutenção e outros custos operacionais. Se tiver interesse nesta máteria na nossa plataforma consegues aceder à ferramenta do cálculo do ROI.\r\n\r\n### Exemplo de Cálculo do ROI\r\nSuponhamos que investiu ´20.000 € numm sistema de energia solar e, ao longo do tempo, economizou 3.000 € anualmente na sua conta de energia elétrica. O cálculo do ROI seria: ROI (%) = (Retorno do Investimento / Custo do Investimento) x 100\r\n\r\n\r\nIsso significa que, em média, terá um retorno de 15% do valor investido a cada ano.\r\n\r\n### Conclusão\r\nO cálculo do ROI ajuda a determinar se o investimento em energia solar vale a pena. Com os dados certos, consegue analisar e avaliar a viabilidade financeira e o tempo de retorno do seu investimento em energia solar.");

            migrationBuilder.UpdateData(
                table: "Artigos",
                keyColumn: "ArtigoId",
                keyValue: 3,
                column: "Conteudo",
                value: "\r\nPlanear e instalar um sistema de energia solar requer um processo detalhado e bem coordenado. Este artigo apresenta um guia completo sobre como planear e executar a instalação de um sistema fotovoltaico de forma eficiente.\r\n\r\n### Passos para o Planeamento\r\n1. **Análise de Viabilidade:** Antes de iniciar, é importante realizar uma análise detalhada do local, levando em consideração fatores como o consumo de energia, a localização e a inclinação do telhado.\r\n2. **Dimensionamento do Sistema:** A quantidade de energia que um sistema solar pode gerar depende do número de painéis e da capacidade de cada um. O dimensionamento correto do sistema é crucial para maximizar a eficiência.\r\n3. **Escolha dos Componentes:** Os componentes principais de um sistema solar são os painéis solares, o inversor e a estrutura de montagem. Escolher materiais de boa qualidade é essencial para garantir o bom funcionamento e a longevidade do sistema.\r\n\r\n### Processo de Instalação\r\n- **Instalação dos Painéis Solares:** Os painéis solares devem ser instalados de forma a otimizar a exposição solar, garantindo que eles recebam a maior quantidade de luz possível ao longo do dia.\r\n- **Conexão Elétrica:** A instalação elétrica envolve a ligação dos painéis solares ao inversor, que converte a energia gerada em energia utilizável para a residência ou empresa.\r\n- **Testes e Comissionamento:** Após a instalação, é necessário realizar testes para garantir que o sistema está funcionando corretamente e de forma segura.\r\n\r\n### Conclusão\r\nA instalação de sistemas solares é um processo técnico que exige um planeamento cuidadoso. Um bom planeamento e a escolha de profissionais qualificados podem garantir que o sistema solar seja eficiente e tenha uma vida útil longa.");

            migrationBuilder.UpdateData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 1,
                column: "Autor",
                value: "Rodrigo");

            migrationBuilder.UpdateData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 2,
                column: "Autor",
                value: "Rodrigo");

            migrationBuilder.UpdateData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 3,
                column: "Autor",
                value: "Rodrigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Artigos",
                keyColumn: "ArtigoId",
                keyValue: 1,
                column: "Conteudo",
                value: "\r\n				A energia solar é uma fonte renovável e limpa que se está a tornar cada vez mais popular devido aos seus benefícios econômicos e ambientais. Este artigo explora as vantagens de adotar a energia solar tanto para residências quanto para empresas.\r\n\r\n				### Benefícios Econômicos\r\n				- **Redução de Custos:** A principal vantagem da energia solar é a redução da conta de energia elétrica. Ao gerar sua própria eletricidade, diminui a dependência da rede elétrica.\r\n				- **Valorização do Imóvel:** Imóveis que possuem sistemas de energia solar são geralmente mais valorizados no mercado, uma vez que têm custos operacionais menores e atraem compradores interessados em soluções sustentáveis.\r\n				- **Incentivos e Subsídios:** Em muitas regiões, o governo oferece incentivos fiscais e subsídios para a instalação de sistemas fotovoltaicos, tornando o investimento mais acessível.\r\n\r\n				### Benefícios Ambientais\r\n				- **Redução da Pegada de Carbono:** A energia solar não emite gases de efeito estufa, o que contribui significativamente para a redução da pegada de carbono.\r\n				- **Fontes Renováveis:** Ao contrário das fontes de energia tradicionais, como o carvão e o gás natural, a energia solar é renovável e não esgota os recursos naturais do planeta.\r\n\r\n				### Conclusão\r\n				Investir em sistemas de energia solar é uma escolha inteligente tanto do ponto de vista econômico quanto ambiental. Ao reduzir os custos com eletricidade e contribuir para a preservação do meio ambiente, a energia solar torna-se uma solução cada vez mais viável e atraente.");

            migrationBuilder.UpdateData(
                table: "Artigos",
                keyColumn: "ArtigoId",
                keyValue: 2,
                column: "Conteudo",
                value: "\r\n				Calcular o Retorno sobre o Investimento (ROI) em sistemas fotovoltaicos é essencial para avaliar a viabilidade financeira de um projeto. Este artigo explica como calcular o ROI e por que ele é importante para qualquer instalação de energia solar.\r\n\r\n				### O que é o ROI?\r\n				O ROI é uma métrica financeira usada para avaliar o desempenho de um investimento. Ele calcula o lucro ou perda relativa ao valor investido e é expresso como uma porcentagem.\r\n\r\n				### Fórmula do ROI\r\n				A fórmula básica para calcular o ROI é a seguinte:\r\n				Para um sistema de energia solar, o retorno pode incluir a economia na conta de energia elétrica, o valor dos incentivos fiscais, e a possível valorização do imóvel. O custo do investimento inclui a instalação dos painéis solares, manutenção e outros custos operacionais. Se tiver interesse nesta máteria na nossa plataforma consegues aceder à ferramenta do cálculo do ROI.\r\n\r\n				### Exemplo de Cálculo do ROI\r\n				Suponhamos que investiu ´20.000 € numm sistema de energia solar e, ao longo do tempo, economizou 3.000 € anualmente na sua conta de energia elétrica. O cálculo do ROI seria: ROI (%) = (Retorno do Investimento / Custo do Investimento) x 100\r\n\r\n\r\n				Isso significa que, em média, terá um retorno de 15% do valor investido a cada ano.\r\n\r\n				### Conclusão\r\n				O cálculo do ROI ajuda a determinar se o investimento em energia solar vale a pena. Com os dados certos, consegue analisar e avaliar a viabilidade financeira e o tempo de retorno do seu investimento em energia solar.");

            migrationBuilder.UpdateData(
                table: "Artigos",
                keyColumn: "ArtigoId",
                keyValue: 3,
                column: "Conteudo",
                value: "\r\n				Planear e instalar um sistema de energia solar requer um processo detalhado e bem coordenado. Este artigo apresenta um guia completo sobre como planear e executar a instalação de um sistema fotovoltaico de forma eficiente.\r\n\r\n				### Passos para o Planeamento\r\n				1. **Análise de Viabilidade:** Antes de iniciar, é importante realizar uma análise detalhada do local, levando em consideração fatores como o consumo de energia, a localização e a inclinação do telhado.\r\n				2. **Dimensionamento do Sistema:** A quantidade de energia que um sistema solar pode gerar depende do número de painéis e da capacidade de cada um. O dimensionamento correto do sistema é crucial para maximizar a eficiência.\r\n				3. **Escolha dos Componentes:** Os componentes principais de um sistema solar são os painéis solares, o inversor e a estrutura de montagem. Escolher materiais de boa qualidade é essencial para garantir o bom funcionamento e a longevidade do sistema.\r\n\r\n				### Processo de Instalação\r\n				- **Instalação dos Painéis Solares:** Os painéis solares devem ser instalados de forma a otimizar a exposição solar, garantindo que eles recebam a maior quantidade de luz possível ao longo do dia.\r\n				- **Conexão Elétrica:** A instalação elétrica envolve a ligação dos painéis solares ao inversor, que converte a energia gerada em energia utilizável para a residência ou empresa.\r\n				- **Testes e Comissionamento:** Após a instalação, é necessário realizar testes para garantir que o sistema está funcionando corretamente e de forma segura.\r\n\r\n				### Conclusão\r\n				A instalação de sistemas solares é um processo técnico que exige um planeamento cuidadoso. Um bom planeamento e a escolha de profissionais qualificados podem garantir que o sistema solar seja eficiente e tenha uma vida útil longa.");

            migrationBuilder.UpdateData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 1,
                column: "Autor",
                value: "nome");

            migrationBuilder.UpdateData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 2,
                column: "Autor",
                value: "nome");

            migrationBuilder.UpdateData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 3,
                column: "Autor",
                value: "nome");
        }
    }
}
