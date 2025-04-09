using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class populate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Artigos",
                columns: new[] { "ArtigoId", "ArtigoId1", "Categoria", "Conteudo", "DataPublicacao", "DescricaoCurta", "ImagemUrl", "Titulo" },
                values: new object[,]
                {
                    { 4, null, "Energia Renovável", "\r\nA eficiência energética é uma das estratégias mais eficazes para reduzir a fatura de eletricidade. Este artigo apresenta algumas práticas simples e acessíveis que podem fazer a diferença no consumo de energia.\r\n\r\n### Dicas de Eficiência Energética\r\n1. **Iluminação LED:** Trocar lâmpadas incandescentes por LEDs pode reduzir significativamente o consumo de energia em casa ou na empresa.\r\n2. **Equipamentos Eficientes:** Optar por eletrodomésticos com classificação energética A+++ garante menor consumo de energia.\r\n3. **Isolamento Térmico:** Melhorar o isolamento térmico das divisões reduz o uso excessivo de aquecedores e ar-condicionado.\r\n\r\n### Benefícios\r\n- Redução imediata na fatura mensal;\r\n- Menor impacto ambiental;\r\n- Valorização do imóvel.\r\n\r\n### Conclusão\r\nInvestir em eficiência energética é uma solução sustentável que permite poupar dinheiro e proteger o meio ambiente.", new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Descobre como poupar energia e reduzir custos com medidas simples e eficientes.", "/images/artigos/eficiencia-energetica.jpg", "Como Reduzir a Fatura de Energia com Eficiência Energética" },
                    { 5, null, "Energia Renovável", "\r\nPortugal tem-se destacado como um dos países europeus com maior aposta em energias renováveis. Conhece as principais vantagens de investir nesta área.\r\n\r\n### Benefícios das Renováveis\r\n1. **Redução de Custos:** A longo prazo, os investimentos em energia solar ou eólica geram poupanças consideráveis na fatura de energia.\r\n2. **Sustentabilidade Ambiental:** Reduzem a pegada de carbono e protegem os recursos naturais.\r\n3. **Incentivos e Apoios:** Existem vários programas e incentivos estatais para quem pretende investir em energias renováveis.\r\n\r\n### Perspetivas Futuras\r\nPortugal continuará a expandir a sua produção de energia limpa, criando oportunidades de investimento e desenvolvimento tecnológico.\r\n\r\n### Conclusão\r\nInvestir em energias renováveis em Portugal é uma decisão inteligente, com benefícios económicos e ambientais a curto e longo prazo.", new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Conhece as principais vantagens de apostar nas energias renováveis em Portugal.", "/images/artigos/renovaveis-portugal.jpg", "Vantagens de Investir em Energia Renovável em Portugal" },
                    { 6, null, "Painéis Solares", "\r\nA manutenção preventiva dos painéis solares é fundamental para garantir o seu correto funcionamento e prolongar a sua vida útil. Neste artigo, partilhamos boas práticas essenciais.\r\n\r\n### Cuidados a Ter\r\n1. **Limpeza Regular:** A acumulação de poeira, folhas ou resíduos nos painéis pode reduzir a eficiência da produção de energia.\r\n2. **Inspeção de Cablagens:** Verificar se os cabos e conexões estão em bom estado evita perdas de energia ou avarias.\r\n3. **Monitorização de Desempenho:** Utilizar sistemas de monitorização permite detetar rapidamente qualquer anomalia.\r\n\r\n### Benefícios da Manutenção\r\n- Aumento da eficiência energética;\r\n- Prevenção de danos graves;\r\n- Maior rentabilidade do investimento.\r\n\r\n### Conclusão\r\nA manutenção preventiva dos painéis solares é um passo simples, mas essencial, para garantir o melhor desempenho do sistema a longo prazo.", new DateTime(2024, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aprende boas práticas para manter os painéis solares sempre eficientes e seguros.", "/images/artigos/manutencao-paineis.jpg", "Manutenção Preventiva de Painéis Solares: Boas Práticas" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "LightInvest");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsAdmin", "Name", "Password" },
                values: new object[,]
                {
                    { 4, "tiagosilva@gmail.com", false, "Tiago Silva", "Password123" },
                    { 5, "anacosta@hotmail.com", false, "Ana Costa", "Password123" },
                    { 6, "joaoferreira@sapo.pt", false, "João Ferreira", "Password123" },
                    { 7, "martasantos@gmail.com", false, "Marta Santos", "Password123" },
                    { 8, "brunorocha@hotmail.com", false, "Bruno Rocha", "Password123" },
                    { 9, "carlamendes@sapo.pt", false, "Carla Mendes", "Password123" },
                    { 10, "diogogomes@gmail.com", false, "Diogo Gomes", "Password123" },
                    { 11, "filiparibeiro@hotmail.com", false, "Filipa Ribeiro", "Password123" },
                    { 12, "andresousa@sapo.pt", false, "André Sousa", "Password123" },
                    { 13, "raquelalmeida@gmail.com", false, "Raquel Almeida", "Password123" },
                    { 14, "pedromartins@hotmail.com", false, "Pedro Martins", "Password123" },
                    { 15, "sofialopes@sapo.pt", false, "Sofia Lopes", "Password123" },
                    { 16, "ricardopinto@gmail.com", false, "Ricardo Pinto", "Password123" },
                    { 17, "patricianunes@hotmail.com", false, "Patrícia Nunes", "Password123" },
                    { 18, "luiscarvalho@sapo.pt", false, "Luís Carvalho", "Password123" },
                    { 19, "beatrizfonseca@gmail.com", false, "Beatriz Fonseca", "Password123" },
                    { 20, "miguelteixeira@hotmail.com", false, "Miguel Teixeira", "Password123" },
                    { 21, "catiabarros@sapo.pt", false, "Cátia Barros", "Password123" },
                    { 22, "hugocorreia@gmail.com", false, "Hugo Correia", "Password123" },
                    { 23, "danielafaria@hotmail.com", false, "Daniela Faria", "Password123" },
                    { 24, "rodrigo.elias2003@gmail.com", true, "Rodrigo Elias", "rodrigoR123" },
                    { 25, "veragfernandes04@gmail.com", true, "Vera Fernandes", "veraF123" }
                });

            migrationBuilder.InsertData(
                table: "Comentario",
                columns: new[] { "Id", "ArtigoId", "Autor", "DataCriacao", "Texto", "UserId" },
                values: new object[,]
                {
                    { 4, 1, "Tiago Silva", new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Artigo muito esclarecedor, obrigado pela partilha!", 4 },
                    { 5, 1, "Ana Costa", new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gostava de ver mais exemplos práticos.", 5 },
                    { 6, 1, "João Ferreira", new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Concordo plenamente com o que foi escrito.", 6 },
                    { 7, 1, "Marta Santos", new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Já sigo o vosso site há algum tempo, excelente trabalho.", 7 },
                    { 8, 2, "Bruno Rocha", new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Também tenho dúvidas em relação ao ROI, podiam fazer um artigo só sobre isso.", 8 },
                    { 9, 2, "Carla Mendes", new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Muito bom conteúdo, continue assim!", 9 },
                    { 10, 2, "Diogo Gomes", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Conseguem partilhar fontes adicionais sobre este tema?", 10 },
                    { 11, 2, "Filipa Ribeiro", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gostei muito deste artigo, bem explicado.", 11 },
                    { 12, 2, "André Sousa", new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tenho uma sugestão de tema: fundos de investimento.", 12 },
                    { 13, 3, "Raquel Almeida", new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Excelente conteúdo, parabéns!", 13 },
                    { 14, 3, "Pedro Martins", new DateTime(2025, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Muito completo e detalhado.", 14 },
                    { 15, 3, "Sofia Lopes", new DateTime(2025, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Era bom ter uma versão em vídeo também.", 15 },
                    { 16, 3, "Ricardo Pinto", new DateTime(2025, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Óptima explicação dos conceitos base.", 16 },
                    { 17, 3, "Patrícia Nunes", new DateTime(2025, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "A parte dos exemplos ajudou-me muito.", 17 },
                    { 18, 1, "Luís Carvalho", new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gostava de saber mais sobre análise técnica.", 18 },
                    { 19, 1, "Beatriz Fonseca", new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Parabéns pelo artigo, muito bem escrito.", 19 },
                    { 20, 1, "Miguel Teixeira", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "O conteúdo foi muito útil para mim, obrigado.", 20 },
                    { 21, 2, "Cátia Barros", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seria interessante aprofundar sobre ETFs.", 21 },
                    { 22, 2, "Hugo Correia", new DateTime(2025, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Já partilhei com amigos, muito bom!", 22 },
                    { 23, 2, "Daniela Faria", new DateTime(2025, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ajudou-me a perceber melhor o mercado.", 23 },
                    { 24, 5, "Tiago Silva", new DateTime(2025, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Este artigo me ajudou a entender melhor a importância da análise de mercado. Parabéns!", 4 },
                    { 25, 5, "Ana Costa", new DateTime(2025, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Muito interessante, mas eu gostaria de mais exemplos de ferramentas.", 5 },
                    { 26, 5, "João Ferreira", new DateTime(2025, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Acho que poderiam adicionar mais estudos de caso sobre ROI. Isso ajudaria bastante.", 6 },
                    { 27, 5, "Marta Santos", new DateTime(2025, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adorei a forma como o conteúdo foi estruturado. Fácil de entender!", 7 },
                    { 28, 6, "Bruno Rocha", new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Muito bom, já compartilhei com a minha rede de contatos. Espero ver mais artigos assim.", 8 },
                    { 29, 6, "Carla Mendes", new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Preciso de mais informações sobre como aplicar isso em investimentos pessoais.", 9 },
                    { 30, 6, "Diogo Gomes", new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Muito relevante para quem está iniciando no mercado financeiro. Obrigado pelo conteúdo.", 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Artigos",
                keyColumn: "ArtigoId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Comentario",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Artigos",
                keyColumn: "ArtigoId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Artigos",
                keyColumn: "ArtigoId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "LightInvest ");
        }
    }
}
