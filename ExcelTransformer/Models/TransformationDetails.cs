using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace ExcelTransformer.Models
{
    class TransformationDetails
    {
        public int rowCount { get; set; }

        public int colCount { get; set; }

        public string filePath { get; set; }

        public List<string> columns { get; set; }

        public List<string> data { get; set; }

        public TransformationDetails(string _filePath)
        {
            this.filePath = _filePath;
            this.columns = new List<string>();
        }


        public int readHeaderRow()
        {
            readExcelSheet(1);
            return this.rowCount;
        }

        public void readExcelSheet(int _row)
        {
            Application excelApp = new Application();
            if (excelApp != null)
            {
                return;
            }
            Workbook excelBook = excelApp.Workbooks.Open(filePath);

            Worksheet excelSheet = excelBook.Sheets[1];
            Range excelRange = excelSheet.UsedRange;

            this.rowCount = excelRange.Rows.Count;
            this.colCount = excelRange.Columns.Count;
            this.data = new List<string>();
           
            for (int col = 1; col <= colCount; col++)
            {
                if (_row == 1)
                {
                    //Table Columns
                    columns.Add(excelRange.Cells[_row, col].Value2.ToString());
                }
                else
                {
                    data.Add(excelRange.Cells[_row, col].Value2.ToString());
                }
            }
            
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }
    }
}
