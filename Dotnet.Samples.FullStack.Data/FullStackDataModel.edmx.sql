
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/16/2014 15:06:59
-- Generated from EDMX file: C:\src\Dotnet.Samples.FullStack\Dotnet.Samples.FullStack.Data\FullStackDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Catalog];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[CatalogModelStoreContainer].[Books]', 'U') IS NOT NULL
    DROP TABLE [CatalogModelStoreContainer].[Books];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Books'
CREATE TABLE [dbo].[Books] (
    [Isbn] nvarchar(17)  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Author] nvarchar(max)  NOT NULL,
    [Published] datetime  NOT NULL,
    [Publisher] nvarchar(max)  NOT NULL,
    [Pages] int  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [InStock] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Isbn] in table 'Books'
ALTER TABLE [dbo].[Books]
ADD CONSTRAINT [PK_Books]
    PRIMARY KEY CLUSTERED ([Isbn] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------