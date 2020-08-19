using Squirrel;

namespace NAVService
{
    internal class UpdateHelper
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();

        public static void CheckForUpdates() => AsyncUpdate();

        private static void AsyncUpdate()
        {
            if (Properties.Settings.Default.Sync)
            {
                try
                {
                    string UpdateLocation = TargetLocation();

                    LogHelper.TraceWriteLine($"TRACE - Async update is enabled [Targeting { UpdateLocation }]");

                    _ = AsyncAwaitUpdateLookup();

                    async System.Threading.Tasks.Task AsyncAwaitUpdateLookup()
                    {
                        // Note: To avoid an exception ensure the UpdateLocation() URI is populated with a valid set of release files
                        using (UpdateManager manager = new UpdateManager(UpdateLocation))
                        {
                            await manager.UpdateApp().ConfigureAwait(false);
                        }

                    }

                }
                catch (System.Threading.AbandonedMutexException ex)
                {
                    // Note: As this error occurs within Squirrel it is highly unlikely to be captured here, but lets check for it anyway
                    System.Windows.Forms.MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_SQUIRREL_UPDATE_ERROR, System.Environment.NewLine), Properties.Resources.CAPTION_APPLICATION, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    if (log != null) log.Error(Properties.Resources.NOTIFY_SQUIRREL_UPDATE_ERROR, ex);
                }

                string TargetLocation()
                {
                    switch (GetTarget())
                    {
                        case Constants.CONNECTION_UPDATE_LOCAL:
                            return GetAttribute(System.Configuration.ConfigurationManager.ConnectionStrings[Constants.CONNECTION_LOCAL_UPDATE].ConnectionString);

                        case Constants.CONNECTION_UPDATE_CLOUD:
                            return GetAttribute(System.Configuration.ConfigurationManager.ConnectionStrings[Constants.CONNECTION_CLOUD_UPDATE].ConnectionString);

                        default:
                            return Constants.CONNECTION_PREPROD_UPDATE_DEFAULT;

                    }

                    string GetAttribute(string cipher)
                    {
                        byte[] byteCryptKey = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(0, 64));
                        byte[] byteAuthKey = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(64, 64));

                        return Encryption.AESThenHMAC.SimpleDecrypt(cipher, byteCryptKey, byteAuthKey);
                    }

                    int GetTarget()
                    {
                        int UpdateTargetDefault = -1;

                        return (ConnectionHelper.ProductionEnabled) ? GetUpdateApplicationPointer() : UpdateTargetDefault;

                        int GetUpdateApplicationPointer()
                        {
                            byte[] byteCryptKey = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(0, 64));
                            byte[] byteAuthKey = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.HashKey.Substring(64, 64));
                            byte[] byteUpdate = ClassLibraryFramework.StringMethods.GetStringToBytes(Properties.Settings.Default.Update);

                            return System.Convert.ToInt32(Encryption.AESThenHMAC.SimpleDecrypt(System.Text.Encoding.Default.GetString(byteUpdate), byteCryptKey, byteAuthKey), UserHelper.culture);
                        }

                    }

                }

            }

        }

    }

}
