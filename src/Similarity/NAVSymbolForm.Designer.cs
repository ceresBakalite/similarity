namespace NAVService
{
    partial class NAVSymbolForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NAVSymbolForm));
            this.WordLabel = new System.Windows.Forms.Label();
            this.AbbreviationLabel = new System.Windows.Forms.Label();
            this.WordTextBox = new System.Windows.Forms.TextBox();
            this.AbbreviationTextBox = new System.Windows.Forms.TextBox();
            this.AbbreviationDescriptionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.AbbreviationDescriptionLabel = new System.Windows.Forms.Label();
            this.AlwaysUseComboBox = new System.Windows.Forms.CheckBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.AbbreviationFormBasePanel = new System.Windows.Forms.Panel();
            this.SymbolFormToolTips = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // WordLabel
            // 
            resources.ApplyResources(this.WordLabel, "WordLabel");
            this.WordLabel.Name = "WordLabel";
            this.SymbolFormToolTips.SetToolTip(this.WordLabel, resources.GetString("WordLabel.ToolTip"));
            // 
            // AbbreviationLabel
            // 
            resources.ApplyResources(this.AbbreviationLabel, "AbbreviationLabel");
            this.AbbreviationLabel.Name = "AbbreviationLabel";
            this.SymbolFormToolTips.SetToolTip(this.AbbreviationLabel, resources.GetString("AbbreviationLabel.ToolTip"));
            // 
            // WordTextBox
            // 
            resources.ApplyResources(this.WordTextBox, "WordTextBox");
            this.WordTextBox.Name = "WordTextBox";
            // 
            // AbbreviationTextBox
            // 
            resources.ApplyResources(this.AbbreviationTextBox, "AbbreviationTextBox");
            this.AbbreviationTextBox.Name = "AbbreviationTextBox";
            // 
            // AbbreviationDescriptionRichTextBox
            // 
            resources.ApplyResources(this.AbbreviationDescriptionRichTextBox, "AbbreviationDescriptionRichTextBox");
            this.AbbreviationDescriptionRichTextBox.Name = "AbbreviationDescriptionRichTextBox";
            // 
            // AbbreviationDescriptionLabel
            // 
            resources.ApplyResources(this.AbbreviationDescriptionLabel, "AbbreviationDescriptionLabel");
            this.AbbreviationDescriptionLabel.Name = "AbbreviationDescriptionLabel";
            this.SymbolFormToolTips.SetToolTip(this.AbbreviationDescriptionLabel, resources.GetString("AbbreviationDescriptionLabel.ToolTip"));
            // 
            // AlwaysUseComboBox
            // 
            resources.ApplyResources(this.AlwaysUseComboBox, "AlwaysUseComboBox");
            this.AlwaysUseComboBox.Name = "AlwaysUseComboBox";
            this.SymbolFormToolTips.SetToolTip(this.AlwaysUseComboBox, resources.GetString("AlwaysUseComboBox.ToolTip"));
            this.AlwaysUseComboBox.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.OKButton, "OKButton");
            this.OKButton.Name = "OKButton";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButtonClick);
            // 
            // ExitButton
            // 
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.ExitButton, "ExitButton");
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // AbbreviationFormBasePanel
            // 
            this.AbbreviationFormBasePanel.BackColor = System.Drawing.Color.Transparent;
            this.AbbreviationFormBasePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.AbbreviationFormBasePanel, "AbbreviationFormBasePanel");
            this.AbbreviationFormBasePanel.Name = "AbbreviationFormBasePanel";
            // 
            // NAVSymbolForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.ControlBox = false;
            this.Controls.Add(this.WordTextBox);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.WordLabel);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.AbbreviationLabel);
            this.Controls.Add(this.AlwaysUseComboBox);
            this.Controls.Add(this.AbbreviationTextBox);
            this.Controls.Add(this.AbbreviationDescriptionLabel);
            this.Controls.Add(this.AbbreviationDescriptionRichTextBox);
            this.Controls.Add(this.AbbreviationFormBasePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NAVSymbolForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WordLabel;
        private System.Windows.Forms.Label AbbreviationLabel;
        private System.Windows.Forms.TextBox WordTextBox;
        private System.Windows.Forms.TextBox AbbreviationTextBox;
        private System.Windows.Forms.RichTextBox AbbreviationDescriptionRichTextBox;
        private System.Windows.Forms.Label AbbreviationDescriptionLabel;
        private System.Windows.Forms.CheckBox AlwaysUseComboBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Panel AbbreviationFormBasePanel;
        private System.Windows.Forms.ToolTip SymbolFormToolTips;
    }
}