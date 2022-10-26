
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/26/2022 07:30:33
-- Generated from EDMX file: C:\Users\paulo\source\repos\unoProyect\Server\UnoEntitys\unoDbModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [unodb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_friendsList_player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[friendsList] DROP CONSTRAINT [FK_friendsList_player];
GO
IF OBJECT_ID(N'[dbo].[FK_friendsList_player1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[friendsList] DROP CONSTRAINT [FK_friendsList_player1];
GO
IF OBJECT_ID(N'[dbo].[FK_credentialsplayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CredentialsSet1] DROP CONSTRAINT [FK_credentialsplayer];
GO
IF OBJECT_ID(N'[dbo].[FK_imagesplayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerSet1] DROP CONSTRAINT [FK_imagesplayer];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[PlayerSet1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayerSet1];
GO
IF OBJECT_ID(N'[dbo].[CredentialsSet1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CredentialsSet1];
GO
IF OBJECT_ID(N'[dbo].[ImagesSet1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ImagesSet1];
GO
IF OBJECT_ID(N'[dbo].[friendsList]', 'U') IS NOT NULL
    DROP TABLE [dbo].[friendsList];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'PlayerSet1'
CREATE TABLE [dbo].[PlayerSet1] (
    [IdPlayer] int IDENTITY(1,1) NOT NULL,
    [wins] int  NOT NULL,
    [losts] bigint  NOT NULL,
    [Images_Id] int  NOT NULL
);
GO

-- Creating table 'CredentialsSet1'
CREATE TABLE [dbo].[CredentialsSet1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [password] nvarchar(max)  NOT NULL,
    [username] nvarchar(max)  NOT NULL,
    [email] nvarchar(max)  NOT NULL,
    [Player_IdPlayer] int  NOT NULL
);
GO

-- Creating table 'ImagesSet1'
CREATE TABLE [dbo].[ImagesSet1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [path] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'friendsList'
CREATE TABLE [dbo].[friendsList] (
    [Friends_IdPlayer] int  NOT NULL,
    [friendsList_player_IdPlayer] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdPlayer] in table 'PlayerSet1'
ALTER TABLE [dbo].[PlayerSet1]
ADD CONSTRAINT [PK_PlayerSet1]
    PRIMARY KEY CLUSTERED ([IdPlayer] ASC);
GO

-- Creating primary key on [Id] in table 'CredentialsSet1'
ALTER TABLE [dbo].[CredentialsSet1]
ADD CONSTRAINT [PK_CredentialsSet1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ImagesSet1'
ALTER TABLE [dbo].[ImagesSet1]
ADD CONSTRAINT [PK_ImagesSet1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Friends_IdPlayer], [friendsList_player_IdPlayer] in table 'friendsList'
ALTER TABLE [dbo].[friendsList]
ADD CONSTRAINT [PK_friendsList]
    PRIMARY KEY CLUSTERED ([Friends_IdPlayer], [friendsList_player_IdPlayer] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Friends_IdPlayer] in table 'friendsList'
ALTER TABLE [dbo].[friendsList]
ADD CONSTRAINT [FK_friendsList_player]
    FOREIGN KEY ([Friends_IdPlayer])
    REFERENCES [dbo].[PlayerSet1]
        ([IdPlayer])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [friendsList_player_IdPlayer] in table 'friendsList'
ALTER TABLE [dbo].[friendsList]
ADD CONSTRAINT [FK_friendsList_player1]
    FOREIGN KEY ([friendsList_player_IdPlayer])
    REFERENCES [dbo].[PlayerSet1]
        ([IdPlayer])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_friendsList_player1'
CREATE INDEX [IX_FK_friendsList_player1]
ON [dbo].[friendsList]
    ([friendsList_player_IdPlayer]);
GO

-- Creating foreign key on [Player_IdPlayer] in table 'CredentialsSet1'
ALTER TABLE [dbo].[CredentialsSet1]
ADD CONSTRAINT [FK_credentialsplayer]
    FOREIGN KEY ([Player_IdPlayer])
    REFERENCES [dbo].[PlayerSet1]
        ([IdPlayer])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_credentialsplayer'
CREATE INDEX [IX_FK_credentialsplayer]
ON [dbo].[CredentialsSet1]
    ([Player_IdPlayer]);
GO

-- Creating foreign key on [Images_Id] in table 'PlayerSet1'
ALTER TABLE [dbo].[PlayerSet1]
ADD CONSTRAINT [FK_imagesplayer]
    FOREIGN KEY ([Images_Id])
    REFERENCES [dbo].[ImagesSet1]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_imagesplayer'
CREATE INDEX [IX_FK_imagesplayer]
ON [dbo].[PlayerSet1]
    ([Images_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------