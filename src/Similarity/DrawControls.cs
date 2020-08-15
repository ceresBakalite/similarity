using System.Data;
using System.Linq;
using System.Windows;

namespace NAVService
{
    internal class DrawControls
    {
        public static void SuspendDataGridViews()
        {
            WorkTables.SetSheetDataGridViewFocus();

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            SuspendDataGridViewDrawing();

            void SuspendDataGridViewDrawing()
            {
                NAVForm.SuspendSheetDataGridView();
                ExplorerForm.SuspendResultDataGridView();
            }

        }

        public static void ResumeDataGridViews()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

            ResumeDataGridViewDrawing();

            void ResumeDataGridViewDrawing()
            {
                NAVForm.ResumeSheetDataGridView();
                ExplorerForm.ResumeResultDataGridView();
            }

        }

        public static void LabelBackColorChange(object sender, System.EventArgs e)
        {
            System.Windows.Forms.Control control = (System.Windows.Forms.Control)sender;

            if (control != null)
            {
                control.BackColor = control.BackColor == Constants.COLOR_DEFAULT_WINDOW ? Constants.COLOR_MATCH_ASSOCIATE : Constants.COLOR_DEFAULT_WINDOW;
                control.ForeColor = control.ForeColor == Constants.COLOR_TEXT ? Constants.COLOR_DEFAULT_TEXT : Constants.COLOR_TEXT;
            }

        }

        public static void SetDataGridViewRowBackgroundColour()
        {
            foreach (System.Windows.Forms.DataGridViewRow row in ExplorerForm.GetResultDataGridView().Rows)
            {
                bool bDeleteRowFlag = System.Convert.ToBoolean(row.Cells[Constants.COLUMN_DELETE_FLAG].Value, UserHelper.culture);

                switch (row.Cells[Constants.COLUMN_MATCHTYPE].Value)
                {
                    case Constants.MATCH_ASSOCIATE:
                        row.DefaultCellStyle.BackColor = (bDeleteRowFlag) ? Constants.COLOR_MATCH_ASSOCIATE_DELETE : Constants.COLOR_MATCH_ASSOCIATE;
                        break;

                    case Constants.MATCH_PRINCIPLE:
                        row.DefaultCellStyle.BackColor = (bDeleteRowFlag) ? Constants.COLOR_MATCH_PRINCIPLE_DELETE : Constants.COLOR_MATCH_PRINCIPLE;
                        break;

                    default:
                        row.DefaultCellStyle.BackColor = Constants.COLOR_DEFAULT;
                        break;
                }

                SetRowBackgroundColor(row.Cells[Constants.COLUMN_ROW_ID].Value, row.Cells[Constants.COLUMN_MATCHTYPE].Value, bDeleteRowFlag);
            }

        }

        public static void DataGridViewCellContentClick()
        {
            ExplorerForm.GetResultDataGridView().CommitEdit(System.Windows.Forms.DataGridViewDataErrorContexts.Commit);

            System.Windows.Forms.DataGridViewRow row = ExplorerForm.GetResultDataGridView().CurrentRow;

            bool bDeleteRowFlag = System.Convert.ToBoolean(row.Cells[Constants.COLUMN_DELETE_FLAG].Value, UserHelper.culture);

            switch (row.Cells[Constants.COLUMN_MATCHTYPE].Value)
            {
                case Constants.MATCH_ASSOCIATE:
                    row.DefaultCellStyle.BackColor = (bDeleteRowFlag) ? Constants.COLOR_MATCH_ASSOCIATE_DELETE : Constants.COLOR_MATCH_ASSOCIATE;
                    break;

                case Constants.MATCH_PRINCIPLE:
                    row.DefaultCellStyle.BackColor = (bDeleteRowFlag) ? Constants.COLOR_MATCH_PRINCIPLE_DELETE : Constants.COLOR_MATCH_PRINCIPLE;
                    break;

                default:
                    row.DefaultCellStyle.BackColor = Constants.COLOR_DEFAULT;
                    break;
            }

            SetRowBackgroundColor(row.Cells[Constants.COLUMN_ROW_ID].Value, row.Cells[Constants.COLUMN_MATCHTYPE].Value, bDeleteRowFlag);
        }

        private static void SetRowBackgroundColor(object iRowID, object nvMatchType, bool bDeleteRowFlag)
        {
            if (iRowID == null) return;

            System.Windows.Forms.DataGridViewRow row = NAVForm.GetSheetDataGridView().Rows.Cast<System.Windows.Forms.DataGridViewRow>()
                .Where(x => !x.IsNewRow)
                .Where(x => ((DataRowView)x.DataBoundItem).Row.Field<int>(Constants.KEY_COLUMN).Equals(iRowID))
                .FirstOrDefault();

            if (GetResetPending(row)) return;

            switch (nvMatchType)
            {
                case Constants.MATCH_ASSOCIATE:
                    row.DefaultCellStyle.BackColor = (bDeleteRowFlag) ? Constants.COLOR_MATCH_ASSOCIATE_DELETE : Constants.COLOR_MATCH_ASSOCIATE;
                    break;

                case Constants.MATCH_PRINCIPLE:
                    row.DefaultCellStyle.BackColor = (bDeleteRowFlag) ? Constants.COLOR_MATCH_PRINCIPLE_DELETE : Constants.COLOR_MATCH_PRINCIPLE;
                    break;

                default:
                    row.DefaultCellStyle.BackColor = Constants.COLOR_DEFAULT;
                    break;
            }

        }

        private static bool GetResetPending(System.Windows.Forms.DataGridViewRow row)
        {
            if (row == null) return true;

            if (row.IsNewRow)
            {
                if (row.Index == 0) ExplorerForm.ResetExplorerForm();
                return true;
            }

            return false;
        }

    }

}
