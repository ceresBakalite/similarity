namespace NAVService
{
    internal class Constants
    {
        // global constants

        protected internal const int ACTION_PROGRESS_ESTIMATE = 15000;
        protected internal const int ACTION_PROGRESS_WARNING = ACTION_PROGRESS_ESTIMATE / 3;
        protected internal const int ACTION_PROGRESS_DETAIL = ACTION_PROGRESS_ESTIMATE * 3;
        protected internal const int ACTION_INITIALISE_DISPLAY = 500;
        protected internal const int ACTION_ABBREVIATION_ESTIMATE = 120;
        protected internal const int ACTION_PARSE = 1;
        protected internal const int ACTION_REMOVE = 2;

        protected internal const int INVALID_USER_ID = -9000;
        protected internal const int PARSE_WORKSHEET_ERROR = -9001;
        protected internal const int NETWORK_UNAVAILABLE_ERROR = -9002;

        protected internal const string METHOD_RATCLIFF_OBERSHELP = "Ratcliff Obershelp";
        protected internal const string METHOD_LEVENSHTEIN_DISTANCE = "Levenshtein Distance";
        protected internal const string METHOD_HAMMING_DISTANCE = "Hamming Distance";
        protected internal const int METHOD_RATCLIFF_OBERSHELP_VALUE = 0;
        protected internal const int METHOD_LEVENSHTEIN_DISTANCE_VALUE = 1;
        protected internal const int METHOD_HAMMING_DISTANCE_VALUE = 2;

        protected internal const string COLUMN_ROW_ID = "iRowID";
        protected internal const string COLUMN_DATA = "nvExplorerString";
        protected internal const string COLUMN_DELETE_FLAG = "bDeleteRowFlag";
        protected internal const string COLUMN_PARENT_ID = "iParentRowID";
        protected internal const string COLUMN_COMPARISON = "iCompareValue";
        protected internal const string COLUMN_MATCHTYPE = "nvMatchType";

        protected internal const string CONTROL_RESTOREBUTTON_SYMBOL = "RestoreDefaultsButton";
        protected internal const string CONTROL_PREFERENCEHEADER_SYMBOL = "preferenceDescriptionHeader";
        protected internal const string CONTROL_PREFERENCEDESCRIPTION_SYMBOL = "preferenceDescription";
        protected internal const string CONTROL_DIVIDER_SYMBOL = "navDivider";
        protected internal const string CONTROL_PREFERENCE_SYMBOL = "navPreference";
        protected internal const string CONTROL_VALUE_SYMBOL = "navValue";
        protected internal const string CONTROL_PARSEBUTTON_SYMBOL = "parseButton";
        protected internal const string CONTROL_DELETEBUTTON_SYMBOL = "deleteButton";

        protected internal const string COLUMN_ABBREVIATION_ID = "iAbbreviationID";
        protected internal const string COLUMN_ABBREVIATIONTYPE_ID = "iAbbreviationTypeID";
        protected internal const string COLUMN_ABBREVIATIONWORD_ID = "iWordID";
        protected internal const string COLUMN_ABBREVIATION_LANGUAGE_ID = "iLanguageID";
        protected internal const string COLUMN_ABBREVIATION_WORD = "nvWord";
        protected internal const string COLUMN_ABBREVIATION = "nvAbbreviation";
        protected internal const string COLUMN_ABBREVIATION_TYPE = "nvAbbreviationType";
        protected internal const string COLUMN_ABBREVIATION_DESCRIPTION = "nvAbbreviationDescription";
        protected internal const string COLUMN_ABBREVIATION_FLAG = "bAlwaysUse";
        protected internal const string COLUMN_ABBREVIATION_RETURNCODE = "iReturnCode";

        protected internal const string COLUMN_CLIENT_PREFERENCE_TYPE_ID = "iClientPreferenceTypeID";
        protected internal const string COLUMN_CLIENT_PREFERENCE_TYPE = "nvClientPreferenceType";
        protected internal const string COLUMN_CLIENT_PREFERENCE_NAME = "nvClientPreferenceName";
        protected internal const string COLUMN_CLIENT_PREFERENCE_VALUE = "nvClientPreferenceValue";
        protected internal const string COLUMN_CLIENT_PREFERENCE_DESCRIPTION = "nvClientPreferenceDescription";
        protected internal const string COLUMN_CLIENT_VALUE_REQUIRED_FLAG = "bClientValueRequired";
        protected internal const string COLUMN_CLIENT_OVERRIDE_FLAG = "bClientOverride";
        protected internal const string COLUMN_CLIENT_PREFERENCE_FLAG = "bClientPreference";

        protected internal const string COLUMN_USER_ID = "iUserID";
        protected internal const string COLUMN_USER_PREFERENCE_ID = "iUserPreferenceID";
        protected internal const string COLUMN_USER_PREFERENCE_VALUE = "nvUserPreferenceValue";
        protected internal const string COLUMN_USER_PREFERENCE_FLAG = "bUserPreference";
        protected internal const string COLUMN_USER_LAST_FILENAME_OPENED = "nvLastFilenameOpened";
        protected internal const string COLUMN_USER_LAST_TAB_FOCUS = "nvLastTabFocus";
        protected internal const string COLUMN_USER_LAST_TABLE_FOCUS = "nvLastTableFocus";
        protected internal const string COLUMN_USER_BUILD_VERSION = "nvBuildVersion";
        
        protected internal const string COLUMN_SYSTEM_OVERRIDE_FLAG = "bSystemOverride";
        protected internal const string NOT_IMPLEMENTED = "Not implemented";

        protected internal const string MATCH_PRINCIPLE = "PRINCIPLE";
        protected internal const string MATCH_ASSOCIATE = "ASSOCIATE";

        protected internal const string FILE_XLSX_EXTENSION = ".XLSX";
        protected internal const string FILE_XLS_EXTENSION = ".XLS";
        protected internal const string FILE_CSV_EXTENSION = ".CSV";
        protected internal const string FILE_SAVEFILE_EXTENSION = "xlsx";

        protected internal const string DB_SLIDE_PANEL_TAB_VIEWS = "Slide Panel Tab Views";
        protected internal const string DB_HIDE_ABBREVIATIONS = "Hide Abbreviations";
        protected internal const string DB_HIDE_EXPLORER = "Hide Explorer";
        protected internal const string DB_OPEN_LAST_TAB = "Open last tab";

        protected internal const string DB_IMPORT_FILE_PERMISSIONS = "Import File Permissions";
        protected internal const string DB_ORDER_COLUMNS = "Order columns";
        protected internal const string DB_DELETE_ROWS = "Delete rows";
        protected internal const string DB_EDIT_CELLS = "Edit cells";
        protected internal const string DB_CREATE_ROWS = "Create rows";
        protected internal const string DB_OPEN_LAST_FILE = "Open last file";
        protected internal const string DB_OPEN_LAST_WORKSHEET = "Open last worksheet";

        protected internal const string DB_ABBREVIATION_TAB_PERMISSIONS = "Abbreviation Tab Permissions";
        protected internal const string DB_REPLACE_WORDS = "Replace words";
        protected internal const string DB_PERMIT_DEFAULTS = "Permit defaults only";
        protected internal const string DB_DELETE_ABBREVIATIONS = "Delete abbreviations";
        protected internal const string DB_ADD_NEW_ABBREVIATIONS = "Add new abbreviations";
        protected internal const string DB_EDIT_ABBREVIATIONS = "Edit abbreviations";

        protected internal const string DB_ERROR_LOG_PREFERENCES = "Error Log Preferences";
        protected internal const string DB_LOG_ERRORS = "Log errors";
        protected internal const string DB_LOG_ERRORS_TO_FILE = "Log errors to file";
        protected internal const string DB_ERROR_LOG_FILE_LOCATION = "Error log file location";
        protected internal const string DB_ERROR_LOG_MAXIMUM_SIZE = "Error log Maximum size";
        protected internal const string DB_ERROR_LOG_ARCHIVE_TOTAL = "Error log archive total";

        protected internal const string DB_DATA_CONNECTION_ATTRIBUTES = "Data Connection Attributes";
        protected internal const string DB_CLOUD_CONNECTION_STRING = "Cloud connection string";
        protected internal const string DB_LOCAL_CONNECTION_STRING = "Local connection string";

        protected internal const string DB_USER_INTERFACE_THEMES = "User Interface Themes";
        protected internal const string DB_COLOUR_SCHEME = "Colour scheme";
        protected internal const string DB_INTERFACE_LANGUAGE = "Interface language";
        protected internal const string DB_APPLY_RTL_ORIENTATION = "Apply RTL orientation";

        protected internal const string DB_SEARCH_PREFERENCES = "Search Preferences";
        protected internal const string DB_PULL_ABBREVIATIONS = "Pull Abbreviations";
        protected internal const string DB_REMOVE_NOISE_CHARACTERS = "Remove noise characters";
        protected internal const string DB_APPLY_CASE_INSENSITIVITY = "Apply case insensitivity";
        protected internal const string DB_PAD_TEXT = "Pad text";
        protected internal const string DB_MATCH_ABBREVIATIONS = "Match abbreviations";
        protected internal const string DB_REVERSE_COMPARE = "Reverse compare";
        protected internal const string DB_PHONETIC_FILTER = "Phonetic filter";
        protected internal const string DB_WHOLE_WORD_MATCH = "Whole word match";
        protected internal const string DB_PERCENTAGE_INTEREST = "Percentage interest";
        protected internal const string DB_MATCHING_ALGORITHM = "Matching algorithm";

        protected internal const int CONNECTION_RETRY_LIMIT = 5;
        protected internal const int CONNECTION_DEV = 210919428;
        protected internal const int CONNECTION_PREPROD = 876305086;

        protected internal const int CONNECTION_PROD_NAV = 799297213;
        protected internal const int CONNECTION_PROD_AWS = 260345997;
        protected internal const int CONNECTION_UPDATE_LOCAL = 638847585;
        protected internal const int CONNECTION_UPDATE_CLOUD = 133872841;
        protected internal const string CONNECTION_LOCAL = "NAVDatabase";
        protected internal const string CONNECTION_CLOUD = "AWSDatabase";
        protected internal const string CONNECTION_LOCAL_UPDATE = "LocalUpdate";
        protected internal const string CONNECTION_CLOUD_UPDATE = "CloudUpdate";

#if DEBUG
        // ENABLE preprod update location and dev database connection string
        protected internal const string CONNECTION_PREPROD_UPDATE_DEFAULT = @"C:\NAVServices\Releases\";
        protected internal const string CONNECTION_PREPROD_TARGET_DEFAULT = "Server=.;Database=NAV;Trusted_Connection=True;";
#else
        // DISABLE preprod update location and dev database connection string
        protected internal const string CONNECTION_PREPROD_UPDATE_DEFAULT = null;
        protected internal const string CONNECTION_PREPROD_TARGET_DEFAULT = "Server=.;Database=NULL;";
#endif

        // runtime constant readonly variables
        protected internal static readonly string KEY_COLUMN = ClassLibraryStandard.HelperMethods.GetMilliseconds(true).ToString(UserHelper.culture);
        protected internal static readonly string BUILD_VERSION = $"v" + ConnectionHelper.AssemblyBuildVersion() + "b";

        protected internal static readonly System.Drawing.Color COLOR_MATCH_ASSOCIATE = System.Drawing.SystemColors.InactiveBorder;
        protected internal static readonly System.Drawing.Color COLOR_MATCH_ASSOCIATE_DELETE = System.Drawing.Color.FromArgb(252, 249, 244);
        protected internal static readonly System.Drawing.Color COLOR_MATCH_PRINCIPLE = System.Drawing.Color.White;
        protected internal static readonly System.Drawing.Color COLOR_MATCH_PRINCIPLE_DELETE = System.Drawing.Color.FromArgb(252, 249, 244);
        protected internal static readonly System.Drawing.Color COLOR_DEFAULT_BACKCOLOR = System.Drawing.Color.FromArgb(255, 255, 252);
        protected internal static readonly System.Drawing.Color COLOR_DEFAULT_WINDOW = System.Drawing.SystemColors.Window;
        protected internal static readonly System.Drawing.Color COLOR_DEFAULT_TEXT = System.Drawing.SystemColors.WindowText;
        protected internal static readonly System.Drawing.Color COLOR_DEFAULT = System.Drawing.Color.White;
        protected internal static readonly System.Drawing.Color COLOR_TEXT = System.Drawing.SystemColors.ControlDarkDark;

        /// <summary>
        ///     Disabled and redundant user defined attributes
        ///     <para>
        ///         Retained for conformity and so that the calling syntax is not lost
        ///     </para>
        /// </summary>

        // Data Connection Attributes (disabled - redundant)
        //protected internal static readonly string CloudConnectionString = DataAccess.GetUserPreferenceByPreferenceName(DB_CLOUD_CONNECTION_STRING);
        //protected internal static readonly string LocalConnectionString = DataAccess.GetUserPreferenceByPreferenceName(DB_LOCAL_CONNECTION_STRING);

        // User Interface Themes (disabled - to do, maybe)
        //protected internal static readonly string ColourScheme = DataAccess.GetUserPreferenceByPreferenceName(DB_COLOUR_SCHEME);
        //protected internal static readonly string InterfaceLanguage = DataAccess.GetUserPreferenceByPreferenceName(DB_INTERFACE_LANGUAGE);
        //protected internal static readonly bool ApplyRTLOrientation = ClassLibraryStandard.GenericHelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(DB_APPLY_RTL_ORIENTATION));

        // Error Log Attributes (disabled - enable, maybe)
        //protected internal static readonly string ErrorLogArchiveTotal = DataAccess.GetUserPreferenceByPreferenceName(DB_ERROR_LOG_ARCHIVE_TOTAL);
        //protected internal static readonly string ErrorLogFileLocation = DataAccess.GetUserPreferenceByPreferenceName(DB_ERROR_LOG_FILE_LOCATION);
        //protected internal static readonly string ErrorLogMaximumSize = DataAccess.GetUserPreferenceByPreferenceName(DB_ERROR_LOG_MAXIMUM_SIZE);
        //protected internal static readonly bool LogErrorsToFile = ClassLibraryStandard.GenericHelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(DB_LOG_ERRORS_TO_FILE));

    }

}
