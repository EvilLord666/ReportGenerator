CREATE TABLE [dbo].[Citizen]
(
   [Id] [uniqueidentifier] NOT NULL,
   [FirstName] [nvarchar] (100) NOT NULL,
   [LastName] [nvarchar] (100) NOT NULL,
   [Age] [int] NOT NULL,
   [Sex] [bit] NOT NULL,
   [CityId] [uniqueidentifier] NOT NULL,
   CONSTRAINT [PK_Citizen] PRIMARY KEY CLUSTERED([Id] ASC)
);

CREATE TABLE [dbo].[City]
(
   [Id] [uniqueidentifier] NOT NULL,
   [Name] [nvarchar] (200) NOT NULL,
   [RegionId] [uniqueidentifier] NOT NULL,
   CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED([Id] ASC)
);

CREATE TABLE [dbo].[Region]
(
   [Id] [uniqueidentifier] NOT NULL,
   [Name] [nvarchar] (200) NOT NULL,
   CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED([Id] ASC)
);

ALTER TABLE [dbo].[City] WITH CHECK ADD  CONSTRAINT [FK_City_Region] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])

ALTER TABLE [dbo].[Citizen] WITH CHECK ADD  CONSTRAINT [FK_Citizen_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])

EXEC('CREATE PROCEDURE [dbo].SelectCitizensWithCities AS
BEGIN
    SELECT [FirstName], [LastName], [Name] AS CityName FROM [dbo].[Citizen] AS Cz
	INNER JOIN [dbo].[City] AS Ci ON  Cz.CityId = Ci.Id;
END');


EXEC('CREATE PROCEDURE [dbo].SelectCitizensWithCitiesByCity @City [nvarchar] (200) AS
BEGIN
    SELECT Cz.[FirstName], Cz.[LastName], Ci.[Name] AS CityName FROM [dbo].[Citizen] AS Cz
	INNER JOIN [dbo].[City] AS Ci ON Cz.CityId = Ci.Id
	WHERE Ci.[Name] = @City;
END');

EXEC('CREATE PROCEDURE [dbo].SelectCitizensWithCitiesByCityAndAge @City [nvarchar] (200), @PersonAge [int] AS
BEGIN
    SELECT Cz.[FirstName], Cz.[LastName], Cz.[Age], Ci.[Name] AS CityName FROM [dbo].[Citizen] AS Cz
	INNER JOIN [dbo].[City] AS Ci ON  Cz.CityId = Ci.Id
	WHERE Ci.[Name] = @City AND Cz.[Age] > @PersonAge;
END');

EXEC('CREATE VIEW [dbo].[CitizensWithRegion] AS
SELECT Cz.[FirstName], Cz.[LastName], Cz.[Age], Ci.[Name] AS City, R.[Name] AS Region 
FROM [dbo].[Citizen] AS Cz INNER JOIN [dbo].[City] AS Ci ON Cz.[CityId] = Ci.[Id]
INNER JOIN [dbo].[Region] AS R ON Ci.[RegionId] = R.[Id];');