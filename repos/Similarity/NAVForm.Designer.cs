namespace NAVService
{
    partial class NAVForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NAVForm));
            this.AddFileButton = new System.Windows.Forms.Button();
            SheetDataGridView = new System.Windows.Forms.DataGridView();
            this.SheetContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteColumnMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RestoreColumnsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ColumnToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteRowsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RowToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.CancelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.SheetComboBox = new System.Windows.Forms.ComboBox();
            this.FilenameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ExitButton = new System.Windows.Forms.Button();
            this.SaveWorkbookButton = new System.Windows.Forms.Button();
            this.NAVFormToolTip = new System.Windows.Forms.ToolTip(this.components);
            SheetCurrentTotal = new System.Windows.Forms.Label();
            this.DropPreLoadPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(SheetDataGridView)).BeginInit();
            this.SheetContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DropPreLoadPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // AddFileButton
            // 
            this.AddFileButton.AccessibleDescription = "";
            this.AddFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddFileButton.BackColor = System.Drawing.SystemColors.Window;
            this.AddFileButton.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.AddFileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddFileButton.Location = new System.Drawing.Point(668, 464);
            this.AddFileButton.Name = "AddFileButton";
            this.AddFileButton.Size = new System.Drawing.Size(75, 23);
            this.AddFileButton.TabIndex = 0;
            this.AddFileButton.Text = "Add File";
            this.AddFileButton.UseVisualStyleBackColor = false;
            this.AddFileButton.Click += new System.EventHandler(this.AddFileButtonClick);
            // 
            // SheetDataGridView
            // 
            SheetDataGridView.AllowDrop = true;
            SheetDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            SheetDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            SheetDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            SheetDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            SheetDataGridView.ContextMenuStrip = this.SheetContextMenuStrip;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(235)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            SheetDataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            SheetDataGridView.EnableHeadersVisualStyles = false;
            SheetDataGridView.Location = new System.Drawing.Point(0, 1);
            SheetDataGridView.Name = "SheetDataGridView";
            SheetDataGridView.RowHeadersWidth = 30;
            SheetDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            SheetDataGridView.Size = new System.Drawing.Size(839, 444);
            SheetDataGridView.TabIndex = 5;
            SheetDataGridView.VirtualMode = true;
            // 
            // SheetContextMenuStrip
            // 
            this.SheetContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteColumnMenuItem,
            this.RestoreColumnsMenuItem,
            this.ColumnToolStripSeparator,
            this.DeleteRowMenuItem,
            this.DeleteRowsMenuItem,
            this.RowToolStripSeparator,
            this.CancelMenuItem});
            this.SheetContextMenuStrip.Name = "SheetContextMenuStrip";
            this.SheetContextMenuStrip.Size = new System.Drawing.Size(165, 126);
            // 
            // DeleteColumnMenuItem
            // 
            this.DeleteColumnMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("DeleteColumnMenuItem.Image")));
            this.DeleteColumnMenuItem.Name = "DeleteColumnMenuItem";
            this.DeleteColumnMenuItem.Size = new System.Drawing.Size(164, 22);
            this.DeleteColumnMenuItem.Text = "Delete Column";
            this.DeleteColumnMenuItem.Click += new System.EventHandler(this.DeleteColumnMenuItemClick);
            // 
            // RestoreColumnsMenuItem
            // 
            this.RestoreColumnsMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("RestoreColumnsMenuItem.Image")));
            this.RestoreColumnsMenuItem.Name = "RestoreColumnsMenuItem";
            this.RestoreColumnsMenuItem.Size = new System.Drawing.Size(164, 22);
            this.RestoreColumnsMenuItem.Text = "Restore Columns";
            this.RestoreColumnsMenuItem.Click += new System.EventHandler(this.RestoreColumnMenuItemClick);
            // 
            // ColumnToolStripSeparator
            // 
            this.ColumnToolStripSeparator.Name = "ColumnToolStripSeparator";
            this.ColumnToolStripSeparator.Size = new System.Drawing.Size(161, 6);
            // 
            // DeleteRowMenuItem
            // 
            this.DeleteRowMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("DeleteRowMenuItem.Image")));
            this.DeleteRowMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.DeleteRowMenuItem.Name = "DeleteRowMenuItem";
            this.DeleteRowMenuItem.Size = new System.Drawing.Size(164, 22);
            this.DeleteRowMenuItem.Text = "Delete Row";
            this.DeleteRowMenuItem.Click += new System.EventHandler(this.DeleteRowMenuItemClick);
            // 
            // DeleteRowsMenuItem
            // 
            this.DeleteRowsMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("DeleteRowsMenuItem.Image")));
            this.DeleteRowsMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.DeleteRowsMenuItem.Name = "DeleteRowsMenuItem";
            this.DeleteRowsMenuItem.Size = new System.Drawing.Size(164, 22);
            this.DeleteRowsMenuItem.Text = "Remove Rows";
            this.DeleteRowsMenuItem.Click += new System.EventHandler(this.GetDeleteRowsMenuItemClick);
            // 
            // RowToolStripSeparator
            // 
            this.RowToolStripSeparator.Name = "RowToolStripSeparator";
            this.RowToolStripSeparator.Size = new System.Drawing.Size(161, 6);
            // 
            // CancelMenuItem
            // 
            this.CancelMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("CancelMenuItem.Image")));
            this.CancelMenuItem.Name = "CancelMenuItem";
            this.CancelMenuItem.Size = new System.Drawing.Size(164, 22);
            this.CancelMenuItem.Text = "Cancel";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 469);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "File Name:";
            // 
            // SheetComboBox
            // 
            this.SheetComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SheetComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.SheetComboBox.Enabled = false;
            this.SheetComboBox.FormattingEnabled = true;
            this.SheetComboBox.Location = new System.Drawing.Point(78, 493);
            this.SheetComboBox.MaxDropDownItems = 50;
            this.SheetComboBox.Name = "SheetComboBox";
            this.SheetComboBox.Size = new System.Drawing.Size(133, 21);
            this.SheetComboBox.TabIndex = 2;
            this.SheetComboBox.SelectedIndexChanged += new System.EventHandler(this.WorksheetIndexChanged);
            this.SheetComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SheetComboBoxKeyPress);
            // 
            // FilenameTextBox
            // 
            this.FilenameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilenameTextBox.Location = new System.Drawing.Point(78, 466);
            this.FilenameTextBox.Name = "FilenameTextBox";
            this.FilenameTextBox.ReadOnly = true;
            this.FilenameTextBox.Size = new System.Drawing.Size(584, 20);
            this.FilenameTextBox.TabIndex = 99;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 495);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Worksheet:";
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.BackColor = System.Drawing.SystemColors.Window;
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.Location = new System.Drawing.Point(749, 464);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "Cancel";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButtonClick);
            // 
            // SaveWorkbookButton
            // 
            this.SaveWorkbookButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveWorkbookButton.BackColor = System.Drawing.SystemColors.Window;
            this.SaveWorkbookButton.Enabled = false;
            this.SaveWorkbookButton.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.SaveWorkbookButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveWorkbookButton.Location = new System.Drawing.Point(217, 492);
            this.SaveWorkbookButton.Name = "SaveWorkbookButton";
            this.SaveWorkbookButton.Size = new System.Drawing.Size(125, 23);
            this.SaveWorkbookButton.TabIndex = 3;
            this.SaveWorkbookButton.Text = "Save Workbook";
            this.SaveWorkbookButton.UseVisualStyleBackColor = false;
            this.SaveWorkbookButton.Click += new System.EventHandler(this.SaveWorkbookButtonClick);
            // 
            // SheetCurrentTotal
            // 
            SheetCurrentTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            SheetCurrentTotal.AutoSize = true;
            SheetCurrentTotal.BackColor = System.Drawing.SystemColors.Window;
            SheetCurrentTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            SheetCurrentTotal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            SheetCurrentTotal.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            SheetCurrentTotal.Location = new System.Drawing.Point(0, 444);
            SheetCurrentTotal.Name = "SheetCurrentTotal";
            SheetCurrentTotal.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            SheetCurrentTotal.Size = new System.Drawing.Size(57, 15);
            SheetCurrentTotal.TabIndex = 0;
            SheetCurrentTotal.Text = "0 Rows";
            // 
            // DropPreLoadPictureBox
            // 
            this.DropPreLoadPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DropPreLoadPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.DropPreLoadPictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("DropPreLoadPictureBox.BackgroundImage")));
            this.DropPreLoadPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DropPreLoadPictureBox.Location = new System.Drawing.Point(102, 40);
            this.DropPreLoadPictureBox.Name = "DropPreLoadPictureBox";
            this.DropPreLoadPictureBox.Size = new System.Drawing.Size(604, 357);
            this.DropPreLoadPictureBox.TabIndex = 100;
            this.DropPreLoadPictureBox.TabStop = false;
            // 
            // NAVForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(839, 533);
            this.Controls.Add(this.DropPreLoadPictureBox);
            this.Controls.Add(SheetCurrentTotal);
            this.Controls.Add(this.SaveWorkbookButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FilenameTextBox);
            this.Controls.Add(this.SheetComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(SheetDataGridView);
            this.Controls.Add(this.AddFileButton);
            this.DoubleBuffered = true;
            this.Name = "NAVForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "NAV Search";
            this.TransparencyKey = System.Drawing.Color.White;
            ((System.ComponentModel.ISupportInitialize)(SheetDataGridView)).EndInit();
            this.SheetContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DropPreLoadPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddFileButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FilenameTextBox;
        private System.Windows.Forms.Button SaveWorkbookButton;
        private System.Windows.Forms.ToolTip NAVFormToolTip;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.ComboBox SheetComboBox;
        private System.Windows.Forms.ContextMenuStrip SheetContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem DeleteColumnMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RestoreColumnsMenuItem;
        private System.Windows.Forms.ToolStripSeparator RowToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem CancelMenuItem;
        private System.Windows.Forms.ToolStripSeparator ColumnToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem DeleteRowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteRowsMenuItem;
        private System.Windows.Forms.PictureBox DropPreLoadPictureBox;
        private protected static System.Windows.Forms.DataGridView SheetDataGridView;
        private protected static System.Windows.Forms.Label SheetCurrentTotal;

        /// <summary>
        /// Similar to private internal but is now both private AND protected within this class in this assembly
        /// 
        /// private protected static System.Windows.Forms.DataGridView SheetDataGridView;
        /// private protected static System.Windows.Forms.Label SheetCurrentTotal;
        /// 
        /// </summary>

    }
}

