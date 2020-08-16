using System;

/// <summary>
/// The main entry point for the application.
/// </summary>

namespace NAVService
{
    static class Program
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();

        [STAThread]
        static void Main()
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = UserHelper.culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = UserHelper.culture;

                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

                SplashScreen SplashWindow = new SplashScreen();

                System.Threading.Thread SplashThread = new System.Threading.Thread(new System.Threading.ThreadStart(() => System.Windows.Forms.Application.Run(SplashWindow)));
                SplashThread.SetApartmentState(System.Threading.ApartmentState.MTA);
                SplashThread.Start();

                LogHelper.GetLogState();

                NAVPanelForm NAVApplication = new NAVPanelForm();

                NAVApplication.Load += NAVApplicationLoad;
                System.Windows.Forms.Application.Run(NAVApplication);

                void NAVApplicationLoad(object sender, EventArgs e)
                {
                    if (SplashWindow != null && !SplashWindow.Disposing && !SplashWindow.IsDisposed) 
                    {
                        SplashWindow.Invoke(new Action(() => SplashWindow.Close()));
                    }

                    NAVApplication.TopMost = true;
                    NAVApplication.Activate();
                    NAVApplication.TopMost = false;
                }

                SplashWindow.Dispose();
                NAVApplication.Dispose();
            }
            catch (NullReferenceException ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_SQLNULLREFERENCE_ERROR, Environment.NewLine), Properties.Resources.CAPTION_APPLICATION, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                if (log != null) log.Fatal(Properties.Resources.NOTIFY_SQLNULLREFERENCE_ERROR, ex);

                LogHelper.ApplicationKill();
            }

        }

    }

}
