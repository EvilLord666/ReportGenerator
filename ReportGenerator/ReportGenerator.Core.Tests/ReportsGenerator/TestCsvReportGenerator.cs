using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.ReportsGenerator;
using ReportGenerator.Core.Tests.TestUtils;
using Xunit;

namespace ReportGenerator.Core.Tests.ReportsGenerator
{
    public class TestCsvReportGenerator
    {
        [Fact]
        public void TestGenerate()
        {
            string reportFile = string.Format(ReportFileTemplate, Guid.NewGuid());
            if (File.Exists(reportFile))
                File.Delete(reportFile);
            ILoggerFactory loggerFactory = new LoggerFactory();
            IReportGenerator generator = new CsvReportGenerator(loggerFactory, TestCsvTemplate, CommaSeparator, reportFile);
            object[] parameters = {};
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
        
        private const string TestCsvTemplate = @"..\..\..\TestExcelTemplates\CitizensTemplate.csv";
        private const string ReportFileTemplate = @".\Report_{0}.csv";
        private const string CommaSeparator = ",";
    }
}