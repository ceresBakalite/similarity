USE [NAV]
GO
/****** Object:  StoredProcedure [dbo].[pCreateLanguageFromTemplate]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pDropLanguage]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pDropWordByID]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pEditAbbreviation]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetAbbreviationByUserID]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetAbbreviationType]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetAbbreviationTypeByLanguage]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetLastUserStateLogEntry]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetPhraseAbbreviationsByUserID]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetStringAbbreviations]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetUserPreferenceByPreferenceName]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetUserPreferenceDictionaryModel]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetUserPreferencesByPreferenceType]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetUserPreferencesByUserID]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetUserProperties]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pGetWordAbbreviationsByLanguageByType]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pInsertAbbreviation]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pInsertErrorLogEntry]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pInsertUserStateLogEntry]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pSetStringAbbreviations]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pTestCreateLanguageFromTemplate]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pUpdateAbbreviation]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pUpdateUserPreference]    Script Date: 15/08/2020 10:51:28 PM ******/
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
/****** Object:  StoredProcedure [dbo].[pUserExists]    Script Date: 15/08/2020 10:51:28 PM ******/
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
