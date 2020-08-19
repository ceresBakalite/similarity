namespace NAVService
{

#pragma warning disable IDE1006 // for ease of maintenance the camelCase naming convention emulates their TSQL camelCase column name equivalents

    public class NAVAbbreviationTypeModel
    {
        public int iAbbreviationTypeID { get; set; }
        public string nvAbbreviationType { get; set; }
        public string nvAbbreviationTypeDescription { get; set; }
    }

    public class NAVAbbreviationModel
    {
        public int iAbbreviationID { get; set; }
        public int iAbbreviationTypeID { get; set; }
        public int iWordID { get; set; }
        public string nvWord { get; set; }
        public string nvAbbreviation { get; set; }
        public string nvAbbreviationDescription { get; set; }
        public int bAlwaysUse { get; set; }
        public int iReturnCode { get; }
    }

    public class NAVAbbreviationsModel
    {
        public string nvWord { get; }
        public int iWordLength { get; }
        public string nvAbbreviation { get;}
        public int iAbbreviationLength { get; }
        public int iReturnCode { get; }
    }

    public class NAVUserPropertiesModel
    {
        public int iUserID { get; set; }
        public int iCountryID { get; set; }
        public string nvCountryName { get; set; }
        public string nAlphaCode2 { get; set; }
        public string nAlphaCode3 { get; set; }
        public string nNumeric { get; set; }
        public int iLanguageID { get; set; }
        public string nvLanguage { get; set; }
        public int iClientID { get; set; }
        public string nvClientName { get; set; }
        public int bRegistered { get; set; }
        public string nvEmailAddress { get; set; }
        public int bEmailConfirmed { get; set; }
        public System.DateTime dLockoutEndDateUTC { get; set; }
        public int bLockoutEnabled{ get; set; }
        public int iAccessFailedCount { get; set; }
        public string nvPreferredName { get; set; }
        public string nvUserName { get; set; }
        public int iReturnCode { get; }
    }

    public class NAVUserPreferencesModel
    {
        public int iClientPreferenceID { get; set; }
        public int iUserPreferenceID { get; set; }
        public int iClientPreferenceTypeID { get; set; }
        public string nvClientPreferenceType { get; set; }
        public string nvClientPreferenceName { get; set; }
        public string nvClientPreferenceDescription { get; set; }
        public int bClientPreference { get; set; }
        public int bUserPreference { get; set; }
        public int bClientOverride { get; set; }
        public int bSystemOverride { get; set; }
        public int bClientValueRequired { get; set; }
        public string nvClientPreferenceValue { get; set; }
        public string nvUserPreferenceValue { get; set; }
    }

    public class NAVChangePreferencesModel
    {
        public int iPreferenceID { get; set; }
        public int iClientPreferenceTypeID { get; set; }
        public string nvClientPreferenceType { get; set; }
        public string nvClientPreferenceName { get; set; }
        public string nvPreferenceValue { get; set; }
        public bool bPreference { get; set; }
        public bool bClientValueRequired { get; set; }
        public bool bClientOverride { get; set; }
        public bool bSystemOverride { get; set; }
    }

    public class NAVUserStateModel
    {
        public string nvLastFilenameOpened { get; set; }
        public string nvLastTabFocus { get; set; }
        public string nvLastTableFocus { get; set; }
        public bool bConfirmed { get; }
        public int iReturnCode { get; }
    }

    public class NAVComparisonStringModel
    {
        public string nvString { get; set; }
        public int iReturnCode { get; }
    }

#pragma warning restore IDE1006 // for conformity the naming convention returns to C# PascalCase

}