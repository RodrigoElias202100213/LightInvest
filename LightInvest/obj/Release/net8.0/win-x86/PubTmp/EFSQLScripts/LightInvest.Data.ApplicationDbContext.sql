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

COMMIT;
GO

