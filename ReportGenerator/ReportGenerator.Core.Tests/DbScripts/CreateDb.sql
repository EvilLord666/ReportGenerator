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