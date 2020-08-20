USE [NAV]
GO
/****** Object:  View [dbo].[vAbbreviationByLanguage]    Script Date: 15/08/2020 10:52:42 PM ******/
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
/****** Object:  View [dbo].[vAbbreviationByType]    Script Date: 15/08/2020 10:52:42 PM ******/
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
/****** Object:  View [dbo].[vAbbreviationFlagged]    Script Date: 15/08/2020 10:52:42 PM ******/
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
/****** Object:  View [dbo].[vDistinctAbbreviationTypesByLanguage]    Script Date: 15/08/2020 10:52:42 PM ******/
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
/****** Object:  View [dbo].[vLanguageByCountry]    Script Date: 15/08/2020 10:52:42 PM ******/
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
/****** Object:  View [dbo].[vLanguageByUser]    Script Date: 15/08/2020 10:52:42 PM ******/
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
/****** Object:  View [dbo].[vUserPreferencesByUserID]    Script Date: 15/08/2020 10:52:42 PM ******/
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
/****** Object:  View [dbo].[vWordAbbreviationByUserID]    Script Date: 15/08/2020 10:52:42 PM ******/
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
/****** Object:  View [dbo].[vWordAbbreviationsByCountry]    Script Date: 15/08/2020 10:52:42 PM ******/
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
/****** Object:  View [dbo].[vWordByLanguage]    Script Date: 15/08/2020 10:52:42 PM ******/
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
