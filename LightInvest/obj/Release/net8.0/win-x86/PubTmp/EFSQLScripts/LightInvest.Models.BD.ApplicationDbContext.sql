IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE TABLE [Artigos] (
        [ArtigoId] int NOT NULL IDENTITY,
        [Titulo] nvarchar(200) NOT NULL,
        [Conteudo] nvarchar(max) NOT NULL,
        [ImagemUrl] nvarchar(max) NOT NULL,
        [Categoria] nvarchar(max) NOT NULL,
        [DescricaoCurta] nvarchar(500) NOT NULL,
        [DataPublicacao] datetime2 NOT NULL,
        [ArtigoId1] int NULL,
        CONSTRAINT [PK_Artigos] PRIMARY KEY ([ArtigoId]),
        CONSTRAINT [FK_Artigos_Artigos_ArtigoId1] FOREIGN KEY ([ArtigoId1]) REFERENCES [Artigos] ([ArtigoId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE TABLE [Cidades] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Cidades] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE TABLE [EnergyConsumptions] (
        [Id] int NOT NULL IDENTITY,
        [UserEmail] nvarchar(max) NOT NULL,
        [ConsumoDiaSemana] nvarchar(max) NOT NULL,
        [ConsumoFimSemana] nvarchar(max) NOT NULL,
        [MesesOcupacao] nvarchar(max) NOT NULL,
        [MediaSemana] decimal(18,2) NOT NULL,
        [MediaFimSemana] decimal(18,2) NOT NULL,
        [MediaAnual] decimal(18,2) NOT NULL,
        [ConsumoTotal] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_EnergyConsumptions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE TABLE [ModelosDePaineisSolares] (
        [Id] int NOT NULL IDENTITY,
        [ModeloNome] nvarchar(100) NOT NULL,
        [Preco] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_ModelosDePaineisSolares] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE TABLE [PasswordResetTokens] (
        [Id] int NOT NULL IDENTITY,
        [Email] nvarchar(max) NOT NULL,
        [Token] nvarchar(max) NOT NULL,
        [Expiration] datetime2 NOT NULL,
        CONSTRAINT [PK_PasswordResetTokens] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE TABLE [ROICalculators] (
        [Id] int NOT NULL IDENTITY,
        [UserEmail] nvarchar(max) NOT NULL,
        [CustoInstalacao] decimal(18,2) NOT NULL,
        [CustoManutencaoAnual] decimal(18,2) NOT NULL,
        [ConsumoEnergeticoMedio] decimal(18,2) NOT NULL,
        [ConsumoEnergeticoRede] decimal(18,2) NOT NULL,
        [RetornoEconomia] decimal(18,2) NOT NULL,
        [ROI] decimal(18,2) NOT NULL,
        [DataCalculado] datetime2 NOT NULL,
        CONSTRAINT [PK_ROICalculators] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE TABLE [Tarifas] (
        [Id] int NOT NULL IDENTITY,
        [PrecoKWh] decimal(18,2) NOT NULL,
        [UserEmail] nvarchar(max) NOT NULL,
        [DataAlteracao] datetime2 NOT NULL,
        [Tipo] int NOT NULL,
        CONSTRAINT [PK_Tarifas] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE TABLE [PotenciasDePaineisSolares] (
        [Id] int NOT NULL IDENTITY,
        [Potencia] decimal(18,2) NOT NULL,
        [ModeloPainelId] int NOT NULL,
        CONSTRAINT [PK_PotenciasDePaineisSolares] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PotenciasDePaineisSolares_ModelosDePaineisSolares_ModeloPainelId] FOREIGN KEY ([ModeloPainelId]) REFERENCES [ModelosDePaineisSolares] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE TABLE [DadosInstalacao] (
        [Id] int NOT NULL IDENTITY,
        [UserEmail] nvarchar(max) NOT NULL,
        [CidadeId] int NOT NULL,
        [ModeloPainelId] int NOT NULL,
        [PotenciaId] int NOT NULL,
        [NumeroPaineis] int NOT NULL,
        [Inclinacao] decimal(18,2) NOT NULL,
        [Dificuldade] int NOT NULL,
        [PrecoInstalacao] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_DadosInstalacao] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DadosInstalacao_Cidades_CidadeId] FOREIGN KEY ([CidadeId]) REFERENCES [Cidades] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_DadosInstalacao_ModelosDePaineisSolares_ModeloPainelId] FOREIGN KEY ([ModeloPainelId]) REFERENCES [ModelosDePaineisSolares] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_DadosInstalacao_PotenciasDePaineisSolares_PotenciaId] FOREIGN KEY ([PotenciaId]) REFERENCES [PotenciasDePaineisSolares] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ArtigoId', N'ArtigoId1', N'Categoria', N'Conteudo', N'DataPublicacao', N'DescricaoCurta', N'ImagemUrl', N'Titulo') AND [object_id] = OBJECT_ID(N'[Artigos]'))
        SET IDENTITY_INSERT [Artigos] ON;
    EXEC(N'INSERT INTO [Artigos] ([ArtigoId], [ArtigoId1], [Categoria], [Conteudo], [DataPublicacao], [DescricaoCurta], [ImagemUrl], [Titulo])
    VALUES (1, NULL, N''Energia Renovável'', CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''A energia solar é uma fonte renovável e limpa que está se tornando cada vez mais popular devido aos seus benefícios econômicos e ambientais. Este artigo explora as vantagens de adotar a energia solar tanto para residências quanto para empresas.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Benefícios Econômicos'', nchar(13), nchar(10), N''- **Redução de Custos:** A principal vantagem da energia solar é a redução da conta de energia elétrica. Ao gerar sua própria eletricidade, você diminui a dependência da rede elétrica.'', nchar(13), nchar(10), N''- **Valorização do Imóvel:** Imóveis que possuem sistemas de energia solar são geralmente mais valorizados no mercado, uma vez que têm custos operacionais menores e atraem compradores interessados em soluções sustentáveis.'', nchar(13), nchar(10), N''- **Incentivos e Subsídios:** Em muitas regiões, o governo oferece incentivos fiscais e subsídios para a instalação de sistemas fotovoltaicos, tornando o investimento mais acessível.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Benefícios Ambientais'', nchar(13), nchar(10), N''- **Redução da Pegada de Carbono:** A energia solar não emite gases de efeito estufa, o que contribui significativamente para a redução da pegada de carbono.'', nchar(13), nchar(10), N''- **Fontes Renováveis:** Ao contrário das fontes de energia tradicionais, como carvão e gás natural, a energia solar é renovável e não esgota os recursos naturais do planeta.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Conclusão'', nchar(13), nchar(10), N''Investir em energia solar é uma escolha inteligente tanto do ponto de vista econômico quanto ambiental. Ao reduzir os custos com eletricidade e contribuir para a preservação do meio ambiente, a energia solar se torna uma solução cada vez mais viável e atraente.''), ''2025-02-15T00:00:00.0000000'', N''Entenda os benefícios da energia solar para sua residência ou empresa.'', N''/images/artigos/energia-solar.jpg'', N''Benefícios da Energia Solar''),
    (2, NULL, N''ROI'', CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''Calcular o Retorno sobre o Investimento (ROI) em sistemas fotovoltaicos é essencial para avaliar a viabilidade financeira de um projeto. Este artigo explica como calcular o ROI e por que ele é importante para qualquer instalação de energia solar.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### O que é o ROI?'', nchar(13), nchar(10), N''O ROI é uma métrica financeira usada para avaliar o desempenho de um investimento. Ele calcula o lucro ou perda relativa ao valor investido e é expresso como uma porcentagem.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Fórmula do ROI'', nchar(13), nchar(10), N''A fórmula básica para calcular o ROI é a seguinte:'', nchar(13), nchar(10), N''Para um sistema de energia solar, o retorno pode incluir a economia na conta de energia elétrica, o valor dos incentivos fiscais, e a possível valorização do imóvel. O custo do investimento inclui a instalação dos painéis solares, manutenção e outros custos operacionais.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Exemplo de Cálculo do ROI'', nchar(13), nchar(10), N''Suponhamos que você tenha investido ´20.000 € em um sistema de energia solar e, ao longo do tempo, tenha economizado 3.000 € anualmente na sua conta de energia elétrica. O cálculo do ROI seria: ROI (%) = (Retorno do Investimento / Custo do Investimento) x 100'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''Isso significa que, em média, você terá um retorno de 15% do valor investido a cada ano.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Conclusão'', nchar(13), nchar(10), N''O cálculo do ROI ajuda a determinar se o investimento em energia solar vale a pena. Com os dados certos, você pode projetar a viabilidade financeira e o tempo de retorno do seu investimento em energia solar.''), ''2022-02-15T00:00:00.0000000'', N''Aprenda a calcular o ROI de um sistema fotovoltaico e entenda se o investimento vale a pena.'', N''/images/artigos/calcular-roi.jpg'', N''Como Calcular o Retorno sobre o Investimento em Energia Solar''),
    (3, NULL, N''Painéis Solares'', CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''Planejar e instalar um sistema de energia solar requer um processo detalhado e bem coordenado. Este artigo apresenta um guia completo sobre como planejar e executar a instalação de um sistema fotovoltaico de forma eficiente.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Passos para o Planejamento'', nchar(13), nchar(10), N''1. **Análise de Viabilidade:** Antes de iniciar, é importante realizar uma análise detalhada do local, levando em consideração fatores como o consumo de energia, a localização e a inclinação do telhado.'', nchar(13), nchar(10), N''2. **Dimensionamento do Sistema:** A quantidade de energia que um sistema solar pode gerar depende do número de painéis e da capacidade de cada um. O dimensionamento correto do sistema é crucial para maximizar a eficiência.'', nchar(13), nchar(10), N''3. **Escolha dos Componentes:** Os componentes principais de um sistema solar são os painéis solares, o inversor e a estrutura de montagem. Escolher materiais de boa qualidade é essencial para garantir o bom funcionamento e a longevidade do sistema.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Processo de Instalação'', nchar(13), nchar(10), N''- **Instalação dos Painéis Solares:** Os painéis solares devem ser instalados de forma a otimizar a exposição solar, garantindo que eles recebam a maior quantidade de luz possível ao longo do dia.'', nchar(13), nchar(10), N''- **Conexão Elétrica:** A instalação elétrica envolve a ligação dos painéis solares ao inversor, que converte a energia gerada em energia utilizável para a residência ou empresa.'', nchar(13), nchar(10), N''- **Testes e Comissionamento:** Após a instalação, é necessário realizar testes para garantir que o sistema está funcionando corretamente e de forma segura.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Conclusão'', nchar(13), nchar(10), N''A instalação de sistemas solares é um processo técnico que exige planejamento cuidadoso. Um bom planejamento e a escolha de profissionais qualificados podem garantir que o sistema solar seja eficiente e tenha uma vida útil longa.''), ''2024-02-15T00:00:00.0000000'', N''Dicas essenciais para planejar e instalar um sistema de energia solar de forma eficiente.'', N''/images/artigos/planejamento-solar.jpg'', N''Planejamento e Instalação de Sistemas de Energia Solar'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ArtigoId', N'ArtigoId1', N'Categoria', N'Conteudo', N'DataPublicacao', N'DescricaoCurta', N'ImagemUrl', N'Titulo') AND [object_id] = OBJECT_ID(N'[Artigos]'))
        SET IDENTITY_INSERT [Artigos] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome') AND [object_id] = OBJECT_ID(N'[Cidades]'))
        SET IDENTITY_INSERT [Cidades] ON;
    EXEC(N'INSERT INTO [Cidades] ([Id], [Nome])
    VALUES (1, N''Albufeira''),
    (2, N''Almada''),
    (3, N''Amadora''),
    (4, N''Aveiro''),
    (5, N''Barcelos''),
    (6, N''Beja''),
    (7, N''Braga''),
    (8, N''Bragança''),
    (9, N''Caldas da Rainha''),
    (10, N''Cascais''),
    (11, N''Coimbra''),
    (12, N''Évora''),
    (13, N''Faro''),
    (14, N''Figueira da Foz''),
    (15, N''Funchal''),
    (16, N''Guarda''),
    (17, N''Guimarães''),
    (18, N''Leiria''),
    (19, N''Lisboa''),
    (20, N''Matosinhos''),
    (21, N''Montijo''),
    (22, N''Odivelas''),
    (23, N''Oeiras''),
    (24, N''Portalegre''),
    (25, N''Portimão''),
    (26, N''Porto''),
    (27, N''Póvoa de Varzim''),
    (28, N''Santarem''),
    (29, N''Setúbal''),
    (30, N''Barreiro''),
    (31, N''Sintra''),
    (32, N''Tomar''),
    (33, N''Torres Vedras''),
    (34, N''Viana do castelo''),
    (35, N''Vila do Conde''),
    (36, N''Vila Nova de Gaia''),
    (37, N''Viseu'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome') AND [object_id] = OBJECT_ID(N'[Cidades]'))
        SET IDENTITY_INSERT [Cidades] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ModeloNome', N'Preco') AND [object_id] = OBJECT_ID(N'[ModelosDePaineisSolares]'))
        SET IDENTITY_INSERT [ModelosDePaineisSolares] ON;
    EXEC(N'INSERT INTO [ModelosDePaineisSolares] ([Id], [ModeloNome], [Preco])
    VALUES (1, N''Aiko - Comet 2U'', 1250.0),
    (2, N''Maxeon 7'', 1320.0),
    (3, N''Longi - HI-MO X6'', 1280.0),
    (4, N''Huasun - Himalaya'', 1300.0),
    (5, N''TW Solar'', 1230.0),
    (6, N''JA Solar DeepBlue 4.0 Pro'', 1270.0),
    (7, N''Astroenergy - Astro N5'', 1260.0),
    (8, N''Grand Sunergy'', 1240.0),
    (9, N''DMEGC - Infinity RT'', 1290.0),
    (10, N''Spic'', 910.0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ModeloNome', N'Preco') AND [object_id] = OBJECT_ID(N'[ModelosDePaineisSolares]'))
        SET IDENTITY_INSERT [ModelosDePaineisSolares] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ModeloPainelId', N'Potencia') AND [object_id] = OBJECT_ID(N'[PotenciasDePaineisSolares]'))
        SET IDENTITY_INSERT [PotenciasDePaineisSolares] ON;
    EXEC(N'INSERT INTO [PotenciasDePaineisSolares] ([Id], [ModeloPainelId], [Potencia])
    VALUES (1, 1, 670.0),
    (2, 1, 680.0),
    (3, 1, 690.0),
    (4, 1, 700.0),
    (5, 2, 445.0),
    (6, 2, 455.0),
    (7, 2, 465.0),
    (8, 2, 475.0),
    (9, 3, 600.0),
    (10, 3, 610.0),
    (11, 3, 620.0),
    (12, 3, 630.0),
    (13, 4, 720.0),
    (14, 4, 730.0),
    (15, 4, 740.0),
    (16, 4, 750.0),
    (17, 5, 715.0),
    (18, 5, 725.0),
    (19, 5, 735.0),
    (20, 5, 745.0),
    (21, 5, 590.0),
    (22, 5, 600.0),
    (23, 5, 610.0),
    (24, 5, 620.0),
    (25, 6, 595.0),
    (26, 6, 605.0),
    (27, 6, 615.0),
    (28, 6, 625.0),
    (29, 7, 640.0),
    (30, 7, 650.0),
    (31, 7, 660.0),
    (32, 7, 670.0),
    (33, 8, 710.0),
    (34, 8, 720.0),
    (35, 8, 730.0),
    (36, 8, 740.0),
    (37, 9, 615.0),
    (38, 9, 625.0),
    (39, 9, 635.0),
    (40, 9, 645.0),
    (41, 10, 410.0),
    (42, 10, 420.0);
    INSERT INTO [PotenciasDePaineisSolares] ([Id], [ModeloPainelId], [Potencia])
    VALUES (43, 10, 430.0),
    (44, 10, 440.0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ModeloPainelId', N'Potencia') AND [object_id] = OBJECT_ID(N'[PotenciasDePaineisSolares]'))
        SET IDENTITY_INSERT [PotenciasDePaineisSolares] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Artigos_ArtigoId1] ON [Artigos] ([ArtigoId1]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DadosInstalacao_CidadeId] ON [DadosInstalacao] ([CidadeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DadosInstalacao_ModeloPainelId] ON [DadosInstalacao] ([ModeloPainelId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DadosInstalacao_PotenciaId] ON [DadosInstalacao] ([PotenciaId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PotenciasDePaineisSolares_ModeloPainelId] ON [PotenciasDePaineisSolares] ([ModeloPainelId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250324210209_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250324210209_InitialCreate', N'8.0.0');
END;
GO

COMMIT;
GO

