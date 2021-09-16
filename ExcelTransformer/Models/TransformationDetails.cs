using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace ExcelTransformer.Models
{
    class TransformationDetails
    {

        public List<string> columns { get; set; }

        public List<string> data { get; set; }

        public static string _filepath { get; set; }

        public static int getRowCount()
        {
            
            Application excelApp = new Application();
            if (excelApp == null)
            {
                return 0;
            }
            string file = (string)_filepath.Clone();
            Workbook excelBook = excelApp.Workbooks.Open(file);

            Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Sheets[1];
            Range excelRange = excelSheet.UsedRange;

            var rowCount = excelRange.Rows.Count;

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            return rowCount;
        }

        public static List<string> readExcelSheet(int _row)
        {
            
            Application excelApp = new Application();
            if (excelApp == null)
            {
                return null;
            }
            List<string> data = new List<string>();
            string file = (string)_filepath.Clone();
            try
            {
                Workbook excelBook = excelApp.Workbooks.Open(file);

                Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Sheets[1];
                Range excelRange = excelSheet.UsedRange;

                var rowCount = excelRange.Rows.Count;
                var colCount = excelRange.Columns.Count;

                for (int col = 1; col <= colCount; col++)
                {

                    if (_row == 1)
                    {
                        //Table Columns
                        var value = ((Microsoft.Office.Interop.Excel.Range)excelRange.Cells[_row, col]).Value2.ToString().Replace(" ", "_");
                        data.Add(value);
                    }
                    else
                    {
                        var value = ((Microsoft.Office.Interop.Excel.Range)excelRange.Cells[_row, col]).Value2;
                        data.Add(value != null? value.ToString() : "NULL");
                    }
                }
            }
             catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    return null;
                }
            }
            finally
            {
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
            return data;
        }
    }
}
