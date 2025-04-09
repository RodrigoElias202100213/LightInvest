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
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [IsAdmin] bit NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    CREATE TABLE [Comentario] (
        [Id] int NOT NULL IDENTITY,
        [Texto] nvarchar(500) NOT NULL,
        [Autor] nvarchar(100) NOT NULL,
        [DataCriacao] datetime2 NOT NULL,
        [ArtigoId] int NOT NULL,
        [UserId] int NULL,
        CONSTRAINT [PK_Comentario] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Comentario_Artigos_ArtigoId] FOREIGN KEY ([ArtigoId]) REFERENCES [Artigos] ([ArtigoId]) ON DELETE CASCADE,
        CONSTRAINT [FK_Comentario_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ArtigoId', N'ArtigoId1', N'Categoria', N'Conteudo', N'DataPublicacao', N'DescricaoCurta', N'ImagemUrl', N'Titulo') AND [object_id] = OBJECT_ID(N'[Artigos]'))
        SET IDENTITY_INSERT [Artigos] ON;
    EXEC(N'INSERT INTO [Artigos] ([ArtigoId], [ArtigoId1], [Categoria], [Conteudo], [DataPublicacao], [DescricaoCurta], [ImagemUrl], [Titulo])
    VALUES (1, NULL, N''Energia Renovável'', CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''				A energia solar é uma fonte renovável e limpa que se está a tornar cada vez mais popular devido aos seus benefícios econômicos e ambientais. Este artigo explora as vantagens de adotar a energia solar tanto para residências quanto para empresas.'', nchar(13), nchar(10), nchar(13), nchar(10), N''				### Benefícios Econômicos'', nchar(13), nchar(10), N''				- **Redução de Custos:** A principal vantagem da energia solar é a redução da conta de energia elétrica. Ao gerar sua própria eletricidade, diminui a dependência da rede elétrica.'', nchar(13), nchar(10), N''				- **Valorização do Imóvel:** Imóveis que possuem sistemas de energia solar são geralmente mais valorizados no mercado, uma vez que têm custos operacionais menores e atraem compradores interessados em soluções sustentáveis.'', nchar(13), nchar(10), N''				- **Incentivos e Subsídios:** Em muitas regiões, o governo oferece incentivos fiscais e subsídios para a instalação de sistemas fotovoltaicos, tornando o investimento mais acessível.'', nchar(13), nchar(10), nchar(13), nchar(10), N''				### Benefícios Ambientais'', nchar(13), nchar(10), N''				- **Redução da Pegada de Carbono:** A energia solar não emite gases de efeito estufa, o que contribui significativamente para a redução da pegada de carbono.'', nchar(13), nchar(10), N''				- **Fontes Renováveis:** Ao contrário das fontes de energia tradicionais, como o carvão e o gás natural, a energia solar é renovável e não esgota os recursos naturais do planeta.'', nchar(13), nchar(10), nchar(13), nchar(10), N''				### Conclusão'', nchar(13), nchar(10), N''				Investir em sistemas de energia solar é uma escolha inteligente tanto do ponto de vista econômico quanto ambiental. Ao reduzir os custos com eletricidade e contribuir para a preservação do meio ambiente, a energia solar torna-se uma solução cada vez mais viável e atraente.''), ''2025-02-15T00:00:00.0000000'', N''Entenda os benefícios da energia solar para sua residência ou empresa.'', N''~/images/energia-solar.jpg'', N''Benefícios da Energia Solar''),
    (2, NULL, N''ROI'', CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''				Calcular o Retorno sobre o Investimento (ROI) em sistemas fotovoltaicos é essencial para avaliar a viabilidade financeira de um projeto. Este artigo explica como calcular o ROI e por que ele é importante para qualquer instalação de energia solar.'', nchar(13), nchar(10), nchar(13), nchar(10), N''				### O que é o ROI?'', nchar(13), nchar(10), N''				O ROI é uma métrica financeira usada para avaliar o desempenho de um investimento. Ele calcula o lucro ou perda relativa ao valor investido e é expresso como uma porcentagem.'', nchar(13), nchar(10), nchar(13), nchar(10), N''				### Fórmula do ROI'', nchar(13), nchar(10), N''				A fórmula básica para calcular o ROI é a seguinte:'', nchar(13), nchar(10), N''				Para um sistema de energia solar, o retorno pode incluir a economia na conta de energia elétrica, o valor dos incentivos fiscais, e a possível valorização do imóvel. O custo do investimento inclui a instalação dos painéis solares, manutenção e outros custos operacionais. Se tiver interesse nesta máteria na nossa plataforma consegues aceder à ferramenta do cálculo do ROI.'', nchar(13), nchar(10), nchar(13), nchar(10), N''				### Exemplo de Cálculo do ROI'', nchar(13), nchar(10), N''				Suponhamos que investiu ´20.000 € numm sistema de energia solar e, ao longo do tempo, economizou 3.000 € anualmente na sua conta de energia elétrica. O cálculo do ROI seria: ROI (%) = (Retorno do Investimento / Custo do Investimento) x 100'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''				Isso significa que, em média, terá um retorno de 15% do valor investido a cada ano.'', nchar(13), nchar(10), nchar(13), nchar(10), N''				### Conclusão'', nchar(13), nchar(10), N''				O cálculo do ROI ajuda a determinar se o investimento em energia solar vale a pena. Com os dados certos, consegue analisar e avaliar a viabilidade financeira e o tempo de retorno do seu investimento em energia solar.''), ''2022-02-15T00:00:00.0000000'', N''Aprenda a calcular o ROI de um sistema fotovoltaico e entenda se o investimento vale a pena.'', N''/images/artigos/calcular-roi.jpg'', N''Como Calcular o Retorno sobre o Investimento em Energia Solar''),
    (3, NULL, N''Painéis Solares'', CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''				Planear e instalar um sistema de energia solar requer um processo detalhado e bem coordenado. Este artigo apresenta um guia completo sobre como planear e executar a instalação de um sistema fotovoltaico de forma eficiente.'', nchar(13), nchar(10), nchar(13), nchar(10), N''				### Passos para o Planeamento'', nchar(13), nchar(10), N''				1. **Análise de Viabilidade:** Antes de iniciar, é importante realizar uma análise detalhada do local, levando em consideração fatores como o consumo de energia, a localização e a inclinação do telhado.'', nchar(13), nchar(10), N''				2. **Dimensionamento do Sistema:** A quantidade de energia que um sistema solar pode gerar depende do número de painéis e da capacidade de cada um. O dimensionamento correto do sistema é crucial para maximizar a eficiência.'', nchar(13), nchar(10), N''				3. **Escolha dos Componentes:** Os componentes principais de um sistema solar são os painéis solares, o inversor e a estrutura de montagem. Escolher materiais de boa qualidade é essencial para garantir o bom funcionamento e a longevidade do sistema.'', nchar(13), nchar(10), nchar(13), nchar(10), N''				### Processo de Instalação'', nchar(13), nchar(10), N''				- **Instalação dos Painéis Solares:** Os painéis solares devem ser instalados de forma a otimizar a exposição solar, garantindo que eles recebam a maior quantidade de luz possível ao longo do dia.'', nchar(13), nchar(10), N''				- **Conexão Elétrica:** A instalação elétrica envolve a ligação dos painéis solares ao inversor, que converte a energia gerada em energia utilizável para a residência ou empresa.'', nchar(13), nchar(10), N''				- **Testes e Comissionamento:** Após a instalação, é necessário realizar testes para garantir que o sistema está funcionando corretamente e de forma segura.'', nchar(13), nchar(10), nchar(13), nchar(10), N''				### Conclusão'', nchar(13), nchar(10), N''				A instalação de sistemas solares é um processo técnico que exige um planeamento cuidadoso. Um bom planeamento e a escolha de profissionais qualificados podem garantir que o sistema solar seja eficiente e tenha uma vida útil longa.''), ''2024-02-15T00:00:00.0000000'', N''Dicas essenciais para planear e instalar um sistema de energia solar de forma eficiente.'', N''/images/artigos/planeamento-solar.jpg'', N''Planeamento e Instalação de Sistemas de Energia Solar'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ArtigoId', N'ArtigoId1', N'Categoria', N'Conteudo', N'DataPublicacao', N'DescricaoCurta', N'ImagemUrl', N'Titulo') AND [object_id] = OBJECT_ID(N'[Artigos]'))
        SET IDENTITY_INSERT [Artigos] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'IsAdmin', N'Name', N'Password') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] ON;
    EXEC(N'INSERT INTO [Users] ([Id], [Email], [IsAdmin], [Name], [Password])
    VALUES (1, N''rodrigo.elias2003@gmail.com'', CAST(1 AS bit), N''Rodrigo'', N''rodrigoR123'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'IsAdmin', N'Name', N'Password') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ArtigoId', N'Autor', N'DataCriacao', N'Texto', N'UserId') AND [object_id] = OBJECT_ID(N'[Comentario]'))
        SET IDENTITY_INSERT [Comentario] ON;
    EXEC(N'INSERT INTO [Comentario] ([Id], [ArtigoId], [Autor], [DataCriacao], [Texto], [UserId])
    VALUES (1, 1, N''nome'', ''2025-02-16T00:00:00.0000000'', N''Ótimo artigo! Muito informativo.'', 1),
    (2, 1, N''nome'', ''2025-02-17T00:00:00.0000000'', N''Gostei bastante das explicações sobre os benefícios ambientais!'', 1),
    (3, 2, N''nome'', ''2025-03-01T00:00:00.0000000'', N''A ferramenta de cálculo do ROI seria muito útil! Vocês têm alguma recomendação?'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ArtigoId', N'Autor', N'DataCriacao', N'Texto', N'UserId') AND [object_id] = OBJECT_ID(N'[Comentario]'))
        SET IDENTITY_INSERT [Comentario] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
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
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    CREATE INDEX [IX_Artigos_ArtigoId1] ON [Artigos] ([ArtigoId1]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    CREATE INDEX [IX_Comentario_ArtigoId] ON [Comentario] ([ArtigoId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    CREATE INDEX [IX_Comentario_UserId] ON [Comentario] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    CREATE INDEX [IX_DadosInstalacao_CidadeId] ON [DadosInstalacao] ([CidadeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    CREATE INDEX [IX_DadosInstalacao_ModeloPainelId] ON [DadosInstalacao] ([ModeloPainelId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    CREATE INDEX [IX_DadosInstalacao_PotenciaId] ON [DadosInstalacao] ([PotenciaId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    CREATE INDEX [IX_PotenciasDePaineisSolares_ModeloPainelId] ON [PotenciasDePaineisSolares] ([ModeloPainelId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330010802_innitial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250330010802_innitial', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330120502_AtualizacaoBD'
)
BEGIN
    EXEC(N'UPDATE [Artigos] SET [Conteudo] = CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''A energia solar é uma fonte renovável e limpa que se está a tornar cada vez mais popular devido aos seus benefícios econômicos e ambientais. Este artigo explora as vantagens de adotar a energia solar tanto para residências quanto para empresas.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Benefícios Econômicos'', nchar(13), nchar(10), N''- **Redução de Custos:** A principal vantagem da energia solar é a redução da conta de energia elétrica. Ao gerar sua própria eletricidade, diminui a dependência da rede elétrica.'', nchar(13), nchar(10), N''- **Valorização do Imóvel:** Imóveis que possuem sistemas de energia solar são geralmente mais valorizados no mercado, uma vez que têm custos operacionais menores e atraem compradores interessados em soluções sustentáveis.'', nchar(13), nchar(10), N''- **Incentivos e Subsídios:** Em muitas regiões, o governo oferece incentivos fiscais e subsídios para a instalação de sistemas fotovoltaicos, tornando o investimento mais acessível.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Benefícios Ambientais'', nchar(13), nchar(10), N''- **Redução da Pegada de Carbono:** A energia solar não emite gases de efeito estufa, o que contribui significativamente para a redução da pegada de carbono.'', nchar(13), nchar(10), N''- **Fontes Renováveis:** Ao contrário das fontes de energia tradicionais, como o carvão e o gás natural, a energia solar é renovável e não esgota os recursos naturais do planeta.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Conclusão'', nchar(13), nchar(10), N''Investir em sistemas de energia solar é uma escolha inteligente tanto do ponto de vista econômico quanto ambiental. Ao reduzir os custos com eletricidade e contribuir para a preservação do meio ambiente, a energia solar torna-se uma solução cada vez mais viável e atraente.'')
    WHERE [ArtigoId] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330120502_AtualizacaoBD'
)
BEGIN
    EXEC(N'UPDATE [Artigos] SET [Conteudo] = CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''Calcular o Retorno sobre o Investimento (ROI) em sistemas fotovoltaicos é essencial para avaliar a viabilidade financeira de um projeto. Este artigo explica como calcular o ROI e por que ele é importante para qualquer instalação de energia solar.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### O que é o ROI?'', nchar(13), nchar(10), N''O ROI é uma métrica financeira usada para avaliar o desempenho de um investimento. Ele calcula o lucro ou perda relativa ao valor investido e é expresso como uma porcentagem.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Fórmula do ROI'', nchar(13), nchar(10), N''A fórmula básica para calcular o ROI é a seguinte:'', nchar(13), nchar(10), N''Para um sistema de energia solar, o retorno pode incluir a economia na conta de energia elétrica, o valor dos incentivos fiscais, e a possível valorização do imóvel. O custo do investimento inclui a instalação dos painéis solares, manutenção e outros custos operacionais. Se tiver interesse nesta máteria na nossa plataforma consegues aceder à ferramenta do cálculo do ROI.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Exemplo de Cálculo do ROI'', nchar(13), nchar(10), N''Suponhamos que investiu ´20.000 € numm sistema de energia solar e, ao longo do tempo, economizou 3.000 € anualmente na sua conta de energia elétrica. O cálculo do ROI seria: ROI (%) = (Retorno do Investimento / Custo do Investimento) x 100'', nchar(13), nchar(10), nchar(13), nchar(10), nchar(13), nchar(10), N''Isso significa que, em média, terá um retorno de 15% do valor investido a cada ano.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Conclusão'', nchar(13), nchar(10), N''O cálculo do ROI ajuda a determinar se o investimento em energia solar vale a pena. Com os dados certos, consegue analisar e avaliar a viabilidade financeira e o tempo de retorno do seu investimento em energia solar.'')
    WHERE [ArtigoId] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330120502_AtualizacaoBD'
)
BEGIN
    EXEC(N'UPDATE [Artigos] SET [Conteudo] = CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''Planear e instalar um sistema de energia solar requer um processo detalhado e bem coordenado. Este artigo apresenta um guia completo sobre como planear e executar a instalação de um sistema fotovoltaico de forma eficiente.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Passos para o Planeamento'', nchar(13), nchar(10), N''1. **Análise de Viabilidade:** Antes de iniciar, é importante realizar uma análise detalhada do local, levando em consideração fatores como o consumo de energia, a localização e a inclinação do telhado.'', nchar(13), nchar(10), N''2. **Dimensionamento do Sistema:** A quantidade de energia que um sistema solar pode gerar depende do número de painéis e da capacidade de cada um. O dimensionamento correto do sistema é crucial para maximizar a eficiência.'', nchar(13), nchar(10), N''3. **Escolha dos Componentes:** Os componentes principais de um sistema solar são os painéis solares, o inversor e a estrutura de montagem. Escolher materiais de boa qualidade é essencial para garantir o bom funcionamento e a longevidade do sistema.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Processo de Instalação'', nchar(13), nchar(10), N''- **Instalação dos Painéis Solares:** Os painéis solares devem ser instalados de forma a otimizar a exposição solar, garantindo que eles recebam a maior quantidade de luz possível ao longo do dia.'', nchar(13), nchar(10), N''- **Conexão Elétrica:** A instalação elétrica envolve a ligação dos painéis solares ao inversor, que converte a energia gerada em energia utilizável para a residência ou empresa.'', nchar(13), nchar(10), N''- **Testes e Comissionamento:** Após a instalação, é necessário realizar testes para garantir que o sistema está funcionando corretamente e de forma segura.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Conclusão'', nchar(13), nchar(10), N''A instalação de sistemas solares é um processo técnico que exige um planeamento cuidadoso. Um bom planeamento e a escolha de profissionais qualificados podem garantir que o sistema solar seja eficiente e tenha uma vida útil longa.'')
    WHERE [ArtigoId] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330120502_AtualizacaoBD'
)
BEGIN
    EXEC(N'UPDATE [Comentario] SET [Autor] = N''Rodrigo''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330120502_AtualizacaoBD'
)
BEGIN
    EXEC(N'UPDATE [Comentario] SET [Autor] = N''Rodrigo''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330120502_AtualizacaoBD'
)
BEGIN
    EXEC(N'UPDATE [Comentario] SET [Autor] = N''Rodrigo''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250330120502_AtualizacaoBD'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250330120502_AtualizacaoBD', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250403092752_AddLikesToComentario'
)
BEGIN
    EXEC(N'UPDATE [Artigos] SET [DescricaoCurta] = N''Entenda os benefícios da energia solar para a sua residência ou empresa.''
    WHERE [ArtigoId] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250403092752_AddLikesToComentario'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250403092752_AddLikesToComentario', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250403093033_AddComentarioLikeTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250403093033_AddComentarioLikeTable', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250403094327_AddComentarioLikeTable2'
)
BEGIN
    CREATE TABLE [ComentarioLike] (
        [Id] int NOT NULL IDENTITY,
        [ComentarioId] int NOT NULL,
        [UserId] int NOT NULL,
        CONSTRAINT [PK_ComentarioLike] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ComentarioLike_Comentario_ComentarioId] FOREIGN KEY ([ComentarioId]) REFERENCES [Comentario] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ComentarioLike_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250403094327_AddComentarioLikeTable2'
)
BEGIN
    CREATE INDEX [IX_ComentarioLike_ComentarioId] ON [ComentarioLike] ([ComentarioId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250403094327_AddComentarioLikeTable2'
)
BEGIN
    CREATE INDEX [IX_ComentarioLike_UserId] ON [ComentarioLike] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250403094327_AddComentarioLikeTable2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250403094327_AddComentarioLikeTable2', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250403095917_AddComentarioLikeTable3'
)
BEGIN
    ALTER TABLE [ComentarioLike] ADD [IsLike] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250403095917_AddComentarioLikeTable3'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250403095917_AddComentarioLikeTable3', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250404115254_AddComentarioLikeTable4'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'IsAdmin', N'Name', N'Password') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] ON;
    EXEC(N'INSERT INTO [Users] ([Id], [Email], [IsAdmin], [Name], [Password])
    VALUES (2, N''veragfernandes04@gmail.com'', CAST(1 AS bit), N''Vera'', N''Vera1234''),
    (3, N''ligthinvestuser@gmail.com'', CAST(0 AS bit), N''LightInvest '', N''LightInvest123'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'IsAdmin', N'Name', N'Password') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250404115254_AddComentarioLikeTable4'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250404115254_AddComentarioLikeTable4', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250406102345_NomeDaMigração'
)
BEGIN
    EXEC(N'DELETE FROM [Users]
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250406102345_NomeDaMigração'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [Email] = N''lightinvestsup2425@gmail.com'', [Name] = N''LighInvestSupport'', [Password] = N''LightInvestSup123''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250406102345_NomeDaMigração'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250406102345_NomeDaMigração', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250409191013_populate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ArtigoId', N'ArtigoId1', N'Categoria', N'Conteudo', N'DataPublicacao', N'DescricaoCurta', N'ImagemUrl', N'Titulo') AND [object_id] = OBJECT_ID(N'[Artigos]'))
        SET IDENTITY_INSERT [Artigos] ON;
    EXEC(N'INSERT INTO [Artigos] ([ArtigoId], [ArtigoId1], [Categoria], [Conteudo], [DataPublicacao], [DescricaoCurta], [ImagemUrl], [Titulo])
    VALUES (4, NULL, N''Energia Renovável'', CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''A eficiência energética é uma das estratégias mais eficazes para reduzir a fatura de eletricidade. Este artigo apresenta algumas práticas simples e acessíveis que podem fazer a diferença no consumo de energia.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Dicas de Eficiência Energética'', nchar(13), nchar(10), N''1. **Iluminação LED:** Trocar lâmpadas incandescentes por LEDs pode reduzir significativamente o consumo de energia em casa ou na empresa.'', nchar(13), nchar(10), N''2. **Equipamentos Eficientes:** Optar por eletrodomésticos com classificação energética A+++ garante menor consumo de energia.'', nchar(13), nchar(10), N''3. **Isolamento Térmico:** Melhorar o isolamento térmico das divisões reduz o uso excessivo de aquecedores e ar-condicionado.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Benefícios'', nchar(13), nchar(10), N''- Redução imediata na fatura mensal;'', nchar(13), nchar(10), N''- Menor impacto ambiental;'', nchar(13), nchar(10), N''- Valorização do imóvel.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Conclusão'', nchar(13), nchar(10), N''Investir em eficiência energética é uma solução sustentável que permite poupar dinheiro e proteger o meio ambiente.''), ''2024-03-10T00:00:00.0000000'', N''Descobre como poupar energia e reduzir custos com medidas simples e eficientes.'', N''/images/artigos/eficiencia-energetica.jpg'', N''Como Reduzir a Fatura de Energia com Eficiência Energética''),
    (5, NULL, N''Energia Renovável'', CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''Portugal tem-se destacado como um dos países europeus com maior aposta em energias renováveis. Conhece as principais vantagens de investir nesta área.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Benefícios das Renováveis'', nchar(13), nchar(10), N''1. **Redução de Custos:** A longo prazo, os investimentos em energia solar ou eólica geram poupanças consideráveis na fatura de energia.'', nchar(13), nchar(10), N''2. **Sustentabilidade Ambiental:** Reduzem a pegada de carbono e protegem os recursos naturais.'', nchar(13), nchar(10), N''3. **Incentivos e Apoios:** Existem vários programas e incentivos estatais para quem pretende investir em energias renováveis.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Perspetivas Futuras'', nchar(13), nchar(10), N''Portugal continuará a expandir a sua produção de energia limpa, criando oportunidades de investimento e desenvolvimento tecnológico.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Conclusão'', nchar(13), nchar(10), N''Investir em energias renováveis em Portugal é uma decisão inteligente, com benefícios económicos e ambientais a curto e longo prazo.''), ''2024-04-05T00:00:00.0000000'', N''Conhece as principais vantagens de apostar nas energias renováveis em Portugal.'', N''/images/artigos/renovaveis-portugal.jpg'', N''Vantagens de Investir em Energia Renovável em Portugal''),
    (6, NULL, N''Painéis Solares'', CONCAT(CAST(nchar(13) AS nvarchar(max)), nchar(10), N''A manutenção preventiva dos painéis solares é fundamental para garantir o seu correto funcionamento e prolongar a sua vida útil. Neste artigo, partilhamos boas práticas essenciais.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Cuidados a Ter'', nchar(13), nchar(10), N''1. **Limpeza Regular:** A acumulação de poeira, folhas ou resíduos nos painéis pode reduzir a eficiência da produção de energia.'', nchar(13), nchar(10), N''2. **Inspeção de Cablagens:** Verificar se os cabos e conexões estão em bom estado evita perdas de energia ou avarias.'', nchar(13), nchar(10), N''3. **Monitorização de Desempenho:** Utilizar sistemas de monitorização permite detetar rapidamente qualquer anomalia.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Benefícios da Manutenção'', nchar(13), nchar(10), N''- Aumento da eficiência energética;'', nchar(13), nchar(10), N''- Prevenção de danos graves;'', nchar(13), nchar(10), N''- Maior rentabilidade do investimento.'', nchar(13), nchar(10), nchar(13), nchar(10), N''### Conclusão'', nchar(13), nchar(10), N''A manutenção preventiva dos painéis solares é um passo simples, mas essencial, para garantir o melhor desempenho do sistema a longo prazo.''), ''2024-05-02T00:00:00.0000000'', N''Aprende boas práticas para manter os painéis solares sempre eficientes e seguros.'', N''/images/artigos/manutencao-paineis.jpg'', N''Manutenção Preventiva de Painéis Solares: Boas Práticas'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ArtigoId', N'ArtigoId1', N'Categoria', N'Conteudo', N'DataPublicacao', N'DescricaoCurta', N'ImagemUrl', N'Titulo') AND [object_id] = OBJECT_ID(N'[Artigos]'))
        SET IDENTITY_INSERT [Artigos] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250409191013_populate'
)
BEGIN
    EXEC(N'UPDATE [Users] SET [Name] = N''LightInvest''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250409191013_populate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'IsAdmin', N'Name', N'Password') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] ON;
    EXEC(N'INSERT INTO [Users] ([Id], [Email], [IsAdmin], [Name], [Password])
    VALUES (4, N''tiagosilva@gmail.com'', CAST(0 AS bit), N''Tiago Silva'', N''Password123''),
    (5, N''anacosta@hotmail.com'', CAST(0 AS bit), N''Ana Costa'', N''Password123''),
    (6, N''joaoferreira@sapo.pt'', CAST(0 AS bit), N''João Ferreira'', N''Password123''),
    (7, N''martasantos@gmail.com'', CAST(0 AS bit), N''Marta Santos'', N''Password123''),
    (8, N''brunorocha@hotmail.com'', CAST(0 AS bit), N''Bruno Rocha'', N''Password123''),
    (9, N''carlamendes@sapo.pt'', CAST(0 AS bit), N''Carla Mendes'', N''Password123''),
    (10, N''diogogomes@gmail.com'', CAST(0 AS bit), N''Diogo Gomes'', N''Password123''),
    (11, N''filiparibeiro@hotmail.com'', CAST(0 AS bit), N''Filipa Ribeiro'', N''Password123''),
    (12, N''andresousa@sapo.pt'', CAST(0 AS bit), N''André Sousa'', N''Password123''),
    (13, N''raquelalmeida@gmail.com'', CAST(0 AS bit), N''Raquel Almeida'', N''Password123''),
    (14, N''pedromartins@hotmail.com'', CAST(0 AS bit), N''Pedro Martins'', N''Password123''),
    (15, N''sofialopes@sapo.pt'', CAST(0 AS bit), N''Sofia Lopes'', N''Password123''),
    (16, N''ricardopinto@gmail.com'', CAST(0 AS bit), N''Ricardo Pinto'', N''Password123''),
    (17, N''patricianunes@hotmail.com'', CAST(0 AS bit), N''Patrícia Nunes'', N''Password123''),
    (18, N''luiscarvalho@sapo.pt'', CAST(0 AS bit), N''Luís Carvalho'', N''Password123''),
    (19, N''beatrizfonseca@gmail.com'', CAST(0 AS bit), N''Beatriz Fonseca'', N''Password123''),
    (20, N''miguelteixeira@hotmail.com'', CAST(0 AS bit), N''Miguel Teixeira'', N''Password123''),
    (21, N''catiabarros@sapo.pt'', CAST(0 AS bit), N''Cátia Barros'', N''Password123''),
    (22, N''hugocorreia@gmail.com'', CAST(0 AS bit), N''Hugo Correia'', N''Password123''),
    (23, N''danielafaria@hotmail.com'', CAST(0 AS bit), N''Daniela Faria'', N''Password123''),
    (24, N''rodrigo.elias2003@gmail.com'', CAST(1 AS bit), N''Rodrigo Elias'', N''rodrigoR123''),
    (25, N''veragfernandes04@gmail.com'', CAST(1 AS bit), N''Vera Fernandes'', N''veraF123'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Email', N'IsAdmin', N'Name', N'Password') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250409191013_populate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ArtigoId', N'Autor', N'DataCriacao', N'Texto', N'UserId') AND [object_id] = OBJECT_ID(N'[Comentario]'))
        SET IDENTITY_INSERT [Comentario] ON;
    EXEC(N'INSERT INTO [Comentario] ([Id], [ArtigoId], [Autor], [DataCriacao], [Texto], [UserId])
    VALUES (4, 1, N''Tiago Silva'', ''2025-03-02T00:00:00.0000000'', N''Artigo muito esclarecedor, obrigado pela partilha!'', 4),
    (5, 1, N''Ana Costa'', ''2025-03-02T00:00:00.0000000'', N''Gostava de ver mais exemplos práticos.'', 5),
    (6, 1, N''João Ferreira'', ''2025-03-03T00:00:00.0000000'', N''Concordo plenamente com o que foi escrito.'', 6),
    (7, 1, N''Marta Santos'', ''2025-03-03T00:00:00.0000000'', N''Já sigo o vosso site há algum tempo, excelente trabalho.'', 7),
    (8, 2, N''Bruno Rocha'', ''2025-03-04T00:00:00.0000000'', N''Também tenho dúvidas em relação ao ROI, podiam fazer um artigo só sobre isso.'', 8),
    (9, 2, N''Carla Mendes'', ''2025-03-04T00:00:00.0000000'', N''Muito bom conteúdo, continue assim!'', 9),
    (10, 2, N''Diogo Gomes'', ''2025-03-05T00:00:00.0000000'', N''Conseguem partilhar fontes adicionais sobre este tema?'', 10),
    (11, 2, N''Filipa Ribeiro'', ''2025-03-05T00:00:00.0000000'', N''Gostei muito deste artigo, bem explicado.'', 11),
    (12, 2, N''André Sousa'', ''2025-03-06T00:00:00.0000000'', N''Tenho uma sugestão de tema: fundos de investimento.'', 12),
    (13, 3, N''Raquel Almeida'', ''2025-03-06T00:00:00.0000000'', N''Excelente conteúdo, parabéns!'', 13),
    (14, 3, N''Pedro Martins'', ''2025-03-07T00:00:00.0000000'', N''Muito completo e detalhado.'', 14),
    (15, 3, N''Sofia Lopes'', ''2025-03-07T00:00:00.0000000'', N''Era bom ter uma versão em vídeo também.'', 15),
    (16, 3, N''Ricardo Pinto'', ''2025-03-08T00:00:00.0000000'', N''Óptima explicação dos conceitos base.'', 16),
    (17, 3, N''Patrícia Nunes'', ''2025-03-08T00:00:00.0000000'', N''A parte dos exemplos ajudou-me muito.'', 17),
    (18, 1, N''Luís Carvalho'', ''2025-03-09T00:00:00.0000000'', N''Gostava de saber mais sobre análise técnica.'', 18),
    (19, 1, N''Beatriz Fonseca'', ''2025-03-09T00:00:00.0000000'', N''Parabéns pelo artigo, muito bem escrito.'', 19),
    (20, 1, N''Miguel Teixeira'', ''2025-03-10T00:00:00.0000000'', N''O conteúdo foi muito útil para mim, obrigado.'', 20),
    (21, 2, N''Cátia Barros'', ''2025-03-10T00:00:00.0000000'', N''Seria interessante aprofundar sobre ETFs.'', 21),
    (22, 2, N''Hugo Correia'', ''2025-03-11T00:00:00.0000000'', N''Já partilhei com amigos, muito bom!'', 22),
    (23, 2, N''Daniela Faria'', ''2025-03-11T00:00:00.0000000'', N''Ajudou-me a perceber melhor o mercado.'', 23),
    (24, 5, N''Tiago Silva'', ''2025-03-12T00:00:00.0000000'', N''Este artigo me ajudou a entender melhor a importância da análise de mercado. Parabéns!'', 4),
    (25, 5, N''Ana Costa'', ''2025-03-12T00:00:00.0000000'', N''Muito interessante, mas eu gostaria de mais exemplos de ferramentas.'', 5),
    (26, 5, N''João Ferreira'', ''2025-03-13T00:00:00.0000000'', N''Acho que poderiam adicionar mais estudos de caso sobre ROI. Isso ajudaria bastante.'', 6),
    (27, 5, N''Marta Santos'', ''2025-03-13T00:00:00.0000000'', N''Adorei a forma como o conteúdo foi estruturado. Fácil de entender!'', 7),
    (28, 6, N''Bruno Rocha'', ''2025-03-14T00:00:00.0000000'', N''Muito bom, já compartilhei com a minha rede de contatos. Espero ver mais artigos assim.'', 8),
    (29, 6, N''Carla Mendes'', ''2025-03-14T00:00:00.0000000'', N''Preciso de mais informações sobre como aplicar isso em investimentos pessoais.'', 9),
    (30, 6, N''Diogo Gomes'', ''2025-03-15T00:00:00.0000000'', N''Muito relevante para quem está iniciando no mercado financeiro. Obrigado pelo conteúdo.'', 10)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ArtigoId', N'Autor', N'DataCriacao', N'Texto', N'UserId') AND [object_id] = OBJECT_ID(N'[Comentario]'))
        SET IDENTITY_INSERT [Comentario] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250409191013_populate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250409191013_populate', N'8.0.0');
END;
GO

COMMIT;
GO

