using System.Windows.Forms;

namespace NAVService
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            InitializeSplashForm();
        }

        private void InitializeSplashForm()
        {
            SplashFormVersionLabel.Parent = SplashFormPictureBox;
            SplashFormVersionLabel.BackColor = System.Drawing.Color.Transparent;
            SplashFormVersionLabel.Text = string.Format(UserHelper.culture, Properties.Resources.NOTIFY_SPLASHFORM_VERSION, "Beta ", Constants.BUILD_VERSION);

            SplashFormLicenceLabel.Parent = SplashFormPictureBox;
            SplashFormLicenceLabel.BackColor = System.Drawing.Color.Transparent;
            SplashFormLicenceLabel.Text = Properties.Resources.NOTIFY_APPLICATION_LICENCE;

            SplashFormStartingLabel.Parent = SplashFormPictureBox;
            SplashFormStartingLabel.BackColor = System.Drawing.Color.Transparent;
            SplashFormStartingLabel.Text = Properties.Resources.NOTIFY_APPLICATION_STARTING;
        }

    }

}
