USE [master]
GO
/****** Object:  Database [NAV]    Script Date: 21/08/2020 5:20:55 AM ******/
CREATE DATABASE [NAV]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NAV', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\NAV.mdf' , SIZE = 71488KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NAV_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\NAV_log.ldf' , SIZE = 24576KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [NAV] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NAV].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NAV] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NAV] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NAV] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NAV] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NAV] SET ARITHABORT OFF 
GO
ALTER DATABASE [NAV] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NAV] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NAV] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NAV] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NAV] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NAV] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NAV] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NAV] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NAV] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NAV] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NAV] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NAV] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NAV] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NAV] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NAV] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NAV] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NAV] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NAV] SET RECOVERY FULL 
GO
ALTER DATABASE [NAV] SET  MULTI_USER 
GO
ALTER DATABASE [NAV] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NAV] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NAV] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NAV] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NAV] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'NAV', N'ON'
GO
ALTER DATABASE [NAV] SET QUERY_STORE = OFF
GO
USE [NAV]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetAbbreviationByUserID]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 26/05/2020
-- Description:	Get the first occurance of a word and return the word abbreviation
--
-- Parameters:
--
--	@iUserID: The ID of the user making the request
--	@nvWord: The word to search for
--	@bFlaggedRowsOnly: When present, the boolean value defining those abbreviations that can be searched
--
--	Notes:
--
--	When a word is not found the procedure returns the original search word in place of an abbreviation
--
-- =================================================================================================================================

CREATE FUNCTION [dbo].[fn_GetAbbreviationByUserID](
	@iUserID int = 0,
	@nvWord nvarchar(100) = null,
	@bFlaggedRowsOnly bit = 0
)
RETURNS nvarchar(100)
AS
BEGIN

	DECLARE @TRUE bit = 1
	DECLARE @FALSE bit = 0
	DECLARE @nvAbbreviation nvarchar(100) = NULL

	IF @bFlaggedRowsOnly = @TRUE
	BEGIN
		SELECT TOP 1 @nvAbbreviation = nvAbbreviation
		FROM vWordAbbreviationByUserID
		WHERE iUserID = @iUserID AND nvWord = @nvWord AND bAlwaysUse = @TRUE
	END ELSE
	BEGIN
		SELECT TOP 1 @nvAbbreviation = nvAbbreviation
		FROM vWordAbbreviationByUserID
		WHERE iUserID = @iUserID AND nvWord = @nvWord
	END

	RETURN  ISNULL(@nvAbbreviation, @nvWord)

END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetSentenceAbbreviation]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 26/05/2020
-- Description:		Find available abbreviations for each word in a sentence and return the altered sentence
--
-- Parameters:
--
--	@iUserID: The ID of the user making the request
--	@inputString: The word or phrase to be parsed
--	@bFlaggedRowsOnly: When present, the boolean value defining those abbreviations that can be searched
--
--	Notes:
--
--	When an abbreviation is not found the procedure returns the original search word in place of an abbreviation
--
-- =================================================================================================================================

CREATE FUNCTION [dbo].[fn_GetSentenceAbbreviation](
	@iUserID int = 0,
	@inputString nvarchar(MAX) = null,
	@bFlaggedRowsOnly bit = 0
)
RETURNS nvarchar(MAX)
AS
BEGIN

	DECLARE @table TABLE (nvWord nvarchar(100) NOT NULL)

	INSERT INTO @table
	SELECT value
	FROM STRING_SPLIT(dbo.[fn_RemoveNonAlphanumericChars](@inputString), ' ')  
	WHERE LEN(value) > 0;

	RETURN  RTRIM(STUFF((SELECT dbo.fn_GetAbbreviationByUserID(@iUserID, nvWord, @bFlaggedRowsOnly) + ' ' FROM @table FOR XML PATH, TYPE).value(N'.[1]', N'varchar(MAX)'), 1, 2, ''))

END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RegexSimpleStringMethod]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 25/05/2020
-- Description:	Remove extended ASCII characters from a string
--
-- Parameters:
--
--	@InputString: The string to be parsed
--  @RegExp: The regular expression
--
--	Notes:
--
--	The regular expression is not embeded in the function and, as such, is normalised each time the function is invoked
--	A typical regular expression passed to the function may look something like this '%[^A-Za-z0-9 .,(?)-/'']%'
--
-- =================================================================================================================================

CREATE FUNCTION [dbo].[fn_RegexSimpleStringMethod](@InputString nvarchar(MAX), @RegExp nvarchar(100))
RETURNS nvarchar(MAX)
AS
BEGIN

	WHILE PATINDEX(@RegExp, @InputString) > 0

		SET @InputString = Stuff(@InputString, PatIndex(@RegExp, @InputString), 1, '')
		
	RETURN @InputString

END

