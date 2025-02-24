CREATE TABLE [dbo].[ROICalculators] (
    [Id]                     INT             IDENTITY (1, 1) NOT NULL,
    [UserEmail]              NVARCHAR (MAX)  NOT NULL,
    [CustoInstalacao]        DECIMAL (18, 2) NULL,
    [CustoManutencaoAnual]   DECIMAL (18, 2) NULL,
    [ConsumoEnergeticoMedio] DECIMAL (18, 2) NULL,
    [ConsumoEnergeticoRede]  DECIMAL (18, 2) NULL,
    [RetornoEconomia]        DECIMAL (18, 2) NULL,
    [ROI]                    DECIMAL (18, 2) NULL,
    [DataCalculado]          DATETIME2 (7)   NULL,
    CONSTRAINT [PK_ROICalculators] PRIMARY KEY CLUSTERED ([Id] ASC)
);

