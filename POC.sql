CREATE DATABASE POC
USE POC

CREATE TABLE [dbo].[UserMaster] (
    [UserId]    INT          IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (50) NOT NULL,
    [LastName]  VARCHAR (50) NOT NULL,
    [UserName]  VARCHAR (50) NOT NULL,
    [Password]  VARCHAR (50) NOT NULL,
    [Email]     VARCHAR (50) NULL,
    [Phone]     VARCHAR (50) NULL,
    [Gender]    VARCHAR (15) NOT NULL,
    [Age]       INT          NOT NULL,
    [CountryId] INT          NOT NULL,
    [StateId]   INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

CREATE TABLE [dbo].[Country] (
    [CountryId]   INT          IDENTITY (1, 1) NOT NULL,
    [CountryName] VARCHAR (50) NOT NULL,
    [CountryCode] VARCHAR (5)  NOT NULL,
    PRIMARY KEY CLUSTERED ([CountryId] ASC)
);

CREATE TABLE [dbo].[State] (
    [SId]       INT          IDENTITY (1, 1) NOT NULL,
    [StateName] VARCHAR (50) NOT NULL,
    [CountryId] INT          NULL,
    PRIMARY KEY CLUSTERED ([SId] ASC),
    FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([CountryId])
);

