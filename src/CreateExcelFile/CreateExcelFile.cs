//#define INCLUDE_WEB_FUNCTIONS
//#define DATA_CONTAINS_FORMULAE

using System;
using DocumentFormat.OpenXml.Packaging;

namespace ExportToExcel
{
    //
    //  September 2016
    //  http://www.mikesknowledgebase.com
    //
    //  Note: if you plan to use this in an ASP.Net application, remember to add a reference to "System.Web", and to uncomment
    //  the "INCLUDE_WEB_FUNCTIONS" definition at the top of this file.
    //
    //  Release history
    //  -  Sep 2016: 
    //        Make sure figures with a decimal part are formatted with a full-stop as a decimal point.
    //  -  Feb 2015: 
    //        Needed to replace "Response.End();" with some other code, to make sure the Excel was fully written to the HTTP Response
    //        New ReplaceHexadecimalSymbols() function to prevent hex characters from crashing the export. 
    //        Changed GetExcelColumnName() to cope with more than 702 columns (!)
    //   - Jan 2015: 
    //        Throwing an exception when trying to export a DateTime containing null.
    //        Was missing the function declaration for "CreateExcelDocument(DataSet ds, string filename, System.Web.HttpResponse Response)"
    //        Removed the "Response.End();" from the web version, as recommended in: https://support.microsoft.com/kb/312629/EN-US/?wa=wsignin1.0
    //   - Mar 2014: 
    //        Now writes the Excel data using the OpenXmlWriter classes, which are much more memory efficient.
    //   - Nov 2013: 
    //        Changed "CreateExcelDocument(DataTable dt, string xlsxFilePath)" to remove the DataTable from the DataSet after creating the Excel file.
    //        You can now create an Excel file via a Stream (making it more ASP.Net friendly)
    //   - Jan 2013: Fix: Couldn't open .xlsx files using OLEDB  (was missing "WorkbookStylesPart" part)
    //   - Nov 2012: 
    //        List<>s with Nullable columns weren't be handled properly.
    //        If a value in a numeric column doesn't have any data, don't write anything to the Excel file (previously, it'd write a '0')
    //   - Jul 2012: Fix: Some worksheets weren't exporting their numeric data properly, causing "Excel found unreadable content in '___.xslx'" errors.
    //   - Mar 2012: Fixed issue, where Microsoft.ACE.OLEDB.12.0 wasn't able to connect to the Excel files created using this class.
    //
    //
    //   (c) www.mikesknowledgebase.com 2016 
    //   
    //   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files 
    //   (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
    //   publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
    //   subject to the following conditions:
    //   
    //   The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
    //   
    //   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
    //   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
    //   FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
    //   WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    //   
    public class CreateExcelFile
    {
        public static bool CreateExcelDocument<T>(System.Collections.Generic.List<T> list, string xlsxFilePath)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(ListToDataTable(list));

            return CreateExcelDocument(ds, xlsxFilePath);
        }

        #region HELPER_FUNCTIONS

        //  This function is adapated from: http://www.codeguru.com/forum/showthread.php?t=450171
        //  My thanks to Carl Quirion, for making it "nullable-friendly".

        public static System.Data.DataTable ListToDataTable<T>(System.Collections.Generic.List<T> list)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            foreach (System.Reflection.PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new System.Data.DataColumn(info.Name, GetNullableType(info.PropertyType)));
            }

            foreach (T t in list)
            {
                System.Data.DataRow row = dt.NewRow();
                foreach (System.Reflection.PropertyInfo info in typeof(T).GetProperties())
                {
                    if (!IsNullableType(info.PropertyType))
                        row[info.Name] = info.GetValue(t, null);
                    else
                        row[info.Name] = (info.GetValue(t, null) ?? DBNull.Value);
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        private static Type GetNullableType(Type t)
        {
            Type returnType = t;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }

            return returnType;
        }

        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) || type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }

        public static bool CreateExcelDocument(System.Data.DataTable dt, string xlsxFilePath)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(dt);
            bool result = CreateExcelDocument(ds, xlsxFilePath);
            ds.Tables.Remove(dt);

            return result;
        }

        #endregion

