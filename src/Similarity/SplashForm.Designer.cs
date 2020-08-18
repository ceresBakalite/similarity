namespace NAVService
{
    partial class SplashScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
            this.SplashFormPictureBox = new System.Windows.Forms.PictureBox();
            this.SplashFormVersionLabel = new System.Windows.Forms.Label();
            this.SplashFormLicenceLabel = new System.Windows.Forms.Label();
            this.SplashFormStartingLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SplashFormPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SplashFormPictureBox
            // 
            this.SplashFormPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SplashFormPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplashFormPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("SplashFormPictureBox.Image")));
            this.SplashFormPictureBox.Location = new System.Drawing.Point(0, 0);
            this.SplashFormPictureBox.Name = "SplashFormPictureBox";
            this.SplashFormPictureBox.Size = new System.Drawing.Size(400, 300);
            this.SplashFormPictureBox.TabIndex = 0;
            this.SplashFormPictureBox.TabStop = false;
            // 
            // SplashFormVersionLabel
            // 
            this.SplashFormVersionLabel.AutoSize = true;
            this.SplashFormVersionLabel.BackColor = System.Drawing.Color.Transparent;
            this.SplashFormVersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SplashFormVersionLabel.ForeColor = System.Drawing.Color.White;
            this.SplashFormVersionLabel.Location = new System.Drawing.Point(76, 116);
            this.SplashFormVersionLabel.Name = "SplashFormVersionLabel";
            this.SplashFormVersionLabel.Size = new System.Drawing.Size(0, 17);
            this.SplashFormVersionLabel.TabIndex = 1;
            // 
            // SplashFormLicenceLabel
            // 
            this.SplashFormLicenceLabel.AutoSize = true;
            this.SplashFormLicenceLabel.BackColor = System.Drawing.Color.Transparent;
            this.SplashFormLicenceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.SplashFormLicenceLabel.ForeColor = System.Drawing.Color.White;
            this.SplashFormLicenceLabel.Location = new System.Drawing.Point(227, 278);
            this.SplashFormLicenceLabel.Name = "SplashFormLicenceLabel";
            this.SplashFormLicenceLabel.Size = new System.Drawing.Size(0, 13);
            this.SplashFormLicenceLabel.TabIndex = 2;
            // 
            // SplashFormStartingLabel
            // 
            this.SplashFormStartingLabel.AutoSize = true;
            this.SplashFormStartingLabel.BackColor = System.Drawing.Color.Transparent;
            this.SplashFormStartingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SplashFormStartingLabel.ForeColor = System.Drawing.Color.White;
            this.SplashFormStartingLabel.Location = new System.Drawing.Point(6, 278);
            this.SplashFormStartingLabel.Name = "SplashFormStartingLabel";
            this.SplashFormStartingLabel.Size = new System.Drawing.Size(0, 13);
            this.SplashFormStartingLabel.TabIndex = 3;
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.SplashFormStartingLabel);
            this.Controls.Add(this.SplashFormLicenceLabel);
            this.Controls.Add(this.SplashFormVersionLabel);
            this.Controls.Add(this.SplashFormPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashScreen";
            ((System.ComponentModel.ISupportInitialize)(this.SplashFormPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox SplashFormPictureBox;
        private System.Windows.Forms.Label SplashFormVersionLabel;
        private System.Windows.Forms.Label SplashFormLicenceLabel;
        private System.Windows.Forms.Label SplashFormStartingLabel;
    }
}