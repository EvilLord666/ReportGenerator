using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Task<int> generatorTask = generator.GenerateAsync(data, parameters);
            
            generatorTask.Wait();
            int result = generatorTask.Result;
            Assert.True(result > 0);
            Assert.True(File.Exists(reportFile));

            IList<string> header = File.ReadAllLines(TestCsvTemplate).ToList();
            IList<string> actualLines = File.ReadAllLines(reportFile).ToList();
            IList<string> expectedLines = TestData.GetCsvSampleData(header, CommaSeparator);

            Assert.Equal(expectedLines.Count, actualLines.Count);
            for (int i = 0; i < expectedLines.Count; i++)
            {
                Assert.Equal(expectedLines[i], actualLines[i]);
            }

            if (File.Exists(reportFile))
                File.Delete(reportFile);
        }
        
        private const string TestCsvTemplate = @"..\..\..\TestCsvTemplates\CitizensTemplate.csv";
        private const string ReportFileTemplate = @".\Report_{0}.csv";
        private const string CommaSeparator = ",";
    }
}