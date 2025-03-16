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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    CREATE TABLE [Cidades] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Cidades] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    CREATE TABLE [ModelosDePaineisSolares] (
        [Id] int NOT NULL IDENTITY,
        [Modelo] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_ModelosDePaineisSolares] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    CREATE TABLE [Tarifas] (
        [Id] int NOT NULL IDENTITY,
        [UserEmail] nvarchar(max) NOT NULL,
        [DataAlteracao] datetime2 NOT NULL,
        [Nome] int NOT NULL,
        [PrecoKwh] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_Tarifas] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    CREATE TABLE [DadosInstalacao] (
        [Id] int NOT NULL IDENTITY,
        [UserEmail] nvarchar(max) NOT NULL,
        [CidadeId] int NOT NULL,
        [ModeloPainelId] int NOT NULL,
        [NumeroPaineis] int NOT NULL,
        [ConsumoPainel] decimal(18,2) NOT NULL,
        [Inclinacao] decimal(18,2) NOT NULL,
        [Dificuldade] nvarchar(max) NOT NULL,
        [PrecoInstalacao] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_DadosInstalacao] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DadosInstalacao_Cidades_CidadeId] FOREIGN KEY ([CidadeId]) REFERENCES [Cidades] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_DadosInstalacao_ModelosDePaineisSolares_ModeloPainelId] FOREIGN KEY ([ModeloPainelId]) REFERENCES [ModelosDePaineisSolares] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    CREATE TABLE [PotenciasDePaineisSolares] (
        [Id] int NOT NULL IDENTITY,
        [PainelSolarId] int NOT NULL,
        [Potencia] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_PotenciasDePaineisSolares] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PotenciasDePaineisSolares_ModelosDePaineisSolares_PainelSolarId] FOREIGN KEY ([PainelSolarId]) REFERENCES [ModelosDePaineisSolares] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome') AND [object_id] = OBJECT_ID(N'[Cidades]'))
        SET IDENTITY_INSERT [Cidades] ON;
    EXEC(N'INSERT INTO [Cidades] ([Id], [Nome])
    VALUES (1, N''Lisboa''),
    (2, N''Porto''),
    (3, N''Coimbra''),
    (4, N''Funchal'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome') AND [object_id] = OBJECT_ID(N'[Cidades]'))
        SET IDENTITY_INSERT [Cidades] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Modelo') AND [object_id] = OBJECT_ID(N'[ModelosDePaineisSolares]'))
        SET IDENTITY_INSERT [ModelosDePaineisSolares] ON;
    EXEC(N'INSERT INTO [ModelosDePaineisSolares] ([Id], [Modelo])
    VALUES (1, N''Modelo A''),
    (2, N''Modelo B''),
    (3, N''Modelo C'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Modelo') AND [object_id] = OBJECT_ID(N'[ModelosDePaineisSolares]'))
        SET IDENTITY_INSERT [ModelosDePaineisSolares] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PainelSolarId', N'Potencia') AND [object_id] = OBJECT_ID(N'[PotenciasDePaineisSolares]'))
        SET IDENTITY_INSERT [PotenciasDePaineisSolares] ON;
    EXEC(N'INSERT INTO [PotenciasDePaineisSolares] ([Id], [PainelSolarId], [Potencia])
    VALUES (1, 1, 250.0),
    (2, 1, 270.0),
    (3, 1, 300.0),
    (4, 2, 300.0),
    (5, 2, 320.0),
    (6, 2, 350.0),
    (7, 3, 350.0),
    (8, 3, 380.0),
    (9, 3, 400.0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PainelSolarId', N'Potencia') AND [object_id] = OBJECT_ID(N'[PotenciasDePaineisSolares]'))
        SET IDENTITY_INSERT [PotenciasDePaineisSolares] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DadosInstalacao_CidadeId] ON [DadosInstalacao] ([CidadeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DadosInstalacao_ModeloPainelId] ON [DadosInstalacao] ([ModeloPainelId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PotenciasDePaineisSolares_PainelSolarId] ON [PotenciasDePaineisSolares] ([PainelSolarId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250315231224_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250315231224_InitialCreate', N'9.0.2');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [Cidades] SET [Nome] = N''Albufeira''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [Cidades] SET [Nome] = N''Almada''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [Cidades] SET [Nome] = N''Amadora''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [Cidades] SET [Nome] = N''Aveiro''
    WHERE [Id] = 4;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome') AND [object_id] = OBJECT_ID(N'[Cidades]'))
        SET IDENTITY_INSERT [Cidades] ON;
    EXEC(N'INSERT INTO [Cidades] ([Id], [Nome])
    VALUES (5, N''Barcelos''),
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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [ModelosDePaineisSolares] SET [Modelo] = N''Aiko - Comet 2U''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [ModelosDePaineisSolares] SET [Modelo] = N''Maxeon 7''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [ModelosDePaineisSolares] SET [Modelo] = N''Longi - HI-MO X6''
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Modelo') AND [object_id] = OBJECT_ID(N'[ModelosDePaineisSolares]'))
        SET IDENTITY_INSERT [ModelosDePaineisSolares] ON;
    EXEC(N'INSERT INTO [ModelosDePaineisSolares] ([Id], [Modelo])
    VALUES (4, N''Huasun - Himalaya''),
    (5, N''TW Solar''),
    (6, N''JA Solar DeepBlue 4.0 Pro''),
    (7, N''Astroenergy - Astro N5''),
    (8, N''Grand Sunergy''),
    (9, N''DMEGC - Infinity RT''),
    (10, N''Spic'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Modelo') AND [object_id] = OBJECT_ID(N'[ModelosDePaineisSolares]'))
        SET IDENTITY_INSERT [ModelosDePaineisSolares] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [Potencia] = 670.0
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 2, [Potencia] = 445.0
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 3, [Potencia] = 600.0
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 4, [Potencia] = 720.0
    WHERE [Id] = 4;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 5, [Potencia] = 715.0
    WHERE [Id] = 5;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 5, [Potencia] = 590.0
    WHERE [Id] = 6;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 6, [Potencia] = 595.0
    WHERE [Id] = 7;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 7, [Potencia] = 640.0
    WHERE [Id] = 8;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 8, [Potencia] = 710.0
    WHERE [Id] = 9;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PainelSolarId', N'Potencia') AND [object_id] = OBJECT_ID(N'[PotenciasDePaineisSolares]'))
        SET IDENTITY_INSERT [PotenciasDePaineisSolares] ON;
    EXEC(N'INSERT INTO [PotenciasDePaineisSolares] ([Id], [PainelSolarId], [Potencia])
    VALUES (10, 9, 615.0),
    (11, 10, 410.0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PainelSolarId', N'Potencia') AND [object_id] = OBJECT_ID(N'[PotenciasDePaineisSolares]'))
        SET IDENTITY_INSERT [PotenciasDePaineisSolares] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195205_population'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250316195205_population', N'9.0.2');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 1, [Potencia] = 680.0
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 1, [Potencia] = 690.0
    WHERE [Id] = 3;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 1, [Potencia] = 700.0
    WHERE [Id] = 4;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 2, [Potencia] = 445.0
    WHERE [Id] = 5;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 2, [Potencia] = 455.0
    WHERE [Id] = 6;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 2, [Potencia] = 465.0
    WHERE [Id] = 7;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 2, [Potencia] = 475.0
    WHERE [Id] = 8;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 3, [Potencia] = 600.0
    WHERE [Id] = 9;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 3, [Potencia] = 610.0
    WHERE [Id] = 10;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    EXEC(N'UPDATE [PotenciasDePaineisSolares] SET [PainelSolarId] = 3, [Potencia] = 620.0
    WHERE [Id] = 11;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PainelSolarId', N'Potencia') AND [object_id] = OBJECT_ID(N'[PotenciasDePaineisSolares]'))
        SET IDENTITY_INSERT [PotenciasDePaineisSolares] ON;
    EXEC(N'INSERT INTO [PotenciasDePaineisSolares] ([Id], [PainelSolarId], [Potencia])
    VALUES (12, 3, 630.0),
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
    (42, 10, 420.0),
    (43, 10, 430.0),
    (44, 10, 440.0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'PainelSolarId', N'Potencia') AND [object_id] = OBJECT_ID(N'[PotenciasDePaineisSolares]'))
        SET IDENTITY_INSERT [PotenciasDePaineisSolares] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250316195936_populationPOWER'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250316195936_populationPOWER', N'9.0.2');
END;

COMMIT;
GO

