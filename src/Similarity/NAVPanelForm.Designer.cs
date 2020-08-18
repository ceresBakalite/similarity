namespace NAVService
{
    partial class NAVPanelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NAVPanelForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TABToolTips = new System.Windows.Forms.ToolTip(this.components);
            this.PanelSliderPictureBox = new System.Windows.Forms.PictureBox();
            this.NewButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.NAVFormSplitContainer = new System.Windows.Forms.SplitContainer();
            this.NAVPanelFormTabControl = new System.Windows.Forms.TabControl();
            this.TABExplorer = new System.Windows.Forms.TabPage();
            this.ExplorerFormPanel = new System.Windows.Forms.Panel();
            this.LABELExplorer = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TABPreferences = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.LABELPreferences = new System.Windows.Forms.Label();
            this.PreferenceFormPanel = new System.Windows.Forms.Panel();
            this.TABAbbreviations = new System.Windows.Forms.TabPage();
            AbbreviationsDataGridView = new System.Windows.Forms.DataGridView();
            this.iAbbreviationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iAbbreviationTypeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iWordID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nvWord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nvAbbreviation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nvAbbreviationDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bAlwaysUse = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.iReturnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AbbreviationMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AbbreviationMenuItemNew = new System.Windows.Forms.ToolStripMenuItem();
            this.AbbreviationMenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.AbbreviationMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.AbbreviationMenuSeparator01 = new System.Windows.Forms.ToolStripSeparator();
            this.AbbreviationMenuItemCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.TablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.AbbreviationTypeComboBox = new System.Windows.Forms.ComboBox();
            this.LABELAbbreviations = new System.Windows.Forms.Label();
            this.CogsPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PanelSliderPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NAVFormSplitContainer)).BeginInit();
            this.NAVFormSplitContainer.Panel1.SuspendLayout();
            this.NAVFormSplitContainer.Panel2.SuspendLayout();
            this.NAVFormSplitContainer.SuspendLayout();
            this.NAVPanelFormTabControl.SuspendLayout();
            this.TABExplorer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.TABPreferences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.TABAbbreviations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(AbbreviationsDataGridView)).BeginInit();
            this.AbbreviationMenu.SuspendLayout();
            this.TablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CogsPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelSliderPictureBox
            // 
            this.PanelSliderPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelSliderPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PanelSliderPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("PanelSliderPictureBox.Image")));
            this.PanelSliderPictureBox.Location = new System.Drawing.Point(1043, 915);
            this.PanelSliderPictureBox.Name = "PanelSliderPictureBox";
            this.PanelSliderPictureBox.Size = new System.Drawing.Size(37, 31);
            this.PanelSliderPictureBox.TabIndex = 0;
            this.PanelSliderPictureBox.TabStop = false;
            this.PanelSliderPictureBox.Click += new System.EventHandler(this.PanelSliderClick);
            // 
            // NewButton
            // 
            this.NewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.NewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NewButton.Location = new System.Drawing.Point(230, 57);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(51, 23);
            this.NewButton.TabIndex = 110;
            this.NewButton.Text = "New";
            this.NewButton.UseVisualStyleBackColor = true;
            this.NewButton.Click += new System.EventHandler(this.InsertAbbreviationWord);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteButton.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.DeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteButton.Location = new System.Drawing.Point(174, 57);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(51, 23);
            this.DeleteButton.TabIndex = 111;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteAbbreviationWords);
            // 
            // NAVFormSplitContainer
            // 
            this.NAVFormSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NAVFormSplitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.NAVFormSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NAVFormSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.NAVFormSplitContainer.MinimumSize = new System.Drawing.Size(100, 0);
            this.NAVFormSplitContainer.Name = "NAVFormSplitContainer";
            // 
            // NAVFormSplitContainer.Panel1
            // 
            this.NAVFormSplitContainer.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.NAVFormSplitContainer.Panel1.Controls.Add(this.PanelSliderPictureBox);
            this.NAVFormSplitContainer.Panel1MinSize = 440;
            // 
            // NAVFormSplitContainer.Panel2
            // 
            this.NAVFormSplitContainer.Panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.NAVFormSplitContainer.Panel2.Controls.Add(this.NAVPanelFormTabControl);
            this.NAVFormSplitContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.NAVFormSplitContainer.Panel2.Leave += new System.EventHandler(this.NAVFormSplitContainerPanel2Leave);
            this.NAVFormSplitContainer.Panel2MinSize = 290;
            this.NAVFormSplitContainer.Size = new System.Drawing.Size(1392, 960);
            this.NAVFormSplitContainer.SplitterDistance = 1097;
            this.NAVFormSplitContainer.TabIndex = 100;
            // 
            // NAVPanelFormTabControl
            // 
            this.NAVPanelFormTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NAVPanelFormTabControl.Controls.Add(this.TABExplorer);
            this.NAVPanelFormTabControl.Controls.Add(this.TABPreferences);
            this.NAVPanelFormTabControl.Controls.Add(this.TABAbbreviations);
            this.NAVPanelFormTabControl.HotTrack = true;
            this.NAVPanelFormTabControl.Location = new System.Drawing.Point(-1, -1);
            this.NAVPanelFormTabControl.Name = "NAVPanelFormTabControl";
            this.NAVPanelFormTabControl.SelectedIndex = 0;
            this.NAVPanelFormTabControl.Size = new System.Drawing.Size(291, 960);
            this.NAVPanelFormTabControl.TabIndex = 104;
            // 
            // TABExplorer
            // 
            this.TABExplorer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.TABExplorer.Controls.Add(this.ExplorerFormPanel);
            this.TABExplorer.Controls.Add(this.LABELExplorer);
            this.TABExplorer.Controls.Add(this.pictureBox1);
            this.TABExplorer.Location = new System.Drawing.Point(4, 22);
            this.TABExplorer.Name = "TABExplorer";
            this.TABExplorer.Padding = new System.Windows.Forms.Padding(3);
            this.TABExplorer.Size = new System.Drawing.Size(283, 934);
            this.TABExplorer.TabIndex = 0;
            this.TABExplorer.Text = "Explorer";
            // 
            // ExplorerFormPanel
            // 
            this.ExplorerFormPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExplorerFormPanel.AutoScroll = true;
            this.ExplorerFormPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.ExplorerFormPanel.Location = new System.Drawing.Point(0, 51);
            this.ExplorerFormPanel.Name = "ExplorerFormPanel";
            this.ExplorerFormPanel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ExplorerFormPanel.Size = new System.Drawing.Size(283, 905);
            this.ExplorerFormPanel.TabIndex = 111;
            // 
            // LABELExplorer
            // 
            this.LABELExplorer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LABELExplorer.AutoSize = true;
            this.LABELExplorer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LABELExplorer.ForeColor = System.Drawing.Color.SteelBlue;
            this.LABELExplorer.Location = new System.Drawing.Point(53, 15);
            this.LABELExplorer.Name = "LABELExplorer";
            this.LABELExplorer.Size = new System.Drawing.Size(133, 16);
            this.LABELExplorer.TabIndex = 112;
            this.LABELExplorer.Text = "Similarity Patterns";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(10, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 39);
            this.pictureBox1.TabIndex = 102;
            this.pictureBox1.TabStop = false;
            // 
            // TABPreferences
            // 
            this.TABPreferences.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.TABPreferences.Controls.Add(this.pictureBox2);
            this.TABPreferences.Controls.Add(this.LABELPreferences);
            this.TABPreferences.Controls.Add(this.PreferenceFormPanel);
            this.TABPreferences.Location = new System.Drawing.Point(4, 22);
            this.TABPreferences.Name = "TABPreferences";
            this.TABPreferences.Padding = new System.Windows.Forms.Padding(3);
            this.TABPreferences.Size = new System.Drawing.Size(283, 923);
            this.TABPreferences.TabIndex = 1;
            this.TABPreferences.Text = "Preferences";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(7, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(35, 39);
            this.pictureBox2.TabIndex = 108;
            this.pictureBox2.TabStop = false;
            // 
            // LABELPreferences
            // 
            this.LABELPreferences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LABELPreferences.AutoSize = true;
            this.LABELPreferences.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LABELPreferences.ForeColor = System.Drawing.Color.SteelBlue;
            this.LABELPreferences.Location = new System.Drawing.Point(53, 15);
            this.LABELPreferences.Name = "LABELPreferences";
            this.LABELPreferences.Size = new System.Drawing.Size(167, 16);
            this.LABELPreferences.TabIndex = 109;
            this.LABELPreferences.Text = "Preferences and Views";
            // 
            // PreferenceFormPanel
            // 
            this.PreferenceFormPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PreferenceFormPanel.AutoScroll = true;
            this.PreferenceFormPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.PreferenceFormPanel.Location = new System.Drawing.Point(0, 58);
            this.PreferenceFormPanel.Name = "PreferenceFormPanel";
            this.PreferenceFormPanel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PreferenceFormPanel.Size = new System.Drawing.Size(286, 832);
            this.PreferenceFormPanel.TabIndex = 110;
            // 
            // TABAbbreviations
            // 
            this.TABAbbreviations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.TABAbbreviations.Controls.Add(AbbreviationsDataGridView);
            this.TABAbbreviations.Controls.Add(this.TablePanel);
            this.TABAbbreviations.Controls.Add(this.DeleteButton);
            this.TABAbbreviations.Controls.Add(this.NewButton);
            this.TABAbbreviations.Controls.Add(this.LABELAbbreviations);
            this.TABAbbreviations.Controls.Add(this.CogsPictureBox);
            this.TABAbbreviations.Location = new System.Drawing.Point(4, 22);
            this.TABAbbreviations.Name = "TABAbbreviations";
            this.TABAbbreviations.Padding = new System.Windows.Forms.Padding(3);
            this.TABAbbreviations.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TABAbbreviations.Size = new System.Drawing.Size(283, 923);
            this.TABAbbreviations.TabIndex = 2;
            this.TABAbbreviations.Text = "Abbreviations";
            // 
            // AbbreviationsDataGridView
            // 
            AbbreviationsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            AbbreviationsDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            AbbreviationsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            AbbreviationsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            AbbreviationsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AbbreviationsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iAbbreviationID,
            this.iAbbreviationTypeID,
            this.iWordID,
            this.nvWord,
            this.nvAbbreviation,
            this.nvAbbreviationDescription,
            this.bAlwaysUse,
            this.iReturnCode});
            AbbreviationsDataGridView.ContextMenuStrip = this.AbbreviationMenu;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(235)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            AbbreviationsDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            AbbreviationsDataGridView.GridColor = System.Drawing.SystemColors.Control;
            AbbreviationsDataGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            AbbreviationsDataGridView.Location = new System.Drawing.Point(-1, 93);
            AbbreviationsDataGridView.Name = "AbbreviationsDataGridView";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            AbbreviationsDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            AbbreviationsDataGridView.RowHeadersVisible = false;
            AbbreviationsDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            AbbreviationsDataGridView.Size = new System.Drawing.Size(282, 820);
            AbbreviationsDataGridView.TabIndex = 108;
            // 
            // iAbbreviationID
            // 
            this.iAbbreviationID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.iAbbreviationID.DataPropertyName = "iAbbreviationID";
            this.iAbbreviationID.FillWeight = 5F;
            this.iAbbreviationID.HeaderText = "iAbbreviationID";
            this.iAbbreviationID.Name = "iAbbreviationID";
            this.iAbbreviationID.ReadOnly = true;
            this.iAbbreviationID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.iAbbreviationID.Visible = false;
            this.iAbbreviationID.Width = 5;
            // 
            // iAbbreviationTypeID
            // 
            this.iAbbreviationTypeID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.iAbbreviationTypeID.DataPropertyName = "iAbbreviationTypeID";
            this.iAbbreviationTypeID.HeaderText = "iAbbreviationTypeID";
            this.iAbbreviationTypeID.Name = "iAbbreviationTypeID";
            this.iAbbreviationTypeID.ReadOnly = true;
            this.iAbbreviationTypeID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.iAbbreviationTypeID.Visible = false;
            this.iAbbreviationTypeID.Width = 5;
            // 
            // iWordID
            // 
            this.iWordID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.iWordID.DataPropertyName = "iWordID";
            this.iWordID.HeaderText = "iWordID";
            this.iWordID.Name = "iWordID";
            this.iWordID.ReadOnly = true;
            this.iWordID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.iWordID.Visible = false;
            // 
            // nvWord
            // 
            this.nvWord.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nvWord.DataPropertyName = "nvWord";
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.Window;
            this.nvWord.DefaultCellStyle = dataGridViewCellStyle2;
            this.nvWord.HeaderText = "Word";
            this.nvWord.Name = "nvWord";
            this.nvWord.ReadOnly = true;
            // 
            // nvAbbreviation
            // 
            this.nvAbbreviation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nvAbbreviation.DataPropertyName = "nvAbbreviation";
            this.nvAbbreviation.HeaderText = "Symbol";
            this.nvAbbreviation.Name = "nvAbbreviation";
            this.nvAbbreviation.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.nvAbbreviation.Width = 66;
            // 
            // nvAbbreviationDescription
            // 
            this.nvAbbreviationDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.nvAbbreviationDescription.DataPropertyName = "nvAbbreviationDescription";
            this.nvAbbreviationDescription.HeaderText = "Description";
            this.nvAbbreviationDescription.Name = "nvAbbreviationDescription";
            this.nvAbbreviationDescription.ReadOnly = true;
            this.nvAbbreviationDescription.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.nvAbbreviationDescription.Visible = false;
            // 
            // bAlwaysUse
            // 
            this.bAlwaysUse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.bAlwaysUse.DataPropertyName = "bAlwaysUse";
            this.bAlwaysUse.FalseValue = "0";
            this.bAlwaysUse.HeaderText = "Default";
            this.bAlwaysUse.IndeterminateValue = "0";
            this.bAlwaysUse.Name = "bAlwaysUse";
            this.bAlwaysUse.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.bAlwaysUse.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.bAlwaysUse.TrueValue = "1";
            this.bAlwaysUse.Width = 66;
            // 
            // iReturnCode
            // 
            this.iReturnCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.iReturnCode.DataPropertyName = "iReturnCode";
            this.iReturnCode.FillWeight = 5F;
            this.iReturnCode.HeaderText = "iReturnCode";
            this.iReturnCode.Name = "iReturnCode";
            this.iReturnCode.ReadOnly = true;
            this.iReturnCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.iReturnCode.Visible = false;
            this.iReturnCode.Width = 5;
            // 
            // AbbreviationMenu
            // 
            this.AbbreviationMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AbbreviationMenuItemNew,
            this.AbbreviationMenuItemEdit,
            this.AbbreviationMenuItemDelete,
            this.AbbreviationMenuSeparator01,
            this.AbbreviationMenuItemCancel});
            this.AbbreviationMenu.Name = "AbbreviationContextMenuStrip";
            this.AbbreviationMenu.Size = new System.Drawing.Size(111, 98);
            // 
            // AbbreviationMenuItemNew
            // 
            this.AbbreviationMenuItemNew.Image = ((System.Drawing.Image)(resources.GetObject("AbbreviationMenuItemNew.Image")));
            this.AbbreviationMenuItemNew.Name = "AbbreviationMenuItemNew";
            this.AbbreviationMenuItemNew.Size = new System.Drawing.Size(110, 22);
            this.AbbreviationMenuItemNew.Text = "New";
            this.AbbreviationMenuItemNew.Click += new System.EventHandler(this.InsertAbbreviationWord);
            // 
            // AbbreviationMenuItemEdit
            // 
            this.AbbreviationMenuItemEdit.Image = ((System.Drawing.Image)(resources.GetObject("AbbreviationMenuItemEdit.Image")));
            this.AbbreviationMenuItemEdit.Name = "AbbreviationMenuItemEdit";
            this.AbbreviationMenuItemEdit.Size = new System.Drawing.Size(110, 22);
            this.AbbreviationMenuItemEdit.Text = "Edit";
            this.AbbreviationMenuItemEdit.Click += new System.EventHandler(this.EditAbbreviationWord);
            // 
            // AbbreviationMenuItemDelete
            // 
            this.AbbreviationMenuItemDelete.Image = ((System.Drawing.Image)(resources.GetObject("AbbreviationMenuItemDelete.Image")));
            this.AbbreviationMenuItemDelete.Name = "AbbreviationMenuItemDelete";
            this.AbbreviationMenuItemDelete.Size = new System.Drawing.Size(110, 22);
            this.AbbreviationMenuItemDelete.Text = "Delete";
            this.AbbreviationMenuItemDelete.Click += new System.EventHandler(this.DeleteAbbreviationWords);
            // 
            // AbbreviationMenuSeparator01
            // 
            this.AbbreviationMenuSeparator01.Name = "AbbreviationMenuSeparator01";
            this.AbbreviationMenuSeparator01.Size = new System.Drawing.Size(107, 6);
            // 
            // AbbreviationMenuItemCancel
            // 
            this.AbbreviationMenuItemCancel.Image = ((System.Drawing.Image)(resources.GetObject("AbbreviationMenuItemCancel.Image")));
            this.AbbreviationMenuItemCancel.Name = "AbbreviationMenuItemCancel";
            this.AbbreviationMenuItemCancel.Size = new System.Drawing.Size(110, 22);
            this.AbbreviationMenuItemCancel.Text = "Cancel";
            // 
            // TablePanel
            // 
            this.TablePanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.TablePanel.ColumnCount = 1;
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TablePanel.Controls.Add(this.AbbreviationTypeComboBox, 0, 0);
            this.TablePanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.TablePanel.Location = new System.Drawing.Point(0, 57);
            this.TablePanel.Margin = new System.Windows.Forms.Padding(0);
            this.TablePanel.Name = "TablePanel";
            this.TablePanel.RowCount = 1;
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TablePanel.Size = new System.Drawing.Size(153, 23);
            this.TablePanel.TabIndex = 112;
            this.TablePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.TableLayoutPanelCellPaint);
            // 
            // AbbreviationTypeComboBox
            // 
            this.AbbreviationTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AbbreviationTypeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(252)))));
            this.AbbreviationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AbbreviationTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AbbreviationTypeComboBox.FormattingEnabled = true;
            this.AbbreviationTypeComboBox.Location = new System.Drawing.Point(1, 1);
            this.AbbreviationTypeComboBox.Margin = new System.Windows.Forms.Padding(0);
            this.AbbreviationTypeComboBox.Name = "AbbreviationTypeComboBox";
            this.AbbreviationTypeComboBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.AbbreviationTypeComboBox.Size = new System.Drawing.Size(151, 21);
            this.AbbreviationTypeComboBox.TabIndex = 110;
            // 
            // LABELAbbreviations
            // 
            this.LABELAbbreviations.AutoSize = true;
            this.LABELAbbreviations.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LABELAbbreviations.ForeColor = System.Drawing.Color.SteelBlue;
            this.LABELAbbreviations.Location = new System.Drawing.Point(53, 15);
            this.LABELAbbreviations.Name = "LABELAbbreviations";
            this.LABELAbbreviations.Size = new System.Drawing.Size(198, 16);
            this.LABELAbbreviations.TabIndex = 106;
            this.LABELAbbreviations.Text = "Abbreviations and Symbols";
            // 
            // CogsPictureBox
            // 
            this.CogsPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("CogsPictureBox.Image")));
            this.CogsPictureBox.Location = new System.Drawing.Point(5, 6);
            this.CogsPictureBox.Name = "CogsPictureBox";
            this.CogsPictureBox.Size = new System.Drawing.Size(42, 39);
            this.CogsPictureBox.TabIndex = 103;
            this.CogsPictureBox.TabStop = false;
            // 
            // NAVPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1392, 960);
            this.Controls.Add(this.NAVFormSplitContainer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(750, 39);
            this.Name = "NAVPanelForm";
            this.Text = "Similarity Explorer";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.PanelSliderPictureBox)).EndInit();
            this.NAVFormSplitContainer.Panel1.ResumeLayout(false);
            this.NAVFormSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NAVFormSplitContainer)).EndInit();
            this.NAVFormSplitContainer.ResumeLayout(false);
            this.NAVPanelFormTabControl.ResumeLayout(false);
            this.TABExplorer.ResumeLayout(false);
            this.TABExplorer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.TABPreferences.ResumeLayout(false);
            this.TABPreferences.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.TABAbbreviations.ResumeLayout(false);
            this.TABAbbreviations.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(AbbreviationsDataGridView)).EndInit();
            this.AbbreviationMenu.ResumeLayout(false);
            this.TablePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CogsPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        NAVForm NAVForm = new NAVForm();

        private System.Windows.Forms.PictureBox PanelSliderPictureBox;
        private System.Windows.Forms.SplitContainer NAVFormSplitContainer;
        private System.Windows.Forms.ToolTip TABToolTips;
        private System.Windows.Forms.ContextMenuStrip AbbreviationMenu;
        private System.Windows.Forms.ToolStripMenuItem AbbreviationMenuItemNew;
        private System.Windows.Forms.ToolStripMenuItem AbbreviationMenuItemDelete;
        private System.Windows.Forms.ToolStripSeparator AbbreviationMenuSeparator01;
        private System.Windows.Forms.ToolStripMenuItem AbbreviationMenuItemCancel;
        private System.Windows.Forms.TabControl NAVPanelFormTabControl;
        private System.Windows.Forms.TabPage TABExplorer;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabPage TABPreferences;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label LABELPreferences;
        private System.Windows.Forms.TabPage TABAbbreviations;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.Label LABELAbbreviations;
        private System.Windows.Forms.PictureBox CogsPictureBox;
        private System.Windows.Forms.Panel PreferenceFormPanel;
        private System.Windows.Forms.TableLayoutPanel TablePanel;
        private System.Windows.Forms.ComboBox AbbreviationTypeComboBox;
        private System.Windows.Forms.Label LABELExplorer;
        private System.Windows.Forms.Panel ExplorerFormPanel;
        private System.Windows.Forms.ToolStripMenuItem AbbreviationMenuItemEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn iAbbreviationID;
        private System.Windows.Forms.DataGridViewTextBoxColumn iAbbreviationTypeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn iWordID;
        private System.Windows.Forms.DataGridViewTextBoxColumn nvWord;
        private System.Windows.Forms.DataGridViewTextBoxColumn nvAbbreviation;
        private System.Windows.Forms.DataGridViewTextBoxColumn nvAbbreviationDescription;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bAlwaysUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn iReturnCode;
        private protected static System.Windows.Forms.DataGridView AbbreviationsDataGridView;

        /// <summary>
        /// Similar to private internal but is now both private AND protected within this class in this assembly
        /// 
        /// private protected static System.Windows.Forms.DataGridView AbbreviationsDataGridView;
        /// 
        /// </summary>

    }
}