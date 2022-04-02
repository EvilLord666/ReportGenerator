using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.ReportsGenerator;
using ReportGenerator.Core.Tests.TestUtils;
using Xunit;

namespace ReportGenerator.Core.Tests.ReportsGenerator
{
    public class TestExcelReportGenerator
    {
        [Fact]
        public void TestGenerate()
        {
            string reportFile = string.Format(ReportFileTemplate, Guid.NewGuid());
            if (File.Exists(reportFile))
                File.Delete(reportFile);
            ILoggerFactory loggerFactory = new LoggerFactory();
            ILogger<ExcelReportGenerator> logger = loggerFactory.CreateLogger<ExcelReportGenerator>();
            IReportGenerator generator = new ExcelReportGenerator(logger, TestExcelTemplate, reportFile);
            // Worksheet - 1, start row - 2, start column - 3
            object[] parameters = {1, 2, 3};
            DbData data = TestData.GetSampleData();
            Task<bool> generatorTask = generator.GenerateAsync(data, parameters);
            generatorTask.Wait();
            bool result = generatorTask.Result;
            Assert.True(result);
            Assert.True(File.Exists(reportFile));

            // todo: umv: add read excel doc and check

            if (File.Exists(reportFile))
                File.Delete(reportFile);
        }

        private const string TestExcelTemplate = @"..\..\..\TestExcelTemplates\CitizensTemplate.xlsx";
        private const string ReportFileTemplate = @".\Report_{0}.xlsx";
    }
}
