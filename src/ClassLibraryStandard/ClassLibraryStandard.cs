using System.Linq;

namespace ClassLibraryStandard
{
    public static class Extensions
    {
        #region List conversion

        public static System.Collections.Generic.List<T> ToList<T>(this System.Data.DataTable table) where T : new()
        {
            System.Collections.Generic.IList<System.Reflection.PropertyInfo> properties = typeof(T).GetProperties().ToList();
            System.Collections.Generic.List<T> result = new System.Collections.Generic.List<T>();

            foreach (object row in table.Rows)
            {
                T item = CreateItemFromRow<T>((System.Data.DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        private static T CreateItemFromRow<T>(System.Data.DataRow row, System.Collections.Generic.IList<System.Reflection.PropertyInfo> properties) where T : new()
        {
            T item = new T();

            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    System.DayOfWeek day = (System.DayOfWeek)System.Enum.Parse(typeof(System.DayOfWeek), row[property.Name].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    if (row[property.Name] == System.DBNull.Value)
                        property.SetValue(item, null, null);
                    else
                        property.SetValue(item, row[property.Name], null);
                }
            }

            return item;
        }

        #endregion
    }

    public static class InternetInteropServices
    {
        #region Windows Forms Internet Connected State

        [System.Runtime.InteropServices.DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

        public static bool InternetConnectedState()
        {
            return InternetGetConnectedState(out _, 0);
        }

        #endregion
    }

    public class FileInteropServices
    {
        #region Capture file lock and sharing violations

        public static bool IsFileLocked(System.Exception exception)
        {
            const int ERROR_SHARING_VIOLATION = 32;
            const int ERROR_LOCK_VIOLATION = 33;

            int errorCode = System.Runtime.InteropServices.Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION;
        }

        #endregion
    }

    public class TextBoxInteropServices
    {
        #region Windows Forms Hide Caret TextBox extension

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool HideCaret(System.IntPtr hWnd);

        #endregion
    }

    public static class DataTableMethods
    {
        #region Various DataTable methods

        public static System.Data.DataTable GetDataTable<T>(this System.Collections.Generic.List<T> iList)
        {
            if (iList != null)
            {
                using (System.Data.DataTable dataTable = new System.Data.DataTable())
                {
                    System.ComponentModel.PropertyDescriptorCollection propertyDescriptorCollection = System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));

                    for (int i = 0; i < propertyDescriptorCollection.Count; i++)
                    {
                        System.ComponentModel.PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                        System.Type type = propertyDescriptor.PropertyType;

                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(System.Nullable<>)) type = System.Nullable.GetUnderlyingType(type);

                        dataTable.Columns.Add(propertyDescriptor.Name, type);
                    }

                    object[] row = new object[propertyDescriptorCollection.Count];

                    foreach (T iListItem in iList)
                    {
                        for (int i = 0; i < row.Length; i++)
                        {
                            row[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                        }

                        dataTable.Rows.Add(row);
                    }

                    return dataTable;
                }

            }

            return null;
        }

        public static System.Data.DataTable CreateDataTable<T>(System.Collections.Generic.IEnumerable<T> list)
        {
            using (System.Data.DataTable dataTable = new System.Data.DataTable())
            {
                System.Type type = typeof(T);
                System.Reflection.PropertyInfo[] properties = type.GetProperties();

                foreach (System.Reflection.PropertyInfo item in properties)
                {
                    dataTable.Columns.Add(new System.Data.DataColumn(item.Name, System.Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType));
                }

                foreach (T item in list)
                {
                    object[] row = new object[properties.Length];

                    for (int i = 0; i < properties.Length; i++)
                    {
                        row[i] = properties[i].GetValue(item);
                    }

                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }

        }

        public static string ConcatenateColumns(System.Data.DataTable table, string RowDelimiter, string ColDelimiter)
        {
            System.Text.StringBuilder concatenatedRow = new System.Text.StringBuilder();

            foreach (System.Data.DataRow row in table.Rows)
            {
                concatenatedRow.Append(string.Join(ColDelimiter, row.ItemArray)).Append(RowDelimiter);
            }

            return concatenatedRow.ToString().TrimEnd(RowDelimiter);
        }

        public static void DeleteColumn(System.Data.DataTable table, string columnName)
        {
            if (table.Columns.Contains(columnName))
            {
                table.Columns.Remove(columnName);
            }

            table.AcceptChanges();
        }

        public static bool DeleteRowByID(System.Data.DataTable table, string expression)
        {
            System.Data.DataRow[] rows = table.Select(expression);

            if (rows.Length == 1)
            {
                if (rows[0].HasVersion(System.Data.DataRowVersion.Current))
                {
                    table.Rows.Remove(rows[0]);
                    table.AcceptChanges();
                }

                return true;
            }

            return false;
        }

        public static void DeleteRows(System.Data.DataTable table, string expression)
        {
            System.Data.DataRow[] rows = table.Select(expression);

            for (int i = rows.Length - 1; i > -1; i--)
            {
                if (rows[i].HasVersion(System.Data.DataRowVersion.Current)) table.Rows.Remove(rows[i]);
            }

            table.AcceptChanges();
        }

        public static void CreateAutoIncrementID(object DataTableObject, string columnName, bool bColumnMappingHidden = false)
        {
            System.Data.DataTableCollection TableCollection = (DataTableObject.GetType() == typeof(System.Data.DataSet)) ? ((System.Data.DataSet)DataTableObject).Tables : (System.Data.DataTableCollection)DataTableObject;

            int iUniqueID = 1;

            foreach (System.Data.DataTable table in TableCollection)
            {
                int i = 0;

                if (!table.Columns.Contains(columnName))
                {
                    System.Data.DataColumn column;

                    column = new System.Data.DataColumn
                    {
                        DataType = System.Type.GetType("System.Int32"),
                        ColumnName = columnName,
                        AutoIncrement = true
                    };

                    table.Columns.Add(column);

                    while (i < table.Rows.Count)
                    {
                        table.Rows[i][columnName] = iUniqueID++;
                        i++;
                    }

                    column.Unique = true;
                    column.ReadOnly = true;
                }

                if (bColumnMappingHidden) table.Columns[columnName].ColumnMapping = System.Data.MappingType.Hidden;

                table.AcceptChanges();
            }

        }

        #endregion 
    }

    public static class HelperMethods
    {
        #region Common generic methods

        public static string GetGUID(string symbol = null, bool bLarge = false)
        {
            return symbol + (bLarge ? System.Guid.NewGuid().ToString("n") : System.Guid.NewGuid().ToString().GetHashCode().ToString("x").ToString());
        }

        public static long GetMilliseconds(bool utc = false)
        {
            return (utc) ? System.DateTime.UtcNow.Ticks / System.TimeSpan.TicksPerMillisecond : System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
        }

        public static bool ToBoolean(object value)
        {
            if (value == null) return false;

            if (int.TryParse(value.ToString(), out int outValue)) return System.Convert.ToBoolean(outValue);

            System.TypeCode type = System.Type.GetTypeCode(value.GetType());

            switch (type)
            {
                case System.TypeCode.String:

                    return SymbolToBoolean();

                case System.TypeCode.Char:

                    return SymbolToBoolean();

                case System.TypeCode.Boolean:

                    return System.Convert.ToBoolean(value);

                default:

                    return false;

            }

            bool SymbolToBoolean()
            {
                string symbol = (string)value;

                if (string.IsNullOrWhiteSpace(symbol))
                {
                    return false;
                }
                else
                {
                    switch (symbol.Trim().ToUpperInvariant())
                    {
                        case "TRUE": return true;
                        case "T": return true;
                        case "YES": return true;
                        case "Y": return true;
                        case "1": return true;
                        default: return false;
                    }

                }

            }

        }

        public static bool IsInteger(object value)
        {
            if (value == null) return false;

            if (int.TryParse(value.ToString(), out _)) return true;

            System.TypeCode type = System.Type.GetTypeCode(value.GetType());

            switch (type)
            {
                case System.TypeCode.String:

                    return IsNumeric(value);

                default:

                    return false;
            }

        }

        public static bool IsNumeric(object value)
        {
            string charString = (string)value;
            char[] delimiter = { '-' };

            foreach (char character in charString.TrimStart(delimiter))
            {
                int asciiValue = character;

                if ((asciiValue > 57) || (asciiValue < 48)) return false;
            }

            return true;
        }

        public static void ProcessSleep(int millisecondsToWait = 50)
        {
            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
            while (true)
            {
                if (stopwatch.ElapsedMilliseconds >= millisecondsToWait) break;
                System.Threading.Thread.Sleep(1); //so processor can rest for a while
            }

        }

        #endregion
    }

    public static class StringMethods
    {
        #region Common string methods

        public static string TrimEnd(this string strValue, string trimStr, bool repeatTrim = true, System.StringComparison comparisonType = System.StringComparison.OrdinalIgnoreCase) => TrimString(strValue, trimStr, true, repeatTrim, comparisonType);

        public static string TrimStart(this string strValue, string trimStr, bool repeatTrim = true, System.StringComparison comparisonType = System.StringComparison.OrdinalIgnoreCase) => TrimString(strValue, trimStr, false, repeatTrim, comparisonType);

        public static string Trim(this string strValue, string trimStr, bool repeatTrim = true, System.StringComparison comparisonType = System.StringComparison.OrdinalIgnoreCase) => strValue.TrimStart(trimStr, repeatTrim, comparisonType).TrimEnd(trimStr, repeatTrim, comparisonType);

        public static string TrimString(this string strValue, string trimStr, bool trimEnd = true, bool repeatTrim = true, System.StringComparison comparisonType = System.StringComparison.OrdinalIgnoreCase)
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

        public static System.Collections.Generic.List<string> GetListItemFromString(string str, char delimiter = (char)32)
        {
            System.Collections.Generic.List<string> itemList = new System.Collections.Generic.List<string>();

            string[] splitString = str.Split(new[] { delimiter }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in splitString)
            {
                itemList.Add(item);
            }

            return itemList;
        }

        public static System.Collections.Generic.List<int> GetListIntFromString(string str, char delimiter = (char)32)
        {
            System.Collections.Generic.List<int> itemList = new System.Collections.Generic.List<int>();

            string[] splitString = str.Split(new[] { delimiter }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in splitString)
            {
                itemList.Add(System.Convert.ToInt32(item));
            }

            return itemList;
        }

        public static string SortWordsInString(string str, char delimiter = (char)32)
        {
            string[] wordArray = str.Split(delimiter);
            System.Array.Sort(wordArray);

            return string.Join(delimiter.ToString(), wordArray);
        }

        public static string RemoveDuplicateWords(string str, char delimiter = (char)32)
        {
            return string.Join(delimiter.ToString(), str.Split(delimiter).Distinct(System.StringComparer.CurrentCultureIgnoreCase));
        }

        public static string RemoveNoiseCharacters(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? str : RemovePunctuation(RemoveWhitespace(str));
        }

        public static string RemovePunctuation(string str)
        {
            return new string(str.Where(c => !char.IsPunctuation(c)).ToArray());
        }

        public static string RemoveWhitespace(string str)
        {
            return new string(str.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        public static string RemoveNoiseDelimiters(string str)
        {
            return new string(str.Where(x => char.IsWhiteSpace(x) || char.IsLetterOrDigit(x) || '@'.Equals(x) || '.'.Equals(x)).ToArray());
        }

        public static string GetRemoveWhitespace(string str)
        {
            char[] charArray = str.ToCharArray();
            int j = 0;

            for (int i = 0; i < str.Length; i++)
            {

                switch (charArray[i])
                {
                    case '\u0020':
                    case '\u00A0':
                    case '\u1680':
                    case '\u2000':
                    case '\u2001':
                    case '\u2002':
                    case '\u2003':
                    case '\u2004':
                    case '\u2005':
                    case '\u2006':
                    case '\u2007':
                    case '\u2008':
                    case '\u2009':
                    case '\u200A':
                    case '\u202F':
                    case '\u205F':
                    case '\u3000':
                    case '\u2028':
                    case '\u2029':
                    case '\u0009':
                    case '\u000A':
                    case '\u000B':
                    case '\u000C':
                    case '\u000D':
                    case '\u0085':

                        break;

                    default:

                        charArray[j++] = charArray[i];

                        break;
                }

            }

            return new string(charArray, 0, j);
        }

        public static string ReverseString(string str)
        {
            char[] charArray = str.ToCharArray();
            System.Array.Reverse(charArray);

            return new string(charArray);
        }

        public static string PadString(string strPadString, int iPadStringLength = 0, char delimiter = (char)32, bool bPadLeft = false)
        {
            if (strPadString.Length < iPadStringLength)
            {
                strPadString = bPadLeft? strPadString.PadLeft(iPadStringLength, delimiter) : strPadString.PadRight(iPadStringLength, delimiter);
            }

            return strPadString;
        }

        public static string PadStringBuffer(int strLength, char delimiter = (char)32)
        {
            int j = 0;

            char[] charArray = new char[strLength];

            for (int i = 0; i < strLength; ++i)
            {
                charArray[j] = delimiter;
                ++j;
            }

            return new string(charArray);
        }

        public static string MakeCaseInsensitive(string str, bool bUpperCase = true)
        {
            return bUpperCase ? str.ToUpperInvariant() : str.ToLowerInvariant();
        }

        public static bool InString(string str, string testStr, bool bCaseSensitive = true)
        {
            if (string.IsNullOrWhiteSpace(str) || string.IsNullOrWhiteSpace(testStr))
            {
                return false;
            }

            return (bCaseSensitive) ? str.Contains(testStr) : str.Trim().ToUpper().Contains(testStr);
        }

        public static string EscapeStringExpression(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(str.Length);

            for (int i = 0; i < str.Length; i++)
            {
                char chr = str[i];

                switch (chr)
                {
                    case '\'':
                        sb.Append("''");
                        break;

                    case '*':
                        sb.Append("[").Append(chr).Append("]");
                        break;

                    case '%':
                    case ']':
                    case '[':

                    default:
                        sb.Append(chr);
                        break;
                }

            }

            return sb.ToString();
        }

        #endregion
    }

}