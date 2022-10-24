
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/23/2022 20:32:43
-- Generated from EDMX file: C:\Users\paulo\source\repos\unoProyect\Server\Services\UnoEntitys\unoDbModel.edmx
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

IF OBJECT_ID(N'[dbo].[FK_credentialsplayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[credentialsSet] DROP CONSTRAINT [FK_credentialsplayer];
GO
IF OBJECT_ID(N'[dbo].[FK_friendsList_player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[friendsList] DROP CONSTRAINT [FK_friendsList_player];
GO
IF OBJECT_ID(N'[dbo].[FK_friendsList_player1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[friendsList] DROP CONSTRAINT [FK_friendsList_player1];
GO
IF OBJECT_ID(N'[dbo].[FK_imagesplayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[playerSet] DROP CONSTRAINT [FK_imagesplayer];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[credentialsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[credentialsSet];
GO
IF OBJECT_ID(N'[dbo].[friendsList]', 'U') IS NOT NULL
    DROP TABLE [dbo].[friendsList];
GO
IF OBJECT_ID(N'[dbo].[imagesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[imagesSet];
GO
IF OBJECT_ID(N'[dbo].[playerSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[playerSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'playerSet'
CREATE TABLE [dbo].[playerSet] (
    [IdPlayer] int IDENTITY(1,1) NOT NULL,
    [wins] int  NOT NULL,
    [losts] bigint  NOT NULL,
    [images_Id] int  NOT NULL
);
GO

-- Creating table 'credentialsSet'
CREATE TABLE [dbo].[credentialsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [password] nvarchar(max)  NOT NULL,
    [username] nvarchar(max)  NOT NULL,
    [email] nvarchar(max)  NOT NULL,
    [player_IdPlayer] int  NOT NULL
);
GO

-- Creating table 'imagesSet'
CREATE TABLE [dbo].[imagesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [path] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'friendsList'
CREATE TABLE [dbo].[friendsList] (
    [friends_IdPlayer] int  NOT NULL,
    [friendsList_player_IdPlayer] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdPlayer] in table 'playerSet'
ALTER TABLE [dbo].[playerSet]
ADD CONSTRAINT [PK_playerSet]
    PRIMARY KEY CLUSTERED ([IdPlayer] ASC);
GO

-- Creating primary key on [Id] in table 'credentialsSet'
ALTER TABLE [dbo].[credentialsSet]
ADD CONSTRAINT [PK_credentialsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'imagesSet'
ALTER TABLE [dbo].[imagesSet]
ADD CONSTRAINT [PK_imagesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [friends_IdPlayer], [friendsList_player_IdPlayer] in table 'friendsList'
ALTER TABLE [dbo].[friendsList]
ADD CONSTRAINT [PK_friendsList]
    PRIMARY KEY CLUSTERED ([friends_IdPlayer], [friendsList_player_IdPlayer] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [friends_IdPlayer] in table 'friendsList'
ALTER TABLE [dbo].[friendsList]
ADD CONSTRAINT [FK_friendsList_player]
    FOREIGN KEY ([friends_IdPlayer])
    REFERENCES [dbo].[playerSet]
        ([IdPlayer])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [friendsList_player_IdPlayer] in table 'friendsList'
ALTER TABLE [dbo].[friendsList]
ADD CONSTRAINT [FK_friendsList_player1]
    FOREIGN KEY ([friendsList_player_IdPlayer])
    REFERENCES [dbo].[playerSet]
        ([IdPlayer])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_friendsList_player1'
CREATE INDEX [IX_FK_friendsList_player1]
ON [dbo].[friendsList]
    ([friendsList_player_IdPlayer]);
GO

-- Creating foreign key on [player_IdPlayer] in table 'credentialsSet'
ALTER TABLE [dbo].[credentialsSet]
ADD CONSTRAINT [FK_credentialsplayer]
    FOREIGN KEY ([player_IdPlayer])
    REFERENCES [dbo].[playerSet]
        ([IdPlayer])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_credentialsplayer'
CREATE INDEX [IX_FK_credentialsplayer]
ON [dbo].[credentialsSet]
    ([player_IdPlayer]);
GO

-- Creating foreign key on [images_Id] in table 'playerSet'
ALTER TABLE [dbo].[playerSet]
ADD CONSTRAINT [FK_imagesplayer]
    FOREIGN KEY ([images_Id])
    REFERENCES [dbo].[imagesSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_imagesplayer'
CREATE INDEX [IX_FK_imagesplayer]
ON [dbo].[playerSet]
    ([images_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------