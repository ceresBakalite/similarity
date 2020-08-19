namespace ClassLibraryFramework
{
    public static class ScreenAttributes
    {
        #region Windows Forms System.Drawing code

        public static System.Drawing.Rectangle GetScreenDimensions(System.Windows.Forms.Form form)
        {
            return System.Windows.Forms.Screen.FromControl(form).Bounds;
        }

        public static System.Drawing.Point GetControlLocation(System.Windows.Forms.Control control)
        {
            return control.FindForm().PointToClient(control.Parent.PointToScreen(control.Location));
        }

        #endregion
    }

    public class ComboBoxEx : System.Windows.Forms.ComboBox
    {
        #region Windows Forms ComboBox extension code

        public ComboBoxEx()
        {
            DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.State == System.Windows.Forms.DrawItemState.Focus) e.DrawFocusRectangle();

            if (e.Index < 0 || e.Index >= Items.Count) return;

            string text = (Items[e.Index] == null) ? "(null)" : Items[e.Index].ToString();

            using (System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(e.ForeColor))
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                e.Graphics.DrawString(text, e.Font, brush, e.Bounds);
            }

        }

        #endregion 
    }

    public class PanelWithBorder : System.Windows.Forms.Panel
    {
        #region Windows Forms Panel extension code

        public PanelWithBorder()
        {
            BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            System.Drawing.Rectangle r = ClientRectangle;
            r.Width -= 1;
            r.Height -= 1;
            e.Graphics.DrawRectangle(System.Drawing.Pens.DeepSkyBlue, r);
        }

        #endregion
    }

    public static class DrawingInteropServices
    {
        #region Windows Forms Suspend and Resume drawing extensions

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(System.IntPtr hWnd, int wMsg, bool wParam, int lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(System.Windows.Forms.Control parent)
        {
            _ = SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
            parent.Refresh();
        }

        public static void ResumeDrawing(System.Windows.Forms.Control parent)
        {
            _ = SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }

        #endregion
    }

    public static class DataGridViewMethods
    {
        #region Various DataGridView methods

        public static string ConcatenateColumns(System.Windows.Forms.DataGridView datagridview, string RowDelimiter, string ColDelimiter)
        {
            string concatenatedData = null;

            foreach (System.Windows.Forms.DataGridViewRow row in datagridview.Rows)
            {
                if (row.IsNewRow) break;

                System.Text.StringBuilder concatenateRow = new System.Text.StringBuilder();

                foreach (System.Windows.Forms.DataGridViewCell cell in row.Cells)
                {
                    if (cell.OwningColumn.Visible == true) concatenateRow.Append(cell.Value + ColDelimiter);
                }

                concatenatedData += concatenateRow.ToString().TrimEnd(ColDelimiter) + RowDelimiter;
            }

            return concatenatedData.TrimEnd(RowDelimiter);
        }

        private static string TrimEnd(this string strValue, string trimStr, bool repeatTrim = true, System.StringComparison comparisonType = System.StringComparison.OrdinalIgnoreCase) => TrimString(strValue, trimStr, true, repeatTrim, comparisonType);

        private static string TrimString(this string strValue, string trimStr, bool trimEnd = true, bool repeatTrim = true, System.StringComparison comparisonType = System.StringComparison.OrdinalIgnoreCase)
        {
            int strLen = 0;

            while (repeatTrim && strLen > strValue.Length)
            {
                if (!(strValue ?? "").EndsWith(trimStr)) return strValue;

                strLen = strValue.Length;

                if (trimEnd)
                {
                    var pos = strValue.LastIndexOf(trimStr, comparisonType);
                    if ((!(pos >= 0)) || (!(strValue.Length - trimStr.Length == pos))) break;
                    strValue = strValue.Substring(0, pos);
                }
                else
                {
                    var pos = strValue.IndexOf(trimStr, comparisonType);
                    if (!(pos == 0)) break;
                    strValue = strValue.Substring(trimStr.Length, strValue.Length - trimStr.Length);
                }

            }

            return strValue;
        }

        public static System.Collections.ArrayList GetColumnValue(System.Windows.Forms.DataGridView datagridview, string columnName)
        {
            System.Collections.ArrayList cellArray = new System.Collections.ArrayList();

            foreach (System.Windows.Forms.DataGridViewRow row in datagridview.Rows)
            {
                cellArray.Add(row.Cells[columnName].Value);
            }

            return cellArray;
        }

        public static System.Collections.ArrayList GetColumnDistinctValue(System.Windows.Forms.DataGridView datagridview, string columnName)
        {
            System.Collections.ArrayList cellArray = new System.Collections.ArrayList();

            foreach (System.Windows.Forms.DataGridViewRow row in datagridview.Rows)
            {
                if (!cellArray.Contains(row.Cells[columnName].Value)) cellArray.Add(row.Cells[columnName].Value);
            }

            return cellArray;
        }

        public static System.Collections.ArrayList GetSelectedColumnValue(System.Windows.Forms.DataGridView datagridview, string columnName)
        {
            System.Collections.ArrayList cellArray = new System.Collections.ArrayList();

            foreach (System.Windows.Forms.DataGridViewRow row in datagridview.SelectedRows)
            {
                cellArray.Add(row.Cells[columnName].Value);
            }

            return cellArray;
        }


        public static System.Collections.ArrayList GetSelectedColumnDistinctValue(System.Windows.Forms.DataGridView datagridview, string columnName)
        {
            System.Collections.ArrayList cellArray = new System.Collections.ArrayList();

            foreach (System.Windows.Forms.DataGridViewRow row in datagridview.SelectedRows)
            {
                if (!cellArray.Contains(row.Cells[columnName].Value)) cellArray.Add(row.Cells[columnName].Value);
            }

            return cellArray;
        }

        public static System.Collections.ArrayList GetSelectedRowCellValue(System.Windows.Forms.DataGridView datagridview, string columnName)
        {
            System.Collections.ArrayList value = new System.Collections.ArrayList
            {
                datagridview.Rows[datagridview.CurrentCell.RowIndex].Cells[columnName].Value
            };

            return value;
        }

        public static System.Collections.ArrayList GetSelectedRowsIndex(System.Windows.Forms.DataGridView datagridview)
        {
            System.Collections.ArrayList cellArray = new System.Collections.ArrayList();

            foreach (System.Windows.Forms.DataGridViewRow row in datagridview.SelectedRows)
            {
                cellArray.Add(row.Index);
            }

            return cellArray;
        }

        public static System.Data.DataTable GetSelectedRows(System.Windows.Forms.DataGridView datagridview)
        {
            using (System.Data.DataTable table = ((System.Data.DataTable)datagridview.DataSource).Clone())
            {
                foreach (System.Windows.Forms.DataGridViewRow row in datagridview.SelectedRows)
                {
                    table.ImportRow(((System.Data.DataTable)datagridview.DataSource).Rows[row.Index]);
                }

                return table;
            }

        }

        public static void SetBackgroundColour(System.Windows.Forms.DataGridView datagridview, System.Drawing.Color backcolor)
        {
            foreach (System.Windows.Forms.DataGridViewRow row in datagridview.Rows)
            {
                row.DefaultCellStyle.BackColor = backcolor;
            }

            datagridview.Refresh();
        }

        public static void SetForegroundColour(System.Windows.Forms.DataGridView datagridview, System.Drawing.Color forecolor)
        {
            foreach (System.Windows.Forms.DataGridViewRow row in datagridview.Rows)
            {
                row.DefaultCellStyle.ForeColor = forecolor;
            }

            datagridview.Refresh();
        }

        public static void SetSelectedBackgroundColour(System.Windows.Forms.DataGridView datagridview, System.Drawing.Color backcolor)
        {
            foreach (System.Windows.Forms.DataGridViewRow row in datagridview.SelectedRows)
            {
                row.DefaultCellStyle.BackColor = backcolor;
            }

            datagridview.Refresh();
        }

        public static void SetSelectedForegroundColour(System.Windows.Forms.DataGridView datagridview, System.Drawing.Color forecolor)
        {
            foreach (System.Windows.Forms.DataGridViewRow row in datagridview.SelectedRows)
            {
                row.DefaultCellStyle.ForeColor = forecolor;
            }

            datagridview.Refresh();
        }

        public static void SortOnFirstVisibleColumn(System.Windows.Forms.DataGridView datagridview, bool bSortAscending = true)
        {
            if (datagridview.FirstDisplayedScrollingColumnIndex > -1)
            {
                if (bSortAscending)
                {
                    datagridview.Sort(datagridview.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Visible), System.ComponentModel.ListSortDirection.Ascending);
                }
                else
                {
                    datagridview.Sort(datagridview.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Visible), System.ComponentModel.ListSortDirection.Descending);
                }

            }

            datagridview.Refresh();
        }

        #endregion
    }

    public static class StringMethods
    {
        #region Common string methods

        public static string GetBytesToString(byte[] value)
        {
            System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary soapHexBinary = new System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary(value);
            return soapHexBinary.ToString();
        }

        public static byte[] GetStringToBytes(string value)
        {
            System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary soapHexBinary = System.Runtime.Remoting.Metadata.W3cXsd2001.SoapHexBinary.Parse(value);
            return soapHexBinary.Value;
        }

        #endregion

    }

}