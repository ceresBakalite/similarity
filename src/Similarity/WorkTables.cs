using System.Data;
using System.Linq;

namespace NAVService
{
    public static class WorkTables
    {
        public static DataTable BuildParentTable()
        {
            using (DataTable table = new DataTable())
            {
                DataColumn column = new DataColumn
                {
                    DataType = System.Type.GetType("System.Int32"),
                    ColumnName = Constants.COLUMN_ROW_ID,
                    ReadOnly = true,
                    Unique = true
                };

                table.Columns.Add(column);

                column = new DataColumn
                {
                    DataType = System.Type.GetType("System.String"),
                    ColumnName = Constants.COLUMN_DATA
                };

                table.Columns.Add(column);

                DataColumn[] PrimaryKeyColumns = new DataColumn[1];
                PrimaryKeyColumns[0] = table.Columns[Constants.KEY_COLUMN];
                table.PrimaryKey = PrimaryKeyColumns;

                if (NAVForm.GetSheetDataGridView() != null) PopulateParentTable();

                return table;

                void PopulateParentTable()
                {
                    int SheetDataGridViewColumnCount = NAVForm.GetSheetDataGridView().Columns.GetColumnCount(System.Windows.Forms.DataGridViewElementStates.Visible);

                    if (SheetDataGridViewColumnCount > 0)
                    {
                        foreach (System.Windows.Forms.DataGridViewRow row in NAVForm.GetSheetDataGridView().Rows)
                        {
                            if (row.IsNewRow) break;

                            if (row.Cells[Constants.KEY_COLUMN].Value != null)
                            {
                                System.Text.StringBuilder ConcatenateRow = new System.Text.StringBuilder();

                                foreach (System.Windows.Forms.DataGridViewCell cell in row.Cells)
                                {
                                    if ((cell.OwningColumn.Visible == true) && (cell.OwningColumn.Name != Constants.KEY_COLUMN)) ConcatenateRow.Append(cell.Value + " ");
                                }

                                string ConcatenateRowToString = ConcatenateRow.ToString().TrimEnd((char)32);

                                while (ConcatenateRowToString.Contains("  ")) ConcatenateRowToString = ConcatenateRowToString.Replace("  ", " ");

                                DataRow tableRow = table.NewRow();
                                tableRow[Constants.COLUMN_ROW_ID] = row.Cells[Constants.KEY_COLUMN].Value;
                                tableRow[Constants.COLUMN_DATA] = ConcatenateRowToString.TrimEnd();
                                table.Rows.Add(tableRow);

                            }

                        }

                        table.AcceptChanges();
                    }

                }

            }

        }

        public static DataTable BuildResultTable()
        {
            using (DataTable table = new DataTable())
            {
                DataColumn column = new DataColumn
                {
                    DataType = System.Type.GetType("System.Int32"),
                    ColumnName = Constants.COLUMN_DELETE_FLAG
                };

                table.Columns.Add(column);

                column = new DataColumn
                {
                    DataType = System.Type.GetType("System.Int32"),
                    ColumnName = Constants.COLUMN_ROW_ID
                };

                table.Columns.Add(column);

                column = new DataColumn
                {
                    DataType = System.Type.GetType("System.Int32"),
                    ColumnName = Constants.COLUMN_PARENT_ID
                };

                table.Columns.Add(column);

                column = new DataColumn
                {
                    DataType = System.Type.GetType("System.Int32"),
                    ColumnName = Constants.COLUMN_COMPARISON
                };

                table.Columns.Add(column);

                column = new DataColumn
                {
                    DataType = System.Type.GetType("System.String"),
                    ColumnName = Constants.COLUMN_MATCHTYPE
                };

                table.Columns.Add(column);

                column = new DataColumn
                {
                    DataType = System.Type.GetType("System.String"),
                    ColumnName = Constants.COLUMN_DATA
                };

                table.Columns.Add(column);

                return table;
            }

        }

