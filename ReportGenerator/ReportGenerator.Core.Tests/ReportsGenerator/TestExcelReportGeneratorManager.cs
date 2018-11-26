using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Helpers;
using ReportGenerator.Core.ReportsGenerator;
using ReportGenerator.Core.Tests.TestUtils;
using Xunit;

namespace ReportGenerator.Core.Tests.ReportsGenerator
{
    public class TestExcelReportGeneratorManager
    {
        [Fact]
        public void TestGenerateReportMsSql()
        {
            _testDbName = TestSqlServerDatabasePattern + "_" + DateTime.Now.ToString("YYYYMMDDHHmmss");
            SetUpSqlServerTestData();
            // executing extraction ...
            object[] parameters = ExcelReportGeneratorHelper.CreateParameters(1, 2, 3);
            ILoggerFactory loggerFactory = new LoggerFactory();
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(loggerFactory, Server, _testDbName);
            Task<bool> result = manager.GenerateAsync(TestExcelTemplate, DataExecutionConfig, ReportFile, parameters);
            result.Wait();
            Assert.True(result.Result);
            TearDownSqlServerTestData();
        }

        [Fact]
        public void TestGenerateReportSqlLite()
        {
            SetUpSqLiteTestData();
            // test impl ...
            TearDownSqLiteTestData();
        }
        
        public void TestGenerateReportPostgres()
        {
            
        }
        
        public void TestGenerateReportMySql()
        {
            
        }

        private void SetUpSqlServerTestData()
        {
            TestSqlServerDatabaseManager.CreateDatabase(Server, _testDbName);
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(CreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(InsertDataScript));
            TestSqlServerDatabaseManager.ExecuteSql(Server, _testDbName, createDatabaseStatement);
            TestSqlServerDatabaseManager.ExecuteSql(Server, _testDbName, insertDataStatement);
        }

        private void TearDownSqlServerTestData()
        {
            TestSqlServerDatabaseManager.DropDatabase(Server, _testDbName);
        }

        private void SetUpSqLiteTestData()
        {
            TestSqLiteDatabaseManager.CreateDatabase(TestSqLiteDatabase);
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(CreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(InsertDataScript));
            TestSqLiteDatabaseManager.ExecuteSql(TestSqLiteDatabase, createDatabaseStatement);
            TestSqLiteDatabaseManager.ExecuteSql(TestSqLiteDatabase, insertDataStatement);
        }

        private void TearDownSqLiteTestData()
        {
            TestSqLiteDatabaseManager.DropDatabase(TestSqLiteDatabase);
        }

        private const string TestExcelTemplate = @"..\..\..\TestExcelTemplates\CitizensTemplate.xlsx";
        private const string ReportFile = @".\Report.xlsx";
        private const string DataExecutionConfig = @"..\..\..\ExampleConfig\dataExtractionParams.xml";

        private const string Server = @"(localdb)\mssqllocaldb";
        private const string TestSqlServerDatabasePattern = "ReportGeneratorTestDb";
        private const string TestSqLiteDatabase = "ReportGeneratorTestDb.sqlite";

        private const string CreateDatabaseScript = @"..\..\..\DbScripts\CreateDb.sql";
        private const string InsertDataScript = @"..\..\..\DbScripts\CreateData.sql";

        private string _testDbName;
    }
}
