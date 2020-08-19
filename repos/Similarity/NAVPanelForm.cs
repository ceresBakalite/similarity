using System;
using System.Linq;
using System.Windows.Forms;

namespace NAVService
{
    public partial class NAVPanelForm : Form
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private static bool NotifyPreferenceChange { get; set; }
        private static bool GetNotifyPreferenceChange() { return NotifyPreferenceChange; }

        private void PanelSliderClick(object sender, EventArgs e) => NAVFormSplitContainer.Panel2Collapsed = !NAVFormSplitContainer.Panel2Collapsed;
        private bool EnablePreferenceChange { get; set; }
        private bool SetEnablePreferenceChange(bool value) => EnablePreferenceChange = value;
        private bool GetEnablePreferenceChange() { return EnablePreferenceChange; }

        private readonly bool HideAbbreviations = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_HIDE_ABBREVIATIONS));
        private readonly bool HideExplorer = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_HIDE_EXPLORER));
        private readonly bool MaintainTabState = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_OPEN_LAST_TAB));
        private readonly bool AddNewAbbreviations = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_ADD_NEW_ABBREVIATIONS));
        private readonly bool DeleteAbbreviations = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_DELETE_ABBREVIATIONS));
        private readonly bool EditAbbreviations = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_EDIT_ABBREVIATIONS));

        protected internal static bool SetNotifyPreferenceChange(bool value) => NotifyPreferenceChange = value;
        protected internal static DataGridView GetAbbreviationsDataGridView() { return AbbreviationsDataGridView; }

        public NAVPanelForm()
        {
            InitializeComponent();
            InitializePanels();
        }

        protected internal static void ApplyPreferenceChange(System.Collections.Generic.Dictionary<int, NAVChangePreferencesModel> map)
        {
            if (map != null)
            {

                string nvClientPreferenceType = map[1].nvClientPreferenceType.Trim();
                string nvClientPreferenceName = map[1].nvClientPreferenceName.Trim();
                bool bPreference = map[1].bPreference;

                switch (nvClientPreferenceType)
                {
                    case Constants.DB_SLIDE_PANEL_TAB_VIEWS:

                        switch (nvClientPreferenceName)
                        {
                            case Constants.DB_HIDE_ABBREVIATIONS: break;
                            case Constants.DB_HIDE_EXPLORER: break;
                            case Constants.DB_OPEN_LAST_TAB: break;
                            default: ClientPreferenceNameError(); break;
                        }

                        break;

                    case Constants.DB_IMPORT_FILE_PERMISSIONS:

                        switch (nvClientPreferenceName)
                        {
                            case Constants.DB_ORDER_COLUMNS: break;
                            case Constants.DB_DELETE_ROWS: break;
                            case Constants.DB_EDIT_CELLS: break;
                            case Constants.DB_CREATE_ROWS: break;
                            case Constants.DB_OPEN_LAST_FILE: break;
                            case Constants.DB_OPEN_LAST_WORKSHEET: break;
                            default: ClientPreferenceNameError(); break;
                        }

                        break;

                    case Constants.DB_ABBREVIATION_TAB_PERMISSIONS:

                        switch (nvClientPreferenceName)
                        {
                            case Constants.DB_REPLACE_WORDS:
                                UserHelper.SetReplaceAllWords(bPreference);
                                ExplorerForm.SetComparisonReset(true);
                                break;

                            case Constants.DB_PERMIT_DEFAULTS:
                                UserHelper.SetReplaceDefaultWordsOnly(bPreference);
                                ExplorerForm.SetComparisonReset(true);
                                break;

                            case Constants.DB_PULL_ABBREVIATIONS:
                                UserHelper.SetPullAbbreviations(bPreference);
                                ExplorerForm.SetComparisonReset(true);
                                break;

                            case Constants.DB_DELETE_ABBREVIATIONS: break;
                            case Constants.DB_ADD_NEW_ABBREVIATIONS: break;
                            case Constants.DB_EDIT_ABBREVIATIONS: break;
                            default: ClientPreferenceNameError(); break;
                        }

                        break;

                    case Constants.DB_ERROR_LOG_PREFERENCES:

                        switch (nvClientPreferenceName)
                        {
                            case Constants.DB_LOG_ERRORS:
                                UserHelper.SetLogErrors(bPreference);
                                break;

                            case Constants.DB_LOG_ERRORS_TO_FILE: break; // TO DO: log4net - disabled (redundant?)
                            case Constants.DB_ERROR_LOG_FILE_LOCATION: break; // TO DO: log4net - disabled (redundant?)
                            case Constants.DB_ERROR_LOG_MAXIMUM_SIZE: break; // TO DO: log4net - disabled (redundant?)
                            case Constants.DB_ERROR_LOG_ARCHIVE_TOTAL: break; // TO DO: log4net - disabled (redundant?)
                            default: ClientPreferenceNameError(); break;
                        }

                        break;

                    case Constants.DB_DATA_CONNECTION_ATTRIBUTES:

                        switch (nvClientPreferenceName)
                        {
                            case Constants.DB_CLOUD_CONNECTION_STRING: break; // TO DO: return value in ConnectionHelper.cs - disabled (redundant?)
                            case Constants.DB_LOCAL_CONNECTION_STRING: break; // TO DO: return value in ConnectionHelper.cs - disabled (redundant?)
                            default: ClientPreferenceNameError(); break;
                        }

                        break;

                    case Constants.DB_USER_INTERFACE_THEMES:

                        switch (nvClientPreferenceName)
                        {
                            case Constants.DB_COLOUR_SCHEME: break; // TO DO: future imiplementation - disabled
                            case Constants.DB_INTERFACE_LANGUAGE: break; // TO DO: future imiplementation - disabled
                            case Constants.DB_APPLY_RTL_ORIENTATION: break; // TO DO: future imiplementation - disabled
                            default: ClientPreferenceNameError(); break;
                        }

                        break;

                    case Constants.DB_SEARCH_PREFERENCES:

                        switch (nvClientPreferenceName)
                        {
                            case Constants.DB_PULL_ABBREVIATIONS: break;
                            case Constants.DB_REMOVE_NOISE_CHARACTERS: break;
                            case Constants.DB_APPLY_CASE_INSENSITIVITY: break;
                            case Constants.DB_PAD_TEXT: break;
                            case Constants.DB_REVERSE_COMPARE: break;
                            case Constants.DB_MATCH_ABBREVIATIONS: break;
                            case Constants.DB_PHONETIC_FILTER: break;
                            case Constants.DB_WHOLE_WORD_MATCH: break;
                            case Constants.DB_PERCENTAGE_INTEREST: break;
                            case Constants.DB_MATCHING_ALGORITHM: break;
                            default: ClientPreferenceNameError(); break;
                        }

                        break;

                    default:

                        ClientPreferenceTypeError();
                        break;

                }

                void ClientPreferenceTypeError()
                {
                    SetNotifyPreferenceChange(false);
                    MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_UPDATE_PREFERENCE, nvClientPreferenceName, nvClientPreferenceType), Properties.Resources.CAPTION_PREFERENCES_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (log != null) log.Error(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_UPDATE_PREFERENCE, nvClientPreferenceName, nvClientPreferenceType));
                }

                void ClientPreferenceNameError()
                {
                    MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_UPDATE_PREFERENCENAME, nvClientPreferenceName), Properties.Resources.CAPTION_PREFERENCES_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (log != null) log.Error(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_UPDATE_PREFERENCENAME, nvClientPreferenceName));
                }

            }

        }

        private void InitializePanels()
        {
            BackColor = Constants.COLOR_DEFAULT_BACKCOLOR;

            SetNotifyPreferenceChange(false);
            SetEnablePreferenceChange(true);

            InitializeLeftPanelForm();
            InitializeRightPanelForm();

            initializeCaptions();
            initializeToolTips();

            LogHelper.TraceInitialisationComplete();

            void NAVFormShownEventHandler(object sender, EventArgs e)
            {
                UpdateHelper.CheckForUpdates();
            }

            void initializeCaptions()
            {
                Text = ConnectionHelper.ApplicationName();
                TABExplorer.Text = Properties.Resources.CAPTION_TAB_EXPLORER;
                TABPreferences.Text = Properties.Resources.CAPTION_TAB_PREFERENCES;
                TABAbbreviations.Text = Properties.Resources.CAPTION_TAB_ABBREVIATIONS;
                LABELExplorer.Text = Properties.Resources.CAPTION_SIMILARITY;
                LABELPreferences.Text = Properties.Resources.CAPTION_PREFERENCES;
                LABELAbbreviations.Text = Properties.Resources.CAPTION_ABBREVIATIONS;
            }

            void initializeToolTips()
            {
                TABToolTips.SetToolTip(PanelSliderPictureBox, Properties.Resources.TOOLTIP_PANELSLIDERPICTUREBOX);
                TABToolTips.SetToolTip(NewButton, Properties.Resources.TOOLTIP_NEWBUTTON);
                TABToolTips.SetToolTip(DeleteButton, Properties.Resources.TOOLTIP_DELETEBUTTON);

                nvWord.ToolTipText = Properties.Resources.TOOLTIP_WORDTEXT;
                nvAbbreviation.ToolTipText = Properties.Resources.TOOLTIP_ABBREVIATIONTEXT;
                nvAbbreviationDescription.ToolTipText = Properties.Resources.TOOLTIP_ABBREVIATIONDESCRIPTIONTEXT;
                bAlwaysUse.ToolTipText = Properties.Resources.TOOLTIP_ALWAYSUSETEXT;
            }

            void InitializeLeftPanelForm()
            {
                NAVFormSplitContainer.BackColor = Constants.COLOR_DEFAULT_BACKCOLOR;
                NAVFormSplitContainer.Panel2Collapsed = true;

                NAVForm.Shown += new EventHandler(NAVFormShownEventHandler);

                NAVForm.TopLevel = false;
                NAVForm.FormBorderStyle = FormBorderStyle.None;
                NAVForm.Parent = NAVFormSplitContainer.Panel1;
                NAVForm.Dock = DockStyle.Fill;

                NAVForm.Show();
            }

            void InitializeRightPanelForm()
            {
                Cursor = Cursors.WaitCursor;

                InitializeAbbreviations();
                InitializePreferences();
                InitializeExplorer();

                Cursor = Cursors.Default;

                void InitializeAbbreviations()
                {
                    AbbreviationsDataGridView.ReadOnly = !EditAbbreviations;
                    AbbreviationsDataGridView.AllowUserToAddRows = false;
                    AbbreviationsDataGridView.AllowUserToDeleteRows = false;
                    AbbreviationsDataGridView.AllowUserToOrderColumns = true;
                    AbbreviationsDataGridView.AllowUserToResizeColumns = false;
                    AbbreviationsDataGridView.AllowUserToResizeRows = false;

                    try
                    {
                        AbbreviationTypeComboBox.BackColor = Constants.COLOR_DEFAULT_BACKCOLOR;
                        AbbreviationTypeComboBox.DataSource = DataAccess.GetAbbreviationTypes();
                        AbbreviationTypeComboBox.DisplayMember = Constants.COLUMN_ABBREVIATION_TYPE;
                        AbbreviationTypeComboBox.SelectedIndexChanged += (object sender, EventArgs e) => GetAbbreviationDataGridView();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        MessageBox.Show(Properties.Resources.NOTIFY_ABBREVIATION_FAIL, Properties.Resources.CAPTION_ABBREVIATIONS, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (log != null) log.Error(Properties.Resources.NOTIFY_ABBREVIATION_FAIL, ex);
                    }

                    NAVPanelFormTabControl.SelectedIndexChanged += TABPreferencesLeave;

                    AbbreviationsDataGridView.CellValueChanged += AbbreviationsCellValueChanged;
                    AbbreviationsDataGridView.RowValidating += AbbreviationsRowValidating;
                    AbbreviationsDataGridView.MouseDown += AbbreviationsDataGridViewMouseDownEvent;
                    AbbreviationsDataGridView.MouseUp += AbbreviationsMouseUp;

                    AbbreviationMenuItemDelete.Enabled = DeleteAbbreviations;
                    AbbreviationMenuItemNew.Enabled = AddNewAbbreviations;

                    DeleteButton.Enabled = DeleteAbbreviations;
                    NewButton.Enabled = AddNewAbbreviations;

                    using (ClassLibraryFramework.DrawingInteropServices.PauseDrawing(NAVPanelFormTabControl))
                    {
                        if (HideAbbreviations)
                        {
                            if (NAVPanelFormTabControl.TabPages.Contains(TABAbbreviations))
                            {
                                NAVPanelFormTabControl.TabPages.Remove(TABAbbreviations);
                            }

                        }
                        else
                        {
                            GetAbbreviationDataGridView();
                        }

                        if (HideExplorer)
                        {
                            if (NAVPanelFormTabControl.TabPages.Contains(TABExplorer))
                            {
                                NAVPanelFormTabControl.TabPages.Remove(TABExplorer);
                            }

                        }

                    }

                }

                void InitializeExplorer()
                {
                    ExplorerForm explorerForm = new ExplorerForm
                    {
                        TopLevel = false,
                        FormBorderStyle = FormBorderStyle.None,
                        Dock = DockStyle.Fill
                    };

                    ExplorerFormPanel.BackColor = Constants.COLOR_DEFAULT_WINDOW;
                    ExplorerFormPanel.Controls.Add(explorerForm);

                    explorerForm.Show();
                }

                void InitializePreferences()
                {
                    PreferencesForm preferencesForm = new PreferencesForm
                    {
                        TopLevel = false,
                        FormBorderStyle = FormBorderStyle.None,
                        Dock = DockStyle.Fill
                    };

                    TABPreferences.BackColor = Constants.COLOR_DEFAULT_WINDOW;

                    PreferenceFormPanel.Controls.Add(preferencesForm);
                    preferencesForm.Show();
                }

            }

        }    

        private void TableLayoutPanelCellPaint(object sender, PaintEventArgs e)
        {
            System.Drawing.Pen penColor = new System.Drawing.Pen(System.Drawing.Color.LightGray);

            e.Graphics.DrawRectangle(penColor, e.ClipRectangle);
            penColor.Dispose();
        }

        private void GetAbbreviationDataGridView(string nvWord = null, string nvAbbreviation = null)
        {
            if (AbbreviationTypeComboBox.SelectedItem is NAVAbbreviationTypeModel objAbbreviationType) 
            {
                Cursor.Current = Cursors.WaitCursor;

                int RowIndex = 0;

                DateTime logHelperStartTime = DateTime.Now;

                using (System.Data.DataTable table = ClassLibraryStandard.DataTableMethods.GetDataTable(DataAccess.GetAbbreviationsByType(objAbbreviationType.iAbbreviationTypeID)))
                {
                    LogHelper.TraceTimeElapsedWriteLine(DateTime.Now, logHelperStartTime, "TRACE - Collecting abbreviations (dataset). Time Elapsed: ");

                    AbbreviationsDataGridView.DataSource = table;

                    if (nvWord != null)
                    {
                        int index = (from row in AbbreviationsDataGridView.Rows.Cast<DataGridViewRow>()
                                     where (string)row.Cells[Constants.COLUMN_ABBREVIATION_WORD].Value == nvWord && (string)row.Cells[Constants.COLUMN_ABBREVIATION].Value == nvAbbreviation
                                     select row.Index).FirstOrDefault();

                        RowIndex = ((object)index is null) ? 0 : index;
                    }

                }

                AbbreviationsDataGridView.FirstDisplayedScrollingRowIndex = AbbreviationsDataGridView.Rows[RowIndex].Index;
                AbbreviationsDataGridView.Rows[RowIndex].Selected = true;

                Cursor.Current = Cursors.Default;
            }

        }

        private void AbbreviationsCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = AbbreviationsDataGridView.CurrentRow;

            if (row == null) return;

            string nvAbbreviation = ((row.Cells[Constants.COLUMN_ABBREVIATION].Value == DBNull.Value) ? string.Empty : row.Cells[Constants.COLUMN_ABBREVIATION].Value).ToString();

            if (!string.IsNullOrWhiteSpace(nvAbbreviation))
            {
                Dapper.DynamicParameters param = new Dapper.DynamicParameters();

                param.Add("@" + Constants.COLUMN_ABBREVIATION_ID, row.Cells[Constants.COLUMN_ABBREVIATION_ID].Value);
                param.Add("@" + Constants.COLUMN_ABBREVIATION, row.Cells[Constants.COLUMN_ABBREVIATION].Value);
                param.Add("@" + Constants.COLUMN_ABBREVIATION_FLAG, row.Cells[Constants.COLUMN_ABBREVIATION_FLAG].Value);

                try
                {
                    DataAccess.UpdateAbbreviation(param);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(Properties.Resources.NOTIFY_ABBREVIATION_CELL, Properties.Resources.CAPTION_ABBREVIATIONS, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (log != null) log.Error(Properties.Resources.NOTIFY_ABBREVIATION_CELL, ex);
                }

            }

        }

        private void AbbreviationsRowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (AbbreviationsDataGridView.Visible)
            {
                DataGridViewRow row = AbbreviationsDataGridView.CurrentRow;

                if (row == null) return;

                string nvAbbreviation = ((row.Cells[Constants.COLUMN_ABBREVIATION].Value == DBNull.Value) ? string.Empty : row.Cells[Constants.COLUMN_ABBREVIATION].Value).ToString();

                if (string.IsNullOrWhiteSpace(nvAbbreviation))
                {
                    row.Cells[Constants.COLUMN_ABBREVIATION].ErrorText = string.Format(UserHelper.culture, Properties.Resources.NOTIFY_ABBREVIATION_DATAGRIDVIEW, row.Cells[Constants.COLUMN_ABBREVIATION_WORD].Value);
                    row.Cells[Constants.COLUMN_ABBREVIATION].Selected = true;

                    e.Cancel = true;
                }
                else
                {
                    row.Cells[Constants.COLUMN_ABBREVIATION].ErrorText = string.Empty;
                }

            }

        }

        private void AbbreviationsMouseUp(object sender, EventArgs e)
        {
            if (AbbreviationsDataGridView.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                if (AbbreviationsDataGridView.AreAllCellsSelected(true))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    foreach (DataGridViewRow row in AbbreviationsDataGridView.Rows) GetSelectedRows(row);
                }

            }

            void GetSelectedRows(DataGridViewRow row)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if ((cell.ColumnIndex == 3) && (cell.Selected))
                    {
                        row.Selected = true;
                        break;
                    }
                }

            }

        }

        private void TABPreferencesLeave(object sender, EventArgs e)
        {
            if (NAVPanelFormTabControl.SelectedTab.Name != "TABPreferences")
            {
                if (GetNotifyPreferenceChange() && GetEnablePreferenceChange())
                {
                        InformPreferenceChanged();
                }

            }

            SaveLastTabOpened(NAVPanelFormTabControl.SelectedTab.Name);

            void SaveLastTabOpened(string selectedTab)
            {
                if (selectedTab != null)
                {
                    if (MaintainTabState)
                    {
                        Dapper.DynamicParameters param = new Dapper.DynamicParameters();

                        param.Add("@" + Constants.COLUMN_USER_ID, UserHelper.UserPropertiesModel.iUserID);
                        param.Add("@" + Constants.COLUMN_USER_LAST_FILENAME_OPENED, UserHelper.UserStateModel.nvLastFilenameOpened);
                        param.Add("@" + Constants.COLUMN_USER_LAST_TAB_FOCUS, selectedTab);
                        param.Add("@" + Constants.COLUMN_USER_LAST_TABLE_FOCUS, UserHelper.UserStateModel.nvLastTableFocus);
                        param.Add("@" + Constants.COLUMN_USER_BUILD_VERSION, Constants.BUILD_VERSION);

                        DataAccess.InsertUserStateLogEntry(param);
                    }

                    UserHelper.UserStateModel.nvLastTabFocus = selectedTab;
                }

            }

        }

        private void NAVFormSplitContainerPanel2Leave(object sender, EventArgs e)
        {
            if (GetNotifyPreferenceChange() && GetEnablePreferenceChange()) InformPreferenceChanged();
        }

        private void InformPreferenceChanged()
        {
            switch (MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_PREFERENCE_CHANGES, System.Environment.NewLine), Properties.Resources.CAPTION_RESTART_APPLICATION, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3))
            {
                case DialogResult.Yes:

                    if (NAVForm.GetSpreadsheetChanges())
                    {
                        switch (MessageBox.Show(Properties.Resources.NOTIFY_SAVEWORKBOOKCHANGES_ON_EXIT, Properties.Resources.CAPTION_SAVE_WORKBOOK, MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                        {
                            case DialogResult.Yes:

                                NAVForm.SaveWorkbook();
                                break;

                            default: break;
                        }

                    }

                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                    Close();

                    break;

                default:
                    SetEnablePreferenceChange(false);
                    break;
            }

            SetNotifyPreferenceChange(false);
        }

        private void AbbreviationsDataGridViewMouseDownEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int visibleRows = (AbbreviationsDataGridView.RowCount == 0) ? 0 : AbbreviationsDataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible);

                AbbreviationMenuItemEdit.Enabled = visibleRows != 0;
                AbbreviationMenuItemDelete.Enabled = visibleRows != 0;

                DataGridView.HitTestInfo hitTest = AbbreviationsDataGridView.HitTest(e.X, e.Y);

                switch (hitTest.Type)
                {
                    case DataGridViewHitTestType.ColumnHeader:

                        AbbreviationsDataGridView.ClearSelection();

                        AbbreviationMenuItemEdit.Enabled = false;
                        AbbreviationMenuItemDelete.Enabled = false;

                        break;

                    case DataGridViewHitTestType.RowHeader:

                        break;

                    case DataGridViewHitTestType.Cell:

                        if (hitTest.RowIndex >= 0)
                        {
                            if (AbbreviationsDataGridView.SelectedRows.Count <= 1)
                            {
                                AbbreviationsDataGridView.ClearSelection();
                                AbbreviationsDataGridView.CurrentCell = AbbreviationsDataGridView[AbbreviationsDataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible).DisplayIndex, hitTest.RowIndex];
                                AbbreviationsDataGridView.Rows[hitTest.RowIndex].Selected = true;
                            }
                            else
                            {
                                if (!ClassLibraryFramework.DataGridViewMethods.GetSelectedRowsIndex(AbbreviationsDataGridView).Contains(hitTest.RowIndex))
                                {
                                    AbbreviationsDataGridView.ClearSelection();
                                    AbbreviationsDataGridView.CurrentCell = AbbreviationsDataGridView[AbbreviationsDataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible).DisplayIndex, hitTest.RowIndex];
                                    AbbreviationsDataGridView.Rows[hitTest.RowIndex].Selected = true;
                                }

                            }

                        }

                        break;

                    default:

                        break;

                }

            }

        }

        private void EditAbbreviationWord(object sender, EventArgs e)
        {
            DataGridViewRow row = AbbreviationsDataGridView.CurrentRow;

            if (row == null) return;

            AbbreviationsDataGridView.ClearSelection();
            AbbreviationsDataGridView.CurrentCell = AbbreviationsDataGridView[AbbreviationsDataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible).DisplayIndex, row.Index];
            AbbreviationsDataGridView.Rows[row.Index].Selected = true;

            using (NAVSymbolForm navSymbolForm = new NAVSymbolForm(true))
            {
                if (navSymbolForm.ShowDialog() == DialogResult.OK)
                {
                    Dapper.DynamicParameters param = new Dapper.DynamicParameters();

                    param.Add("@" + Constants.COLUMN_USER_ID, UserHelper.UserPropertiesModel.iUserID);
                    param.Add("@" + Constants.COLUMN_ABBREVIATION_ID, row.Cells[Constants.COLUMN_ABBREVIATION_ID].Value);
                    param.Add("@" + Constants.COLUMN_ABBREVIATION_WORD, navSymbolForm.nvWord);
                    param.Add("@" + Constants.COLUMN_ABBREVIATION, navSymbolForm.nvAbbreviation);
                    param.Add("@" + Constants.COLUMN_ABBREVIATION_DESCRIPTION, navSymbolForm.nvAbbreviationDescription);
                    param.Add("@" + Constants.COLUMN_ABBREVIATION_FLAG, navSymbolForm.bAlwaysUse);
                    param.Add("@" + Constants.COLUMN_ABBREVIATION_RETURNCODE, dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);

                    try
                    {
                        if (Convert.ToBoolean(DataAccess.EditAbbreviation(param)))
                        {
                            GetAbbreviationDataGridView(navSymbolForm.nvWord, navSymbolForm.nvAbbreviation);
                        }
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        MessageBox.Show(Properties.Resources.NOTIFY_ABBREVIATION_EDIT, Properties.Resources.CAPTION_ABBREVIATIONS, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (log != null) log.Error(Properties.Resources.NOTIFY_ABBREVIATION_EDIT, ex);
                    }

                }

            }

        }

        private void InsertAbbreviationWord(object sender, EventArgs e)
        {
            using (NAVSymbolForm navSymbolForm = new NAVSymbolForm())
            {
                if (navSymbolForm.ShowDialog() == DialogResult.OK)
                {
                    DataGridViewRow row = AbbreviationsDataGridView.CurrentRow;

                    if (row == null) return;

                    Dapper.DynamicParameters param = new Dapper.DynamicParameters();

                    param.Add("@" + Constants.COLUMN_ABBREVIATION_LANGUAGE_ID, UserHelper.UserPropertiesModel.iLanguageID);
                    param.Add("@" + Constants.COLUMN_ABBREVIATIONTYPE_ID, row.Cells[Constants.COLUMN_ABBREVIATIONTYPE_ID].Value);
                    param.Add("@" + Constants.COLUMN_ABBREVIATION_WORD, navSymbolForm.nvWord);
                    param.Add("@" + Constants.COLUMN_ABBREVIATION, navSymbolForm.nvAbbreviation);
                    param.Add("@" + Constants.COLUMN_ABBREVIATION_DESCRIPTION, navSymbolForm.nvAbbreviationDescription);
                    param.Add("@" + Constants.COLUMN_ABBREVIATION_FLAG, navSymbolForm.bAlwaysUse);
                    param.Add("@" + Constants.COLUMN_ABBREVIATION_RETURNCODE, dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.ReturnValue);

                    try
                    {
                        int returnCode = DataAccess.InsertAbbreviation(param);

                        if (returnCode > 0)
                        {
                            try
                            {
                                string nvAbbreviationType = DataAccess.GetAbbreviationType(returnCode);
                                bool bPlural = navSymbolForm.nvWord.Contains(" ");

                                MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_ABBREVIATION_UPDATE, System.Environment.NewLine, (bPlural ? "phrase" : "word"), (bPlural ? "words" : "word"), navSymbolForm.nvWord, navSymbolForm.nvAbbreviation, nvAbbreviationType), Properties.Resources.CAPTION_ABBREVIATIONS, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (log != null) log.Info(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_ABBREVIATION_UPDATE, System.Environment.NewLine, (bPlural ? "phrase" : "word"), (bPlural ? "words" : "word"), navSymbolForm.nvWord, navSymbolForm.nvAbbreviation, nvAbbreviationType));

                            }
                            catch (System.Data.SqlClient.SqlException ex)
                            {
                                MessageBox.Show(Properties.Resources.NOTIFY_ABBREVIATION_TYPE, Properties.Resources.CAPTION_ABBREVIATIONS, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                if (log != null) log.Error(Properties.Resources.NOTIFY_ABBREVIATION_TYPE, ex);
                            }

                        }

                        GetAbbreviationDataGridView(navSymbolForm.nvWord, navSymbolForm.nvAbbreviation);
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        MessageBox.Show(Properties.Resources.NOTIFY_ABBREVIATION_INSERT, Properties.Resources.CAPTION_ABBREVIATIONS, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (log != null) log.Error(Properties.Resources.NOTIFY_ABBREVIATION_INSERT, ex);
                    }

                }

            }

        }

        private void DeleteAbbreviationWords(object sender, EventArgs e)
        {
            int selectedCellCount = AbbreviationsDataGridView.GetCellCount(DataGridViewElementStates.Selected);

            if (selectedCellCount > 0)
            {
                int iRowCount = 0;

                foreach (DataGridViewRow row in AbbreviationsDataGridView.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if ((cell.ColumnIndex == 3) && (cell.Selected))
                        {
                            iRowCount++;
                            break;
                        }

                    }

                }

                if (iRowCount > 0)
                {
                    switch (MessageBox.Show((iRowCount == 1) ? Properties.Resources.NOTIFY_DELETE_ROW : Properties.Resources.NOTIFY_DELETE_SELECTED_ROWS, Properties.Resources.CAPTION_ABBREVIATIONS, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3))
                    {
                        case DialogResult.Yes:

                            Cursor.Current = Cursors.WaitCursor;

                            if (AbbreviationsDataGridView.AreAllCellsSelected(true))
                            {
                                throw new NotImplementedException();
                            }
                            else
                            {
                                foreach (DataGridViewRow row in AbbreviationsDataGridView.Rows)
                                {
                                    foreach (DataGridViewCell cell in row.Cells)
                                    {
                                        if ((cell.ColumnIndex == 3) && (cell.Selected))
                                        {
                                            Dapper.DynamicParameters param = new Dapper.DynamicParameters();

                                            param.Add("@" + Constants.COLUMN_ABBREVIATIONWORD_ID, row.Cells[Constants.COLUMN_ABBREVIATIONWORD_ID].Value);

                                            try
                                            {
                                                DataAccess.DeleteAbbreviation(param);
                                                break;
                                            }
                                            catch (System.Data.SqlClient.SqlException ex)
                                            {
                                                MessageBox.Show(Properties.Resources.NOTIFY_DELETE_ABBREVIATION, Properties.Resources.CAPTION_ABBREVIATIONS, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                if (log != null) log.Error(Properties.Resources.NOTIFY_DELETE_ABBREVIATION, ex);
                                            }
                                        }

                                    }

                                }

                                GetAbbreviationDataGridView();
                            }

                            break;

                        default: break;
                    }

                }
                else
                {
                    MessageBox.Show(Properties.Resources.NOTIFY_SELECT_ABBREVIATION, Properties.Resources.CAPTION_ABBREVIATIONS, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

        }

    }

}