        public static DataTable AbbreviateTable(DataTable datatable)
        {
            if (datatable == null) return datatable;

            using (DataTable tableClone = datatable.Clone())
            {
                System.DateTime logHelperStartTime = System.DateTime.Now;

                string RowDelimiter = ClassLibraryStandard.GenericHelperMethods.GetGUID("Row");
                string ColDelimiter = ClassLibraryStandard.GenericHelperMethods.GetGUID("Col") + (char)32;

                return RebuildDataTable();

                DataTable RebuildDataTable()
                {
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(RowDelimiter);
                    string[] rows = regex.Split(ApplyAbbreviations());

                    regex = new System.Text.RegularExpressions.Regex(ColDelimiter);

                    foreach (string row in rows)
                    {
                        if (row.Length > 0)
                        {
                            string[] cells = regex.Split(row);

                            DataRow tableRow = tableClone.NewRow();

                            tableRow[Constants.COLUMN_ROW_ID] = int.Parse(cells[0], UserHelper.culture);
                            tableRow[Constants.COLUMN_DATA] = cells[1];

                            tableClone.Rows.Add(tableRow);
                        }

                    }

                    return tableClone;
                }

                string ApplyAbbreviations() 
                {
                    return System.Threading.Tasks.Task.Run(() => {

                        string DirtyString = ClassLibraryStandard.GenericStringMethods.RemoveNoiseDelimiters(ClassLibraryStandard.DataTableMethods.ConcatenateColumns(datatable, RowDelimiter, ColDelimiter));

                        string PushSearchCriteria()
                        {
                            DirtyString = DataAccess.GetStringAbbreviations(DirtyString);

                            LogHelper.TraceTimeElapsedWriteLine(System.DateTime.Now, logHelperStartTime, "TRACE - Application parse abbreviations (push dataset) Time Elapsed: ");

                            return DirtyString;
                        }

                        string PullSearchCriteria()
                        {
                            using (DataTable table = ClassLibraryStandard.DataTableMethods.GetDataTable(DataAccess.GetAbbreviationsByUserID()))
                            {
                                int i = 0;

                                while (i < table.Rows.Count)
                                {
                                    DataRow row = table.Rows[i];
                                    DirtyString = DirtyString.Replace((string)row["nvWord"], (string)row["nvAbbreviation"]);
                                    i++;
                                }

                                LogHelper.TraceTimeElapsedWriteLine(System.DateTime.Now, logHelperStartTime, "TRACE - Application parse abbreviations (pull dataset) Time Elapsed: ");

                                return DirtyString;
                            }

                        }

                        return UserHelper.GetPullAbbreviations() ? PullSearchCriteria() : PushSearchCriteria(); 
                    
                    }).Result;

                }

            }

        }

        public static void GetDeleteTableRows(DataTable datatable)
        {
            if (NAVForm.GetSheetDataGridView().RowCount == 0 || datatable == null) return;

            datatable.AcceptChanges();

            object[] rows = SelectDuplicateRows();

            if (rows.Any()) DeleteDuplicateRows();

            void DeleteDuplicateRows()
            {
                for (int i = rows.Length - 1; i > -1; i--)
                {
                    int j = 0;

                    while (j < datatable.Rows.Count)
                    {
                        DataRow row = datatable.Rows[j];

                        if (row.HasVersion(DataRowVersion.Current))
                        {
                            if (row[Constants.COLUMN_PARENT_ID].Equals(rows[i])) row.Delete();
                        }

                        j++;
                    }

                }

                datatable.AcceptChanges();
            }

            object[] SelectDuplicateRows()
            {
                datatable.AcceptChanges();

                return datatable.Rows.Cast<DataRow>()
                    .Where(x => x[Constants.COLUMN_DELETE_FLAG].Equals(1) || x[Constants.COLUMN_COMPARISON].Equals(100))
                    .Select(x => x[Constants.COLUMN_PARENT_ID])
                    .Distinct()
                    .ToArray();

            }

        }

