namespace NAVService
{
    public static class UserHelper
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();

        internal static readonly System.DateTime ApplicationStartTime = System.DateTime.Now;
        internal static readonly System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;
        internal static readonly NAVUserPropertiesModel UserPropertiesModel = DataAccess.InitialiseUserProperties(InitaliseEnvironment());

        internal static void SetReplaceAllWords(bool value) => ReplaceAllWords = value;
        internal static void SetReplaceDefaultWordsOnly(bool value) => ReplaceDefaultWordsOnly = value;
        internal static void SetPullAbbreviations(bool value) => PullAbbreviations = value;
        internal static void SetLogErrors(bool value) => LogErrors = value;

        internal static NAVUserStateModel UserStateModel { get; set; }

        internal static bool GetReplaceAllWords() { return ReplaceAllWords; }
        internal static bool GetReplaceDefaultWordsOnly() { return ReplaceDefaultWordsOnly; }
        internal static bool GetPullAbbreviations() { return PullAbbreviations; }
        internal static bool GetLogErrors() { return LogErrors; }

        private static bool ReplaceAllWords = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_REPLACE_WORDS));
        private static bool ReplaceDefaultWordsOnly = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_PERMIT_DEFAULTS));
        private static bool PullAbbreviations = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_PULL_ABBREVIATIONS));
        private static bool LogErrors = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_LOG_ERRORS));

        // assign a user to their environment attributes and dispose of system sensitive data
        private static int InitaliseEnvironment()
        {
            LogHelper.ConfirmTraceState();

            if (ClassLibraryStandard.InternetInteropServices.InternetConnectedState())
            {
                try
                {
                    LogHelper.TraceWriteLine("TRACE - Confirming connection out");
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
                LogHelper.TraceWriteLine("TRACE - Begin account initialisation");

                byte[] byteCryptKey = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(0, 64));
                byte[] byteAuthKey = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(64, 64));
                byte[] byteSignature = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.Signature);

                return System.Convert.ToInt32(Encryption.AESThenHMAC.SimpleDecrypt(System.Text.Encoding.Default.GetString(byteSignature), byteCryptKey, byteAuthKey), culture);
            }

        }

    }

}
