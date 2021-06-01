using Microsoft.Office.Interop.Excel;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

//Reference dll can be found in "C:\\Program Files\\Office\\".
//Search "Office.dll" and "Interop.Excel.dll".
namespace ExcelToolsTest
{
    public partial class ExcelTools : Form
    {
        public ExcelTools()
        {
            InitializeComponent();
        }
        private Excel.Application excelApplication = null;
        private Workbook excelWorkbook;
        private Worksheet excelWorksheet;
        private void button1_Click(object sender, EventArgs e)
        {
            ExcelOpen(@"C:\Users\Administrator\Desktop\123.xlsx");
            GetSheetbyName("sheet1");
            textBox1.Text += excelWorksheet.Name+"\r\n";
            Excel.Range cells =excelWorksheet.Cells as Excel.Range;
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    textBox1.Text += Convert.ToString(cells[i, j].Value) + "\r\n";
                }
            }
            //textBox1.Text += excelWorksheet.Cells[2, 2].GetType();
            // textBox1.Text += GetCellDouble(2, 2);
        }
        public string[,] GetCellsString(int x1,int y1, int x2, int y2)
        {
            string[,] output = null;
            Excel.Range cells =excelWorksheet.Cells as Excel.Range;
            output = new string[y2-y1, x2-x1];
            for (int i = y1; i <= y2; i++)
            {
                for (int j = x1; j <= x2; j++)
                {
                    output[i,j]= Convert.ToString(cells[i, j].Value);
                }
            }
            return output;
        }
        public string GetCellString(int row, int col)
        {
            Excel.Range cell = excelWorksheet.Cells[row, col] as Excel.Range;
            return Convert.ToString(cell.Value);
        }
        public double GetCellDouble(int row, int col)
        {
            Excel.Range cell = excelWorksheet.Cells[row, col] as Excel.Range;
            return Convert.ToDouble(cell.Value);
        }
        public double GetCellInteger(int row, int col)
        {
            Excel.Range cell = excelWorksheet.Cells[row, col] as Excel.Range;
            return Convert.ToInt32(cell.Value);
        }
        public void GetSheetbyName(string Name)
        {
            excelWorksheet = excelWorkbook.Worksheets[Name] as Excel.Worksheet;
        }
        public void GetSheetbyIndex(int Index)
        {
            excelWorksheet = excelWorkbook.Worksheets[Index] as Excel.Worksheet;
        }
        public void ExcelClose()
        {
            excelWorkbook.Close(true, null, null);
        }
        public void ExcelSaveAs(string path)
        {
            excelWorkbook.SaveAs(path,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

        }
        public void ExcelQuit()
        {
            excelApplication.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApplication);
        }
        public bool ExcelOpen(string path)
        {
            if (!File.Exists(path))
                return false;
            else
            {
                excelApplication = new Excel.Application();
                // Open or Add to open xlsx File
                excelWorkbook = excelApplication.Workbooks.Open(path);
                // excelWorkbook = excelApplication.Workbooks.Add(path);
                return true;
            }
        }
    }
}

