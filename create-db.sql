-- Create SpaceFlowCrmDb database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SpaceFlowCrmDb')
BEGIN
    CREATE DATABASE SpaceFlowCrmDb;
END
GO

USE SpaceFlowCrmDb;
GO

-- Create LeadsClients table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LeadsClients]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LeadsClients] (
        [Id] [int] IDENTITY(1,1) PRIMARY KEY,
        [FullName] [nvarchar](200) NOT NULL,
        [Phone] [nvarchar](50) NOT NULL,
        [Email] [nvarchar](200) NULL,
        [Source] [nvarchar](100) NOT NULL,
        [Status] [nvarchar](50) NOT NULL,
        [NextFollowUpAt] [datetime2] NULL,
        [Notes] [nvarchar](2000) NULL,
        [CreatedAt] [datetime2] NOT NULL,
        [UpdatedAt] [datetime2] NOT NULL
    );
END
GO

-- Create Interactions table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Interactions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Interactions] (
        [Id] [int] IDENTITY(1,1) PRIMARY KEY,
        [LeadClientId] [int] NOT NULL,
        [Channel] [nvarchar](50) NOT NULL,
        [Summary] [nvarchar](1000) NOT NULL,
        [InteractionAt] [datetime2] NOT NULL,
        [NextFollowUpAt] [datetime2] NULL,
        CONSTRAINT [FK_Interactions_LeadsClients] FOREIGN KEY ([LeadClientId]) REFERENCES [dbo].[LeadsClients]([Id]) ON DELETE CASCADE
    );
END
GO

-- Create index on Interactions.LeadClientId
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Interactions_LeadClientId' AND object_id = OBJECT_ID(N'[dbo].[Interactions]'))
BEGIN
    CREATE INDEX [IX_Interactions_LeadClientId] ON [dbo].[Interactions] ([LeadClientId]);
END
GO

PRINT 'SpaceFlowCrmDb database and tables created successfully!';
