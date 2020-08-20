namespace NAVService
{
    internal class Constants
    {
        // global constants

        internal const int ACTION_PROGRESS_ESTIMATE = 15000;
        internal const int ACTION_PROGRESS_WARNING = ACTION_PROGRESS_ESTIMATE / 3;
        internal const int ACTION_PROGRESS_DETAIL = ACTION_PROGRESS_ESTIMATE * 3;
        internal const int ACTION_INITIALISE_DISPLAY = 500;
        internal const int ACTION_ABBREVIATION_ESTIMATE = 120;
        internal const int ACTION_PARSE = 1;
        internal const int ACTION_REMOVE = 2;

        internal const int INVALID_USER_ID = -9000;
        internal const int PARSE_WORKSHEET_ERROR = -9001;
        internal const int NETWORK_UNAVAILABLE_ERROR = -9002;

        internal const string METHOD_RATCLIFF_OBERSHELP = "Ratcliff Obershelp";
        internal const string METHOD_LEVENSHTEIN_DISTANCE = "Levenshtein Distance";
        internal const string METHOD_HAMMING_DISTANCE = "Hamming Distance";
        internal const int METHOD_RATCLIFF_OBERSHELP_VALUE = 0;
        internal const int METHOD_LEVENSHTEIN_DISTANCE_VALUE = 1;
        internal const int METHOD_HAMMING_DISTANCE_VALUE = 2;

        internal const string COLUMN_ROW_ID = "iRowID";
        internal const string COLUMN_DATA = "nvExplorerString";
        internal const string COLUMN_DELETE_FLAG = "bDeleteRowFlag";
        internal const string COLUMN_PARENT_ID = "iParentRowID";
        internal const string COLUMN_COMPARISON = "iCompareValue";
        internal const string COLUMN_MATCHTYPE = "nvMatchType";

        internal const string CONTROL_RESTOREBUTTON_SYMBOL = "RestoreDefaultsButton";
        internal const string CONTROL_PREFERENCEHEADER_SYMBOL = "preferenceDescriptionHeader";
        internal const string CONTROL_PREFERENCEDESCRIPTION_SYMBOL = "preferenceDescription";
        internal const string CONTROL_DIVIDER_SYMBOL = "navDivider";
        internal const string CONTROL_PREFERENCE_SYMBOL = "navPreference";
        internal const string CONTROL_VALUE_SYMBOL = "navValue";
        internal const string CONTROL_PARSEBUTTON_SYMBOL = "parseButton";
        internal const string CONTROL_DELETEBUTTON_SYMBOL = "deleteButton";

        internal const string COLUMN_ABBREVIATION_ID = "iAbbreviationID";
        internal const string COLUMN_ABBREVIATIONTYPE_ID = "iAbbreviationTypeID";
        internal const string COLUMN_ABBREVIATIONWORD_ID = "iWordID";
        internal const string COLUMN_ABBREVIATION_LANGUAGE_ID = "iLanguageID";
        internal const string COLUMN_ABBREVIATION_WORD = "nvWord";
        internal const string COLUMN_ABBREVIATION = "nvAbbreviation";
        internal const string COLUMN_ABBREVIATION_TYPE = "nvAbbreviationType";
        internal const string COLUMN_ABBREVIATION_DESCRIPTION = "nvAbbreviationDescription";
        internal const string COLUMN_ABBREVIATION_FLAG = "bAlwaysUse";
        internal const string COLUMN_ABBREVIATION_RETURNCODE = "iReturnCode";

        internal const string COLUMN_CLIENT_PREFERENCE_TYPE_ID = "iClientPreferenceTypeID";
        internal const string COLUMN_CLIENT_PREFERENCE_TYPE = "nvClientPreferenceType";
        internal const string COLUMN_CLIENT_PREFERENCE_NAME = "nvClientPreferenceName";
        internal const string COLUMN_CLIENT_PREFERENCE_VALUE = "nvClientPreferenceValue";
        internal const string COLUMN_CLIENT_PREFERENCE_DESCRIPTION = "nvClientPreferenceDescription";
        internal const string COLUMN_CLIENT_VALUE_REQUIRED_FLAG = "bClientValueRequired";
        internal const string COLUMN_CLIENT_OVERRIDE_FLAG = "bClientOverride";
        internal const string COLUMN_CLIENT_PREFERENCE_FLAG = "bClientPreference";

        internal const string COLUMN_USER_ID = "iUserID";
        internal const string COLUMN_USER_PREFERENCE_ID = "iUserPreferenceID";
        internal const string COLUMN_USER_PREFERENCE_VALUE = "nvUserPreferenceValue";
        internal const string COLUMN_USER_PREFERENCE_FLAG = "bUserPreference";
        internal const string COLUMN_USER_LAST_FILENAME_OPENED = "nvLastFilenameOpened";
        internal const string COLUMN_USER_LAST_TAB_FOCUS = "nvLastTabFocus";
        internal const string COLUMN_USER_LAST_TABLE_FOCUS = "nvLastTableFocus";
        internal const string COLUMN_USER_BUILD_VERSION = "nvBuildVersion";
        
        internal const string COLUMN_SYSTEM_OVERRIDE_FLAG = "bSystemOverride";
        internal const string NOT_IMPLEMENTED = "Not implemented";

        internal const string MATCH_PRINCIPLE = "PRINCIPLE";
        internal const string MATCH_ASSOCIATE = "ASSOCIATE";

        internal const string FILE_XLSX_EXTENSION = ".XLSX";
        internal const string FILE_XLS_EXTENSION = ".XLS";
        internal const string FILE_CSV_EXTENSION = ".CSV";
        internal const string FILE_SAVEFILE_EXTENSION = "xlsx";

        internal const string DB_SLIDE_PANEL_TAB_VIEWS = "Slide Panel Tab Views";
        internal const string DB_HIDE_ABBREVIATIONS = "Hide Abbreviations";
        internal const string DB_HIDE_EXPLORER = "Hide Explorer";
        internal const string DB_OPEN_LAST_TAB = "Open last tab";

        internal const string DB_IMPORT_FILE_PERMISSIONS = "Import File Permissions";
        internal const string DB_ORDER_COLUMNS = "Order columns";
        internal const string DB_DELETE_ROWS = "Delete rows";
        internal const string DB_EDIT_CELLS = "Edit cells";
        internal const string DB_CREATE_ROWS = "Create rows";
        internal const string DB_OPEN_LAST_FILE = "Open last file";
        internal const string DB_OPEN_LAST_WORKSHEET = "Open last worksheet";

        internal const string DB_ABBREVIATION_TAB_PERMISSIONS = "Abbreviation Tab Permissions";
        internal const string DB_REPLACE_WORDS = "Replace words";
        internal const string DB_PERMIT_DEFAULTS = "Permit defaults only";
        internal const string DB_DELETE_ABBREVIATIONS = "Delete abbreviations";
        internal const string DB_ADD_NEW_ABBREVIATIONS = "Add new abbreviations";
        internal const string DB_EDIT_ABBREVIATIONS = "Edit abbreviations";

        internal const string DB_ERROR_LOG_PREFERENCES = "Error Log Preferences";
        internal const string DB_LOG_ERRORS = "Log errors";
        internal const string DB_LOG_ERRORS_TO_FILE = "Log errors to file";
        internal const string DB_ERROR_LOG_FILE_LOCATION = "Error log file location";
        internal const string DB_ERROR_LOG_MAXIMUM_SIZE = "Error log Maximum size";
        internal const string DB_ERROR_LOG_ARCHIVE_TOTAL = "Error log archive total";

        internal const string DB_DATA_CONNECTION_ATTRIBUTES = "Data Connection Attributes";
        internal const string DB_CLOUD_CONNECTION_STRING = "Cloud connection string";
        internal const string DB_LOCAL_CONNECTION_STRING = "Local connection string";

        internal const string DB_USER_INTERFACE_THEMES = "User Interface Themes";
        internal const string DB_COLOUR_SCHEME = "Colour scheme";
        internal const string DB_INTERFACE_LANGUAGE = "Interface language";
        internal const string DB_APPLY_RTL_ORIENTATION = "Apply RTL orientation";

        internal const string DB_SEARCH_PREFERENCES = "Search Preferences";
        internal const string DB_PULL_ABBREVIATIONS = "Pull Abbreviations";
        internal const string DB_REMOVE_NOISE_CHARACTERS = "Remove noise characters";
        internal const string DB_APPLY_CASE_INSENSITIVITY = "Apply case insensitivity";
        internal const string DB_PAD_TEXT = "Pad text";
        internal const string DB_MATCH_ABBREVIATIONS = "Match abbreviations";
        internal const string DB_REVERSE_COMPARE = "Reverse compare";
        internal const string DB_PHONETIC_FILTER = "Phonetic filter";
        internal const string DB_WHOLE_WORD_MATCH = "Whole word match";
        internal const string DB_PERCENTAGE_INTEREST = "Percentage interest";
        internal const string DB_MATCHING_ALGORITHM = "Matching algorithm";

        internal const int CONNECTION_RETRY_LIMIT = 5;
        internal const int CONNECTION_DEV = 210919428;
        internal const int CONNECTION_PREPROD = 876305086;

        internal const int CONNECTION_PROD_NAV = 799297213;
        internal const int CONNECTION_PROD_AWS = 260345997;
        internal const int CONNECTION_UPDATE_LOCAL = 638847585;
        internal const int CONNECTION_UPDATE_CLOUD = 133872841;
        internal const string CONNECTION_LOCAL = "NAVDatabase";
        internal const string CONNECTION_CLOUD = "AWSDatabase";
        internal const string CONNECTION_LOCAL_UPDATE = "LocalUpdate";
        internal const string CONNECTION_CLOUD_UPDATE = "CloudUpdate";

#if DEBUG
        // ENABLE preprod update location and dev database connection string
        internal const string CONNECTION_PREPROD_UPDATE_DEFAULT = @"C:\NAVServices\Releases\";
        internal const string CONNECTION_PREPROD_TARGET_DEFAULT = "Server=.;Database=NAV;Trusted_Connection=True;";
#else
        // DISABLE preprod update location and dev database connection string
        internal const string CONNECTION_PREPROD_UPDATE_DEFAULT = null;
        internal const string CONNECTION_PREPROD_TARGET_DEFAULT = "Server=.;Database=NULL;";
#endif

        // runtime constant readonly variables
        internal static readonly string KEY_COLUMN = ClassLibraryStandard.HelperMethods.GetMilliseconds(true).ToString(UserHelper.culture);
        internal static readonly string BUILD_VERSION = $"v" + ConnectionHelper.AssemblyBuildVersion() + "b";

        internal static readonly System.Drawing.Color COLOR_MATCH_ASSOCIATE = System.Drawing.SystemColors.InactiveBorder;
        internal static readonly System.Drawing.Color COLOR_MATCH_ASSOCIATE_DELETE = System.Drawing.Color.FromArgb(252, 249, 244);
        internal static readonly System.Drawing.Color COLOR_MATCH_PRINCIPLE = System.Drawing.Color.White;
        internal static readonly System.Drawing.Color COLOR_MATCH_PRINCIPLE_DELETE = System.Drawing.Color.FromArgb(252, 249, 244);
        internal static readonly System.Drawing.Color COLOR_DEFAULT_BACKCOLOR = System.Drawing.Color.FromArgb(255, 255, 252);
        internal static readonly System.Drawing.Color COLOR_DEFAULT_WINDOW = System.Drawing.SystemColors.Window;
        internal static readonly System.Drawing.Color COLOR_DEFAULT_TEXT = System.Drawing.SystemColors.WindowText;
        internal static readonly System.Drawing.Color COLOR_DEFAULT = System.Drawing.Color.White;
        internal static readonly System.Drawing.Color COLOR_TEXT = System.Drawing.SystemColors.ControlDarkDark;

        /// <summary>
        ///     Disabled and redundant user defined attributes
        ///     <para>
        ///         Retained for conformity and so that the calling syntax is not lost
        ///     </para>
        /// </summary>

        // Data Connection Attributes (disabled - redundant)
        //internal static readonly string CloudConnectionString = DataAccess.GetUserPreferenceByPreferenceName(DB_CLOUD_CONNECTION_STRING);
        //internal static readonly string LocalConnectionString = DataAccess.GetUserPreferenceByPreferenceName(DB_LOCAL_CONNECTION_STRING);

        // User Interface Themes (disabled - to do, maybe)
        //internal static readonly string ColourScheme = DataAccess.GetUserPreferenceByPreferenceName(DB_COLOUR_SCHEME);
        //internal static readonly string InterfaceLanguage = DataAccess.GetUserPreferenceByPreferenceName(DB_INTERFACE_LANGUAGE);
        //internal static readonly bool ApplyRTLOrientation = ClassLibraryStandard.GenericHelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(DB_APPLY_RTL_ORIENTATION));

        // Error Log Attributes (disabled - enable, maybe)
        //internal static readonly string ErrorLogArchiveTotal = DataAccess.GetUserPreferenceByPreferenceName(DB_ERROR_LOG_ARCHIVE_TOTAL);
        //internal static readonly string ErrorLogFileLocation = DataAccess.GetUserPreferenceByPreferenceName(DB_ERROR_LOG_FILE_LOCATION);
        //internal static readonly string ErrorLogMaximumSize = DataAccess.GetUserPreferenceByPreferenceName(DB_ERROR_LOG_MAXIMUM_SIZE);
        //internal static readonly bool LogErrorsToFile = ClassLibraryStandard.GenericHelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(DB_LOG_ERRORS_TO_FILE));

    }

}
