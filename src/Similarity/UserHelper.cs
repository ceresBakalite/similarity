namespace NAVService
{
    public static class UserHelper
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();

        public static readonly System.DateTime ApplicationStartTime = System.DateTime.Now;
        public static readonly System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;
        public static readonly NAVUserPropertiesModel UserPropertiesModel = DataAccess.InitialiseUserProperties(InitaliseEnvironment());

        private static bool ReplaceAllWords = ClassLibraryStandard.GenericHelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_REPLACE_WORDS));
        private static bool ReplaceDefaultWordsOnly = ClassLibraryStandard.GenericHelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_PERMIT_DEFAULTS));
        private static bool PullAbbreviations = ClassLibraryStandard.GenericHelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_PULL_ABBREVIATIONS));
        private static bool LogErrors = ClassLibraryStandard.GenericHelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_LOG_ERRORS));

        public static void SetReplaceAllWords(bool value) => ReplaceAllWords = value;
        public static void SetReplaceDefaultWordsOnly(bool value) => ReplaceDefaultWordsOnly = value;
        public static void SetPullAbbreviations(bool value) => PullAbbreviations = value;
        public static void SetLogErrors(bool value) => LogErrors = value;

        public static bool GetReplaceAllWords() { return ReplaceAllWords; }
        public static bool GetReplaceDefaultWordsOnly() { return ReplaceDefaultWordsOnly; }
        public static bool GetPullAbbreviations() { return PullAbbreviations; }
        public static bool GetLogErrors() { return LogErrors; }

        public static NAVUserStateModel UserStateModel { get; set; }

        // assign a user to their environment attributes and dispose of system sensitive data
        private static int InitaliseEnvironment()
        {
            LogHelper.TraceWriteLine("TRACE - Confirming connection out");

            if (ClassLibraryStandard.InternetInteropServices.InternetConnectedState())
            {
                try
                {
                    return ConfigureRuntimeAttributes();
                }
                catch (System.Exception ex)
                {
                    if (log != null) log.Error(string.Format(culture, Constants.INVALID_USER_ID + ": " + Properties.Resources.NOTIFY_SQLINIITIALISATION_LOGON_ERROR, System.Environment.NewLine), ex);
                    throw new System.ArgumentException(Constants.INVALID_USER_ID.ToString(culture), ex);
                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Properties.Resources.NOTIFY_SQLNETWORK_UNAVAILABLE, Properties.Resources.CAPTION_INITIALISATION_ERROR, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                if (log != null) log.Fatal(Properties.Resources.NOTIFY_SQLNETWORK_UNAVAILABLE);

                LogHelper.ApplicationKill();
            }

            return Constants.NETWORK_UNAVAILABLE_ERROR;

            int ConfigureRuntimeAttributes()
            {
                LogHelper.TraceWriteLine("TRACE - Application account initialisation");

                byte[] byteCryptKey = ClassLibraryFramework.GenericStringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(0, 64));
                byte[] byteAuthKey = ClassLibraryFramework.GenericStringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(64, 64));
                byte[] byteSignature = ClassLibraryFramework.GenericStringMethods.GetStringToBytes(Properties.Settings.Default.Signature);

                return System.Convert.ToInt32(Encryption.AESThenHMAC.SimpleDecrypt(System.Text.Encoding.Default.GetString(byteSignature), byteCryptKey, byteAuthKey), culture);
            }

        }

    }

}
