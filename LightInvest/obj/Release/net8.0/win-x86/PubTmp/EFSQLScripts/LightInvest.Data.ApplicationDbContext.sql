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
    WHERE [MigrationId] = N'20250228191911_InitialCommunityMigration'
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
    WHERE [MigrationId] = N'20250228191911_InitialCommunityMigration'
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
    WHERE [MigrationId] = N'20250228191911_InitialCommunityMigration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250228191911_InitialCommunityMigration', N'9.0.1');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250301011214_AddPasswordResetToken'
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
    WHERE [MigrationId] = N'20250301011214_AddPasswordResetToken'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250301011214_AddPasswordResetToken', N'9.0.1');
END;

COMMIT;
GO

