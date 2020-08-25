namespace NAVService
{
    public static class UserHelper
    {
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
                    LogHelper.FatalUserVerificationException(ex);
                    throw;
                }

            }
            else
            {
                LogHelper.FatalInternetUnavailableException();
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
