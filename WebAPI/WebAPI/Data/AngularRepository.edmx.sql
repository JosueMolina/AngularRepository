
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/06/2017 23:34:00
-- Generated from EDMX file: C:\Users\Kevin Molina\Desktop\WebAPI\Data\AngularRepository.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [AngularRepository];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Link_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Link] DROP CONSTRAINT [FK_Link_Category];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[Link]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Link];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Category'
CREATE TABLE [dbo].[Category] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(250)  NULL
);
GO

-- Creating table 'Link'
CREATE TABLE [dbo].[Link] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [LinkValue] nvarchar(max)  NOT NULL,
    [NumberOfRatings] int  NULL,
    [Rating] decimal(18,2)  NULL,
    [IdCategory] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Category'
ALTER TABLE [dbo].[Category]
ADD CONSTRAINT [PK_Category]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Link'
ALTER TABLE [dbo].[Link]
ADD CONSTRAINT [PK_Link]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IdCategory] in table 'Link'
ALTER TABLE [dbo].[Link]
ADD CONSTRAINT [FK_Link_Category]
    FOREIGN KEY ([IdCategory])
    REFERENCES [dbo].[Category]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Link_Category'
CREATE INDEX [IX_FK_Link_Category]
ON [dbo].[Link]
    ([IdCategory]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------