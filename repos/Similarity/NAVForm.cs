using System.Windows.Forms;
using ExcelDataReader;

namespace NAVService
{
    public partial class NAVForm : Form
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();

        private void SheetDataGridViewDragEnter(object sender, DragEventArgs e) => e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.Move : DragDropEffects.None;
        private void SheetComboBoxKeyPress(object sender, KeyPressEventArgs e) => e.Handled = true;
        private void SheetContextMenuStripClosedEvent(object sender, ToolStripDropDownClosedEventArgs e) => DeleteColumnMenuItem.Enabled = true;
        private void GetDeleteRowsMenuItemClick(object sender, System.EventArgs e) => WorkTables.DeleteRowsMenuItemClick();
        private void RestoreColumnMenuItemClick(object sender, System.EventArgs e) => RestoreColumnMenuItemClickEvent();
        private void SaveWorkbookButtonClick(object sender, System.EventArgs e) => SaveWorkbook();

        private static bool SaveSpreadsheetChanges { get; set; }
        private static int SheetDataGridViewInitialTotal { get; set; }
        private static int SheetColumnInitialTotal { get; set; }
        private static int GetSheetColumnInitialTotal() { return SheetColumnInitialTotal; }
        private static int SetSheetColumnInitialTotal(int value) => SheetColumnInitialTotal = value;
        private static void SetSheetDataGridViewInitialTotal(int value) => SheetDataGridViewInitialTotal = value;

