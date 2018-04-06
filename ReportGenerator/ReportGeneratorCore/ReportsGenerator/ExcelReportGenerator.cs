using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using ReportGenerator.Core.Data;

namespace ReportGenerator.Core.ReportsGenerator
{
    public class ExcelReportGenerator : IReportGenerator
    {
        public ExcelReportGenerator(string template)
        {
            if(string.IsNullOrEmpty(template))
                throw new ArgumentNullException("template");
            _template = template;
            FileInfo fileInfo = new FileInfo(Path.GetFullPath(_template));
            if (File.Exists(_template))
                _package = new ExcelPackage(fileInfo);
            else
                throw new ApplicationException("template file does not exists");
        }

        public bool Generate(DbData data, object[] parameters)
        {
            if(parameters.Length < 3)
                throw new ApplicationException("Invalid parameters array length");
            if (data == null)
                return false;
            int workSheetNumber = Convert.ToInt32(parameters[WorkSheetNumberIndex]);
            int startRow = Convert.ToInt32(parameters[StartRowIndex]);
            int startColumn = Convert.ToInt32(parameters[StartColumnIndex]);
            ExcelWorksheet workSheet = _package.Workbook.Worksheets[workSheetNumber];

            foreach (IList<DbValue> dataRow in data.Rows)
            {
                // proccess each row
            }
            return false;
        }

        private const int WorkSheetNumberIndex = 0;
        private const int StartRowIndex = 1;
        private const int StartColumnIndex = 2;

        private readonly string _template;
        private readonly ExcelPackage _package;
    }
}
