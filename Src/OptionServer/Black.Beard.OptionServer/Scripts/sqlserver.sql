USE [master]
GO
/****** Object:  Database [Options]    Script Date: 26/04/2019 18:29:19 ******/
CREATE DATABASE [Options]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Options', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Options.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Options_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Options_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Options] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Options].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Options] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Options] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Options] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Options] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Options] SET ARITHABORT OFF 
GO
ALTER DATABASE [Options] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Options] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Options] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Options] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Options] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Options] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Options] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Options] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Options] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Options] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Options] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Options] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Options] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Options] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Options] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Options] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Options] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Options] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Options] SET  MULTI_USER 
GO
ALTER DATABASE [Options] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Options] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Options] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Options] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Options] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Options] SET QUERY_STORE = OFF
GO
USE [Options]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [Options]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 26/04/2019 18:29:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application](
	[Id] [uniqueidentifier] NOT NULL,
	[LastUpdate] [datetimeoffset](7) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[GroupId] [uniqueidentifier] NOT NULL,
	[SecurityCoherence] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationAccess]    Script Date: 26/04/2019 18:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationAccess](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[AccessApplication] [int] NOT NULL,
	[LastUpdate] [datetimeoffset](7) NOT NULL,
	[SecurityCoherence] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ApplicationAccess] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentVersion]    Script Date: 26/04/2019 18:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentVersion](
	[Id] [uniqueidentifier] NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[LastUpdate] [datetimeoffset](7) NOT NULL,
	[EnvironmentId] [uniqueidentifier] NOT NULL,
	[TypeId] [uniqueidentifier] NOT NULL,
	[TypeVersionId] [uniqueidentifier] NOT NULL,
	[Payload] [nvarchar](MAX) NOT NULL,
	[Sha256] [varchar](70) NOT NULL,
	[Version] int NOT NULL,
	[Deleted] bit NOT NULL,
	[SecurityCoherence] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DocumentVersion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationGroup]    Script Date: 26/04/2019 18:29:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationGroup](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[OwnerUserId] [uniqueidentifier] NOT NULL,
	[LastUpdate] [datetimeoffset](7) NOT NULL,
	[SecurityCoherence] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ApplicationGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationGroupAccess]    Script Date: 26/04/2019 18:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationGroupAccess](
	[ApplicationGroupId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[AccessApplication] [int] NOT NULL,
	[AccessType] [int] NOT NULL,
	[AccessEnvironment] [int] NOT NULL,
	[LastUpdate] [datetimeoffset](7) NOT NULL,
	[SecurityCoherence] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ApplicationGroupAccess] PRIMARY KEY CLUSTERED 
(
	[ApplicationGroupId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Environment]    Script Date: 26/04/2019 18:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Environment](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[GroupId] [uniqueidentifier] NOT NULL,
	[LastUpdate] [datetimeoffset](7) NOT NULL,
	[SecurityCoherence] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Environment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Type]    Script Date: 26/04/2019 18:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Type](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Extension] [nvarchar](10) NOT NULL,
	[GroupId] [uniqueidentifier] NOT NULL,
	[CurrentVersionId] [uniqueidentifier] NULL,
	[LastUpdate] [datetimeoffset](7) NOT NULL,
	[SecurityCoherence] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeVersion]    Script Date: 26/04/2019 18:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeVersion](
	[Id] [uniqueidentifier] NOT NULL,
	[LastUpdate] [datetimeoffset](7) NOT NULL,
	[Version] [int] NOT NULL,
	[TypeId] [uniqueidentifier] NOT NULL,
	[Contract] [nvarchar](max) NOT NULL,
	[Sha256] [varchar](70) NOT NULL,
	[SecurityCoherence] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TypeVersion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 26/04/2019 18:29:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [varchar](250) NOT NULL,
	[Pseudo] [varchar](250) NOT NULL,
	[Email] [varchar](500) NOT NULL,
	[HashPassword] [varchar](250) NOT NULL,
	[LastUpdate] [datetimeoffset](7) NOT NULL,
	[SecurityCoherence] [uniqueidentifier] NOT NULL,
	[AccessProfile] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IndexUniqueApplicationInGroup]    Script Date: 26/04/2019 18:29:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IndexUniqueApplicationInGroup] ON [dbo].[Application]