GO
/****** Object:  UserDefinedFunction [dbo].[fn_RemoveNonAlphanumericChars]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 25/05/2020
-- Description:	Remove extended ASCII characters from a string
--
-- Parameters:
--
--	@InputString: The string to be parsed
--
--	Notes:
--
--	The regular expression is embeded in the function to facilitate compilation
--
-- =================================================================================================================================

CREATE FUNCTION [dbo].[fn_RemoveNonAlphanumericChars](@InputString nvarchar(MAX))
RETURNS nvarchar(MAX)
AS
BEGIN

	DECLARE @RegExp nvarchar(50) = '%[^A-Za-z0-9 .,(?)-/'']%'

	WHILE PATINDEX(@RegExp, @InputString) > 0

		SET @InputString = STUFF(@InputString, PATINDEX(@RegExp, @InputString), 1, '')

	RETURN @InputString

END

GO
/****** Object:  UserDefinedFunction [dbo].[fn_SetStringAbbreviations]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 28/05/2020
-- Description:	Find and replace abbreviations equivalents within a string
--
-- Parameters:
--
--	@iUserID: The ID of the user making the request
--	@inputString: The word, phrase or document to be parsed
--	@bFlaggedRowsOnly: When false all abbreviations. When true, predefined abbreviations
--
--	Notes:
--
--	The function will always apply the smallest abbreviation available for any give word or phrase
--
-- =================================================================================================================================

CREATE FUNCTION [dbo].[fn_SetStringAbbreviations](
	@iUserID int = 0,
	@inputString nvarchar(MAX) = null,
	@bFlaggedRowsOnly bit = 0
)
RETURNS nvarchar(MAX)
AS
BEGIN

	DECLARE @TRUE bit = 1
	DECLARE @FALSE bit = 0

	DECLARE @Phrase AS CURSOR
	DECLARE @nvWord nvarchar(100) = NULL
	DECLARE @iWordLength int
	DECLARE @nvAbbreviation nvarchar(50) = NULL
	DECLARE @iAbbreviationLength int
	DECLARE @delimeter char(1) = ' '

	IF LEN(TRIM(@inputString)) > 0
	BEGIN
	
		DECLARE @outputString nvarchar(MAX) = @delimeter + TRIM(@inputString) + @delimeter

		DECLARE @table TABLE (
		  nvWord nvarchar(100) NOT NULL,
		  iWordLength int NOT NULL,
		  nvAbbreviation nvarchar(50) NOT NULL,
		  iAbbreviationLength int NOT NULL 
		)

		IF @bFlaggedRowsOnly = @TRUE
		BEGIN

			INSERT INTO @table SELECT DISTINCT a.nvWord, a.iWordLength, a.nvAbbreviation, a.iAbbreviationLength 
			FROM vWordAbbreviationByUserID a  INNER JOIN
			(
				SELECT b.nvWord, MIN(b.iAbbreviationLength) AS iAbbreviationLength 
				FROM vWordAbbreviationByUserID b
				WHERE b.iUserID = 1 AND b.bAlwaysUse = @TRUE AND CHARINDEX(b.nvWord, @inputString) > 0 
				GROUP BY b.nvWord
			) b ON a.nvWord = b.nvWord AND a.iAbbreviationLength = b.iAbbreviationLength
			WHERE a.iUserID = 1 AND a.bAlwaysUse = @TRUE AND CHARINDEX(a.nvWord, @inputString) > 0 

		END ELSE BEGIN

			INSERT INTO @table SELECT DISTINCT a.nvWord, a.iWordLength, a.nvAbbreviation, a.iAbbreviationLength 
			FROM vWordAbbreviationByUserID a  INNER JOIN
			(
				SELECT b.nvWord, MIN(b.iAbbreviationLength) AS iAbbreviationLength 
				FROM vWordAbbreviationByUserID b
				WHERE b.iUserID = 1 AND CHARINDEX(b.nvWord, @inputString) > 0 
				GROUP BY b.nvWord
			) b ON a.nvWord = b.nvWord AND a.iAbbreviationLength = b.iAbbreviationLength
			WHERE a.iUserID = 1 AND CHARINDEX(a.nvWord, @inputString) > 0 
			
		END

		SET @Phrase = CURSOR FOR 
		SELECT @delimeter + nvWord + @delimeter AS nvWord, @delimeter + nvAbbreviation + @delimeter AS nvAbbreviation FROM @table ORDER BY iWordLength DESC, iAbbreviationLength ASC

		OPEN @Phrase
		FETCH NEXT FROM @Phrase INTO @nvWord, @nvAbbreviation
		
		WHILE @@FETCH_STATUS = 0
		BEGIN

			WHILE CHARINDEX(@nvWord, @outputString) > 0
			BEGIN

				SET @outputString = REPLACE(@outputString, @nvWord, @nvAbbreviation)

			END

			FETCH NEXT FROM @Phrase INTO @nvWord, @nvAbbreviation
		END

	END

	RETURN  ISNULL(NULLIF(RTRIM(@outputString),''), @inputString)

END
GO
/****** Object:  Table [dbo].[tNAVUserPreferences]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tNAVUserPreferences](
	[iUserPreferenceID] [int] IDENTITY(1,1) NOT NULL,
	[iUserID] [int] NOT NULL,
	[iClientPreferenceID] [int] NOT NULL,
	[bUserPreference] [bit] NOT NULL,
	[nvUserPreferenceValue] [nvarchar](1000) NULL,
 CONSTRAINT [PK_tNAVUserPreferences] PRIMARY KEY CLUSTERED 
(
	[iUserPreferenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tNAVClientPreferences]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tNAVClientPreferences](
	[iClientPreferenceID] [int] IDENTITY(1,1) NOT NULL,
	[iClientID] [int] NOT NULL,
	[iClientPreferenceTypeID] [int] NOT NULL,
	[nvClientPreferenceName] [nvarchar](250) NOT NULL,
	[nvClientPreferenceDescription] [nvarchar](500) NOT NULL,
	[bClientPreference] [bit] NOT NULL,
	[bClientOverride] [bit] NOT NULL,
	[bSystemOverride] [bit] NOT NULL,
	[bClientValueRequired] [bit] NOT NULL,
	[nvClientPreferenceValue] [nvarchar](1000) NULL,
	[iClientOrderPreference] [int] NOT NULL,
	[bDisablePreference] [bit] NOT NULL,
 CONSTRAINT [PK_tNAVClientPreferences] PRIMARY KEY CLUSTERED 
(
	[iClientPreferenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tNAVClientPreferenceType]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tNAVClientPreferenceType](
	[iClientPreferenceTypeID] [int] IDENTITY(1,1) NOT NULL,
	[nvClientPreferenceType] [nvarchar](250) NOT NULL,
	[iOrderByPreference] [int] NOT NULL,
	[bDisablePreferenceType] [bit] NOT NULL,
 CONSTRAINT [PK_tNAVClientPreferenceType] PRIMARY KEY CLUSTERED 
(
	[iClientPreferenceTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vUserPreferencesByUserID]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vUserPreferencesByUserID]
AS
SELECT        dbo.tNAVClientPreferences.iClientID, dbo.tNAVClientPreferences.iClientPreferenceID, dbo.tNAVUserPreferences.iUserID, dbo.tNAVUserPreferences.iUserPreferenceID, dbo.tNAVClientPreferenceType.iOrderByPreference, 
                         dbo.tNAVClientPreferences.iClientOrderPreference, dbo.tNAVClientPreferences.iClientPreferenceTypeID, dbo.tNAVClientPreferences.bDisablePreference, dbo.tNAVClientPreferenceType.bDisablePreferenceType, 
                         dbo.tNAVClientPreferenceType.nvClientPreferenceType, dbo.tNAVClientPreferences.nvClientPreferenceName, dbo.tNAVClientPreferences.nvClientPreferenceDescription, dbo.tNAVClientPreferences.bClientPreference, 
                         dbo.tNAVUserPreferences.bUserPreference, dbo.tNAVClientPreferences.bClientOverride, dbo.tNAVClientPreferences.bSystemOverride, dbo.tNAVClientPreferences.bClientValueRequired, 
                         dbo.tNAVClientPreferences.nvClientPreferenceValue, dbo.tNAVUserPreferences.nvUserPreferenceValue
FROM            dbo.tNAVClientPreferences INNER JOIN
                         dbo.tNAVClientPreferenceType ON dbo.tNAVClientPreferences.iClientPreferenceTypeID = dbo.tNAVClientPreferenceType.iClientPreferenceTypeID INNER JOIN
                         dbo.tNAVUserPreferences ON dbo.tNAVClientPreferences.iClientPreferenceID = dbo.tNAVUserPreferences.iClientPreferenceID
GO
/****** Object:  Table [dbo].[tCountryLanguageJoin]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tCountryLanguageJoin](
	[iCountryID] [int] NOT NULL,
	[iLanguageID] [int] NOT NULL,
 CONSTRAINT [PK_tCountryLanguageJoin] PRIMARY KEY CLUSTERED 
(
	[iCountryID] ASC,
	[iLanguageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tCountry]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tCountry](
	[iCountryID] [int] IDENTITY(1,1) NOT NULL,
	[nvCountryName] [nvarchar](100) NOT NULL,
	[nAlphaCode2] [nchar](2) NOT NULL,
	[nAlphaCode3] [nchar](3) NOT NULL,
	[nNumeric] [nchar](3) NOT NULL,
 CONSTRAINT [PK_tCountry] PRIMARY KEY CLUSTERED 
(
	[iCountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tLanguage]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tLanguage](
	[iLanguageID] [int] IDENTITY(1,1) NOT NULL,
	[nvLanguage] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tLanguage] PRIMARY KEY CLUSTERED 
(
	[iLanguageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vLanguageByCountry]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vLanguageByCountry]
AS
SELECT        TOP (100) PERCENT dbo.tCountryLanguageJoin.iCountryID, dbo.tCountryLanguageJoin.iLanguageID, dbo.tLanguage.nvLanguage, dbo.tCountry.nvCountryName, dbo.tCountry.nAlphaCode2, dbo.tCountry.nAlphaCode3, 
                         dbo.tCountry.nNumeric
FROM            dbo.tCountry INNER JOIN
                         dbo.tCountryLanguageJoin ON dbo.tCountry.iCountryID = dbo.tCountryLanguageJoin.iCountryID INNER JOIN
                         dbo.tLanguage ON dbo.tCountryLanguageJoin.iLanguageID = dbo.tLanguage.iLanguageID
GO
/****** Object:  Table [dbo].[tAbbreviationType]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tAbbreviationType](
	[iAbbreviationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[nvAbbreviationType] [nvarchar](100) NOT NULL,
	[nvAbbreviationTypeDescription] [nvarchar](1000) NULL,
 CONSTRAINT [PK_tAbbreviationType] PRIMARY KEY CLUSTERED 
(
	[iAbbreviationTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tLanguageAbbreviationTypeJoin]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tLanguageAbbreviationTypeJoin](
	[iLanguageID] [int] NOT NULL,
	[iAbbreviationTypeID] [int] NOT NULL,
 CONSTRAINT [PK_tLanguageAbbreviationTypeJoin] PRIMARY KEY CLUSTERED 
(
	[iLanguageID] ASC,
	[iAbbreviationTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tAbbreviation]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tAbbreviation](
	[iAbbreviationID] [int] IDENTITY(1,1) NOT NULL,
	[iAbbreviationTypeID] [int] NOT NULL,
	[nvAbbreviation] [nvarchar](50) NOT NULL,
	[nvAbbreviationDescription] [nvarchar](1000) NULL,
	[bAlwaysUse] [bit] NOT NULL,
	[iLength]  AS (len([nvAbbreviation])) PERSISTED,
 CONSTRAINT [PK_tAbbreviation] PRIMARY KEY CLUSTERED 
(
	[iAbbreviationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vAbbreviationByLanguage]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vAbbreviationByLanguage]
AS
SELECT        dbo.tLanguageAbbreviationTypeJoin.iLanguageID, dbo.tAbbreviation.iAbbreviationID, dbo.tLanguageAbbreviationTypeJoin.iAbbreviationTypeID, dbo.tAbbreviation.nvAbbreviation, dbo.tAbbreviation.nvAbbreviationDescription, 
                         dbo.tAbbreviation.bAlwaysUse, dbo.tAbbreviationType.nvAbbreviationType, dbo.tAbbreviationType.nvAbbreviationTypeDescription
FROM            dbo.tAbbreviation INNER JOIN
                         dbo.tAbbreviationType ON dbo.tAbbreviation.iAbbreviationTypeID = dbo.tAbbreviationType.iAbbreviationTypeID INNER JOIN
                         dbo.tLanguageAbbreviationTypeJoin ON dbo.tAbbreviationType.iAbbreviationTypeID = dbo.tLanguageAbbreviationTypeJoin.iAbbreviationTypeID
GO
/****** Object:  Table [dbo].[tLanguageWordJoin]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tLanguageWordJoin](
	[iLanguageID] [int] NOT NULL,
	[iWordID] [int] NOT NULL,
 CONSTRAINT [PK_tLanguageWordJoin] PRIMARY KEY CLUSTERED 
(
	[iLanguageID] ASC,
	[iWordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tWord]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tWord](
	[iWordID] [int] IDENTITY(1,1) NOT NULL,
	[iWordTypeID] [int] NOT NULL,
	[nvWord] [nvarchar](100) NOT NULL,
	[iLength]  AS (len([nvWord])) PERSISTED,
 CONSTRAINT [PK_tWord] PRIMARY KEY CLUSTERED 
(
	[iWordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tWordAbbreviationJoin]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tWordAbbreviationJoin](
	[iWordID] [int] NOT NULL,
	[iAbbreviationID] [int] NOT NULL,
 CONSTRAINT [PK_tWordAbbreviationJoin] PRIMARY KEY CLUSTERED 
(
	[iWordID] ASC,
	[iAbbreviationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vWordByLanguage]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vWordByLanguage]
AS
SELECT        dbo.tCountryLanguageJoin.iCountryID, dbo.tLanguageWordJoin.iLanguageID, dbo.tWordAbbreviationJoin.iWordID, dbo.tWordAbbreviationJoin.iAbbreviationID, dbo.tWord.iWordTypeID, dbo.tAbbreviation.iAbbreviationTypeID, 
                         dbo.tAbbreviation.nvAbbreviation, dbo.tWord.nvWord, dbo.tCountry.nvCountryName, dbo.tCountry.nAlphaCode2, dbo.tCountry.nAlphaCode3, dbo.tCountry.nNumeric, dbo.tLanguage.nvLanguage
FROM            dbo.tLanguageWordJoin INNER JOIN
                         dbo.tWord ON dbo.tLanguageWordJoin.iWordID = dbo.tWord.iWordID INNER JOIN
                         dbo.tLanguage ON dbo.tLanguageWordJoin.iLanguageID = dbo.tLanguage.iLanguageID INNER JOIN
                         dbo.tCountryLanguageJoin ON dbo.tLanguage.iLanguageID = dbo.tCountryLanguageJoin.iLanguageID INNER JOIN
                         dbo.tCountry ON dbo.tCountryLanguageJoin.iCountryID = dbo.tCountry.iCountryID INNER JOIN
                         dbo.tWordAbbreviationJoin ON dbo.tWord.iWordID = dbo.tWordAbbreviationJoin.iWordID INNER JOIN
                         dbo.tAbbreviation ON dbo.tWordAbbreviationJoin.iAbbreviationID = dbo.tAbbreviation.iAbbreviationID
GO
/****** Object:  View [dbo].[vWordAbbreviationsByCountry]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vWordAbbreviationsByCountry]
AS
SELECT        TOP (100) PERCENT dbo.tCountryLanguageJoin.iCountryID, dbo.tLanguageWordJoin.iLanguageID, dbo.tLanguageWordJoin.iWordID, dbo.tWordAbbreviationJoin.iAbbreviationID, dbo.tAbbreviation.iAbbreviationTypeID, 
                         dbo.tWord.iWordTypeID, dbo.tCountry.nAlphaCode2, dbo.tCountry.nAlphaCode3, dbo.tCountry.nNumeric, dbo.tLanguage.nvLanguage, dbo.tWord.nvWord, dbo.tAbbreviation.nvAbbreviation, dbo.tAbbreviation.bAlwaysUse, 
                         dbo.tAbbreviation.nvAbbreviationDescription
FROM            dbo.tWordAbbreviationJoin INNER JOIN
                         dbo.tAbbreviation ON dbo.tWordAbbreviationJoin.iAbbreviationID = dbo.tAbbreviation.iAbbreviationID INNER JOIN
                         dbo.tWord ON dbo.tWordAbbreviationJoin.iWordID = dbo.tWord.iWordID INNER JOIN
                         dbo.tLanguageWordJoin ON dbo.tWord.iWordID = dbo.tLanguageWordJoin.iWordID INNER JOIN
                         dbo.tCountryLanguageJoin INNER JOIN
                         dbo.tCountry ON dbo.tCountryLanguageJoin.iCountryID = dbo.tCountry.iCountryID INNER JOIN
                         dbo.tLanguage ON dbo.tCountryLanguageJoin.iLanguageID = dbo.tLanguage.iLanguageID ON dbo.tLanguageWordJoin.iLanguageID = dbo.tLanguage.iLanguageID
GO
/****** Object:  Table [dbo].[tNAVUserProperties]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tNAVUserProperties](
	[iUserID] [int] IDENTITY(1,1) NOT NULL,
	[iClientID] [int] NOT NULL,
	[iLanguageID] [int] NOT NULL,
	[nvEmailAddress] [nvarchar](256) NULL,
	[bEmailConfirmed] [bit] NOT NULL,
	[nvPasswordHash] [nvarchar](max) NULL,
	[nvSecurityStamp] [nvarchar](max) NULL,
	[bTwoFactorEnabled] [bit] NOT NULL,
	[dLockoutEndDateUTC] [datetime] NULL,
	[bLockoutEnabled] [bit] NOT NULL,
	[iAccessFailedCount] [int] NOT NULL,
	[nvPreferredName] [nvarchar](256) NOT NULL,
	[nvUserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_tNAVUserProperties_1] PRIMARY KEY CLUSTERED 
(
	[iUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tNAVClient]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tNAVClient](
	[iClientID] [int] IDENTITY(1,1) NOT NULL,
	[nvClientName] [nvarchar](250) NOT NULL,
	[bRegistered] [bit] NOT NULL,
	[nvClientDescription] [nvarchar](1000) NULL,
 CONSTRAINT [PK_tNAVClient] PRIMARY KEY CLUSTERED 
(
	[iClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vLanguageByUser]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vLanguageByUser]
AS
SELECT        dbo.tCountryLanguageJoin.iCountryID, dbo.tCountryLanguageJoin.iLanguageID, dbo.tNAVUserProperties.iUserID, dbo.tNAVClient.iClientID, dbo.tCountry.nvCountryName, dbo.tCountry.nAlphaCode2, dbo.tCountry.nAlphaCode3, 
                         dbo.tCountry.nNumeric, dbo.tLanguage.nvLanguage, dbo.tNAVClient.nvClientName, dbo.tNAVClient.bRegistered, dbo.tNAVUserProperties.nvEmailAddress, dbo.tNAVUserProperties.bEmailConfirmed, 
                         dbo.tNAVUserProperties.dLockoutEndDateUTC, dbo.tNAVUserProperties.bLockoutEnabled, dbo.tNAVUserProperties.iAccessFailedCount, dbo.tNAVUserProperties.nvPreferredName, dbo.tNAVUserProperties.nvUserName
FROM            dbo.tCountry INNER JOIN
                         dbo.tCountryLanguageJoin ON dbo.tCountry.iCountryID = dbo.tCountryLanguageJoin.iCountryID INNER JOIN
                         dbo.tLanguage ON dbo.tCountryLanguageJoin.iLanguageID = dbo.tLanguage.iLanguageID INNER JOIN
                         dbo.tNAVUserProperties ON dbo.tLanguage.iLanguageID = dbo.tNAVUserProperties.iLanguageID INNER JOIN
                         dbo.tNAVClient ON dbo.tNAVUserProperties.iClientID = dbo.tNAVClient.iClientID
GO
/****** Object:  View [dbo].[vWordAbbreviationByUserID]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vWordAbbreviationByUserID]
AS
SELECT        dbo.tNAVUserProperties.iUserID, dbo.tWordAbbreviationJoin.iWordID, dbo.tWordAbbreviationJoin.iAbbreviationID, dbo.tWord.nvWord, dbo.tAbbreviation.nvAbbreviation, dbo.tWord.iLength AS iWordLength, 
                         dbo.tAbbreviation.iLength AS iAbbreviationLength, dbo.tAbbreviation.bAlwaysUse
FROM            dbo.tAbbreviation INNER JOIN
                         dbo.tLanguageAbbreviationTypeJoin ON dbo.tAbbreviation.iAbbreviationTypeID = dbo.tLanguageAbbreviationTypeJoin.iAbbreviationTypeID INNER JOIN
                         dbo.tNAVUserProperties ON dbo.tLanguageAbbreviationTypeJoin.iLanguageID = dbo.tNAVUserProperties.iLanguageID INNER JOIN
                         dbo.tWordAbbreviationJoin ON dbo.tAbbreviation.iAbbreviationID = dbo.tWordAbbreviationJoin.iAbbreviationID INNER JOIN
                         dbo.tWord ON dbo.tWordAbbreviationJoin.iWordID = dbo.tWord.iWordID
GO
/****** Object:  View [dbo].[vDistinctAbbreviationTypesByLanguage]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vDistinctAbbreviationTypesByLanguage]
AS
SELECT        dbo.tLanguageAbbreviationTypeJoin.iLanguageID, dbo.tLanguageAbbreviationTypeJoin.iAbbreviationTypeID, dbo.tAbbreviationType.nvAbbreviationType, dbo.tAbbreviationType.nvAbbreviationTypeDescription
FROM            dbo.tAbbreviationType INNER JOIN
                         dbo.tLanguageAbbreviationTypeJoin ON dbo.tAbbreviationType.iAbbreviationTypeID = dbo.tLanguageAbbreviationTypeJoin.iAbbreviationTypeID
GO
/****** Object:  View [dbo].[vAbbreviationFlagged]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vAbbreviationFlagged]
AS
SELECT        dbo.tNAVUserProperties.iUserID, dbo.tAbbreviation.iAbbreviationID, dbo.tAbbreviation.nvAbbreviation
FROM            dbo.tAbbreviation INNER JOIN
                         dbo.tLanguageAbbreviationTypeJoin ON dbo.tAbbreviation.iAbbreviationTypeID = dbo.tLanguageAbbreviationTypeJoin.iAbbreviationTypeID INNER JOIN
                         dbo.tNAVUserProperties ON dbo.tLanguageAbbreviationTypeJoin.iLanguageID = dbo.tNAVUserProperties.iLanguageID
WHERE        (dbo.tAbbreviation.bAlwaysUse = 1)
GO
/****** Object:  View [dbo].[vAbbreviationByType]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vAbbreviationByType]
AS
SELECT        dbo.tLanguageAbbreviationTypeJoin.iLanguageID, dbo.tAbbreviation.iAbbreviationID, dbo.tLanguageAbbreviationTypeJoin.iAbbreviationTypeID, dbo.tAbbreviation.nvAbbreviation, dbo.tAbbreviation.nvAbbreviationDescription, 
                         dbo.tAbbreviation.bAlwaysUse, dbo.tAbbreviationType.nvAbbreviationType, dbo.tAbbreviationType.nvAbbreviationTypeDescription
FROM            dbo.tAbbreviation INNER JOIN
                         dbo.tAbbreviationType ON dbo.tAbbreviation.iAbbreviationTypeID = dbo.tAbbreviationType.iAbbreviationTypeID INNER JOIN
                         dbo.tLanguageAbbreviationTypeJoin ON dbo.tAbbreviationType.iAbbreviationTypeID = dbo.tLanguageAbbreviationTypeJoin.iAbbreviationTypeID
GO
/****** Object:  Table [dbo].[tLanguageWordTypeJoin]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tLanguageWordTypeJoin](
	[iLanguageID] [int] NOT NULL,
	[iWordTypeID] [int] NOT NULL,
 CONSTRAINT [PK_tLanguageWordTypeJoin] PRIMARY KEY CLUSTERED 
(
	[iLanguageID] ASC,
	[iWordTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tNAVLog]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tNAVLog](
	[iLogID] [int] IDENTITY(1,1) NOT NULL,
	[iUserID] [int] NOT NULL,
	[dDateTime] [datetime] NOT NULL,
	[nvThread] [nvarchar](255) NOT NULL,
	[nvLevel] [nvarchar](50) NOT NULL,
	[nvSource] [nvarchar](255) NOT NULL,
	[nvMessage] [nvarchar](4000) NOT NULL,
	[nvException] [nvarchar](4000) NOT NULL,
	[nvBuildVersion] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_tNAVLog_1] PRIMARY KEY CLUSTERED 
(
	[iLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tNAVSysAdmin]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tNAVSysAdmin](
	[iSysAdminID] [int] IDENTITY(1,1) NOT NULL,
	[nvSysAdminName] [nvarchar](250) NOT NULL,
	[nvSysAdminDescription] [nvarchar](1000) NULL,
 CONSTRAINT [PK_tNAVSysAdmin] PRIMARY KEY CLUSTERED 
(
	[iSysAdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tNAVSysAdminClientJoin]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tNAVSysAdminClientJoin](
	[iSysAdminID] [int] NOT NULL,
	[iClientID] [int] NOT NULL,
 CONSTRAINT [PK_tNAVSysAdminClientJoin] PRIMARY KEY CLUSTERED 
(
	[iSysAdminID] ASC,
	[iClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tNAVUserStateLogEntry]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tNAVUserStateLogEntry](
	[iUserStateLogEntryID] [int] IDENTITY(1,1) NOT NULL,
	[iUserID] [int] NOT NULL,
	[nvLastFilenameOpened] [nvarchar](250) NULL,
	[nvLastTabFocus] [nvarchar](25) NULL,
	[nvLastTableFocus] [nvarchar](250) NULL,
	[dDateTimeUTC] [datetime] NOT NULL,
	[nvBuildVersion] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_tNAVUserStateLogEntry] PRIMARY KEY CLUSTERED 
(
	[iUserStateLogEntryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tWordType]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tWordType](
	[iWordTypeID] [int] IDENTITY(1,1) NOT NULL,
	[nvWordType] [nvarchar](100) NOT NULL,
	[nvWordTypeDescription] [nvarchar](1000) NULL,
 CONSTRAINT [PK_tWordType] PRIMARY KEY CLUSTERED 
(
	[iWordTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_tAbbreviation]    Script Date: 21/08/2020 5:20:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_tAbbreviation] ON [dbo].[tAbbreviation]
(
	[iAbbreviationID] ASC,
	[iAbbreviationTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tLanguage]    Script Date: 21/08/2020 5:20:55 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tLanguage] ON [dbo].[tLanguage]
(
	[nvLanguage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tNAVLog]    Script Date: 21/08/2020 5:20:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_tNAVLog] ON [dbo].[tNAVLog]
(
	[iUserID] ASC,
	[nvLevel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tWord]    Script Date: 21/08/2020 5:20:55 AM ******/
