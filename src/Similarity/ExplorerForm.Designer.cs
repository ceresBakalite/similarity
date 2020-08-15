namespace NAVService
{
    partial class ExplorerForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerForm));
            this.ExplorerToolTips = new System.Windows.Forms.ToolTip(this.components);
            this.CancelProgressButton = new System.Windows.Forms.Button();
            ResultDataGridView = new System.Windows.Forms.DataGridView();
            this.bDeleteRowFlag = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.iRowID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iParentRowID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iCompareValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nvMatchType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nvExplorerString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResultMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ResultMenuItemDeleteRow = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultMenuItemRemoveRows = new System.Windows.Forms.ToolStripMenuItem();
            this.AbbreviationMenuSeparator01 = new System.Windows.Forms.ToolStripSeparator();
            this.AbbreviationMenuItemCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.ProgressBarPanel = new System.Windows.Forms.Panel();
            this.navProgressBar = new NAVService.NAVProgressBar();
            AbbreviatedLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(ResultDataGridView)).BeginInit();
            this.ResultMenu.SuspendLayout();
            this.ProgressBarPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CancelProgressButton
            // 
            this.CancelProgressButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelProgressButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.CancelProgressButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.CancelProgressButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelProgressButton.Location = new System.Drawing.Point(375, 295);
            this.CancelProgressButton.Name = "CancelProgressButton";
            this.CancelProgressButton.Size = new System.Drawing.Size(75, 23);
            this.CancelProgressButton.TabIndex = 2;
            this.CancelProgressButton.Text = "Cancel";
            this.CancelProgressButton.UseVisualStyleBackColor = true;
            this.CancelProgressButton.Click += new System.EventHandler(this.CancelProgressButtonClick);
            // 
            // ResultDataGridView
            // 
            ResultDataGridView.AllowUserToAddRows = false;
            ResultDataGridView.AllowUserToDeleteRows = false;
            ResultDataGridView.AllowUserToOrderColumns = true;
            ResultDataGridView.AllowUserToResizeRows = false;
            ResultDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            ResultDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            ResultDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            ResultDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ResultDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.bDeleteRowFlag,
            this.iRowID,
            this.iParentRowID,
            this.iCompareValue,
            this.nvMatchType,
            this.nvExplorerString});
            ResultDataGridView.ContextMenuStrip = this.ResultMenu;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(235)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            ResultDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            ResultDataGridView.GridColor = System.Drawing.SystemColors.Control;
            ResultDataGridView.Location = new System.Drawing.Point(0, 327);
            ResultDataGridView.Name = "ResultDataGridView";
            ResultDataGridView.RowHeadersVisible = false;
            ResultDataGridView.Size = new System.Drawing.Size(453, 50);
            ResultDataGridView.TabIndex = 0;
            // 
            // bDeleteRowFlag
            // 
            this.bDeleteRowFlag.DataPropertyName = "bDeleteRowFlag";
            this.bDeleteRowFlag.FalseValue = "0";
            this.bDeleteRowFlag.FillWeight = 10F;
            this.bDeleteRowFlag.HeaderText = "Remove";
            this.bDeleteRowFlag.IndeterminateValue = "0";
            this.bDeleteRowFlag.Name = "bDeleteRowFlag";
            this.bDeleteRowFlag.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.bDeleteRowFlag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.bDeleteRowFlag.TrueValue = "1";
            this.bDeleteRowFlag.Width = 55;
            // 
            // iRowID
            // 
            this.iRowID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.iRowID.DataPropertyName = "iRowID";
            this.iRowID.FillWeight = 1F;
            this.iRowID.HeaderText = "iRowID";
            this.iRowID.Name = "iRowID";
            this.iRowID.ReadOnly = true;
            this.iRowID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.iRowID.Visible = false;
            this.iRowID.Width = 5;
            // 
            // iParentRowID
            // 
            this.iParentRowID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.iParentRowID.DataPropertyName = "iParentRowID";
            this.iParentRowID.FillWeight = 1F;
            this.iParentRowID.HeaderText = "iParentRowID";
            this.iParentRowID.Name = "iParentRowID";
            this.iParentRowID.ReadOnly = true;
            this.iParentRowID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.iParentRowID.Visible = false;
            this.iParentRowID.Width = 10;
            // 
            // iCompareValue
            // 
            this.iCompareValue.DataPropertyName = "iCompareValue";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.iCompareValue.DefaultCellStyle = dataGridViewCellStyle1;
            this.iCompareValue.FillWeight = 10F;
            this.iCompareValue.HeaderText = "Similarity";
            this.iCompareValue.Name = "iCompareValue";
            this.iCompareValue.ReadOnly = true;
            this.iCompareValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.iCompareValue.Width = 55;
            // 
            // nvMatchType
            // 
            this.nvMatchType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.nvMatchType.DataPropertyName = "nvMatchType";
            this.nvMatchType.FillWeight = 1F;
            this.nvMatchType.HeaderText = "nvMatchType";
            this.nvMatchType.Name = "nvMatchType";
            this.nvMatchType.ReadOnly = true;
            this.nvMatchType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.nvMatchType.Visible = false;
            this.nvMatchType.Width = 10;
            // 
            // nvExplorerString
            // 
            this.nvExplorerString.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nvExplorerString.DataPropertyName = "nvExplorerString";
            this.nvExplorerString.FillWeight = 10F;
            this.nvExplorerString.HeaderText = "Data";
            this.nvExplorerString.Name = "nvExplorerString";
            this.nvExplorerString.ReadOnly = true;
            // 
            // ResultMenu
            // 
            this.ResultMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResultMenuItemDeleteRow,
            this.ResultMenuItemRemoveRows,
            this.AbbreviationMenuSeparator01,
            this.AbbreviationMenuItemCancel});
            this.ResultMenu.Name = "contextMenuStrip1";
            this.ResultMenu.Size = new System.Drawing.Size(181, 98);
            // 
            // ResultMenuItemDeleteRow
            // 
            this.ResultMenuItemDeleteRow.Image = ((System.Drawing.Image)(resources.GetObject("ResultMenuItemDeleteRow.Image")));
            this.ResultMenuItemDeleteRow.Name = "ResultMenuItemDeleteRow";
            this.ResultMenuItemDeleteRow.Size = new System.Drawing.Size(180, 22);
            this.ResultMenuItemDeleteRow.Text = "Delete Row";
            this.ResultMenuItemDeleteRow.Click += new System.EventHandler(this.ResultMenuItemDeleteRowClick);
            // 
            // ResultMenuItemRemoveRows
            // 
            this.ResultMenuItemRemoveRows.Image = ((System.Drawing.Image)(resources.GetObject("ResultMenuItemRemoveRows.Image")));
            this.ResultMenuItemRemoveRows.Name = "ResultMenuItemRemoveRows";
            this.ResultMenuItemRemoveRows.Size = new System.Drawing.Size(180, 22);
            this.ResultMenuItemRemoveRows.Text = "Remove Rows";
            this.ResultMenuItemRemoveRows.Click += new System.EventHandler(this.ResultMenuItemRemoveRowsClick);
            // 
            // AbbreviationMenuSeparator01
            // 
            this.AbbreviationMenuSeparator01.Name = "AbbreviationMenuSeparator01";
            this.AbbreviationMenuSeparator01.Size = new System.Drawing.Size(177, 6);
            // 
            // AbbreviationMenuItemCancel
            // 
            this.AbbreviationMenuItemCancel.Image = ((System.Drawing.Image)(resources.GetObject("AbbreviationMenuItemCancel.Image")));
            this.AbbreviationMenuItemCancel.Name = "AbbreviationMenuItemCancel";
            this.AbbreviationMenuItemCancel.Size = new System.Drawing.Size(180, 22);
            this.AbbreviationMenuItemCancel.Text = "Cancel";
            // 
            // ProgressBarPanel
            // 
            this.ProgressBarPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBarPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.ProgressBarPanel.Controls.Add(this.navProgressBar);
            this.ProgressBarPanel.Location = new System.Drawing.Point(0, 283);
            this.ProgressBarPanel.Name = "ProgressBarPanel";
            this.ProgressBarPanel.Size = new System.Drawing.Size(450, 4);
            this.ProgressBarPanel.TabIndex = 1;
            // 
            // navProgressBar
            // 
            this.navProgressBar.CustomText = null;
            this.navProgressBar.DisplayStyle = NAVService.ProgressBarDisplayText.Percentage;
            this.navProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navProgressBar.Location = new System.Drawing.Point(0, 0);
            this.navProgressBar.Name = "navProgressBar";
            this.navProgressBar.Size = new System.Drawing.Size(450, 4);
            this.navProgressBar.TabIndex = 1;
            // 
            // AbbreviatedLabel
            // 
            AbbreviatedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            AbbreviatedLabel.AutoSize = true;
            AbbreviatedLabel.BackColor = System.Drawing.SystemColors.Window;
            AbbreviatedLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            AbbreviatedLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            AbbreviatedLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            AbbreviatedLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            AbbreviatedLabel.Location = new System.Drawing.Point(0, 0);
            AbbreviatedLabel.Name = "AbbreviatedLabel";
            AbbreviatedLabel.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            AbbreviatedLabel.Size = new System.Drawing.Size(78, 15);
            AbbreviatedLabel.TabIndex = 1;
            AbbreviatedLabel.Text = "Abbreviated";
            AbbreviatedLabel.Visible = false;
            // 
            // ExplorerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(453, 400);
            this.ControlBox = false;
            this.Controls.Add(AbbreviatedLabel);
            this.Controls.Add(this.CancelProgressButton);
            this.Controls.Add(ResultDataGridView);
            this.Controls.Add(this.ProgressBarPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExplorerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(ResultDataGridView)).EndInit();
            this.ResultMenu.ResumeLayout(false);
            this.ProgressBarPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip ExplorerToolTips;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bDeleteRowFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn iRowID;
        private System.Windows.Forms.DataGridViewTextBoxColumn iParentRowID;
        private System.Windows.Forms.DataGridViewTextBoxColumn iCompareValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn nvMatchType;
        private System.Windows.Forms.DataGridViewTextBoxColumn nvExplorerString;
        private System.Windows.Forms.Panel ProgressBarPanel;
        private NAVProgressBar navProgressBar;
        private System.Windows.Forms.Button CancelProgressButton;
        private System.Windows.Forms.ContextMenuStrip ResultMenu;
        private System.Windows.Forms.ToolStripMenuItem ResultMenuItemDeleteRow;
        private System.Windows.Forms.ToolStripMenuItem ResultMenuItemRemoveRows;
        private System.Windows.Forms.ToolStripSeparator AbbreviationMenuSeparator01;
        private System.Windows.Forms.ToolStripMenuItem AbbreviationMenuItemCancel;
        private protected static System.Windows.Forms.Label AbbreviatedLabel;
        private protected static System.Windows.Forms.DataGridView ResultDataGridView;

        /// <summary>
        /// Similar to private internal but is now both private AND protected within this class in this assembly
        /// 
        /// private protected static System.Windows.Forms.Label AbbreviatedLabel;
        /// private protected static System.Windows.Forms.DataGridView ResultDataGridView;
        /// 
        /// </summary>

    }
}