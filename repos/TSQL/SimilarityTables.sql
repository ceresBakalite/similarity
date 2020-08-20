USE [NAV]
GO
/****** Object:  Table [dbo].[tAbbreviation]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tAbbreviationType]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tCountry]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tCountryLanguageJoin]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tLanguage]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tLanguageAbbreviationTypeJoin]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tLanguageWordJoin]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tLanguageWordTypeJoin]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tNAVClient]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tNAVClientPreferences]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tNAVClientPreferenceType]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tNAVLog]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tNAVSysAdmin]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tNAVSysAdminClientJoin]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tNAVUserPreferences]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tNAVUserProperties]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tNAVUserStateLogEntry]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tWord]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tWordAbbreviationJoin]    Script Date: 15/08/2020 10:53:54 PM ******/
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
/****** Object:  Table [dbo].[tWordType]    Script Date: 15/08/2020 10:53:54 PM ******/
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Informs the system that this abbreviation is always used to replace a word associated with this abbreviation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tAbbreviation', @level2type=N'COLUMN',@level2name=N'bAlwaysUse'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Set programmatically when the bLockoutEnabled is set to true' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tNAVUserProperties', @level2type=N'COLUMN',@level2name=N'dLockoutEndDateUTC'
GO
