using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ReportGenerator.Core.Helpers;
using ReportGenerator.Core.ReportsGenerator;
using ReportGenerator.Core.Tests.TestUtils;
using Xunit;

namespace ReportGenerator.Core.Tests.ReportsGenerator
{
    public class TestExcelReportGeneratorManager
    {
        [Fact]
        public void TestGenerateReport()
        {
            _testDbName = TestDatabasePattern + "_" + DateTime.Now.ToString("YYYYMMDDHHmmss");
            SetUpTestData();
            // executing extraction ...
            object[] parameters = ExcelReportGeneratorHelper.CreateParameters(1, 2, 3);
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(Server, _testDbName);
            Task<bool> result = manager.GenerateAsync(TestExcelTemplate, DataExecutionConfig, ReportFile, parameters);
            result.Wait();
            Assert.True(result.Result);
            TearDownTestData();
        }

        private void SetUpTestData()
        {
            TestDatabaseManager.CreateDatabase(Server, _testDbName);
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(CreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(InsertDataScript));
            TestDatabaseManager.ExecuteSql(Server, _testDbName, createDatabaseStatement);
            TestDatabaseManager.ExecuteSql(Server, _testDbName, insertDataStatement);
        }

        private void TearDownTestData()
        {
            TestDatabaseManager.DropDatabase(Server, _testDbName);
        }

        private const string TestExcelTemplate = @"..\..\..\TestExcelTemplates\CitizensTemplate.xlsx";
        private const string ReportFile = @".\Report.xlsx";
        private const string DataExecutionConfig = @"..\..\..\ExampleConfig\dataExtractionParams.xml";

        private const string Server = @"(localdb)\mssqllocaldb";
        private const string TestDatabasePattern = "ReportGeneratorTestDb";

        private const string CreateDatabaseScript = @"..\..\..\DbScripts\CreateDb.sql";
        private const string InsertDataScript = @"..\..\..\DbScripts\CreateData.sql";

        private string _testDbName;
    }
}