CREATE NONCLUSTERED INDEX [IX_tWord] ON [dbo].[tWord]
(
	[nvWord] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tAbbreviation] ADD  CONSTRAINT [DF_tAbbreviation_bAlwaysUse]  DEFAULT ((0)) FOR [bAlwaysUse]
GO
ALTER TABLE [dbo].[tNAVClient] ADD  CONSTRAINT [DF_tNAVClient_bRegistered]  DEFAULT ((0)) FOR [bRegistered]
GO
ALTER TABLE [dbo].[tNAVClientPreferences] ADD  CONSTRAINT [DF_tNAVClientPreferences_bEnablePreference_1]  DEFAULT ((0)) FOR [bClientPreference]
GO
ALTER TABLE [dbo].[tNAVClientPreferences] ADD  CONSTRAINT [DF_tNAVClientPreferences_bEnablePreference]  DEFAULT ((0)) FOR [bClientOverride]
GO
ALTER TABLE [dbo].[tNAVClientPreferences] ADD  CONSTRAINT [DF_tNAVClientPreferences_bSystemOverride]  DEFAULT ((0)) FOR [bSystemOverride]
GO
ALTER TABLE [dbo].[tNAVClientPreferences] ADD  CONSTRAINT [DF_tNAVClientPreferences_bClientValueRequired]  DEFAULT ((0)) FOR [bClientValueRequired]
GO
ALTER TABLE [dbo].[tNAVClientPreferences] ADD  CONSTRAINT [DF_tNAVClientPreferences_iClientOrderPreference]  DEFAULT ((0)) FOR [iClientOrderPreference]
GO
ALTER TABLE [dbo].[tNAVClientPreferences] ADD  CONSTRAINT [DF_tNAVClientPreferences_bDisablePreference]  DEFAULT ((0)) FOR [bDisablePreference]
GO
ALTER TABLE [dbo].[tNAVClientPreferenceType] ADD  CONSTRAINT [DF_tNAVClientPreferenceType_iOrderByPreference]  DEFAULT ((0)) FOR [iOrderByPreference]
GO
ALTER TABLE [dbo].[tNAVClientPreferenceType] ADD  CONSTRAINT [DF_tNAVClientPreferenceType_bDisablePreferenceType_1]  DEFAULT ((0)) FOR [bDisablePreferenceType]
GO
ALTER TABLE [dbo].[tNAVLog] ADD  CONSTRAINT [DF_tNAVLog_nvBuildVersion]  DEFAULT (N'Unknown') FOR [nvBuildVersion]
GO
ALTER TABLE [dbo].[tNAVUserPreferences] ADD  CONSTRAINT [DF_tNAVUserPreferences_iEnableSomething]  DEFAULT ((0)) FOR [bUserPreference]
GO
ALTER TABLE [dbo].[tNAVUserProperties] ADD  CONSTRAINT [DF_tNAVUserProperties_iLanguageID]  DEFAULT ((1)) FOR [iLanguageID]
GO
ALTER TABLE [dbo].[tNAVUserProperties] ADD  CONSTRAINT [DF_tNAVUserProperties_bEmailConfirmed]  DEFAULT ((0)) FOR [bEmailConfirmed]
GO
ALTER TABLE [dbo].[tNAVUserProperties] ADD  CONSTRAINT [DF_tNAVUserProperties_bTwoFactorEnabled]  DEFAULT ((0)) FOR [bTwoFactorEnabled]
GO
ALTER TABLE [dbo].[tNAVUserProperties] ADD  CONSTRAINT [DF_tNAVUserProperties_dLockoutEndDateUTC]  DEFAULT (dateadd(year,(25),getutcdate())) FOR [dLockoutEndDateUTC]
GO
ALTER TABLE [dbo].[tNAVUserProperties] ADD  CONSTRAINT [DF_tNAVUserProperties_bLockoutEnabled]  DEFAULT ((0)) FOR [bLockoutEnabled]
GO
ALTER TABLE [dbo].[tNAVUserProperties] ADD  CONSTRAINT [DF_tNAVUserProperties_iAccessFailedCount]  DEFAULT ((0)) FOR [iAccessFailedCount]
GO
ALTER TABLE [dbo].[tNAVUserStateLogEntry] ADD  CONSTRAINT [DF_tNAVUserStateLogEntry_dDateTimeUTC]  DEFAULT (getutcdate()) FOR [dDateTimeUTC]
GO
ALTER TABLE [dbo].[tNAVUserStateLogEntry] ADD  CONSTRAINT [DF_tNAVUserStateLogEntry_nvBuildVersion]  DEFAULT (N'Unknow') FOR [nvBuildVersion]
GO
ALTER TABLE [dbo].[tAbbreviation]  WITH CHECK ADD  CONSTRAINT [FK_tAbbreviation_tAbbreviationType] FOREIGN KEY([iAbbreviationTypeID])
REFERENCES [dbo].[tAbbreviationType] ([iAbbreviationTypeID])
GO
ALTER TABLE [dbo].[tAbbreviation] CHECK CONSTRAINT [FK_tAbbreviation_tAbbreviationType]
GO
ALTER TABLE [dbo].[tCountryLanguageJoin]  WITH CHECK ADD  CONSTRAINT [FK_tCountryLanguageJoin_tCountry] FOREIGN KEY([iCountryID])
REFERENCES [dbo].[tCountry] ([iCountryID])
GO
ALTER TABLE [dbo].[tCountryLanguageJoin] CHECK CONSTRAINT [FK_tCountryLanguageJoin_tCountry]
GO
ALTER TABLE [dbo].[tCountryLanguageJoin]  WITH CHECK ADD  CONSTRAINT [FK_tCountryLanguageJoin_tLanguage] FOREIGN KEY([iLanguageID])
REFERENCES [dbo].[tLanguage] ([iLanguageID])
GO
ALTER TABLE [dbo].[tCountryLanguageJoin] CHECK CONSTRAINT [FK_tCountryLanguageJoin_tLanguage]
GO
ALTER TABLE [dbo].[tLanguageAbbreviationTypeJoin]  WITH CHECK ADD  CONSTRAINT [FK_tLanguageAbbreviationTypeJoin_tAbbreviationType] FOREIGN KEY([iAbbreviationTypeID])
REFERENCES [dbo].[tAbbreviationType] ([iAbbreviationTypeID])
GO
ALTER TABLE [dbo].[tLanguageAbbreviationTypeJoin] CHECK CONSTRAINT [FK_tLanguageAbbreviationTypeJoin_tAbbreviationType]
GO
ALTER TABLE [dbo].[tLanguageAbbreviationTypeJoin]  WITH CHECK ADD  CONSTRAINT [FK_tLanguageAbbreviationTypeJoin_tLanguage] FOREIGN KEY([iLanguageID])
REFERENCES [dbo].[tLanguage] ([iLanguageID])
GO
ALTER TABLE [dbo].[tLanguageAbbreviationTypeJoin] CHECK CONSTRAINT [FK_tLanguageAbbreviationTypeJoin_tLanguage]
GO
ALTER TABLE [dbo].[tLanguageWordJoin]  WITH CHECK ADD  CONSTRAINT [FK_tLanguageWordJoin_tLanguage] FOREIGN KEY([iLanguageID])
REFERENCES [dbo].[tLanguage] ([iLanguageID])
GO
ALTER TABLE [dbo].[tLanguageWordJoin] CHECK CONSTRAINT [FK_tLanguageWordJoin_tLanguage]
GO
ALTER TABLE [dbo].[tLanguageWordJoin]  WITH CHECK ADD  CONSTRAINT [FK_tLanguageWordJoin_tWord] FOREIGN KEY([iWordID])
REFERENCES [dbo].[tWord] ([iWordID])
GO
ALTER TABLE [dbo].[tLanguageWordJoin] CHECK CONSTRAINT [FK_tLanguageWordJoin_tWord]
GO
ALTER TABLE [dbo].[tLanguageWordTypeJoin]  WITH CHECK ADD  CONSTRAINT [FK_tLanguageWordTypeJoin_tLanguage] FOREIGN KEY([iLanguageID])
REFERENCES [dbo].[tLanguage] ([iLanguageID])
GO
ALTER TABLE [dbo].[tLanguageWordTypeJoin] CHECK CONSTRAINT [FK_tLanguageWordTypeJoin_tLanguage]
GO
ALTER TABLE [dbo].[tLanguageWordTypeJoin]  WITH CHECK ADD  CONSTRAINT [FK_tLanguageWordTypeJoin_tWordType] FOREIGN KEY([iWordTypeID])
REFERENCES [dbo].[tWordType] ([iWordTypeID])
GO
ALTER TABLE [dbo].[tLanguageWordTypeJoin] CHECK CONSTRAINT [FK_tLanguageWordTypeJoin_tWordType]
GO
ALTER TABLE [dbo].[tNAVClientPreferences]  WITH CHECK ADD  CONSTRAINT [FK_tNAVClientPreferences_tNAVClient] FOREIGN KEY([iClientID])
REFERENCES [dbo].[tNAVClient] ([iClientID])
GO
ALTER TABLE [dbo].[tNAVClientPreferences] CHECK CONSTRAINT [FK_tNAVClientPreferences_tNAVClient]
GO
ALTER TABLE [dbo].[tNAVClientPreferences]  WITH CHECK ADD  CONSTRAINT [FK_tNAVClientPreferences_tNAVClientPreferenceType] FOREIGN KEY([iClientPreferenceTypeID])
REFERENCES [dbo].[tNAVClientPreferenceType] ([iClientPreferenceTypeID])
GO
ALTER TABLE [dbo].[tNAVClientPreferences] CHECK CONSTRAINT [FK_tNAVClientPreferences_tNAVClientPreferenceType]
GO
ALTER TABLE [dbo].[tNAVLog]  WITH CHECK ADD  CONSTRAINT [FK_tNAVLog_tNAVUserProperties] FOREIGN KEY([iUserID])
REFERENCES [dbo].[tNAVUserProperties] ([iUserID])
GO
ALTER TABLE [dbo].[tNAVLog] CHECK CONSTRAINT [FK_tNAVLog_tNAVUserProperties]
GO
ALTER TABLE [dbo].[tNAVSysAdminClientJoin]  WITH CHECK ADD  CONSTRAINT [FK_tNAVSysAdminClientJoin_tNAVClient] FOREIGN KEY([iClientID])
REFERENCES [dbo].[tNAVClient] ([iClientID])
GO
ALTER TABLE [dbo].[tNAVSysAdminClientJoin] CHECK CONSTRAINT [FK_tNAVSysAdminClientJoin_tNAVClient]
GO
ALTER TABLE [dbo].[tNAVSysAdminClientJoin]  WITH CHECK ADD  CONSTRAINT [FK_tNAVSysAdminClientJoin_tNAVSysAdmin] FOREIGN KEY([iSysAdminID])
REFERENCES [dbo].[tNAVSysAdmin] ([iSysAdminID])
GO
ALTER TABLE [dbo].[tNAVSysAdminClientJoin] CHECK CONSTRAINT [FK_tNAVSysAdminClientJoin_tNAVSysAdmin]
GO
ALTER TABLE [dbo].[tNAVUserPreferences]  WITH CHECK ADD  CONSTRAINT [FK_tNAVUserPreferences_tNAVClientPreferences] FOREIGN KEY([iClientPreferenceID])
REFERENCES [dbo].[tNAVClientPreferences] ([iClientPreferenceID])
GO
ALTER TABLE [dbo].[tNAVUserPreferences] CHECK CONSTRAINT [FK_tNAVUserPreferences_tNAVClientPreferences]
GO
ALTER TABLE [dbo].[tNAVUserPreferences]  WITH CHECK ADD  CONSTRAINT [FK_tNAVUserPreferences_tNAVUserProperties] FOREIGN KEY([iUserID])
REFERENCES [dbo].[tNAVUserProperties] ([iUserID])
GO
ALTER TABLE [dbo].[tNAVUserPreferences] CHECK CONSTRAINT [FK_tNAVUserPreferences_tNAVUserProperties]
GO
ALTER TABLE [dbo].[tNAVUserProperties]  WITH CHECK ADD  CONSTRAINT [FK_tNAVUserProperties_tLanguage] FOREIGN KEY([iLanguageID])
REFERENCES [dbo].[tLanguage] ([iLanguageID])
GO
ALTER TABLE [dbo].[tNAVUserProperties] CHECK CONSTRAINT [FK_tNAVUserProperties_tLanguage]
GO
ALTER TABLE [dbo].[tNAVUserProperties]  WITH CHECK ADD  CONSTRAINT [FK_tNAVUserProperties_tNAVClient] FOREIGN KEY([iClientID])
REFERENCES [dbo].[tNAVClient] ([iClientID])
GO
ALTER TABLE [dbo].[tNAVUserProperties] CHECK CONSTRAINT [FK_tNAVUserProperties_tNAVClient]
GO
ALTER TABLE [dbo].[tNAVUserStateLogEntry]  WITH CHECK ADD  CONSTRAINT [FK_tNAVUserStateLogEntry_tNAVUserProperties] FOREIGN KEY([iUserID])
REFERENCES [dbo].[tNAVUserProperties] ([iUserID])
GO
ALTER TABLE [dbo].[tNAVUserStateLogEntry] CHECK CONSTRAINT [FK_tNAVUserStateLogEntry_tNAVUserProperties]
GO
ALTER TABLE [dbo].[tWord]  WITH CHECK ADD  CONSTRAINT [FK_tWord_tWordType] FOREIGN KEY([iWordTypeID])
REFERENCES [dbo].[tWordType] ([iWordTypeID])
GO
ALTER TABLE [dbo].[tWord] CHECK CONSTRAINT [FK_tWord_tWordType]
GO
ALTER TABLE [dbo].[tWordAbbreviationJoin]  WITH CHECK ADD  CONSTRAINT [FK_tWordAbbreviationJoin_tAbbreviation] FOREIGN KEY([iAbbreviationID])
REFERENCES [dbo].[tAbbreviation] ([iAbbreviationID])
GO
ALTER TABLE [dbo].[tWordAbbreviationJoin] CHECK CONSTRAINT [FK_tWordAbbreviationJoin_tAbbreviation]
GO
ALTER TABLE [dbo].[tWordAbbreviationJoin]  WITH CHECK ADD  CONSTRAINT [FK_tWordAbbreviationJoin_tWord] FOREIGN KEY([iWordID])
REFERENCES [dbo].[tWord] ([iWordID])
GO
ALTER TABLE [dbo].[tWordAbbreviationJoin] CHECK CONSTRAINT [FK_tWordAbbreviationJoin_tWord]
GO
/****** Object:  StoredProcedure [dbo].[pCreateLanguageFromTemplate]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 21/02/2020
-- Description:	Create a new language from an existing language template
--
-- Parameters:
--
--	@iChosenCountryID:				The country chosen by the user from the system derived country code list
--
--	@iExistingLanguageTemplateID:	The template language chosen by the user from all available languages
--
--										Note:
--										A language already belonging to a country other than the chosen country may 
--										be more important to the user as a language template
--
--	@nvNewLanguageName:				The name given by the user to represent the new language to be associated with the chosen country
--
--
--  The word database will be replicated with an existing language as a template, permitting a user control to add, update or delete 
--  without affecting other languages
--	
--	WARNING! All users of the new language have the potential, given access, to make changes to the new language. A potential work 
--  around is to hold changes for the specific user in an XML file on their local installation, pulling local changes over the top of 
--  the system language nodes at run time
--
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[pCreateLanguageFromTemplate] 
	@iChosenCountryID int = 0, 
	@iExistingLanguageTemplateID int = 0,
	@nvNewLanguageName nvarchar(100) = ''
AS
BEGIN
	-- SET NOCOUNT ON to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Success int = 0
	DECLARE @LanguageNameAlreadyExists int = 50095

    -- throw an error and stop if the new language name already belongs to a language in use by the chosen country

    IF EXISTS (SELECT nvLanguage FROM [vLanguageByCountry] WHERE iCountryID = @iChosenCountryID AND nvLanguage = @nvNewLanguageName) RETURN @LanguageNameAlreadyExists

    -- create a new language name and capture the new language ID

	DECLARE @tCapturedOutput table (iOutputRowID int)

	INSERT INTO tLanguage (nvLanguage) 
		OUTPUT INSERTED.iLanguageID INTO @tCapturedOutput
	VALUES (@nvNewLanguageName)

	DECLARE @iNewLanguageID int = (SELECT iOutputRowID FROM @tCapturedOutput)
	DELETE FROM @tCapturedOutput

	-- join the language with the user country id

	INSERT INTO tCountryLanguageJoin (iCountryID, iLanguageID)
	  VALUES (@iChosenCountryID, @iNewLanguageID)

-- =================================================================================================================================
--
-- The following code is largely self explanitory.  Unfortunately much of the work is not set based as we have had to resort to a number
-- of WHILE loops. This is principly because we use cartesian joins (ie supporting a one, none, many, or many to many relationship 
-- programmatically) to hold the Primary keys of the related tables.  
--
-- The tables themselves hold no guaranteed unique data relating to each other so the join becomes their respective primary keys in their 
-- shared cartesian tables.  Therefore we are forced to capture the new Primary Key ID's using the OUTPUT INSERTED clause to populate the 
-- cartesian joins at the point their respective table rows are populated.  Its reasonably fast, given the while loops overhead. The 
-- alternative is to either use a one to one allowing NULL's, or a one to many but we would then lose the many to many.
--
-- =================================================================================================================================

	DECLARE @iWordID int
	DECLARE @iWordTypeID int
	DECLARE @nvWord nvarchar(100)
	DECLARE @iAbbreviationID int
	DECLARE @iAbbreviationTypeID int
	DECLARE @nvAbbreviation nvarchar(100)
	DECLARE @nvAbbreviationDescription nvarchar(1000)
	DECLARE @bAlwaysUse int

	DECLARE @tempWordAbbreviationsByCountry TABLE (iWordID int, iWordTypeID int, nvWord nvarchar(100), iAbbreviationID int, iAbbreviationTypeID int, nvAbbreviation nvarchar(100), nvAbbreviationDescription nvarchar(1000), bAlwaysUse int)
	DECLARE @tempCapturedWordOutput table (iWordOutputID int)
	DECLARE @tempCapturedAbbreviationOutput table (iAbbreviationOutputID int)
	DECLARE @iWordOutputID int
	DECLARE @iAbbreviationOutputID int

	INSERT INTO @tempWordAbbreviationsByCountry 
		SELECT iWordID, iWordTypeID, nvWord, iAbbreviationID, iAbbreviationTypeID, nvAbbreviation, nvAbbreviationDescription, bAlwaysUse 
	FROM vWordAbbreviationsByCountry 
	WHERE iLanguageID = @iExistingLanguageTemplateID

	WHILE EXISTS (SELECT 1 FROM @tempWordAbbreviationsByCountry)
	BEGIN

		SELECT TOP 1 
			@iWordID = iWordID, 
			@iWordTypeID = iWordTypeID, 
			@nvWord = nvWord, 
			@iAbbreviationID = iAbbreviationID, 
			@iAbbreviationTypeID = iAbbreviationTypeID, 
			@nvAbbreviation = nvAbbreviation, 
			@nvAbbreviationDescription = nvAbbreviationDescription, 
			@bAlwaysUse = bAlwaysUse  
		FROM @tempWordAbbreviationsByCountry

	    DELETE FROM @tempWordAbbreviationsByCountry WHERE iWordID = @iWordID

		INSERT INTO tWord (iWordTypeID, nvWord)
		  OUTPUT INSERTED.iWordID INTO @tempCapturedWordOutput
	    VALUES (@iWordTypeID, @nvWord)

		SELECT @iWordOutputID = iWordOutputID FROM @tempCapturedWordOutput
		DELETE FROM @tempCapturedWordOutput

		INSERT INTO tLanguageWordJoin (iLanguageID, iWordID)
		  VALUES (@iNewLanguageID, @iWordOutputID)

		INSERT INTO tAbbreviation (iAbbreviationTypeID, nvAbbreviation, nvAbbreviationDescription, bAlwaysUse)
		  OUTPUT INSERTED.iAbbreviationID INTO @tempCapturedAbbreviationOutput
	    VALUES (@iAbbreviationTypeID, @nvAbbreviation, @nvAbbreviationDescription, @bAlwaysUse)

		SELECT @iAbbreviationOutputID = iAbbreviationOutputID FROM @tempCapturedAbbreviationOutput
		DELETE FROM @tempCapturedAbbreviationOutput
	 
		INSERT INTO tWordAbbreviationJoin (iAbbreviationID, iWordID)
		  VALUES (@iAbbreviationOutputID, @iWordOutputID)

	END

	DECLARE @tempWordTypeOutput table (iWordTypeOutputID int)
	DECLARE @iWordTypeOutputID int

	DECLARE @tempWordType TABLE (iWordTypeID int, nvWordType nvarchar(100), nvWordTypeDescription nvarchar(1000))
	DECLARE @nvWordType nvarchar(100)
	DECLARE @nvWordTypeDescription nvarchar(1000)

	INSERT INTO @tempWordType
		SELECT tLanguageWordTypeJoin.iWordTypeID, tWordType.nvWordType, tWordType.nvWordTypeDescription
		FROM tLanguageWordTypeJoin 
			INNER JOIN tWordType ON tLanguageWordTypeJoin.iWordTypeID = dbo.tWordType.iWordTypeID
		WHERE iLanguageID = @iExistingLanguageTemplateID

	WHILE EXISTS (SELECT 1 FROM @tempWordType)
	BEGIN

	    SELECT TOP 1 
			@iWordTypeID = iWordTypeID,
			@nvWordType = nvWordType,
			@nvWordTypeDescription = nvWordTypeDescription
		FROM @tempWordType

	    DELETE FROM @tempWordType WHERE iWordTypeID = @iWordTypeID

		INSERT INTO tWordType (nvWordType, nvWordTypeDescription)
		  OUTPUT INSERTED.iWordTypeID INTO @tempWordTypeOutput
		VALUES (@nvWordType, @nvWordTypeDescription)

		SELECT @iWordTypeOutputID = iWordTypeOutputID FROM @tempWordTypeOutput
		DELETE FROM @tempWordTypeOutput

		INSERT INTO tLanguageWordTypeJoin (iLanguageID, iWordTypeID)
		  VALUES (@iNewLanguageID, @iWordTypeOutputID)

	    UPDATE tWord SET iWordTypeID = @iWordTypeOutputID 
		WHERE iWordID IN 
		(
			SELECT iWordID FROM vWordByLanguage WHERE iLanguageID = @iNewLanguageID AND iWordTypeID = @iWordTypeID
		)

	END

	DECLARE @tempCapturedAbbreviationTypeOutput table (iAbbreviationTypeOutputID int)
	DECLARE @iAbbreviationTypeOutputID int

	DECLARE @tempAbbreviationType TABLE (iAbbreviationTypeID int, nvAbbreviationType nvarchar(100), nvAbbreviationTypeDescription nvarchar(1000))
	DECLARE @nvAbbreviationType nvarchar(100)
	DECLARE @nvAbbreviationTypeDescription nvarchar(1000)

	INSERT INTO @tempAbbreviationType
		SELECT tLanguageAbbreviationTypeJoin.iAbbreviationTypeID, nvAbbreviationType, nvAbbreviationTypeDescription 
		FROM dbo.tAbbreviationType 
			INNER JOIN dbo.tLanguageAbbreviationTypeJoin ON dbo.tAbbreviationType.iAbbreviationTypeID = dbo.tLanguageAbbreviationTypeJoin.iAbbreviationTypeID
		WHERE iLanguageID = @iExistingLanguageTemplateID

	WHILE EXISTS (SELECT 1 FROM @tempAbbreviationType)
	BEGIN

		SELECT TOP 1 
			@iAbbreviationTypeID = iAbbreviationTypeID,
			@nvAbbreviationType = nvAbbreviationType,
			@nvAbbreviationTypeDescription = nvAbbreviationTypeDescription
	    FROM @tempAbbreviationType

	    DELETE FROM @tempAbbreviationType WHERE iAbbreviationTypeID = @iAbbreviationTypeID

		INSERT INTO tAbbreviationType (nvAbbreviationType, nvAbbreviationTypeDescription)
		  OUTPUT INSERTED.iAbbreviationTypeID INTO @tempCapturedAbbreviationTypeOutput
	    VALUES (@nvAbbreviationType, @nvAbbreviationTypeDescription)

		SELECT @iAbbreviationTypeOutputID = iAbbreviationTypeOutputID FROM @tempCapturedAbbreviationTypeOutput
		DELETE FROM @tempCapturedAbbreviationTypeOutput

		INSERT INTO tLanguageAbbreviationTypeJoin (iLanguageID, iAbbreviationTypeID)
		  VALUES (@iNewLanguageID, @iAbbreviationTypeOutputID)

	    UPDATE vWordByLanguage SET iAbbreviationTypeID = @iAbbreviationTypeOutputID WHERE iLanguageID = @iNewLanguageID AND iAbbreviationTypeID = @iAbbreviationTypeID 

	END

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pDropLanguage]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alexander Munro
-- Create date: 23/02/2020
-- Description:	Delete a language from the NAV database
--
-- Parameters:
--
--	@iDeleteLanguageID:	The language to be deleted
--
-- =============================================

CREATE PROCEDURE [dbo].[pDropLanguage] 
	@iDeleteLanguageID int = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @SUCCESS int = 0
	DECLARE @LANGUAGENOTFOUND int = 50094
	DECLARE @LANGUAGESYSTEMDEFAULT int = 50093
	DECLARE @LANGUAGELIMITEXCEEDED int = 50092

    -- throw an error and stop if the language is not found, is the system default or is the last available language

    IF NOT EXISTS (SELECT iLanguageID FROM tLanguage WHERE iLanguageID = @iDeleteLanguageID) RETURN @LANGUAGENOTFOUND

	IF @iDeleteLanguageID = 1 RETURN @LANGUAGESYSTEMDEFAULT

	IF (SELECT COUNT(*) FROM tLanguage) = 1 RETURN @LANGUAGELIMITEXCEEDED

    -- create temporary tables to ease the process

	SELECT * INTO #tempWordAbbreviationByCountry FROM vWordAbbreviationsByCountry WHERE iLanguageID = @iDeleteLanguageID
	SELECT * INTO #tempLanguageWordTypeJoin FROM tLanguageWordTypeJoin WHERE iLanguageID = @iDeleteLanguageID
	SELECT * INTO #tempLanguageAbbreviationTypeJoin FROM tLanguageAbbreviationTypeJoin WHERE iLanguageID = @iDeleteLanguageID

	-- begin deletes in strict order

	DELETE FROM tCountryLanguageJoin WHERE iLanguageID = @iDeleteLanguageID
	DELETE FROM tLanguageWordJoin WHERE iLanguageID = @iDeleteLanguageID
	DELETE FROM tLanguageWordTypeJoin WHERE iLanguageID = @iDeleteLanguageID
	DELETE FROM tLanguageAbbreviationTypeJoin WHERE iLanguageID = @iDeleteLanguageID
	DELETE FROM tWordAbbreviationJoin WHERE iWordID IN (SELECT iWordID FROM #tempWordAbbreviationByCountry)
	DELETE FROM tWord WHERE iWordID IN (SELECT iWordID FROM #tempWordAbbreviationByCountry)
	DELETE FROM tAbbreviation WHERE iAbbreviationID IN (SELECT iAbbreviationID FROM #tempWordAbbreviationByCountry)
	DELETE FROM tAbbreviationType WHERE iAbbreviationTypeID IN (SELECT iAbbreviationTypeID FROM #tempLanguageAbbreviationTypeJoin)
	DELETE FROM tWordType WHERE iWordTypeID IN (SELECT iWordTypeID FROM #tempLanguageWordTypeJoin)
	DELETE FROM tLanguage WHERE iLanguageID = @iDeleteLanguageID

    -- drop the temporary tables

	IF OBJECT_ID(N'tempdb..#tempWordAbbreviationByCountry', N'U') IS NOT NULL DROP TABLE #tempWordAbbreviationByCountry  
	IF OBJECT_ID(N'tempdb..#tempLanguageWordTypeJoin', N'U') IS NOT NULL DROP TABLE #tempLanguageWordTypeJoin  
	IF OBJECT_ID(N'tempdb..#tempWordAbbreviationByCountry', N'U') IS NOT NULL DROP TABLE #tempWordAbbreviationByCountry  

	RETURN @SUCCESS

END
GO
/****** Object:  StoredProcedure [dbo].[pDropWordByID]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 25/02/2020
-- Description:	Delete a word and associated dependencies
--
-- Parameters:
--
--	@iWordID:	The word belonging to a specific language.  The references to a word are removed from the following Tables
--
--					tLanguageWordJoin
--					tWordAbbreviationJoin
--					tWord
--
--				The following Tables are affected
--
--					tAbbreviation (we remove an abbreviation when it is nolonger in use by any word in the language)
--					tWordType  (we leave word type in place even when nolonger associated with any word in the language)
--
--				The following Views will ignore orphaned word types
--
--					vWordAbbreviationsByCountry
--					vWordByLanguage
--
--	Notes:
--
--	As simple as deleting a word is, it has a potentially far reaching impact on programmatic design.  
--
-- =================================================================================================================================
CREATE   PROCEDURE [dbo].[pDropWordByID]
	@iWordID int = 0
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @SUCCESS int = 0

	DECLARE @iLanguageID int = (SELECT iLanguageID FROM tLanguageWordJoin WHERE iWordID = @iWordID)
	DECLARE @iAbbreviationID int = (SELECT iAbbreviationID FROM tWordAbbreviationJoin WHERE iWordID = @iWordID)
    DECLARE @iAbbreviationInUseCount int = (SELECT COUNT(iAbbreviationID) FROM tWordAbbreviationJoin WHERE iAbbreviationID = @iAbbreviationID)

    DELETE FROM tLanguageWordJoin WHERE iWordID = @iWordID
    DELETE FROM tWordAbbreviationJoin WHERE iWordID = @iWordID
    DELETE FROM tWord WHERE iWordID = @iWordID

	IF (@iAbbreviationInUseCount = 1) DELETE FROM tAbbreviation WHERE iAbbreviationID = @iAbbreviationID

	RETURN @SUCCESS

END
GO
/****** Object:  StoredProcedure [dbo].[pEditAbbreviation]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 14/06/2020
-- Description:	Update a full abbreviation edit by its ID
--
-- Parameters:
--
--	@iAbbreviationID:				The Abbreviation ID belonging to the selected node
--	@nvWord:						The Word associated with the abbreviation
--	@nvAbbreviation:				The word abbreviation
--	@nvAbbreviationDescription:		The description of the word and its abbreviation (permits NULL's)
--	@bAlwaysUse:					True (1) or False (0). Defines whether the abbreviation permanently 
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pEditAbbreviation]
	@iUserID int = 0,
	@iAbbreviationID int = 0,
	@nvWord nvarchar(100) = '', 
	@nvAbbreviation nvarchar(50) = '', 
	@nvAbbreviationDescription nvarchar(1000) = '', 
	@bAlwaysUse bit = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 1

	UPDATE tWord SET 
		nvWord = @nvWord 
		WHERE iWordID = (SELECT TOP 1 iWordID FROM vWordAbbreviationByUserID WHERE iUserID = @iUserID AND iAbbreviationID = @iAbbreviationID)

	UPDATE tAbbreviation SET 
		nvAbbreviation = @nvAbbreviation, 
		nvAbbreviationDescription = @nvAbbreviationDescription,
		bAlwaysUse = @bAlwaysUse
	WHERE iAbbreviationID = @iAbbreviationID

	RETURN @Success

END

GO
/****** Object:  StoredProcedure [dbo].[pGetAbbreviationByUserID]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 25/05/2020
-- Description:	Get the first occurance of a word and return the word abbreviation
--
-- Parameters:
--
--	@iUserID: The ID of the user making the request
--	@nvWord: The word to search for
--	@bFlaggedRowsOnly: When present, the boolean value defining those abbreviations that can be searched
--
--	Notes:
--
--	When a word is not found the procedure returns the original search word in place of an abbreviation
--
-- =================================================================================================================================
CREATE   PROCEDURE [dbo].[pGetAbbreviationByUserID]
	@iUserID int = 0,
	@nvWord nvarchar(100) = null,
	@bFlaggedRowsOnly bit = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0

	DECLARE @TRUE bit = 1
	DECLARE @FALSE bit = 0
	DECLARE @nvAbbreviation nvarchar(100) = NULL

	IF @bFlaggedRowsOnly = @TRUE
	BEGIN
		SELECT TOP 1 @nvAbbreviation = nvAbbreviation
		FROM vWordAbbreviationByUserID
		WHERE iUserID = @iUserID AND nvWord = @nvWord AND bAlwaysUse = @TRUE
	END ELSE
	BEGIN
		SELECT TOP 1 @nvAbbreviation = nvAbbreviation
		FROM vWordAbbreviationByUserID
		WHERE iUserID = @iUserID AND nvWord = @nvWord
	END

	SELECT ISNULL(@nvAbbreviation, @nvWord) AS nvAbbreviation

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pGetAbbreviationType]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 03/03/2020
-- Description:	Fetch a distinct abbreviation type by abbreviation type ID
--
-- Parameters:
--
--	@iAbbreviationTypeID:	The abbreviation type ID
--
-- =================================================================================================================================
CREATE   PROCEDURE [dbo].[pGetAbbreviationType]
	@iAbbreviationTypeID int = 0
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @Success int = 0

    SELECT nvAbbreviationType, nvAbbreviationTypeDescription
	FROM tAbbreviationType
	WHERE iAbbreviationTypeID = @iAbbreviationTypeID

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pGetAbbreviationTypeByLanguage]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 25/02/2020
-- Description:	Fetch distinct available abbreviation types by language
--
-- Parameters:
--
--	@iLanguageID:	The language ID of the current or selected user
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pGetAbbreviationTypeByLanguage]
	@iLanguageID int = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0

    SELECT iAbbreviationTypeID, nvAbbreviationType, nvAbbreviationTypeDescription
	FROM vDistinctAbbreviationTypesByLanguage
	WHERE iLanguageID = @iLanguageID
	ORDER BY nvAbbreviationType

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pGetLastUserStateLogEntry]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 23/03/2020
-- Description:	Retreave the last known application state utilised by a user
--
-- Parameters:
--
--	@iUserID:	The user ID belonging to the logged user at the time a state change triggered a data entry
--
--  Notes:
--
--  Currently only one entry per user is held as we are only interested in the last known state.  There is, quite correctly, an argument 
--  to suggest that a history of activivity could be held here, however, at the time of writing this is deemed unnecessary. 
--
--  In light of this argument and in an attempt to future proof this procedure, we only return the last known entry for a specific user
--  irrespective of how many rows are actually held
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pGetLastUserStateLogEntry]
	@iUserID int = null 
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0
	
	SELECT iUserID, nvLastFilenameOpened, nvLastTabFocus, nvLastTableFocus, nvBuildVersion 
	FROM tNAVUserStateLogEntry
	WHERE iUserID = @iUserID AND iUserStateLogEntryID = (SELECT MAX(iUserStateLogEntryID) FROM tNAVUserStateLogEntry WHERE iUserID = @iUserID)

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pGetPhraseAbbreviationsByUserID]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 26/05/2020
-- Description:		Find available abbreviations for each word in a sentence and return the altered sentence
--
-- Parameters:
--
--	@iUserID: The ID of the user making the request
--	@inputString: The word or phrase to be parsed
--	@bFlaggedRowsOnly: When present, the boolean value defining those abbreviations that can be searched
--
--	Notes:
--
--	When an abbreviation is not found the procedure returns the original search word in place of an abbreviation
--
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[pGetPhraseAbbreviationsByUserID]
	@iUserID int = 0,
	@nvInputString nvarchar(MAX) = null,
	@bFlaggedRowsOnly bit = 0
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @Success int = 0

	SELECT dbo.fn_GetSentenceAbbreviation(@iUserID, @nvInputString, @bFlaggedRowsOnly) AS nvStringAbbreviation

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pGetStringAbbreviations]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 28/05/2020
-- Description:	Fetch abbreviations and symbols 
--
-- Parameters:
--
--	@iUserID: The ID of the user making the request
--	@bFlaggedRowsOnly: When false all abbreviations. When true, predefined abbreviations
--
--	Notes:
--
--	The function will always apply the smallest abbreviation available for any give word or phrase
--
-- =================================================================================================================================

CREATE procedure [dbo].[pGetStringAbbreviations]
	@iUserID int = 0,
	@bFlaggedRowsOnly bit = 0
AS
BEGIN

	DECLARE @TRUE bit = 1
	DECLARE @FALSE bit = 0

	DECLARE @table TABLE (
	  nvWord nvarchar(100) NOT NULL,
	  iWordLength int NOT NULL,
	  nvAbbreviation nvarchar(50) NOT NULL,
	  iAbbreviationLength int NOT NULL 
	)

	IF @bFlaggedRowsOnly = @TRUE
	BEGIN

		INSERT INTO @table SELECT DISTINCT a.nvWord, a.iWordLength, a.nvAbbreviation, a.iAbbreviationLength 
		FROM vWordAbbreviationByUserID a INNER JOIN
		(
			SELECT b.nvWord, MIN(b.iAbbreviationLength) AS iAbbreviationLength 
			FROM vWordAbbreviationByUserID b
			WHERE b.iUserID = 1 AND b.bAlwaysUse = @TRUE
			GROUP BY b.nvWord
		) b ON a.nvWord = b.nvWord AND a.iAbbreviationLength = b.iAbbreviationLength
		WHERE a.iUserID = 1 AND a.bAlwaysUse = @TRUE

	END ELSE BEGIN

		INSERT INTO @table SELECT DISTINCT a.nvWord, a.iWordLength, a.nvAbbreviation, a.iAbbreviationLength 
		FROM vWordAbbreviationByUserID a INNER JOIN
		(
			SELECT b.nvWord, MIN(b.iAbbreviationLength) AS iAbbreviationLength 
			FROM vWordAbbreviationByUserID b
			WHERE b.iUserID = 1
			GROUP BY b.nvWord
		) b ON a.nvWord = b.nvWord AND a.iAbbreviationLength = b.iAbbreviationLength
		WHERE a.iUserID = 1 
			
	END

	SELECT * FROM @table

END
GO
/****** Object:  StoredProcedure [dbo].[pGetUserPreferenceByPreferenceName]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 11/02/2020
-- Description:	Fetch a specific user preference for a specific user
--
-- Parameters:
--
--	@iUserID:	The user ID of the current or selected user
--	@nvClientPreferenceName:	The preference name belonging to the current or selected user
--
--
--	Notes:
--
--	For ease of maintenance business logic in the procedure has been kept as simple as possible.  At the client end C# could provide 
--	a List or Dictionary to offer similar functionality while obfuscating the preference naming conventions further than they already are.  
--	The NAV application has no control over the accuracy, over time, of the preference naming conventions used here.
--	
--	As such, two parameters are passed to this procedure: 
--	
--		1.	The User ID, which if incorrect, will have already failed on the application prior to this procedure being called
--
--		2.	The Preference Name.  If this paremeter is altered or dropped at he database end this stored procedure ensures that 
--			the nvClientPreferenceName and the nvUserPreferenceValue are returned where the nvUserPreferenceValue return value 
--			is "Invalid Preference Name reference".  
--
--			In this event, the maintenance engineer can simply remove the call to a Preference name nolonger in use (or not, it 
--			will be ignored regardless), or provide the correct value in PreferenceHelper.cs to the following database call:
--
--				DataAccess.GetUserPreferenceByPreferenceName("provide the correct Preference Name here").
--
--	Lastly, the database employs Triggers to safeguard the data used by the NAV application PreferenceHelper class 
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pGetUserPreferenceByPreferenceName]
	@iUserID int = 0,
	@nvClientPreferenceName nvarchar(250) = null
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0

	IF (SELECT COUNT(*) FROM vUserPreferencesByUserID WHERE (iUserID = @iUserID AND nvClientPreferenceName = @nvClientPreferenceName AND bDisablePreference = 0 AND bDisablePreferenceType = 0)) = 0
	BEGIN
		SELECT @nvClientPreferenceName AS nvClientPreferenceName, 0 AS bUserPreference, 1 AS bClientValueRequired, 'Invalid Preference Name reference' AS nvUserPreferenceValue
	END ELSE
	BEGIN
		SELECT iClientPreferenceID, iUserPreferenceID, iClientPreferenceTypeID, nvClientPreferenceType, nvClientPreferenceName, nvClientPreferenceDescription, bClientPreference, bUserPreference, bClientOverride, bSystemOverride, ISNULL(bClientValueRequired,0) AS bClientValueRequired, nvClientPreferenceValue, nvUserPreferenceValue
		FROM vUserPreferencesByUserID
		WHERE (iUserID = @iUserID AND nvClientPreferenceName = @nvClientPreferenceName AND bDisablePreference = 0 AND bDisablePreferenceType = 0)
		ORDER BY iOrderByPreference, iClientPreferenceTypeID, nvClientPreferenceName
	END

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pGetUserPreferenceDictionaryModel]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 11/02/2020
-- Description:	Fetch the set of all user preferences for a specific user for inclusion in a C# Dictionary data type
--
-- Parameters:
--
--	@iUserID:	The user ID of the current or selected user
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pGetUserPreferenceDictionaryModel]
	@iUserID int = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0

	SELECT iClientPreferenceID AS iPreferenceID, iClientPreferenceTypeID AS iPreferenceTypeID, nvClientPreferenceName AS nvPreferenceName, bUserPreference AS bPreference, bClientValueRequired AS bValueRequired, nvUserPreferenceValue AS nvPreferenceValue 
	FROM vUserPreferencesByUserID
	WHERE (iUserID = @iUserID AND bDisablePreference = 0 AND bDisablePreferenceType = 0)
	ORDER BY iOrderByPreference, iClientOrderPreference, iClientPreferenceTypeID, nvClientPreferenceName

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pGetUserPreferencesByPreferenceType]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 04/04/2020
-- Description:	Fetch the set of user preferences for a specific user and specific preference type
--
-- Parameters:
--
--	@iUserID:	The user ID of the current or selected user
--	@nvClientPreferenceType:	The preference type requested
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pGetUserPreferencesByPreferenceType]
	@iUserID int = 0,
	@nvClientPreferenceType nvarchar(250) = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0

	SELECT iClientPreferenceID, iUserPreferenceID, iClientPreferenceTypeID, nvClientPreferenceType, nvClientPreferenceName, nvClientPreferenceDescription, bClientPreference, bUserPreference, bClientOverride, bSystemOverride, bClientValueRequired, nvClientPreferenceValue, nvUserPreferenceValue
	FROM vUserPreferencesByUserID
	WHERE (iUserID = @iUserID AND nvClientPreferenceType = @nvClientPreferenceType AND bDisablePreference = 0 AND bDisablePreferenceType = 0)
	ORDER BY iOrderByPreference, iClientOrderPreference, iClientPreferenceTypeID, nvClientPreferenceName
	
	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pGetUserPreferencesByUserID]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 11/02/2020
-- Description:	Fetch the set of user preferences for a specific user
--
-- Parameters:
--
--	@iUserID:	The user ID of the current or selected user
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pGetUserPreferencesByUserID]
	@iUserID int = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0

	SELECT iClientPreferenceID, iUserPreferenceID, iClientPreferenceTypeID, nvClientPreferenceType, nvClientPreferenceName, nvClientPreferenceDescription, bClientPreference, bUserPreference, bClientOverride, bSystemOverride, bClientValueRequired, nvClientPreferenceValue, nvUserPreferenceValue
	FROM vUserPreferencesByUserID
	WHERE (iUserID = @iUserID AND bDisablePreference = 0 AND bDisablePreferenceType = 0)
	ORDER BY iOrderByPreference, iClientOrderPreference, iClientPreferenceTypeID, nvClientPreferenceName


	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pGetUserProperties]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 25/02/2020
-- Description:	Fetch the principle user details
--
-- Parameters:
--
--	@iUserID:	The ID of the current or selected user
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pGetUserProperties]
	@iUserID int = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0
	DECLARE @TRUE bit = 1
	DECLARE @FALSE bit = 0

--== begin confirm user ==================================================================

	DECLARE @EMAILNOTCONFIRMED int = 50099
	DECLARE @ACCESSFAILEDCOUNT int = 50098
	DECLARE @LOCKOUTENABLED int = 50097
	DECLARE @LOCKOUTENDDATEEXPIRED int = 50096
	DECLARE @USERNOTFOUND int = 50095

	DECLARE @iUserExists int = 0
	DECLARE @bEmailConfirmed bit = @FALSE
	DECLARE @bLockoutEnabled bit = @TRUE
	DECLARE @iAccessFailedCount int = 31
	DECLARE @iAccessFailedAttempts int = 30
	DECLARE @dLockoutEndDateUTC datetime = DATEADD(DAY, +1000, GETDATE())

	SELECT @iUserExists = COUNT(*) FROM vLanguageByUser WHERE iUserID = @iUserID

	IF @iUserExists <> 1 RETURN @USERNOTFOUND

    SELECT @bEmailConfirmed = bEmailConfirmed, @bLockoutEnabled = bLockoutEnabled, @iAccessFailedCount = iAccessFailedCount, @dLockoutEndDateUTC = dLockoutEndDateUTC
	FROM vLanguageByUser 
	WHERE iUserID = @iUserID

	IF NOT @bEmailConfirmed = @TRUE RETURN @EMAILNOTCONFIRMED

	IF @bLockoutEnabled = @TRUE RETURN @LOCKOUTENABLED

	IF (@dLockoutEndDateUTC <= GETDATE())
	BEGIN
		UPDATE tNAVUserProperties SET bLockoutEnabled = @TRUE WHERE iUserID = @iUserID;
		RETURN @LOCKOUTENDDATEEXPIRED
    END 

	IF @iAccessFailedCount > @iAccessFailedAttempts RETURN @ACCESSFAILEDCOUNT

--== end confirm user ==================================================================

    SELECT iUserID, iCountryID, nvCountryName, nAlphaCode2, nAlphaCode3, nNumeric, iLanguageID, nvLanguage, iClientID, nvClientName, bRegistered, nvEmailAddress, bEmailConfirmed, dLockoutEndDateUTC, bLockoutEnabled, iAccessFailedCount, nvPreferredName, nvUserName
	FROM vLanguageByUser 
	WHERE iUserID = @iUserID

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pGetWordAbbreviationsByLanguageByType]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 25/02/2020
-- Description:	Fetch abbreviations by language and by type
--
-- Parameters:
--
--	@iLanguageID:			The language ID of the current or selected user
--	@iAbbreviationTypeID:	The Abbreviation Type ID belonging to the selected language
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pGetWordAbbreviationsByLanguageByType]
	@iLanguageID int = 0,
	@iAbbreviationTypeID int = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0

    SELECT iAbbreviationID, iAbbreviationTypeID, iWordID, nvWord, nvAbbreviation, nvAbbreviationDescription, bAlwaysUse
	FROM vWordAbbreviationsByCountry
	WHERE iLanguageID = @iLanguageID AND iAbbreviationTypeID = @iAbbreviationTypeID
	ORDER BY nvWord

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pInsertAbbreviation]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 25/02/2020
-- Description:	Create an abbreviation
--
-- Parameters:
--
--	@iLanguageID:					The Language ID belonging to the user
--	@iAbbreviationTypeID:			The Abbreviation Type ID belonging to abbreviation (passed to the routine)
--	@nvWord:						The Word associated with the abbreviation
--	@nvAbbreviation:				The word abbreviation
--	@nvAbbreviationDescription:		The description of the word and its abbreviation (permits NULL's)
--	@bAlwaysUse:					True (1) or False (0). Defines whether the abbreviation permanently 
--									replaces the associated word. The default is False. 
--
--  Notes:
--
--	Always insert a new abbreviation even if it is a duplicate.
--
--  A user may wish to display an abbreviation for one word but not for another when they share the same abbreviation.  This enables the user to 
--  set the abbreviation for one word to always display, while never displaying the same abbreviation for another word.  
--
--  Logically, there is a strong argument to suggest that the bAlwaysUse directive should be held on the word table and not the abbreviation table.
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pInsertAbbreviation]
	@iLanguageID int = 0,
	@iAbbreviationTypeID int = 0,
	@nvWord nvarchar(100) = '', 
	@nvAbbreviation nvarchar(50) = '', 
	@nvAbbreviationDescription nvarchar(1000) = '', 
	@bAlwaysUse bit = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0

	DECLARE @tCapturedOutput table (iOutputRowID int)

    DECLARE @iAbbreviationID int = NULL
	DECLARE @iNewWordID int = (SELECT TOP 1 iWordID FROM vWordByLanguage WHERE iLanguageID = @iLanguageID AND nvWord = @nvWord AND nvAbbreviation = @nvAbbreviation)

	IF (@iNewWordID IS NULL)
	BEGIN

		-- insert into tWord the new word and word type if it does not already exist in the language and return the word ID and the word type ID
		-- the word type ID is the lowest word type ID in the language
		DECLARE @iWordTypeID int = (SELECT MIN(iWordTypeID) AS iWordTypeID FROM vWordByLanguage WHERE iLanguageID = @iLanguageID)

		INSERT INTO tWord (iWordTypeID, nvWord) 
			OUTPUT INSERTED.iWordID INTO @tCapturedOutput
		VALUES (@iWordTypeID, @nvWord)

		SET @iNewWordID = (SELECT iOutputRowID FROM @tCapturedOutput)
		DELETE FROM @tCapturedOutput

		-- insert into tLanguageWordJoin the language ID and word ID if it does not already exist
		IF ((SELECT iWordID FROM tLanguageWordJoin WHERE iLanguageID = @iLanguageID AND iWordID = @iNewWordID) IS NULL)
		BEGIN
			INSERT INTO tLanguageWordJoin (iLanguageID, iWordID)
			VALUES (@iLanguageID, @iNewWordID)
		END

		-- insert into tAbbreviation the abbreviation Type ID, the abbreviation, abbreviation description, and the bAlwaysUse value. 
		INSERT INTO tAbbreviation (iAbbreviationTypeID, nvAbbreviation, nvAbbreviationDescription, bAlwaysUse)
			OUTPUT INSERTED.iAbbreviationID INTO @tCapturedOutput
		VALUES (@iAbbreviationTypeID, @nvAbbreviation, @nvAbbreviationDescription, @bAlwaysUse)
		
		-- Return the abbreviation ID
		SET @iAbbreviationID = (SELECT iOutputRowID FROM @tCapturedOutput)
		DELETE FROM @tCapturedOutput

	  	-- insert into tWordAbbreviationJoin the abbreviation ID and the word ID if it does not already exist
		IF (@iAbbreviationID IS NOT NULL)
		BEGIN
			IF NOT EXISTS (SELECT iAbbreviationID FROM tWordAbbreviationJoin WHERE iAbbreviationID = @iAbbreviationID AND iWordID = @iNewWordID)
			BEGIN
				INSERT INTO tWordAbbreviationJoin (iAbbreviationID, iWordID)
				VALUES (@iAbbreviationID, @iNewWordID)
			END
		END 
		
	END ELSE
	BEGIN

		SET @iAbbreviationTypeID = (SELECT iAbbreviationTypeID FROM vWordAbbreviationsByCountry WHERE iWordID = @iNewWordID)
	
		RETURN @iAbbreviationTypeID

	END

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pInsertErrorLogEntry]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 03/03/2020
-- Description:	Log an exception instance
--
-- Parameters:
--
--	@userid:			The user ID belonging to the logged user at the time of the error
--	@log_date:			The date and time the error occurred
--	@thread:			In a typical multithreaded implementation, different threads will handle different clients
--	@log_level:			Will display DEBUG, INFO, WARN, ERROR, FATAL depending on the NAV Service application configuration
--	@logger:			This is the originating source, typically (when not in production) the class where the error occurred
--	@message:			The message will be the whatever the developer used to describe the error
--	@exception:			Is a rendition of the stack trace error description generated by the application.
--  @nvBuildVersion:	The current Similarity application build version
--					
--  Notes:
--
--  The procedure associates an error with a specific user ID.  In the advent that a user ID has not been established the routine 
--  creates a Sysadmin user if one does not already exist, and associates the error with this user.  In addition the routine performs
--  some house cleaning by enforcing a limit to the number of errors that can remain on record.
--
--	At the time of writing the Apache log4net library is the logger of choice. The tool is used to help the programmer output 
--  log statements to a variety of output targets. log4net is a port of the Apache log4j™ framework to the Microsoft® .NET runtime. 
--
--  As part of the Logging Services project, log4net is intended to provide cross-language logging services for the purpose of application 
--  debugging and auditing.
--
--  All filters and logging attributes are defined in the app.config xml file and are readonly in production
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pInsertErrorLogEntry]
	@userid int = null, 
	@log_date datetime = null, 
	@thread nvarchar(255) = null, 
	@log_level nvarchar(50) = null, 
	@logger nvarchar(255) = null, 
	@message nvarchar(4000) = null, 
	@exception nvarchar(4000) = null,
	@nvBuildVersion nvarchar(10) = null,
	@ExceptionLimit int = 10000
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0
	
	DECLARE @SystemName nvarchar(25) = 'NAV System Administrator'
	DECLARE @iVerifiedUserID int = (SELECT iUserID FROM tNAVUserProperties WHERE iUserID = @userid);
	
-- =================================================================================================================================
-- Permit the system to hold no more than 50000, and no less than 100 exceptions in the log
 
	DECLARE @TRUE bit = 1
	DECLARE @FALSE bit = 0
	DECLARE @ResetCount bit = @FALSE

	IF (@ExceptionLimit IS NULL) SET @ResetCount = @TRUE
	IF (@ExceptionLimit < 100) SET @ResetCount = @TRUE
	IF (@ExceptionLimit > 50000) SET @ResetCount = @TRUE

	IF (@ResetCount = @TRUE) SET @ExceptionLimit = 10000

	DECLARE @SQLString nvarchar(100) = 'DELETE FROM tNAVLog WHERE iLogID NOT IN (SELECT TOP ' + CAST(@ExceptionLimit AS nvarchar(10)) + ' iLogID FROM tNAVLog ORDER BY iLogID DESC)'

	EXEC (@SQLString)

-- =================================================================================================================================
-- In the advent that a user ID has not been established, create a Sysadmin default user if one does not already exist, and associate 
-- the error with this user

	IF (@iVerifiedUserID IS NULL) 
	BEGIN
	  
		SET @iVerifiedUserID = (SELECT iUserID FROM tNAVUserProperties WHERE nvPreferredName = @SystemName AND nvUserName = @SystemName)

		IF (@iVerifiedUserID IS NULL) 
		BEGIN

			DECLARE @tCapturedOutput table (iOutputRowID int)

			DECLARE @iVerifiedClientID int = (SELECT iClientID FROM tNAVClient WHERE nvClientName = @SystemName AND nvClientDescription = @SystemName)

			IF (@iVerifiedClientID IS NULL) 
			BEGIN

				DECLARE @iVerifiedSysAdminID int = (SELECT TOP 1 iSysAdminID FROM tNAVSysAdmin WHERE nvSysAdminName = @SystemName)

				IF (@iVerifiedSysAdminID IS NULL) 
				BEGIN

					INSERT INTO tNAVSysAdmin (nvSysAdminName, nvSysAdminDescription)
						OUTPUT INSERTED.iSysAdminID INTO @tCapturedOutput
					VALUES (@SystemName, @SystemName)

					SET @iVerifiedSysAdminID = (SELECT iOutputRowID FROM @tCapturedOutput)
					DELETE FROM @tCapturedOutput

				END

				INSERT INTO tNAVClient (nvClientName, bRegistered, nvClientDescription)
					OUTPUT INSERTED.iClientID INTO @tCapturedOutput
				VALUES (@SystemName, 1, @SystemName)

				SET @iVerifiedClientID = (SELECT iOutputRowID FROM @tCapturedOutput)
				DELETE FROM @tCapturedOutput

				INSERT INTO tNAVSysAdminClientJoin (iSysAdminID, iClientID)
				VALUES (@iVerifiedSysAdminID, @iVerifiedClientID)

			END

			INSERT INTO tNAVUserProperties (iClientID, iLanguageID, nvEmailAddress, bEmailConfirmed, nvPreferredName, nvUserName)
				OUTPUT INSERTED.iUserID INTO @tCapturedOutput
			VALUES (@iVerifiedClientID, 22, @SystemName, 1, @SystemName, @SystemName)

			SET @iVerifiedUserID = (SELECT iOutputRowID FROM @tCapturedOutput)
			DELETE FROM @tCapturedOutput

		END

	END

-- =================================================================================================================================
-- Get the current Similarity application build version

	IF (@nvBuildVersion IS NULL) 
	BEGIN

	 SET @nvBuildVersion = (SELECT nvBuildVersion FROM tNAVUserStateLogEntry WHERE iUserID = @iVerifiedUserID AND iUserStateLogEntryID = (SELECT MAX(iUserStateLogEntryID) FROM tNAVUserStateLogEntry WHERE iUserID = @iVerifiedUserID))

	END	
	
-- =================================================================================================================================
-- Insert the error into the log

	INSERT INTO tNAVLog (iUserID, dDateTime, nvThread, nvLevel, nvSource, nvMessage, nvException, nvBuildVersion) 
	VALUES (@iVerifiedUserID, @log_date, @thread, @log_level, @logger, @message, @exception, @nvBuildVersion)

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pInsertUserStateLogEntry]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 23/03/2020
-- Description:	Log the last known application state utilised by a user
--
-- Parameters:
--
--	@iUserID:				The user ID belonging to the logged user at the time a state change triggered a data entry
--	@nvLastFilenameOpened:	The full name and path of the last file loaded by the user
--	@nvLastTabFocus:		The C# name given to the last Tab opened by the user
--	@nvBuildVersion:		The current Similarity application build version
--
--  Notes:
--
--  We only hold one entry per user as we are only interested in the last known state.  There is, quite correctly, an argument 
--  to suggest that a history of activity could be held here, however, at the time of writing this is deemed unnecessary. 
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pInsertUserStateLogEntry]
	@iUserID int = null, 
	@nvLastFilenameOpened nvarchar(250) = null, 
	@nvLastTabFocus nvarchar(25) = null,
	@nvLastTableFocus nvarchar(250) = null,
	@nvBuildVersion nvarchar(10) = null
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0
	
	DECLARE @iVerifiedUserID int = (SELECT iUserID FROM tNAVUserStateLogEntry WHERE iUserID = @iUserID)

    IF (@iVerifiedUserID IS NOT NULL)
	BEGIN

		IF (@nvLastFilenameOpened IS NOT NULL)
		BEGIN

			UPDATE tNAVUserStateLogEntry SET nvLastFilenameOpened = @nvLastFilenameOpened
			WHERE iUserID = @iUserID

		END
		
		IF (@nvLastTabFocus IS NOT NULL)
		BEGIN

			UPDATE tNAVUserStateLogEntry SET nvLastTabFocus = @nvLastTabFocus
			WHERE iUserID = @iUserID

		END

		IF (@nvLastTableFocus IS NOT NULL)
		BEGIN

			UPDATE tNAVUserStateLogEntry SET nvLastTableFocus = @nvLastTableFocus
			WHERE iUserID = @iUserID

		END

		IF (@nvBuildVersion IS NOT NULL)
		BEGIN

			UPDATE tNAVUserStateLogEntry SET nvBuildVersion = @nvBuildVersion
			WHERE iUserID = @iUserID

		END

	END ELSE
	BEGIN
	
		INSERT INTO tNAVUserStateLogEntry (iUserID, nvLastFilenameOpened, nvLastTabFocus, nvLastTableFocus, nvBuildVersion) 
		VALUES (@iUserID, @nvLastFilenameOpened, @nvLastTabFocus, @nvLastTableFocus, @nvBuildVersion)

	END


	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pSetStringAbbreviations]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 28/05/2020
-- Description:		Find and replace abbreviations equivalents within a string
--
-- Parameters:
--
--	@iUserID: The ID of the user making the request
--	@inputString: The word or phrase to be parsed
--	@bFlaggedRowsOnly: When present, the boolean value defining those abbreviations that can be searched
--
--	Notes:
--
--	The function will always apply the smallest abbreviation available for any give word or phrase
--
-- =================================================================================================================================

CREATE PROCEDURE [dbo].[pSetStringAbbreviations]
	@iUserID int = 0,
	@nvInputString nvarchar(MAX) = null,
	@bFlaggedRowsOnly bit = 0
AS
BEGIN

	SET NOCOUNT ON

	/* EXEC sp_updatestats; // disabled due to permissions on AWS EC2 production server */

	DECLARE @Success int = 0

	SELECT dbo.fn_SetStringAbbreviations(@iUserID, @nvInputString, @bFlaggedRowsOnly) AS nvString

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pTestCreateLanguageFromTemplate]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alexander Munro
-- Create date: 22/02/2020
-- Description:	Create a new language from an existing language template
--
-- Parameters:
--
--	@iChosenCountryID:				The country chosen by the user from the system derived country code list
--
--	@iExistingLanguageTemplateID:	The template language chosen by the user from all available languages
--
--										Note:
--										A language already belonging to a country other than the chosen country may 
--										be more important to use as a language template
--
--	@nvNewLanguageName:				The name given by the user to represent the new language to be associated with the chosen country
--
-- =============================================
CREATE PROCEDURE [dbo].[pTestCreateLanguageFromTemplate] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE	@return_value int

	EXEC @return_value = [dbo].[pCreateLanguageFromTemplate]
		@iChosenCountryID = 2,
		@iExistingLanguageTemplateID = 22,
		@nvNewLanguageName = N'testLanguage'

	SELECT	'Return Value' = @return_value