#if INCLUDE_WEB_FUNCTIONS

        /// <summary>
        /// Create an Excel file, and write it out to a MemoryStream (rather than directly to a file)
        /// </summary>
        /// <param name="dt">DataTable containing the data to be written to the Excel.</param>
        /// <param name="filename">The filename (without a path) to call the new Excel file.</param>
        /// <param name="Response">HttpResponse of the current page.</param>
        /// <returns>True if it was created succesfully, otherwise false.</returns>
        public static bool CreateExcelDocument(DataSet ds, string filename, System.Web.HttpResponse Response)
        {
            try
            {
                CreateExcelDocumentAsStream(ds, filename, Response);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                return false;
            }
        }

        public static bool CreateExcelDocument(DataTable dt, string filename, System.Web.HttpResponse Response)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                CreateExcelDocument(ds, filename, Response);
                ds.Tables.Remove(dt);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                return false;
            }
        }

        public static bool CreateExcelDocument<T>(List<T> list, string filename, System.Web.HttpResponse Response)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(ListToDataTable(list));
                CreateExcelDocumentAsStream(ds, filename, Response);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Create an Excel file, and write it out to a MemoryStream (rather than directly to a file)
        /// </summary>
        /// <param name="ds">DataSet containing the data to be written to the Excel.</param>
        /// <param name="filename">The filename (without a path) to call the new Excel file.</param>
        /// <param name="Response">HttpResponse of the current page.</param>
        /// <returns>Either a MemoryStream, or NULL if something goes wrong.</returns>
        public static bool CreateExcelDocumentAsStream(DataSet ds, string filename, System.Web.HttpResponse Response)
        {
            try
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
                {
                    WriteExcelFile(ds, document);
                }
                stream.Flush();
                stream.Position = 0;

                Response.ClearContent();
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";

                //  NOTE: If you get an "HttpCacheability does not exist" error on the following line, make sure you have
                //  manually added System.Web to this project's References.

                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AppendHeader("content-length", stream.Length.ToString());
                byte[] data1 = new byte[stream.Length];
                stream.Read(data1, 0, data1.Length);
                stream.Close();
                Response.BinaryWrite(data1);
                Response.Flush();

                //  Feb2015: Needed to replace "Response.End();" with the following 3 lines, to make sure the Excel was fully written to the Response
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.SuppressContent = true;
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();

                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed, exception thrown: " + ex.Message);

                //  Display an error on the webpage.
                System.Web.UI.Page page = System.Web.HttpContext.Current.CurrentHandler as System.Web.UI.Page; 
                page.ClientScript.RegisterStartupScript(page.GetType(), "log", "console.log('Failed, exception thrown: " + ex.Message + "')", true);

                return false;
            }
        }

