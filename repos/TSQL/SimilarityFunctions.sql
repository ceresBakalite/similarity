USE [NAV]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GetAbbreviationByUserID]    Script Date: 15/08/2020 10:54:53 PM ******/
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
/****** Object:  UserDefinedFunction [dbo].[fn_GetSentenceAbbreviation]    Script Date: 15/08/2020 10:54:53 PM ******/
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
/****** Object:  UserDefinedFunction [dbo].[fn_RegexSimpleStringMethod]    Script Date: 15/08/2020 10:54:53 PM ******/
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
/****** Object:  UserDefinedFunction [dbo].[fn_RemoveNonAlphanumericChars]    Script Date: 15/08/2020 10:54:53 PM ******/
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
/****** Object:  UserDefinedFunction [dbo].[fn_SetStringAbbreviations]    Script Date: 15/08/2020 10:54:53 PM ******/
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