        private readonly bool AdjustColumns = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_ORDER_COLUMNS));
        private readonly bool CreateRows = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_CREATE_ROWS));
        private readonly bool DeleteRows = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_DELETE_ROWS));
        private readonly bool EditCells = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_EDIT_CELLS));
        private readonly bool MaintainFileState = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_OPEN_LAST_FILE));
        private readonly bool MaintainTableState = ClassLibraryStandard.HelperMethods.ToBoolean(DataAccess.GetUserPreferenceByPreferenceName(Constants.DB_OPEN_LAST_WORKSHEET));

        protected internal static System.Data.DataTableCollection DataTableCollection { get; private set; }
        protected internal static DataGridView GetSheetDataGridView() { return SheetDataGridView; }
        protected internal static bool GetSpreadsheetChanges() { return SaveSpreadsheetChanges; }
        protected internal static int GetSheetDataGridViewInitialTotal() { return SheetDataGridViewInitialTotal; }
        protected internal static void SetSpreadsheetChanges(bool value) => SaveSpreadsheetChanges = value;
        protected internal static void SetSheetCurrentTotal(string value) => SheetCurrentTotal.Text = value;
        protected internal static void SuspendSheetDataGridView() => ClassLibraryFramework.DrawingInteropServices.SuspendDrawing(SheetDataGridView);

        public NAVForm()
        {
            InitializeComponent();
            InitializeNAVForm();
        }

        protected internal static void ResumeSheetDataGridView()
        {
            SheetDataGridView.ResumeLayout();
            ClassLibraryFramework.DrawingInteropServices.ResumeDrawing(SheetDataGridView);
        }

        protected internal void SaveWorkbook()
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.CreatePrompt = false;
                    saveDialog.OverwritePrompt = true;
                    saveDialog.RestoreDirectory = false;
                    saveDialog.InitialDirectory = System.IO.Path.GetDirectoryName(FilenameTextBox.Text);
                    saveDialog.FileName = System.IO.Path.GetFileNameWithoutExtension(FilenameTextBox.Text);
                    saveDialog.Filter = Properties.Resources.FILE_SAVEFILE_FILTER;
                    saveDialog.DefaultExt = Constants.FILE_SAVEFILE_EXTENSION;
                    saveDialog.FilterIndex = 2;

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.Stream dtStream;

                        if ((dtStream = saveDialog.OpenFile()) != null)
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            dtStream.Close();
                            if (System.Threading.Tasks.Task.Run(() => { return ExportDataTableCollection(saveDialog.FileName); }).Result) FilenameTextBox.Text = saveDialog.FileName;

                            Cursor.Current = Cursors.Default;
                        }

                        SaveAsLastFileOpened(saveDialog.FileName);
                    }

                    SetSpreadsheetChanges(false);
                }

            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_FILESAVE_ERROR, System.IO.Path.GetFileNameWithoutExtension(FilenameTextBox.Text)), Properties.Resources.CAPTION_SAVE_WORKBOOK, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (log != null) log.Error(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_FILESAVE_ERROR, System.IO.Path.GetFileNameWithoutExtension(FilenameTextBox.Text)), ex);
            }

            bool ExportDataTableCollection(string destination)
            {
                using (System.Data.DataSet ExportDataSet = new System.Data.DataSet())
                {
                    foreach (System.Data.DataTable WorkTable in DataTableCollection)
                    {
                        WorkTable.AcceptChanges();
                        ExportDataSet.Merge(WorkTable);
                    }

                    System.Data.DataTableCollection ExportTableCollection = ExportDataSet.Tables;

                    foreach (System.Data.DataTable ExportTable in ExportTableCollection)
                    {
                        ExportTable.Constraints.Clear();
                        ClassLibraryStandard.DataTableMethods.DeleteColumn(ExportTable, Constants.KEY_COLUMN);
                    }

                    return CreateExcelFile.CreateExcelFile.ExportExcelDocument(ExportTableCollection, destination);
                }

            }

        }

        private void OpenWorbook(string fileName)
        {
            try
            {
                bool bContinue = true;

                if (string.IsNullOrEmpty(fileName))
                {
                    bContinue = false;

                    using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = Properties.Resources.FILE_OPENFILE_FILTER })
                    {
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            fileName = openFileDialog.FileName;
                            openFileDialog.RestoreDirectory = false;
                            bContinue = true;
                        }

                    }

                }

                if (bContinue)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    System.Threading.Tasks.Task WorbookOpenThread = System.Threading.Tasks.Task.Run(() => { WorbookOpen(); });
                    WorbookOpenThread.Wait();

                    CatchupUIThread();

                    Cursor.Current = Cursors.Default;
                }

                void WorbookOpen()
                {
                    using (System.IO.FileStream stream = System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        using (System.Data.DataSet WorkBookDataSet = new System.Data.DataSet())
                        {
                            System.Data.DataSet workbookdataset = WorkBookDataSet;

                            switch (System.IO.Path.GetExtension(fileName).ToUpperInvariant())
                            {
                                case Constants.FILE_CSV_EXTENSION:

                                    using (IExcelDataReader reader = ExcelReaderFactory.CreateCsvReader(stream))
                                    {
                                        workbookdataset = reader.AsDataSet(new ExcelDataSetConfiguration()
                                        {
                                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                        });

                                        DataTableCollection = workbookdataset.Tables;
                                    }

                                    break;

                                case Constants.FILE_XLS_EXTENSION:

                                    using (IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(stream))
                                    {
                                        workbookdataset = reader.AsDataSet(new ExcelDataSetConfiguration()
                                        {
                                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                        });

                                        DataTableCollection = workbookdataset.Tables;
                                    }

                                    break;

                                default:

                                    using (IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                                    {
                                        workbookdataset = reader.AsDataSet(new ExcelDataSetConfiguration()
                                        {
                                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                        });

                                        DataTableCollection = workbookdataset.Tables;
                                    }

                                    break;

                            }

                            DropPreLoadPictureBox.Visible = false;
                            workbookdataset.Dispose();
                        }

                        ClassLibraryStandard.DataTableMethods.CreateAutoIncrementID(DataTableCollection, Constants.KEY_COLUMN);
                    }

                }

                void CatchupUIThread()
                {
                    FilenameTextBox.Text = fileName;
                    SheetComboBox.Items.Clear();

                    foreach (System.Data.DataTable table in DataTableCollection) SheetComboBox.Items.Add(table.TableName);

                    SaveAsLastFileOpened(fileName);

                    SheetComboBox.SelectedIndex = 0;
                    SheetComboBox.Enabled = true;
                    SaveWorkbookButton.Enabled = true;

                    SetSpreadsheetChanges(false);
                }

            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show(ClassLibraryStandard.FileInteropServices.IsFileLocked(ex) ? string.Format(UserHelper.culture, Properties.Resources.NOTIFY_FILEINUSE, System.Environment.NewLine, fileName) : string.Format(UserHelper.culture, Properties.Resources.NOTIFY_FILEOPEN_FAILED, System.Environment.NewLine, fileName), Properties.Resources.CAPTION_OPEN_WORKBOOK, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (log != null) log.Error(ClassLibraryStandard.FileInteropServices.IsFileLocked(ex) ? string.Format(UserHelper.culture, Properties.Resources.NOTIFY_FILEINUSE, System.Environment.NewLine, fileName) : string.Format(UserHelper.culture, Properties.Resources.NOTIFY_FILEOPEN_FAILED, System.Environment.NewLine, fileName), ex);

                if (FilenameTextBox.Text == fileName) FilenameTextBox.Clear();
            }

        }

        private void InitializeNAVForm()
        {
            InitialiseDragDrop();
            InitialiseSheetDataGridView();
            InitialiseSpreadsheet();
            ReloadLastFileOpened();
            initializeToolTips();

            void InitialiseSpreadsheet()
            {
                SheetCurrentTotal.MouseEnter += DrawControls.LabelBackColorChange;
                SheetCurrentTotal.MouseLeave += DrawControls.LabelBackColorChange;
                SheetContextMenuStrip.Closed += SheetContextMenuStripClosedEvent;
                SetSpreadsheetChanges(false);
            }

            void InitialiseDragDrop()
            {
                DropPreLoadPictureBox.AllowDrop = true;
                DropPreLoadPictureBox.DragDrop += SheetDataGridViewDragDrop;
                DropPreLoadPictureBox.DragEnter += SheetDataGridViewDragEnter;
                SheetDataGridView.DragDrop += SheetDataGridViewDragDrop;
                SheetDataGridView.DragEnter += SheetDataGridViewDragEnter;
            }

            void InitialiseSheetDataGridView()
            {
                SheetDataGridView.CellValueChanged += SheetDataGridViewCellValueChanged;
                SheetDataGridView.UserDeletingRow += UserDeletingRowEvent;
                SheetDataGridView.MouseDown += SheetDataGridViewMouseDownEvent;
                SheetDataGridView.AllowUserToOrderColumns = AdjustColumns;
                SheetDataGridView.AllowUserToAddRows = CreateRows;
                SheetDataGridView.AllowUserToDeleteRows = DeleteRows;
                SheetDataGridView.ReadOnly = !EditCells;
            }


            void ReloadLastFileOpened()
            {
                if (MaintainFileState)
                {
                    if (UserHelper.UserStateModel.nvLastFilenameOpened != null)
                    {
                        if (System.IO.File.Exists(UserHelper.UserStateModel.nvLastFilenameOpened))
                        {
                            string extension = System.IO.Path.GetExtension(UserHelper.UserStateModel.nvLastFilenameOpened).ToUpperInvariant();
                            bool bImportFile = extension == Constants.FILE_XLSX_EXTENSION || extension == Constants.FILE_XLS_EXTENSION || extension == Constants.FILE_CSV_EXTENSION;

                            if (bImportFile) OpenWorbook(UserHelper.UserStateModel.nvLastFilenameOpened);
                        }

                    }

                }

            }

            void initializeToolTips()
            {
                NAVFormToolTip.SetToolTip(AddFileButton, Properties.Resources.TOOLTIP_ADDFILEBUTTON);
                NAVFormToolTip.SetToolTip(SheetComboBox, Properties.Resources.TOOLTIP_SHEETCOMBOBOX);
                NAVFormToolTip.SetToolTip(SaveWorkbookButton, Properties.Resources.TOOLTIP_SAVEWORKBOOKBUTTON);
                NAVFormToolTip.SetToolTip(ExitButton, Properties.Resources.TOOLTIP_EXITBUTTON);
            }

        }

        private static void InitialiseDataSource()
        {
            SheetDataGridView.DataSource = DataTableCollection[UserHelper.UserStateModel.nvLastTableFocus];
            SheetDataGridView.Columns[Constants.KEY_COLUMN].Visible = false;
            SheetDataGridView.Sort(SheetDataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible), System.ComponentModel.ListSortDirection.Ascending);

            SetSheetColumnInitialTotal(SheetDataGridView.Columns.Count - 1);
            SetSheetDataGridViewInitialTotal(SheetDataGridView.RowCount - 1);

            SetSheetCurrentTotal(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_CURRENT_TOTAL, GetSheetDataGridViewInitialTotal()));
        }

        private static void SaveAsLastFileOpened(string fileName)
        {
            if (fileName != null)
            {
                Dapper.DynamicParameters param = new Dapper.DynamicParameters();

                param.Add("@" + Constants.COLUMN_USER_ID, UserHelper.UserPropertiesModel.iUserID);
                param.Add("@" + Constants.COLUMN_USER_LAST_FILENAME_OPENED, fileName);
                param.Add("@" + Constants.COLUMN_USER_LAST_TAB_FOCUS, UserHelper.UserStateModel.nvLastTabFocus);
                param.Add("@" + Constants.COLUMN_USER_LAST_TABLE_FOCUS, UserHelper.UserStateModel.nvLastTableFocus);
                param.Add("@" + Constants.COLUMN_USER_BUILD_VERSION, Constants.BUILD_VERSION);

                DataAccess.InsertUserStateLogEntry(param);
                UserHelper.UserStateModel.nvLastFilenameOpened = fileName;
                ExplorerForm.ResetExplorerForm(); 
            }

        }

        private static void RestoreColumnMenuItemClickEvent()
        {
            DataGridViewColumn firstVisibleColumn = SheetDataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
            bool bComparisonRebuild = false;

            foreach (DataGridViewColumn column in SheetDataGridView.Columns)
            {
                if (column.Name != Constants.KEY_COLUMN)
                {
                    if (!column.Visible)
                    {
                        column.Visible = true;
                        bComparisonRebuild = true;
                    }
                }

            }

            if (bComparisonRebuild)
            {
                if (firstVisibleColumn != SheetDataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible))
                {
                    ClassLibraryFramework.DataGridViewMethods.SortOnFirstVisibleColumn(SheetDataGridView);
                }

                ComparisonReset();
            }

            if (ExplorerForm.GetResultDataGridView() != null)
            {
                if (ExplorerForm.GetResultDataGridView().Rows.Count == 0)
                {
                    ClassLibraryFramework.DataGridViewMethods.SetBackgroundColour(SheetDataGridView, Constants.COLOR_DEFAULT);
                }

            }

            WorkTables.SetSheetDataGridViewFocus();
        }

        private static void ComparisonReset()
        {
            SetSpreadsheetChanges(true);
            ExplorerForm.ResetExplorerForm();
        }

        private static void DeleteSelectedRows(System.Collections.ArrayList keyArray = null)
        {
            ExplorerForm.DeleteSelectedRows(keyArray);
        }

        private void WorksheetIndexChanged(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            SaveAsLastTableOpened(SheetComboBox.SelectedItem.ToString());
            InitialiseDataSource();
            RestoreColumnMenuItemClickEvent();
            ComparisonReset();

            Cursor.Current = Cursors.Default;
        }

        private void SheetDataGridViewCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ExplorerForm.SheetDataGridViewCellValueChanged();
        }

        private void SheetDataGridViewDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop);

                bool bOpenFile = false;

                foreach (string file in fileList)
                {

                    string extension = System.IO.Path.GetExtension(file).ToUpperInvariant();

                    switch (extension)
                    {
                        case Constants.FILE_XLSX_EXTENSION:

                            bOpenFile = true;
                            break;

                        case Constants.FILE_XLS_EXTENSION:

                            bOpenFile = true;
                            break;

                        case Constants.FILE_CSV_EXTENSION:

                            bOpenFile = true;
                            break;

                        default:

                            MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_FILE_UNSUPPORTED, System.Environment.NewLine, file), Properties.Resources.CAPTION_DRAG_DROP, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            e.Effect = DragDropEffects.None;
                            break;
                    }

                    if (bOpenFile)
                    {
                        OpenWorbook(file);
                        break;
                    }

                }

            }

        }

        private void AddFileButtonClick(object sender, System.EventArgs e)
        {
            if (GetSpreadsheetChanges())
            {
                switch (MessageBox.Show(Properties.Resources.NOTIFY_SAVEWORKBOOKCHANGES, Properties.Resources.CAPTION_SAVE_WORKBOOK, MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:

                        SaveWorkbook();
                        break;

                    default:

                        OpenWorbook(null);
                        break;
                }

            }
            else
            {
                OpenWorbook(null);
            }

        }

        private void SaveAsLastTableOpened(string tableName)
        {
            if (tableName != null)
            {
                if (MaintainTableState)
                {
                    Dapper.DynamicParameters param = new Dapper.DynamicParameters();

                    param.Add("@" + Constants.COLUMN_USER_ID, UserHelper.UserPropertiesModel.iUserID);
                    param.Add("@" + Constants.COLUMN_USER_LAST_FILENAME_OPENED, UserHelper.UserStateModel.nvLastFilenameOpened);
                    param.Add("@" + Constants.COLUMN_USER_LAST_TAB_FOCUS, UserHelper.UserStateModel.nvLastTabFocus);
                    param.Add("@" + Constants.COLUMN_USER_LAST_TABLE_FOCUS, tableName);
                    param.Add("@" + Constants.COLUMN_USER_BUILD_VERSION, Constants.BUILD_VERSION);

                    DataAccess.InsertUserStateLogEntry(param);
                }

                UserHelper.UserStateModel.nvLastTableFocus = tableName;
                ExplorerForm.ResetExplorerForm();
            }

        }

        private void ExitButtonClick(object sender, System.EventArgs e)
        {
            if (GetSpreadsheetChanges())
            {
                switch (MessageBox.Show(Properties.Resources.NOTIFY_SAVEWORKBOOKCHANGES, Properties.Resources.CAPTION_SAVE_WORKBOOK, MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:

                        SaveWorkbook();
                        break;

                    default:

                        ParentForm.Close();
                        break;
                }
            }
            else
            {
                ParentForm.Close();
            }
        }

        private void SheetDataGridViewMouseDownEvent(object sender, MouseEventArgs e)
        {
            int columns = SheetDataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible);
            int rows = (ExplorerForm.GetResultDataGridView().RowCount == 0) ? 0 : SheetDataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible);
            int row = (columns == 0) ? 0 : SheetDataGridView.RowCount;

            DeleteColumnMenuItem.Enabled = columns != 0;
            DeleteRowsMenuItem.Enabled = rows != 0;
            DeleteRowMenuItem.Enabled = row != 0;
            RestoreColumnsMenuItem.Enabled = columns < GetSheetColumnInitialTotal();

            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hitTest = SheetDataGridView.HitTest(e.X, e.Y);

                switch (hitTest.Type)
                {
                    case DataGridViewHitTestType.ColumnHeader:

                        SheetDataGridView.ClearSelection();
                        SheetDataGridView.CurrentCell = SheetDataGridView[hitTest.ColumnIndex, 0];
                        break;

                    case DataGridViewHitTestType.RowHeader:

                        if (columns != 0)
                        {
                            SheetDataGridView.ClearSelection();
                            SheetDataGridView.CurrentCell = SheetDataGridView[SheetDataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible).DisplayIndex, hitTest.RowIndex];
                        }
                        break;

                    case DataGridViewHitTestType.Cell:

                        if (hitTest.RowIndex >= 0)
                        {
                            SheetDataGridView.ClearSelection();
                            SheetDataGridView.CurrentCell = SheetDataGridView[hitTest.ColumnIndex, hitTest.RowIndex];
                        }
                        break;

                    default:

                        DeleteColumnMenuItem.Enabled = false;
                        break;

                }

            }

        }

        private void DeleteColumnMenuItemClick(object sender, System.EventArgs e)
        {
            if (SheetDataGridView.CurrentCell != null)
            {
                DataGridViewColumn FirstVisibleColumn = SheetDataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
                int iFirstColumnIndex = FirstVisibleColumn.DisplayIndex;
                int iOwningColumnIndex = SheetDataGridView.CurrentCell.OwningColumn.Index;

                SheetDataGridView.ClearSelection();
                SheetDataGridView.Columns[iOwningColumnIndex].Visible = false;

                if (iFirstColumnIndex == iOwningColumnIndex) ClassLibraryFramework.DataGridViewMethods.SortOnFirstVisibleColumn(SheetDataGridView);

                WorkTables.SetSheetDataGridViewFocus();

                ComparisonReset();
            }

        }

        private void UserDeletingRowEvent(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
            DeleteSelectedRows();
        }

        private void DeleteRowMenuItemClick(object sender, System.EventArgs e)
        {
            DeleteSelectedRows(ClassLibraryFramework.DataGridViewMethods.GetSelectedRowCellValue(SheetDataGridView, Constants.KEY_COLUMN));
        }

    }

}
