using System.Windows.Forms;

namespace NAVService
{
    public enum ProgressBarDisplayText
    {
        Percentage, CustomText
    }

    internal partial class NAVProgressBar : ProgressBar
    {
        public ProgressBarDisplayText DisplayStyle { get; set; }

        public string CustomText { get; set; }

        public NAVProgressBar()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            System.Drawing.Rectangle rect = ClientRectangle;
            System.Drawing.Graphics g = args.Graphics;

            ProgressBarRenderer.DrawHorizontalBar(g, rect);

            if (Value > 0)
            {
                System.Drawing.Rectangle clip = new System.Drawing.Rectangle(rect.X, rect.Y, (int)System.Math.Round(((float)Value / Maximum) * rect.Width), rect.Height);
                ProgressBarRenderer.DrawHorizontalChunks(g, clip);
            }

            // Set the progress display text (as either %age or custom text)
            string DisplayProgress = DisplayStyle == (ProgressBarDisplayText.Percentage) ? string.Format(UserHelper.culture, "{0} %", Value) : CustomText;

            using (System.Drawing.Font font = new System.Drawing.Font(System.Drawing.FontFamily.GenericMonospace, 10))
            {
                System.Drawing.SizeF size = g.MeasureString(DisplayProgress, font);
                System.Drawing.Point location = new System.Drawing.Point((int)((rect.Width / 2) - (size.Width / 2)), (int)((rect.Height / 2) - (size.Height / 2) + 2));
                g.DrawString(DisplayProgress, font, System.Drawing.Brushes.Black, location);
            }

        }

    }

}