        public static void DeleteDataTableRows(System.Collections.ArrayList keyArray, DataTable ResultTable, DataTable ParentTable)
        {
            if (keyArray == null || ResultTable == null || ParentTable == null) return;

            if (!keyArray.Count.Equals(0))
            {
                if (GetDeleteDataGridViewRows())
                {
                    if (!ResultTable.Rows.Count.Equals(0)) DeleteRows();
                }

            }

            void DeleteRows()
            {
                int iOrphaned = 1;

                ResultTable.AcceptChanges();

                foreach (object key in keyArray)
                {
                    DeleteMatchingRows(key);
                }

                DeleteOrpanedRows();

                void DeleteMatchingRows(object key)
                {
                    int i = 0;

                    while (i < ResultTable.Rows.Count)
                    {
                        DataRow row = ResultTable.Rows[i];

                        if (row.HasVersion(DataRowVersion.Current))
                        {
                            if (row[Constants.COLUMN_PARENT_ID].Equals(key) || row[Constants.COLUMN_ROW_ID].Equals(key)) row.Delete();
                        }

                        i++;
                    }

                    ResultTable.AcceptChanges();
                }

                void DeleteOrpanedRows()
                {
                    int j = 0;

                    while (j < ResultTable.Rows.Count)
                    {
                        DataRow row = ResultTable.Rows[j];

                        if (row.HasVersion(DataRowVersion.Current))
                        {
                            string expression = Constants.COLUMN_PARENT_ID + " = " + row[Constants.COLUMN_PARENT_ID];

                            if (ResultTable.Select(expression).Length.Equals(iOrphaned)) row.Delete();
                        }

                        j++;
                    }

                    ResultTable.AcceptChanges();
                }

            }

            bool GetDeleteDataGridViewRows()
            {
                bool bParseAbbreviations = ExplorerForm.ParseAbbreviations;

                if (bParseAbbreviations) ParentTable.AcceptChanges();

                using (DataTable table = NAVForm.DataTableCollection[UserHelper.UserStateModel.nvLastTableFocus])
                {
                    table.AcceptChanges();

                    if (table.Rows.Count.Equals(0)) return false;

                    foreach (object key in keyArray)
                    {
                        int i = 0;

                        while (i < table.Rows.Count)
                        {
                            DataRow row = table.Rows[i];

                            if (row.HasVersion(DataRowVersion.Current))
                            {
                                if (row[Constants.KEY_COLUMN].Equals(key))
                                {
                                    table.Rows.RemoveAt(i);
                                    break;
                                }
                            }

                            i++;
                        }

                        if (bParseAbbreviations)
                        {
                            int j = 0;

                            while (j < ParentTable.Rows.Count)
                            {
                                DataRow row = ParentTable.Rows[j];

                                if (row.HasVersion(DataRowVersion.Current))
                                {
                                    if (row[Constants.COLUMN_ROW_ID].Equals(key))
                                    {
                                        ParentTable.Rows.RemoveAt(i);
                                        break;
                                    }
                                }

                                j++;
                            }

                        }

                    }

                    table.AcceptChanges();

                    return true;
                }

            }

        }