END

GO
/****** Object:  StoredProcedure [dbo].[pUpdateAbbreviation]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 25/02/2020
-- Description:	Update an abbreviation by its ID
--
-- Parameters:
--
--	@iAbbreviationID:	The Abbreviation ID belonging to the selected node
--	@nvAbbreviation :	The word abbreviation
--	@bAlwaysUse:		True (1) or False (0). Defines whether the abbreviation permanently 
--						replaces the associated word. The default is False. 
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pUpdateAbbreviation]
	@iAbbreviationID int = 0,
	@nvAbbreviation nvarchar(50) = '', 
	@bAlwaysUse bit = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0

    UPDATE tAbbreviation SET 
		nvAbbreviation = @nvAbbreviation, 
		bAlwaysUse = @bAlwaysUse
	WHERE iAbbreviationID = @iAbbreviationID

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pUpdateUserPreference]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 13/03/2020
-- Description:	Update a distinct user preference value by its ID
--
-- Parameters:
--
--	@iUserPreferenceID:			The user preference ID belonging to the selected node
--	@bUserPreference:			True (1) or False (0). Defines the true or false value assigned to the preference
--	@nvUserPreferenceValue:		Defines the string value assigned to the preference
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pUpdateUserPreference]
	@iUserPreferenceID int = 0,
	@bUserPreference bit = 0, 
	@nvUserPreferenceValue nvarchar(1000) = null
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Success int = 0

	UPDATE tNAVUserPreferences SET bUserPreference = @bUserPreference, nvUserPreferenceValue = @nvUserPreferenceValue
	WHERE iUserPreferenceID = @iUserPreferenceID

	RETURN @Success

END
GO
/****** Object:  StoredProcedure [dbo].[pUserExists]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================
-- Author:		Alexander Munro
-- Create date: 25/02/2020
-- Description:	Fetch a boolean response to the existence os a user
--
-- Parameters:
--
--	@iUserID:	The ID of the current or selected user.  
--
--  Notes:
--
--  Return false if the user count does not equal one
--
-- =================================================================================================================================
CREATE PROCEDURE [dbo].[pUserExists]
	@iUserID int = 0
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @TRUE bit = 1
	DECLARE @FALSE bit = 0
	DECLARE @bUserExists bit = @FALSE

 	IF (SELECT COUNT(*) FROM vLanguageByUser WHERE iUserID = @iUserID) = 1 SET @bUserExists = @TRUE

	SELECT @bUserExists AS bConfirmed

END
GO
/****** Object:  Trigger [dbo].[trCountryLock_Delete]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trCountryLock_Delete]
ON [dbo].[tCountry]
INSTEAD OF DELETE
AS 
BEGIN

	DECLARE @iCountryID int

	SELECT @iCountryID = DELETED.iCountryID FROM DELETED

	IF (@iCountryID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'A country cannot be deleted', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tCountry] ENABLE TRIGGER [trCountryLock_Delete]
GO
/****** Object:  Trigger [dbo].[trLanguageLock_Delete]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trLanguageLock_Delete]
ON [dbo].[tLanguage]
INSTEAD OF DELETE
AS 
BEGIN

	DECLARE @TRUE bit = 1
	DECLARE @FALSE bit = 0
	DECLARE @nvLanguage nvarchar(100) = NULL

	DECLARE @FOUND bit = @FALSE

	IF @FOUND = @FALSE (SELECT @FOUND = @TRUE, @nvLanguage = DELETED.nvLanguage FROM DELETED WHERE DELETED.nvLanguage = 'English (Generic)')
	IF @FOUND = @FALSE (SELECT @FOUND = @TRUE, @nvLanguage = DELETED.nvLanguage  FROM DELETED WHERE DELETED.nvLanguage = 'English UK')
	IF @FOUND = @FALSE (SELECT @FOUND = @TRUE, @nvLanguage = DELETED.nvLanguage  FROM DELETED WHERE DELETED.nvLanguage = 'English US')
	IF @FOUND = @FALSE (SELECT @FOUND = @TRUE, @nvLanguage = DELETED.nvLanguage  FROM DELETED WHERE DELETED.nvLanguage = 'English AU')

	IF (@FOUND = @TRUE)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION  

		RAISEERROR (SELECT 'The ''' + @nvLanguage + ''' language is a reserved language and cannot be deleted', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tLanguage] ENABLE TRIGGER [trLanguageLock_Delete]
GO
/****** Object:  Trigger [dbo].[trLanguageLock_Update]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trLanguageLock_Update]
ON [dbo].[tLanguage]
INSTEAD OF UPDATE
AS 
BEGIN

	DECLARE @TRUE bit = 1
	DECLARE @FALSE bit = 0
	DECLARE @nvLanguage nvarchar(100) = NULL

	DECLARE @FOUND bit = @FALSE

	IF @FOUND = @FALSE (SELECT @FOUND = @TRUE, @nvLanguage = INSERTED.nvLanguage FROM INSERTED WHERE INSERTED.nvLanguage = 'English (Generic)')
	IF @FOUND = @FALSE (SELECT @FOUND = @TRUE, @nvLanguage = INSERTED.nvLanguage  FROM INSERTED WHERE INSERTED.nvLanguage = 'English UK')
	IF @FOUND = @FALSE (SELECT @FOUND = @TRUE, @nvLanguage = INSERTED.nvLanguage  FROM INSERTED WHERE INSERTED.nvLanguage = 'English US')
	IF @FOUND = @FALSE (SELECT @FOUND = @TRUE, @nvLanguage = INSERTED.nvLanguage  FROM INSERTED WHERE INSERTED.nvLanguage = 'English AU')

	IF (@FOUND = @TRUE)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'The ''' + @nvLanguage + ''' language is a reserved language and cannot be updated', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tLanguage] ENABLE TRIGGER [trLanguageLock_Update]
GO
/****** Object:  Trigger [dbo].[trSysAdminClientLock_Delete]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trSysAdminClientLock_Delete]
ON [dbo].[tNAVClient]
INSTEAD OF DELETE
AS 
BEGIN

	DECLARE @iClientID int
	DECLARE @SysAdminName nvarchar(25) = 'NAV System Administrator'

	SELECT @iClientID = DELETED.iClientID FROM DELETED WHERE DELETED.nvClientName = @SysAdminName AND DELETED.nvClientDescription = @SysAdminName

	IF (@iClientID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'The ' + @SysAdminName + ' account cannot be deleted', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVClient] ENABLE TRIGGER [trSysAdminClientLock_Delete]
GO
/****** Object:  Trigger [dbo].[trSysAdminClientLock_Insert]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trSysAdminClientLock_Insert]
ON [dbo].[tNAVClient]
INSTEAD OF INSERT
AS 
BEGIN

	DECLARE @iClientID int
	DECLARE @SysAdminName nvarchar(25) = 'NAV System Administrator'

	SELECT @iClientID = INSERTED.iClientID FROM INSERTED WHERE INSERTED.nvClientName = @SysAdminName

	IF (@iClientID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'The ' + @SysAdminName + ' account cannot be inserted', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVClient] ENABLE TRIGGER [trSysAdminClientLock_Insert]
GO
/****** Object:  Trigger [dbo].[trSysAdminClientLock_Update]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trSysAdminClientLock_Update]
ON [dbo].[tNAVClient]
INSTEAD OF UPDATE
AS 
BEGIN

	DECLARE @iClientID int
	DECLARE @SysAdminName nvarchar(25) = 'NAV System Administrator'

	SELECT @iClientID = INSERTED.iClientID FROM INSERTED WHERE INSERTED.nvClientName = @SysAdminName AND INSERTED.nvClientDescription = @SysAdminName

	IF (@iClientID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'The ' + @SysAdminName + ' account cannot be updated', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVClient] ENABLE TRIGGER [trSysAdminClientLock_Update]
GO
/****** Object:  Trigger [dbo].[trClientPreferencesLock_Delete]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create TRIGGER [dbo].[trClientPreferencesLock_Delete]
ON [dbo].[tNAVClientPreferences]
INSTEAD OF DELETE
AS 
BEGIN

	DECLARE @iClientPreferenceID int

	SELECT @iClientPreferenceID = DELETED.iClientPreferenceID FROM DELETED

	IF (@iClientPreferenceID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'Client preferrences cannot be deleted', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVClientPreferences] ENABLE TRIGGER [trClientPreferencesLock_Delete]
GO
/****** Object:  Trigger [dbo].[trClientPreferenceTypeLock_Delete]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create TRIGGER [dbo].[trClientPreferenceTypeLock_Delete]
ON [dbo].[tNAVClientPreferenceType]
INSTEAD OF DELETE
AS 
BEGIN

	DECLARE @iClientPreferenceTypeID int

	SELECT @iClientPreferenceTypeID = DELETED.iClientPreferenceTypeID FROM DELETED

	IF (@iClientPreferenceTypeID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'Client preferrences types cannot be deleted', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVClientPreferenceType] ENABLE TRIGGER [trClientPreferenceTypeLock_Delete]
GO
/****** Object:  Trigger [dbo].[trSysAdminLock_Delete]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trSysAdminLock_Delete]
ON [dbo].[tNAVSysAdmin]
INSTEAD OF DELETE
AS 
BEGIN

	DECLARE @iSysAdminID int
	DECLARE @SysAdminName nvarchar(25) = 'NAV System Administrator'

	SELECT @iSysAdminID = DELETED.iSysAdminID FROM DELETED WHERE DELETED.nvSysAdminName = @SysAdminName

	IF (@iSysAdminID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'The ' + @SysAdminName + ' account cannot be deleted', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVSysAdmin] ENABLE TRIGGER [trSysAdminLock_Delete]
GO
/****** Object:  Trigger [dbo].[trSysAdminLock_Insert]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trSysAdminLock_Insert]
ON [dbo].[tNAVSysAdmin]
INSTEAD OF INSERT
AS 
BEGIN

	DECLARE @iSysAdminID int
	DECLARE @SysAdminName nvarchar(25) = 'NAV System Administrator'

	SELECT @iSysAdminID = INSERTED.iSysAdminID FROM INSERTED WHERE INSERTED.nvSysAdminName = @SysAdminName

	IF (@iSysAdminID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'A ' + @SysAdminName + ' account cannot be inserted', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVSysAdmin] ENABLE TRIGGER [trSysAdminLock_Insert]
GO
/****** Object:  Trigger [dbo].[trSysAdminLock_Update]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trSysAdminLock_Update]
ON [dbo].[tNAVSysAdmin]
INSTEAD OF UPDATE
AS 
BEGIN

	DECLARE @iSysAdminID int
	DECLARE @SysAdminName nvarchar(25) = 'NAV System Administrator'

	SELECT @iSysAdminID = INSERTED.iSysAdminID FROM INSERTED WHERE INSERTED.nvSysAdminName = @SysAdminName

	IF (@iSysAdminID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'The ' + @SysAdminName + ' account cannot be updated', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVSysAdmin] ENABLE TRIGGER [trSysAdminLock_Update]
GO
/****** Object:  Trigger [dbo].[trUserPreferencesLock_Delete]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create TRIGGER [dbo].[trUserPreferencesLock_Delete]
ON [dbo].[tNAVUserPreferences]
INSTEAD OF DELETE
AS 
BEGIN

	DECLARE @iUserPreferenceID int

	SELECT @iUserPreferenceID = DELETED.iUserPreferenceID FROM DELETED

	IF (@iUserPreferenceID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'User preferrences cannot be deleted', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVUserPreferences] ENABLE TRIGGER [trUserPreferencesLock_Delete]
GO
/****** Object:  Trigger [dbo].[trSysAdminUserPropertiesLock_Delete]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trSysAdminUserPropertiesLock_Delete]
ON [dbo].[tNAVUserProperties]
INSTEAD OF DELETE
AS 
BEGIN

	DECLARE @iUserID int
	DECLARE @SysAdminName nvarchar(25) = 'NAV System Administrator'

	SELECT @iUserID = INSERTED.iUserID FROM INSERTED WHERE INSERTED.nvPreferredName = @SysAdminName AND nvUserName = @SysAdminName

	IF (@iUserID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'The ' + @SysAdminName + ' account cannot be deleted', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVUserProperties] ENABLE TRIGGER [trSysAdminUserPropertiesLock_Delete]
GO
/****** Object:  Trigger [dbo].[trSysAdminUserPropertiesLock_Insert]    Script Date: 21/08/2020 5:20:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trSysAdminUserPropertiesLock_Insert]
ON [dbo].[tNAVUserProperties]
INSTEAD OF INSERT
AS 
BEGIN

	DECLARE @iUserID int
	DECLARE @SysAdminName nvarchar(25) = 'NAV System Administrator'

	SELECT @iUserID = INSERTED.iUserID FROM INSERTED WHERE INSERTED.nvPreferredName = @SysAdminName AND nvUserName = @SysAdminName

	IF (@iUserID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'A ' + @SysAdminName + ' account already exists', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVUserProperties] ENABLE TRIGGER [trSysAdminUserPropertiesLock_Insert]
GO
/****** Object:  Trigger [dbo].[trSysAdminUserPropertiesLock_Update]    Script Date: 21/08/2020 5:20:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trSysAdminUserPropertiesLock_Update]
ON [dbo].[tNAVUserProperties]
INSTEAD OF UPDATE
AS 
BEGIN

	DECLARE @iUserID int
	DECLARE @SysAdminName nvarchar(25) = 'NAV System Administrator'

	SELECT @iUserID = INSERTED.iUserID FROM INSERTED WHERE INSERTED.nvPreferredName = @SysAdminName AND nvUserName = @SysAdminName

	IF (@iUserID IS NOT NULL)
	BEGIN 

		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION

		RAISEERROR(SELECT 'The ' + @SysAdminName + ' account cannot be updated', 16,1)

	END 

END
GO
ALTER TABLE [dbo].[tNAVUserProperties] ENABLE TRIGGER [trSysAdminUserPropertiesLock_Update]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Informs the system that this abbreviation is always used to replace a word associated with this abbreviation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tAbbreviation', @level2type=N'COLUMN',@level2name=N'bAlwaysUse'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Set programmatically when the bLockoutEnabled is set to true' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tNAVUserProperties', @level2type=N'COLUMN',@level2name=N'dLockoutEndDateUTC'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tAbbreviation"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 268
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tAbbreviationType"
            Begin Extent = 
               Top = 201
               Left = 322
               Bottom = 314
               Right = 577
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tLanguageAbbreviationTypeJoin"
            Begin Extent = 
               Top = 134
               Left = 657
               Bottom = 230
               Right = 853
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vAbbreviationByLanguage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vAbbreviationByLanguage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tAbbreviation"
            Begin Extent = 
               Top = 82
               Left = 1055
               Bottom = 212
               Right = 1285
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "tAbbreviationType"
            Begin Extent = 
               Top = 108
               Left = 501
               Bottom = 221
               Right = 756
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tLanguageAbbreviationTypeJoin"
            Begin Extent = 
               Top = 140
               Left = 96
               Bottom = 236
               Right = 292
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 2100
         Width = 3420
         Width = 3750
         Width = 2865
         Width = 6225
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vAbbreviationByType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vAbbreviationByType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tAbbreviation"
            Begin Extent = 
               Top = 228
               Left = 72
               Bottom = 429
               Right = 302
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tLanguageAbbreviationTypeJoin"
            Begin Extent = 
               Top = 24
               Left = 357
               Bottom = 120
               Right = 553
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tNAVUserProperties"
            Begin Extent = 
               Top = 45
               Left = 674
               Bottom = 175
               Right = 879
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vAbbreviationFlagged'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vAbbreviationFlagged'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tAbbreviationType"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 119
               Right = 293
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tLanguageAbbreviationTypeJoin"
            Begin Extent = 
               Top = 94
               Left = 480
               Bottom = 190
               Right = 676
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 2505
         Width = 3555
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vDistinctAbbreviationTypesByLanguage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vDistinctAbbreviationTypesByLanguage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[11] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tCountry"
            Begin Extent = 
               Top = 152
               Left = 893
               Bottom = 295
               Right = 1070
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "tCountryLanguageJoin"
            Begin Extent = 
               Top = 187
               Left = 647
               Bottom = 283
               Right = 817
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tLanguage"
            Begin Extent = 
               Top = 140
               Left = 426
               Bottom = 247
               Right = 596
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 2235
         Width = 2625
         Width = 2535
         Width = 2700
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vLanguageByCountry'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vLanguageByCountry'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tCountry"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 215
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "tCountryLanguageJoin"
            Begin Extent = 
               Top = 217
               Left = 259
               Bottom = 313
               Right = 429
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tLanguage"
            Begin Extent = 
               Top = 75
               Left = 470
               Bottom = 171
               Right = 640
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tNAVUserProperties"
            Begin Extent = 
               Top = 37
               Left = 755
               Bottom = 351
               Right = 960
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tNAVClient"
            Begin Extent = 
               Top = 215
               Left = 1140
               Bottom = 346
               Right = 1310
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 22
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
    ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vLanguageByUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'     Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vLanguageByUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vLanguageByUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[28] 2[13] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tNAVClientPreferences"
            Begin Extent = 
               Top = 60
               Left = 477
               Bottom = 350
               Right = 732
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tNAVClientPreferenceType"
            Begin Extent = 
               Top = 119
               Left = 127
               Bottom = 278
               Right = 366
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tNAVUserPreferences"
            Begin Extent = 
               Top = 152
               Left = 821
               Bottom = 344
               Right = 1067
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 20
         Width = 284
         Width = 2100
         Width = 2010
         Width = 2820
         Width = 2715
         Width = 2925
         Width = 2625
         Width = 4590
         Width = 2625
         Width = 3120
         Width = 3435
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 5805
         Alias = 900
         Table = 5280
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or =' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vUserPreferencesByUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vUserPreferencesByUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vUserPreferencesByUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tAbbreviation"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 180
               Right = 268
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tLanguageAbbreviationTypeJoin"
            Begin Extent = 
               Top = 173
               Left = 405
               Bottom = 269
               Right = 601
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tNAVUserProperties"
            Begin Extent = 
               Top = 59
               Left = 900
               Bottom = 414
               Right = 1105
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tWordAbbreviationJoin"
            Begin Extent = 
               Top = 308
               Left = 294
               Bottom = 404
               Right = 465
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tWord"
            Begin Extent = 
               Top = 366
               Left = 549
               Bottom = 496
               Right = 719
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2580
         Width = 1500
         Width = 3795
         Width = 1500
         Width = 3855
         Width = 1500
         Width = 1500
      End
   End
   Begin C' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vWordAbbreviationByUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'riteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 2070
         Table = 3450
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vWordAbbreviationByUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vWordAbbreviationByUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[15] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tWordAbbreviationJoin"
            Begin Extent = 
               Top = 186
               Left = 219
               Bottom = 286
               Right = 390
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tAbbreviation"
            Begin Extent = 
               Top = 318
               Left = 418
               Bottom = 448
               Right = 648
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "tWord"
            Begin Extent = 
               Top = 184
               Left = 482
               Bottom = 297
               Right = 652
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tLanguageWordJoin"
            Begin Extent = 
               Top = 164
               Left = 768
               Bottom = 260
               Right = 994
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tCountryLanguageJoin"
            Begin Extent = 
               Top = 37
               Left = 304
               Bottom = 133
               Right = 474
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tCountry"
            Begin Extent = 
               Top = 38
               Left = 47
               Bottom = 168
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "tLanguage"
            Begin Extent = 
               Top = 53
               Left = 569
               Bottom = 149
               Right = 739
 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vWordAbbreviationsByCountry'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'           End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 15
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2355
         Width = 1500
         Width = 1500
         Width = 1980
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 1020
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vWordAbbreviationsByCountry'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vWordAbbreviationsByCountry'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tLanguageWordJoin"
            Begin Extent = 
               Top = 103
               Left = 1174
               Bottom = 199
               Right = 1344
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tWord"
            Begin Extent = 
               Top = 241
               Left = 795
               Bottom = 354
               Right = 965
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tLanguage"
            Begin Extent = 
               Top = 63
               Left = 732
               Bottom = 159
               Right = 902
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tCountryLanguageJoin"
            Begin Extent = 
               Top = 71
               Left = 384
               Bottom = 167
               Right = 554
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tCountry"
            Begin Extent = 
               Top = 56
               Left = 108
               Bottom = 186
               Right = 285
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tWordAbbreviationJoin"
            Begin Extent = 
               Top = 234
               Left = 447
               Bottom = 330
               Right = 618
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tAbbreviation"
            Begin Extent = 
               Top = 237
               Left = 111
               Bottom = 367
               Right = 341' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vWordByLanguage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 14
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2355
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vWordByLanguage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vWordByLanguage'
GO
USE [master]
GO
ALTER DATABASE [NAV] SET  READ_WRITE 
GO
