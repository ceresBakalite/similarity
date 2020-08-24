using System;

/// <summary>
/// The main entry point for the application.
/// </summary>

namespace NAVService
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = UserHelper.culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = UserHelper.culture;

                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

                InitialiseApplication();

                void InitialiseApplication()
                {
                    using (SplashScreen SplashWindow = new SplashScreen())
                    {
                        System.Threading.Thread SplashThread = new System.Threading.Thread(new System.Threading.ThreadStart(() => System.Windows.Forms.Application.Run(SplashWindow)));
                        SplashThread.SetApartmentState(System.Threading.ApartmentState.MTA);
                        SplashThread.Start();

                        LogHelper.GetLogState();

                        using (NAVPanelForm ApplicationWindow = new NAVPanelForm())
                        {
                            ApplicationWindow.Load += ApplicationWindowLoad;
                            System.Windows.Forms.Application.Run(ApplicationWindow);

                            void ApplicationWindowLoad(object sender, EventArgs e)
                            {
                                if (SplashWindow != null && !SplashWindow.Disposing && !SplashWindow.IsDisposed)
                                {
                                    SplashWindow.Invoke(new Action(() => SplashWindow.Close()));
                                }

                                ApplicationWindow.TopMost = true;
                                ApplicationWindow.Activate();
                                ApplicationWindow.TopMost = false;
                            }

                        }

                    }

                }

            }
            catch (NullReferenceException ex)
            {
                LogHelper.FatalException(Properties.Resources.NOTIFY_SQLNULLREFERENCE_ERROR, Properties.Resources.CAPTION_APPLICATION, ex);
            }
        
        }

    }

}