        public static decimal FindParentTableDuplicates(DataTable ResultTable, DataTable ParentTable)
        {
            if (ResultTable == null || ParentTable == null) return 0;

            return System.Threading.Tasks.Task.Run(() => { return InvokeFlagDuplicates(); }).Result;

            int InvokeFlagDuplicates()
            {
                ParentTable.AcceptChanges();

                for (int index = 0; index < ParentTable.Rows.Count; index++)
                {
                    string expression = Constants.COLUMN_DATA + " = '" + ClassLibraryStandard.GenericStringMethods.EscapeStringExpression(ParentTable.Rows[index][Constants.COLUMN_DATA].ToString()) + "'";

                    DataRow[] rows = ParentTable.Select(expression);

                    if (rows.Length > 1) AddNewRow(rows, ParentTable.Rows[index][Constants.COLUMN_ROW_ID]);
                }

                ResultTable.AcceptChanges();

                return ResultTable.Rows.Count;
            }

            void AddNewRow(DataRow[] rows, object iRowID)
            {
                string expression = Constants.COLUMN_ROW_ID + " = " + iRowID.ToString();

                if (!ResultTable.Select(expression).Any())
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        DataRow tableRow = ResultTable.NewRow();
                        tableRow[Constants.COLUMN_DELETE_FLAG] = (i == 0) ? 0 : 1;
                        tableRow[Constants.COLUMN_ROW_ID] = rows[i][Constants.COLUMN_ROW_ID];
                        tableRow[Constants.COLUMN_PARENT_ID] = iRowID;
                        tableRow[Constants.COLUMN_COMPARISON] = 100;
                        tableRow[Constants.COLUMN_MATCHTYPE] = (i == 0) ? Constants.MATCH_PRINCIPLE : Constants.MATCH_ASSOCIATE;
                        tableRow[Constants.COLUMN_DATA] = rows[i][Constants.COLUMN_DATA];

                        ResultTable.Rows.Add(tableRow);
                    }

                }

            }

        }

        public static int GetCumulativeThreshhold(DataTable ParentTable)
        {
            if (ParentTable == null) return 0;

            using (DataTable table = ParentTable.Copy())
            {
                int i = 0;

                for (int index = 0; index < ParentTable.Rows.Count; index++)
                {
                    string expression = Constants.COLUMN_ROW_ID + " = " + ParentTable.Rows[index][Constants.COLUMN_ROW_ID].ToString();

                    if (ClassLibraryStandard.DataTableMethods.DeleteRowByID(table, expression))
                    {
                        for (int tableIndex = 0; tableIndex < table.Rows.Count; tableIndex++) i++;
                    }

                    if (i == Constants.ACTION_PROGRESS_ESTIMATE) break;
                }

                return i;
            }

        }

        public static void SetSheetDataGridViewFocus()
        {
            NAVForm.GetSheetDataGridView().CurrentCell = NAVForm.GetSheetDataGridView().FirstDisplayedCell;
            NAVForm.GetSheetDataGridView().Focus();
        }

        public static void AcceptDataGridViewChanges()
        {
            foreach (DataTable table in NAVForm.DataTableCollection) table.AcceptChanges();

            NAVForm.SetSheetCurrentTotal(value: string.Format(UserHelper.culture, Properties.Resources.NOTIFY_CURRENT_TOTAL, NAVForm.GetSheetDataGridView().RowCount - 1));
            NAVForm.SetSpreadsheetChanges(true);
            SetSheetDataGridViewFocus();
        }

        public static void DeleteRowsMenuItemClick()
        {
            System.Collections.ArrayList keyArray = new System.Collections.ArrayList();

            int rowCount = GetDistinctRows();

            if (rowCount < 2)
            {
                ExplorerForm.DeleteSelectedRows(keyArray);
            }
            else
            {
                ConfirmDeleteRows();
            }

            int GetDistinctRows()
            {
                foreach (System.Windows.Forms.DataGridViewRow row in ExplorerForm.GetResultDataGridView().Rows)
                {
                    if (row.IsNewRow) break;

                    if ((string)row.Cells[Constants.COLUMN_MATCHTYPE].Value == Constants.MATCH_ASSOCIATE)
                    {
                        int i = (int)row.Cells[Constants.COLUMN_ROW_ID].Value;

                        if (!keyArray.Contains(i)) keyArray.Add(i);
                    }
                }

                return keyArray.Count;
            }

            void ConfirmDeleteRows()
            {
                switch (System.Windows.Forms.MessageBox.Show(string.Format(UserHelper.culture, Properties.Resources.NOTIFY_DELETE_ROWSCOUNT, System.Environment.NewLine, rowCount), Properties.Resources.CAPTION_COMPARE, System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button3))
                {
                    case System.Windows.Forms.DialogResult.Yes:

                        ExplorerForm.DeleteSelectedRows(keyArray);
                        break;

                    default:

                        break;
                }

            }

        }

    }

}
