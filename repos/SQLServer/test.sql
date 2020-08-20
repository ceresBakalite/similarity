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
