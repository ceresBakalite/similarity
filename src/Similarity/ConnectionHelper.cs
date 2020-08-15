//#define ENABLE_ENCRYPTION_HELPER

namespace NAVService
{
    internal class ConnectionHelper
    {
        protected internal static readonly string CONNECTION_LOCATION = GetConnectionLocation();
        protected internal static bool GetProductionEnabled() { return ProductionEnabled; }
        protected internal static bool ProductionEnabled { get; set; }

        private static bool PreProdEnabled { get; set; }
        private static int LocationPointer { get; set; }

        public static string ApplicationName(bool bIncludeVersion = true, bool bIncludeUserName = true)
        {
            string application = bIncludeVersion ? Properties.Resources.CAPTION_APPLICATION + " " + Constants.BUILD_VERSION : Properties.Resources.CAPTION_APPLICATION;
            string preproduction = GetProductionEnabled() ? null : " [DEV]";

            return bIncludeUserName ? application + ClassLibraryStandard.GenericHelperMethods.EnumerateChar(8) + UserHelper.UserPropertiesModel.nvUserName + " is signed in" + preproduction : application + preproduction;
        }

        public static string AssemblyBuildVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo version = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);

            return version.FileVersion;
        }

        private static string GetConnectionLocation(object location = null)
        {
            switch (GetTarget())
            {
                case Constants.CONNECTION_DEV:
                    ProductionEnabled = false;
                    return GetAttribute(System.Configuration.ConfigurationManager.ConnectionStrings[Constants.CONNECTION_LOCAL].ConnectionString);

                case Constants.CONNECTION_PREPROD:
                    ProductionEnabled = false;
                    return GetAttribute(System.Configuration.ConfigurationManager.ConnectionStrings[Constants.CONNECTION_LOCAL].ConnectionString);

                case Constants.CONNECTION_PROD_NAV:
                    ProductionEnabled = false;
                    return GetAttribute(System.Configuration.ConfigurationManager.ConnectionStrings[Constants.CONNECTION_LOCAL].ConnectionString);

                case Constants.CONNECTION_PROD_AWS:
                    ProductionEnabled = true;
                    return GetAttribute(System.Configuration.ConfigurationManager.ConnectionStrings[Constants.CONNECTION_CLOUD].ConnectionString);

                default:
                    ProductionEnabled = false;
                    return Constants.CONNECTION_DEV_DEFAULT;

            }

            string GetAttribute(string cipher)
            {
                byte[] byteCryptKey = ClassLibraryFramework.GenericStringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(0, 64));
                byte[] byteAuthKey = ClassLibraryFramework.GenericStringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(64, 64));

                return Encryption.AESThenHMAC.SimpleDecrypt(cipher, byteCryptKey, byteAuthKey);
            }

            int GetTarget()
            {
                GetCompilerDirectives(location);

                return (!PreProdEnabled) ? GetTargetDatabasePointer() : ClassLibraryStandard.GenericHelperMethods.IsInteger(location) ? LocationPointer : 0;

                int GetTargetDatabasePointer()
                {
                    byte[] byteCryptKey = ClassLibraryFramework.GenericStringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(0, 64));
                    byte[] byteAuthKey = ClassLibraryFramework.GenericStringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(64, 64));
                    byte[] byteTarget = ClassLibraryFramework.GenericStringMethods.GetStringToBytes(Properties.Settings.Default.Target);

                    return System.Convert.ToInt32(Encryption.AESThenHMAC.SimpleDecrypt(System.Text.Encoding.Default.GetString(byteTarget), byteCryptKey, byteAuthKey), UserHelper.culture);
                }

            }

        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void GetCompilerDirectives(object location = null)
        {
            int LocationTargetDefault = -1;

            if (location == null) 
            {
                LogHelper.TraceWriteLine("TRACE - Debug/Trace is enabled. Targeting PREPROD");
                PreProdEnabled = true;
                LocationPointer = LocationTargetDefault;
            }

            EnableEncryptionHelper();
        }

        [System.Diagnostics.Conditional("ENABLE_ENCRYPTION_HELPER")]
        public static void EnableEncryptionHelper()
        {
#if DEBUG // otherwise do not compile

            EncryptionHelper.EncryptionExamplesToConsole("The location URL");
            EncryptionHelper.SignatureKeyToConsole("The location pointer number");

#endif
        }

    }

}
