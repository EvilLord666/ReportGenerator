using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using ReportGenerator.Core.Data;

namespace ReportGenerator.Core.ReportsGenerator
{
    public class ExcelReportGenerator : IReportGenerator
    {
        public ExcelReportGenerator(ILogger<ExcelReportGenerator> logger, string template, string reportFile)
        {
            _logger = logger;
            if (string.IsNullOrEmpty(template))
            {
                _logger.LogError("Template is null");
                throw new ArgumentNullException("template");
            }

            _template = template;
            _reportFile = reportFile;
            FileInfo fileInfo = new FileInfo(Path.GetFullPath(_template));
            if (File.Exists(_template))
                _package = new ExcelPackage(fileInfo);
            else
            {
                _logger.LogError("Template file ");
                throw new FileNotFoundException("template file does not exists");
            }
        }

        public async Task<bool> GenerateAsync(DbData data, object[] parameters)
        {
            
            const int parametersNumber = 3;
            if (parameters.Length < parametersNumber)
            {
                _logger.LogError($"Expected at least {parametersNumber} parameters (workSheetNumber, startRow and startColumn)");
                throw new InvalidDataException("Invalid parameters array length");
            }

            if (data == null)
            {
                _logger.LogWarning("Db data is NULL (data obtained from database)");
                return false;
            }

            try
            {
                int workSheetNumber = Convert.ToInt32(parameters[WorkSheetNumberIndex]);
                int startRow = Convert.ToInt32(parameters[StartRowIndex]);
                int startColumn = Convert.ToInt32(parameters[StartColumnIndex]);
                _logger.LogDebug($"Write DB data to excel file with template: {_template} at worksheet: {workSheetNumber}, start row: {startRow}, start column: {startColumn}");
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
                _logger.LogDebug("Write DB data to excel file completed");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred during writing report data to ms excel file, exception is: {e}");
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
        private readonly ILogger<ExcelReportGenerator> _logger;
    }
}