(
	[GroupId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO


/****** Object:  Index [NonUniqueDocumentApplicationId]    Script Date: 26/04/2019 18:29:23 ******/
CREATE NONCLUSTERED INDEX [NonUniqueDocumentApplicationId] ON [dbo].[DocumentVersion]
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [NonUniqueDocumentApplicationName] ON [dbo].[DocumentVersion]
(
	[ApplicationId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

CREATE UNIQUE NONCLUSTERED INDEX [UniqueDocumentApplicationNameVersion] ON [dbo].[DocumentVersion]
(
	[ApplicationId] ASC,
	[Name] ASC,
	[Version]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

SET ANSI_PADDING ON
GO
/****** Object:  Index [ApplicationGroupNameUnique]    Script Date: 26/04/2019 18:29:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [ApplicationGroupNameUnique] ON [dbo].[ApplicationGroup]
(
	[OwnerUserId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UniqueEnvironmentName]    Script Date: 26/04/2019 18:29:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UniqueEnvironmentName] ON [dbo].[Environment]
(
	[GroupId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IndexTypeUniqueExtension]    Script Date: 26/04/2019 18:29:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IndexTypeUniqueExtension] ON [dbo].[Type]
(
	[GroupId] ASC,
	[Extension] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IndexTypeUniqueName]    Script Date: 26/04/2019 18:29:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IndexTypeUniqueName] ON [dbo].[Type]
(
	[GroupId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IndexTypeVersionSha256]    Script Date: 26/04/2019 18:29:23 ******/
CREATE NONCLUSTERED INDEX [IndexTypeVersionSha256] ON [dbo].[TypeVersion]
(
	[TypeId] ASC,
	[Sha256] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UsersUniqueUsername]    Script Date: 26/04/2019 18:29:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UsersUniqueUsername] ON [dbo].[Users]
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserUniqueEmail]    Script Date: 26/04/2019 18:29:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserUniqueEmail] ON [dbo].[Users]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserUniquePseudo]    Script Date: 26/04/2019 18:29:23 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserUniquePseudo] ON [dbo].[Users]
(
	[Pseudo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ApplicationAccess] 
	ADD DEFAULT ((0)) FOR [AccessApplication]
GO
ALTER TABLE [dbo].[ApplicationGroupAccess] 
	ADD DEFAULT ((0)) FOR [AccessApplication]
GO
ALTER TABLE [dbo].[ApplicationGroupAccess] 
	ADD DEFAULT ((0)) FOR [AccessType]
GO
ALTER TABLE [dbo].[ApplicationGroupAccess] 
	ADD DEFAULT ((0)) FOR [AccessEnvironment]
GO
ALTER TABLE [dbo].[Users] 
	ADD DEFAULT ((0)) FOR [AccessProfile]
GO
ALTER TABLE [dbo].[DocumentVersion] 
	ADD DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[Application]  WITH CHECK 
	ADD CONSTRAINT [FK_Application_GroupApplication] FOREIGN KEY([GroupId])
	REFERENCES [dbo].[ApplicationGroup] ([Id])
GO
ALTER TABLE [dbo].[Application] CHECK 
	CONSTRAINT [FK_Application_GroupApplication]
GO

ALTER TABLE [dbo].[ApplicationAccess]  WITH CHECK 
	ADD CONSTRAINT [FK_ApplicationAccess_ApplicationId] FOREIGN KEY([ApplicationId])
	REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[ApplicationAccess] CHECK 
	CONSTRAINT [FK_ApplicationAccess_ApplicationId]
GO

ALTER TABLE [dbo].[ApplicationAccess]  WITH CHECK 
	ADD CONSTRAINT [FK_ApplicationAccess_UserId] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ApplicationAccess] CHECK 
	CONSTRAINT [FK_ApplicationAccess_UserId]
