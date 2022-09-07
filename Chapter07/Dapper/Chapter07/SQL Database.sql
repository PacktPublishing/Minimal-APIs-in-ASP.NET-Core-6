CREATE TABLE [dbo].[Icecreams](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NOT NULL)
GO

INSERT [dbo].[Icecreams] ([Name], [Description]) VALUES ('Icecream 1','Description 1')
INSERT [dbo].[Icecreams] ([Name], [Description]) VALUES ('Icecream 2','Description 2')
INSERT [dbo].[Icecreams] ([Name], [Description]) VALUES ('Icecream 3','Description 3')
