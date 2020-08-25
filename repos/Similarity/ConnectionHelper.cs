//#define ENABLE_ENCRYPTION_HELPER

namespace NAVService
{
    internal class ConnectionHelper
    {
        internal static readonly string CONNECTION_LOCATION = GetConnectionLocation();
        internal static bool GetProductionEnabled() { return ProductionEnabled; }
        internal static bool ProductionEnabled { get; set; }

        private static bool PreProdEnabled { get; set; }
        private static int LocationPointer { get; set; }

        internal static string ApplicationName(bool bIncludeVersion = true, bool bIncludeUserName = true)
        {
            string application = bIncludeVersion ? Properties.Resources.CAPTION_APPLICATION + " " + Constants.BUILD_VERSION : Properties.Resources.CAPTION_APPLICATION;
            string preproduction = GetProductionEnabled() ? null : " [DEV]";

            return bIncludeUserName ? application + UserHelper.UserPropertiesModel.nvUserName.PadLeft(22, (char)32)  + " is signed in" + preproduction : application + preproduction;
        }

        internal static string AssemblyBuildVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo version = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);

            return version.FileVersion;
        }

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void ConfirmDebugState()
        {
            LogHelper.TraceWriteLine("TRACE - System Diagnostics Conditional DEBUG is enabled");
            EnableEncryptionHelper();
        }

        private static string GetConnectionLocation(object location = null)
        {
            switch (GetTarget())
            {
                case Constants.CONNECTION_DEV:

                    LogHelper.TraceWriteLine("TRACE - Confirming connection DEV target");

                    ProductionEnabled = false;
                    return GetAttribute(System.Configuration.ConfigurationManager.ConnectionStrings[Constants.CONNECTION_LOCAL].ConnectionString);

                case Constants.CONNECTION_PREPROD:

                    LogHelper.TraceWriteLine("TRACE - Confirming connection PREPROD target");

                    ProductionEnabled = false;
                    return GetAttribute(System.Configuration.ConfigurationManager.ConnectionStrings[Constants.CONNECTION_LOCAL].ConnectionString);

                case Constants.CONNECTION_PROD_NAV:

                    LogHelper.TraceWriteLine("TRACE - Confirming connection PROD target");

                    ProductionEnabled = false;
                    return GetAttribute(System.Configuration.ConfigurationManager.ConnectionStrings[Constants.CONNECTION_LOCAL].ConnectionString);

                case Constants.CONNECTION_PROD_AWS:

                    LogHelper.TraceWriteLine("TRACE - Confirming connection AWS target");

                    ProductionEnabled = true;
                    return GetAttribute(System.Configuration.ConfigurationManager.ConnectionStrings[Constants.CONNECTION_CLOUD].ConnectionString);

                default:

                    LogHelper.TraceWriteLine("TRACE - Confirming connection PREPROD target default");

                    ProductionEnabled = false;
                    return Constants.CONNECTION_PREPROD_TARGET_DEFAULT;

            }

            string GetAttribute(string cipher)
            {
                byte[] byteCryptKey = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(0, 64));
                byte[] byteAuthKey = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(64, 64));

                return Encryption.AESThenHMAC.SimpleDecrypt(cipher, byteCryptKey, byteAuthKey);
            }

            int GetTarget()
            {
                GetCompilerDirectives(location);

                return (!PreProdEnabled) ? GetTargetDatabasePointer() : ClassLibraryStandard.HelperMethods.IsInteger(location) ? LocationPointer : 0;

                int GetTargetDatabasePointer()
                {
                    byte[] byteCryptKey = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(0, 64));
                    byte[] byteAuthKey = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(64, 64));
                    byte[] byteTarget = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.Target);

                    return System.Convert.ToInt32(Encryption.AESThenHMAC.SimpleDecrypt(System.Text.Encoding.Default.GetString(byteTarget), byteCryptKey, byteAuthKey), UserHelper.culture);
                }

            }

        }

        [System.Diagnostics.Conditional("DEBUG")]
        private static void GetCompilerDirectives(object location = null)
        {
            int LocationTargetDefault = -1;

            if (location == null) 
            {
                PreProdEnabled = true;
                LocationPointer = LocationTargetDefault;
            }

        }

        // requires manual intervention 
        [System.Diagnostics.Conditional("ENABLE_ENCRYPTION_HELPER")]
        private static void EnableEncryptionHelper()
        {

#if DEBUG // otherwise do not compile

            LogHelper.TraceWriteLine("TRACE - System Diagnostics Conditional Encryption Helper is enabled");

            EncryptionHelper.EncryptionExamplesToConsole("The location URL");
            EncryptionHelper.SignatureKeyToConsole("The location URL pointer number");
#endif

        }

    }

}