GO

ALTER TABLE [dbo].[DocumentVersion]  WITH CHECK 
	ADD CONSTRAINT [FK_DocumentVersion_Application] FOREIGN KEY([ApplicationId])
	REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[DocumentVersion] CHECK CONSTRAINT [FK_DocumentVersion_Application]
GO

ALTER TABLE [dbo].[DocumentVersion]  WITH CHECK 
	ADD CONSTRAINT [FK_DocumentVersionVersion_Environment] FOREIGN KEY([EnvironmentId])
	REFERENCES [dbo].[Environment] ([Id])
GO
ALTER TABLE [dbo].[DocumentVersion] CHECK CONSTRAINT [FK_DocumentVersionVersion_Environment]
GO

ALTER TABLE [dbo].[DocumentVersion]  WITH CHECK 
	ADD CONSTRAINT [FK_DocumentVersionType] FOREIGN KEY([TypeId])
	REFERENCES [dbo].[Type] ([Id])
GO
ALTER TABLE [dbo].[DocumentVersion] CHECK CONSTRAINT [FK_DocumentVersionType]
GO

ALTER TABLE [dbo].[DocumentVersion]  WITH CHECK 
	ADD CONSTRAINT [FK_FK_DocumentVersionVersion_Type] FOREIGN KEY([TypeVersionId])
	REFERENCES [dbo].[TypeVersion] ([Id])
GO
ALTER TABLE [dbo].[DocumentVersion] CHECK CONSTRAINT [FK_FK_DocumentVersionVersion_Type]
GO

ALTER TABLE [dbo].[ApplicationGroupAccess]  WITH CHECK 
	ADD CONSTRAINT [FK_ApplicationGroupAccess_ApplicationGroupId] FOREIGN KEY([ApplicationGroupId])
	REFERENCES [dbo].[ApplicationGroup] ([Id])
GO
ALTER TABLE [dbo].[ApplicationGroupAccess] CHECK 
	CONSTRAINT [FK_ApplicationGroupAccess_ApplicationGroupId]
GO

ALTER TABLE [dbo].[ApplicationGroupAccess]  WITH CHECK 
	ADD CONSTRAINT [FK_ApplicationGroupAccess_UserId] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ApplicationGroupAccess] CHECK 
	CONSTRAINT [FK_ApplicationGroupAccess_UserId]
GO

ALTER TABLE [dbo].[Environment]  WITH CHECK 
	ADD CONSTRAINT [FK_Environ_GroupId] FOREIGN KEY([GroupId])
	REFERENCES [dbo].[ApplicationGroup] ([Id])
GO
ALTER TABLE [dbo].[Environment] CHECK 
	CONSTRAINT [FK_Environ_GroupId]
GO

ALTER TABLE [dbo].[Type]  WITH CHECK 
	ADD CONSTRAINT [FK_Type_GroupId] FOREIGN KEY([GroupId])
	REFERENCES [dbo].[ApplicationGroup] ([Id])
GO
ALTER TABLE [dbo].[Type] CHECK 
	CONSTRAINT [FK_Type_GroupId]
GO

ALTER TABLE [dbo].[TypeVersion]  WITH CHECK 
	ADD CONSTRAINT [FK_TypeVersion_TypeId] FOREIGN KEY([TypeId])
	REFERENCES [dbo].[Type] ([Id])
GO
ALTER TABLE [dbo].[TypeVersion] CHECK 
	CONSTRAINT [FK_TypeVersion_TypeId]
GO

USE [master]
GO
ALTER DATABASE [Options] SET  READ_WRITE 
GO
