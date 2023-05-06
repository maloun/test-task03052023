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

CREATE TABLE [HouseConsumers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [ConsumerId] int NOT NULL,
    [UploadDateTime] datetime2 NOT NULL,
    CONSTRAINT [PK_HouseConsumers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PlantsConsumers] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [ConsumerId] int NOT NULL,
    [UploadDateTime] datetime2 NOT NULL,
    CONSTRAINT [PK_PlantsConsumers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [HouseConsumptions] (
    [Id] int NOT NULL IDENTITY,
    [Date] datetime2 NOT NULL,
    [Weather] float NOT NULL,
    [Consumption] float NOT NULL,
    [ConsumerId] int NOT NULL,
    CONSTRAINT [PK_HouseConsumptions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_HouseConsumptions_HouseConsumers_ConsumerId] FOREIGN KEY ([ConsumerId]) REFERENCES [HouseConsumers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlantsConsumptions] (
    [Id] int NOT NULL IDENTITY,
    [Date] datetime2 NOT NULL,
    [Price] float NOT NULL,
    [Consumption] float NOT NULL,
    [ConsumerId] int NOT NULL,
    CONSTRAINT [PK_PlantsConsumptions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PlantsConsumptions_PlantsConsumers_ConsumerId] FOREIGN KEY ([ConsumerId]) REFERENCES [PlantsConsumers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_HouseConsumptions_ConsumerId] ON [HouseConsumptions] ([ConsumerId]);
GO

CREATE INDEX [IX_PlantsConsumptions_ConsumerId] ON [PlantsConsumptions] ([ConsumerId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230504171904_InitialCreate', N'7.0.5');
GO

COMMIT;
GO