#endif      //  End of "INCLUDE_WEB_FUNCTIONS" section


        /// <summary>
        /// Create an Excel workbook, and write it to a file.
        /// </summary>
        /// <param name="ExcelWorkbook">DataTableCollection or DataSet containing the DataTables to be written to an Excel workbook file.</param>
        /// <param name="ExcelWorkbookFilePath">Name of file including path to be written.</param>
        /// <returns>True if successful, otherwise false and throws an exception.</returns>

        public static bool ExportExcelDocument(object ExcelWorkbookObject, string ExcelWorkbookFilePath)
        {
            try
            {
                using (SpreadsheetDocument WorkbookDocumentType = SpreadsheetDocument.Create(ExcelWorkbookFilePath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    System.Data.DataTableCollection ExcelWorkbook = (ExcelWorkbookObject.GetType() == typeof(System.Data.DataSet)) ? ((System.Data.DataSet)ExcelWorkbookObject).Tables : (System.Data.DataTableCollection)ExcelWorkbookObject;
                    ExportDataTableCollection(ExcelWorkbook, WorkbookDocumentType);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Failed to save " + ExcelWorkbookFilePath + ". Exception thrown: " + ex.Message);
                return false;
            }

        }

        /// <summary>
        /// ORIGINAL: Create an Excel file, and write it to a file.
        /// </summary>
        /// <param name="ds">DataSet containing the data to be written to the Excel.</param>
        /// <param name="excelFilename">Name of file to be written.</param>
        /// <returns>True if successful, false if something went wrong.</returns>
        /// 
        public static bool CreateExcelDocument(System.Data.DataSet ds, string excelFilename)
        {
            try
            {
                using (SpreadsheetDocument spreadsheet = SpreadsheetDocument.Create(excelFilename, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    WriteExcelFile(ds, spreadsheet);
                }

                System.Diagnostics.Trace.WriteLine("Successfully created: " + excelFilename);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                return false;
            }

        }

        private static void ExportDataTableCollection(System.Data.DataTableCollection WorksheetCollection, SpreadsheetDocument spreadsheet)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new DocumentFormat.OpenXml.Spreadsheet.BookViews(new DocumentFormat.OpenXml.Spreadsheet.WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
            DocumentFormat.OpenXml.Spreadsheet.Stylesheet stylesheet = new DocumentFormat.OpenXml.Spreadsheet.Stylesheet();
            workbookStylesPart.Stylesheet = stylesheet;


            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = spreadsheet.WorkbookPart.Workbook.AppendChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>(new DocumentFormat.OpenXml.Spreadsheet.Sheets());
            foreach (System.Data.DataTable dt in WorksheetCollection)
            {
                //  For each worksheet you want to create
                string worksheetName = dt.TableName;

                //  Create worksheet part, and add it to the sheets collection in workbook
                WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart), SheetId = worksheetNumber, Name = worksheetName };

                // If you want to define the Column Widths for a Worksheet, you need to do this *before* appending the SheetData
                // http://social.msdn.microsoft.com/Forums/en-US/oxmlsdk/thread/1d93eca8-2949-4d12-8dd9-15cc24128b10/

                sheets.Append(sheet);

                //  Append this worksheet's data to our Workbook, using OpenXmlWriter, to prevent memory problems
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);

                worksheetNumber++;
            }

            spreadsheet.WorkbookPart.Workbook.Save();

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private static void WriteExcelFile(System.Data.DataSet ds, SpreadsheetDocument spreadsheet)
        {
            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new DocumentFormat.OpenXml.Spreadsheet.BookViews(new DocumentFormat.OpenXml.Spreadsheet.WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
            DocumentFormat.OpenXml.Spreadsheet.Stylesheet stylesheet = new DocumentFormat.OpenXml.Spreadsheet.Stylesheet();
            workbookStylesPart.Stylesheet = stylesheet;


            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = spreadsheet.WorkbookPart.Workbook.AppendChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>(new DocumentFormat.OpenXml.Spreadsheet.Sheets());
            foreach (System.Data.DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                string worksheetName = dt.TableName;

                //  Create worksheet part, and add it to the sheets collection in workbook
                WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart), SheetId = worksheetNumber, Name = worksheetName };

                // If you want to define the Column Widths for a Worksheet, you need to do this *before* appending the SheetData
                // http://social.msdn.microsoft.com/Forums/en-US/oxmlsdk/thread/1d93eca8-2949-4d12-8dd9-15cc24128b10/

                sheets.Append(sheet);

                //  Append this worksheet's data to our Workbook, using OpenXmlWriter, to prevent memory problems
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);

                worksheetNumber++;
            }

            spreadsheet.WorkbookPart.Workbook.Save();
        }

        private static void WriteDataTableToExcelWorksheet(System.Data.DataTable dt, WorksheetPart worksheetPart)
        {
            DocumentFormat.OpenXml.OpenXmlWriter writer = DocumentFormat.OpenXml.OpenXmlWriter.Create(worksheetPart, System.Text.Encoding.ASCII);
            writer.WriteStartElement(new DocumentFormat.OpenXml.Spreadsheet.Worksheet());
            writer.WriteStartElement(new DocumentFormat.OpenXml.Spreadsheet.SheetData());

            string cellValue;
            string cellReference;

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.

            int numberOfColumns = dt.Columns.Count;

            bool[] IsIntegerColumn = new bool[numberOfColumns];
            bool[] IsFloatColumn = new bool[numberOfColumns];
            bool[] IsDateColumn = new bool[numberOfColumns];

            string[] excelColumnNames = new string[numberOfColumns];

            for (int n = 0; n < numberOfColumns; n++) excelColumnNames[n] = GetExcelColumnName(n);

            //
            //  Create the Header row in our Excel Worksheet
            //
            uint rowIndex = 1;

            writer.WriteStartElement(new DocumentFormat.OpenXml.Spreadsheet.Row { RowIndex = rowIndex });

            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                System.Data.DataColumn col = dt.Columns[colInx];
                AppendHeaderTextCell(excelColumnNames[colInx] + "1", col.ColumnName, writer);
                IsIntegerColumn[colInx] = (col.DataType.FullName.StartsWith("System.Int"));
                IsFloatColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Double") || (col.DataType.FullName == "System.Single");
                IsDateColumn[colInx] = (col.DataType.FullName == "System.DateTime");
            }

            writer.WriteEndElement();   //  End of header "Row"

            //
            //  Now, step through each row of data in our DataTable...
            //
            double cellFloatValue;

            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InvariantCulture;

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;

                writer.WriteStartElement(new DocumentFormat.OpenXml.Spreadsheet.Row { RowIndex = rowIndex });

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();
                    cellValue = ReplaceHexadecimalSymbols(cellValue);
                    cellReference = excelColumnNames[colInx] + rowIndex.ToString();

                    // Create cell with data
                    if (IsIntegerColumn[colInx] || IsFloatColumn[colInx])
                    {
                        //  For numeric cells without any decimal places.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        cellFloatValue = 0;
                        if (double.TryParse(cellValue, out cellFloatValue))
                        {
                            cellValue = cellFloatValue.ToString(ci);
                            AppendNumericCell(cellReference, cellValue, writer);
                        }
                    }
                    else if (IsDateColumn[colInx])
                    {
                        //  This is a date value.
                        if (DateTime.TryParse(cellValue, out DateTime dateValue))
                        {
                            AppendDateCell(cellReference, dateValue, writer);
                        }
                        else
                        {
                            //  This should only happen if we have a DataColumn of type "DateTime", but this particular value is null/blank.
                            AppendTextCell(cellReference, cellValue, writer);
                        }

                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(cellReference, cellValue, writer);
                    }

                }

                writer.WriteEndElement(); //  End of Row
            }

            writer.WriteEndElement(); //  End of SheetData
            writer.WriteEndElement(); //  End of worksheet
            writer.Close();
        }

        private static void AppendHeaderTextCell(string cellReference, string cellStringValue, DocumentFormat.OpenXml.OpenXmlWriter writer)
        {
            //  Add a new "text" Cell to the first row in our Excel worksheet
            //  We set these cells to use "Style # 3", so they have a gray background color & white text.
            writer.WriteElement(new DocumentFormat.OpenXml.Spreadsheet.Cell
            {
                CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cellStringValue),
                CellReference = cellReference,
                DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String
            });

        }

        private static void AppendTextCell(string cellReference, string cellStringValue, DocumentFormat.OpenXml.OpenXmlWriter writer)
        {
            //  Add a new "text" Cell to our Row 

#if DATA_CONTAINS_FORMULAE

            //  If this item of data looks like a formula, let's store it in the Excel file as a formula rather than a string.
            if (cellStringValue.StartsWith("="))
            {
                AppendFormulaCell(cellReference, cellStringValue, writer);
                return;
            }

            void AppendFormulaCell()
            {
                //  Add a new "formula" Excel Cell to our Row 
                writer.WriteElement(new DocumentFormat.OpenXml.Spreadsheet.Cell
                {
                    CellFormula = new DocumentFormat.OpenXml.Spreadsheet.CellFormula(cellStringValue),
                    CellReference = cellReference,
                    DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number
                });

            }

#endif

            //  Add a new Excel Cell to our Row 
            writer.WriteElement(new DocumentFormat.OpenXml.Spreadsheet.Cell
            {
                CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cellStringValue),
                CellReference = cellReference,
                DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String
            });

        }

        private static void AppendDateCell(string cellReference, DateTime dateTimeValue, DocumentFormat.OpenXml.OpenXmlWriter writer)
        {
            //  Add a new "datetime" Excel Cell to our Row.
            //
            string cellStringValue = dateTimeValue.ToShortDateString();

            writer.WriteElement(new DocumentFormat.OpenXml.Spreadsheet.Cell
            {
                CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cellStringValue),
                CellReference = cellReference,
                DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String
            });

        }

        private static void AppendNumericCell(string cellReference, string cellStringValue, DocumentFormat.OpenXml.OpenXmlWriter writer)
        {
            //  Add a new numeric Excel Cell to our Row.
            writer.WriteElement(new DocumentFormat.OpenXml.Spreadsheet.Cell
            {
                CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cellStringValue),
                CellReference = cellReference,
                DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number
            });

        }

        private static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return System.Text.RegularExpressions.Regex.Replace(txt, r, "", System.Text.RegularExpressions.RegexOptions.Compiled);
        }

        //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
        public static string GetExcelColumnName(int columnIndex)
        {
            //  eg  (0) should return "A"
            //      (1) should return "B"
            //      (25) should return "Z"
            //      (26) should return "AA"
            //      (27) should return "AB"
            //      ..etc..
            char firstChar;
            char secondChar;
            char thirdChar;

            if (columnIndex < 26)
            {
                return ((char)('A' + columnIndex)).ToString();
            }

            if (columnIndex < 702)
            {
                firstChar = (char)('A' + (columnIndex / 26) - 1);
                secondChar = (char)('A' + (columnIndex % 26));

                return string.Format("{0}{1}", firstChar, secondChar);
            }

            int firstInt = columnIndex / 676;
            int secondInt = (columnIndex % 676) / 26;

            if (secondInt == 0)
            {
                secondInt = 26;
                firstInt -= 1;
            }

            int thirdInt = (columnIndex % 26);

            firstChar = (char)('A' + firstInt - 1);
            secondChar = (char)('A' + secondInt - 1);
            thirdChar = (char)('A' + thirdInt);

            return string.Format("{0}{1}{2}", firstChar, secondChar, thirdChar);
        }

    }

}
