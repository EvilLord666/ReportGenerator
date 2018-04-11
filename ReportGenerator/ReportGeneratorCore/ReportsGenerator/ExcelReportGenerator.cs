using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using ReportGenerator.Core.Data;

namespace ReportGenerator.Core.ReportsGenerator
{
    public class ExcelReportGenerator : IReportGenerator
    {
        public ExcelReportGenerator(string template, string reportFile)
        {
            if(string.IsNullOrEmpty(template))
                throw new ArgumentNullException("template");
            _template = template;
            _reportFile = reportFile;
            FileInfo fileInfo = new FileInfo(Path.GetFullPath(_template));
            if (File.Exists(_template))
                _package = new ExcelPackage(fileInfo);
            else
                throw new FileNotFoundException("template file does not exists");
        }

        public bool Generate(DbData data, object[] parameters)
        {
            if(parameters.Length < 3)
                throw new InvalidDataException("Invalid parameters array length");
            if (data == null)
                return false;
            try
            {
                int workSheetNumber = Convert.ToInt32(parameters[WorkSheetNumberIndex]);
                int startRow = Convert.ToInt32(parameters[StartRowIndex]);
                int startColumn = Convert.ToInt32(parameters[StartColumnIndex]);
                ExcelWorksheet workSheet = _package.Workbook.Worksheets[workSheetNumber];

                int row = startRow;
                int column = startColumn;

                foreach (IList<DbValue> dataRow in data.Rows)
                {
                    // proccess each row
                    WhiteExcelRow(workSheet, row, column, dataRow);
                    row++;
                }

                // save excel file
                FileInfo fileInfo = new FileInfo(_reportFile);

                if (fileInfo.Exists)
                    fileInfo.Delete();
                if (Directory.Exists(fileInfo.DirectoryName) == false)
                    Directory.CreateDirectory(fileInfo.DirectoryName);

                _package.SaveAs(fileInfo);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void WhiteExcelRow(ExcelWorksheet workSheet, int row, int column, IList<DbValue> dataRow)
        {         
            foreach (DbValue columnValue in dataRow)
            {
                ExcelRange range = workSheet.Cells[row, column];
                range.Value = columnValue.Value;
                column++;
            }
        }

        private const int WorkSheetNumberIndex = 0;
        private const int StartRowIndex = 1;
        private const int StartColumnIndex = 2;

        private readonly string _template;
        private readonly string _reportFile;
        private readonly ExcelPackage _package;
    }
}
