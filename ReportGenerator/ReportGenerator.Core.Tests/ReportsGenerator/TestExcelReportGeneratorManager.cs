using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Database;
using ReportGenerator.Core.Database.Managers;
using ReportGenerator.Core.Database.Utils;
using ReportGenerator.Core.Helpers;
using ReportGenerator.Core.ReportsGenerator;
using Xunit;

namespace ReportGenerator.Core.Tests.ReportsGenerator
{
    public class TestExcelReportGeneratorManager
    {
        [Fact]
        public void TestGenerateReportSqlServer()
        {
            _testSqlServerDbName = TestSqlServerDatabasePattern + "_" + DateTime.Now.Millisecond.ToString();
            SetUpSqlServerTestData();
            // executing extraction ...
            object[] parameters = ExcelReportGeneratorHelper.CreateParameters(1, 2, 3);
            ILoggerFactory loggerFactory = new LoggerFactory();
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(loggerFactory, DbEngine.SqlServer, 
                                                                              TestSqlServerHost, _testSqlServerDbName);
            Task<bool> result = manager.GenerateAsync(TestExcelTemplate, SqlServerDataExecutionConfig, ReportFile, parameters);
            result.Wait();
            Assert.True(result.Result);
            TearDownSqlServerTestData();
        }

        [Fact]
        public void TestGenerateReportSqLite()
        {
            SetUpSqLiteTestData();
            object[] parameters = ExcelReportGeneratorHelper.CreateParameters(1, 2, 3);
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(loggerFactory, DbEngine.SqLite, _connectionString);
            Task<bool> result = manager.GenerateAsync(TestExcelTemplate, SqLiteDataExecutionConfig, ReportFile, parameters);
            result.Wait();
            Assert.True(result.Result);
            TearDownSqLiteTestData();
        }
        
        [Fact]
        public void TestGenerateReportPostgres()
        {
            SetUpPostgresSqlTestData();
            object[] parameters = ExcelReportGeneratorHelper.CreateParameters(1, 2, 3);
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(loggerFactory, DbEngine.PostgresSql, _connectionString);
            Task<bool> result = manager.GenerateAsync(TestExcelTemplate, PostgresSqlDataExecutionConfig, ReportFile, parameters);
            result.Wait();
            Assert.True(result.Result);
            TearDownPostgresSqlTestData();
        }
        
        [Fact]
        public void TestGenerateReportMySql()
        {
            SetUpMySqlTestData();
            object[] parameters = ExcelReportGeneratorHelper.CreateParameters(1, 2, 3);
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(loggerFactory, DbEngine.MySql, _connectionString);
            Task<bool> result = manager.GenerateAsync(TestExcelTemplate, MySqlDataExecutionConfig, ReportFile, parameters);
            result.Wait();
            Assert.True(result.Result);
            TearDownMySqlTestData();
        }

        private void SetUpSqlServerTestData()
        {
            _dbManager = new CommonDbManager(DbEngine.SqlServer, _loggerFactory.CreateLogger<CommonDbManager>());
            IDictionary<string, string> connectionStringParams = new Dictionary<string, string>()
            {
                {DbParametersKeys.HostKey, TestSqlServerHost},
                {DbParametersKeys.DatabaseKey, _testSqlServerDbName},
                {DbParametersKeys.UseIntegratedSecurityKey, "true"},
                {DbParametersKeys.UseTrustedConnectionKey, "true"}
            };
            _connectionString = ConnectionStringBuilder.Build(DbEngine.SqlServer, connectionStringParams);
            _dbManager.CreateDatabase(_connectionString, true);
            // 
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(SqlServerCreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(SqlServerInsertDataScript));
            _dbManager.ExecuteNonQueryAsync(_connectionString, createDatabaseStatement).Wait();
            _dbManager.ExecuteNonQueryAsync(_connectionString, insertDataStatement).Wait();
        }

        private void TearDownSqlServerTestData()
        {
            _dbManager.DropDatabase(_connectionString);
        }

        private void SetUpSqLiteTestData()
        {
            _dbManager = new CommonDbManager(DbEngine.SqLite, _loggerFactory.CreateLogger<CommonDbManager>());
            IDictionary<string, string> connectionStringParams = new Dictionary<string, string>()
            {
                {DbParametersKeys.DatabaseKey, TestSqLiteDatabase},
                {DbParametersKeys.DatabaseEngineVersion, "3"}
            };
            _connectionString = ConnectionStringBuilder.Build(DbEngine.SqLite, connectionStringParams);
            _dbManager.CreateDatabase(_connectionString, true);
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(SqLiteCreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(SqLiteInsertDataScript));
            _dbManager.ExecuteNonQueryAsync(_connectionString, createDatabaseStatement).Wait();
            _dbManager.ExecuteNonQueryAsync(_connectionString, insertDataStatement).Wait();
        }

        private void TearDownSqLiteTestData()
        {
            _dbManager.DropDatabase(_connectionString);
        }
        
        private void SetUpMySqlTestData()
        {
            _dbManager = new CommonDbManager(DbEngine.MySql, _loggerFactory.CreateLogger<CommonDbManager>());
            IDictionary<string, string> connectionStringParams = new Dictionary<string, string>()
            {
                {DbParametersKeys.HostKey, TestMySqlHost},
                {DbParametersKeys.DatabaseKey, TestMySqlDatabase},
                // {DbParametersKeys.UseIntegratedSecurityKey, "true"} // is not working ...
                {DbParametersKeys.LoginKey, "root"},
                {DbParametersKeys.PasswordKey, "123"}
            };
            _connectionString = ConnectionStringBuilder.Build(DbEngine.MySql, connectionStringParams);
            _dbManager.CreateDatabase(_connectionString, true);
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(MySqlCreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(MySqlInsertDataScript));
            _dbManager.ExecuteNonQueryAsync(_connectionString, createDatabaseStatement).Wait();
            _dbManager.ExecuteNonQueryAsync(_connectionString, insertDataStatement).Wait();
        }

        private void TearDownMySqlTestData()
        {
            _dbManager.DropDatabase(_connectionString);
        }

        private void SetUpPostgresSqlTestData()
        {
            _dbManager = new CommonDbManager(DbEngine.PostgresSql, _loggerFactory.CreateLogger<CommonDbManager>());
            IDictionary<string, string> connectionStringParams = new Dictionary<string, string>()
            {
                {DbParametersKeys.HostKey, TestPostgresSqlHost},
                {DbParametersKeys.DatabaseKey, TestPostgresSqlDatabase},
                // {DbParametersKeys.UseIntegratedSecurityKey, "true"} // is not working ...
                {DbParametersKeys.LoginKey, "postgres"},
                {DbParametersKeys.PasswordKey, "123"}
            };
            _connectionString = ConnectionStringBuilder.Build(DbEngine.PostgresSql, connectionStringParams);
            _dbManager.CreateDatabase(_connectionString, true);
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(PostgresSqlCreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(PostgresSqlInsertDataScript));
            _dbManager.ExecuteNonQueryAsync(_connectionString, createDatabaseStatement).Wait();
            _dbManager.ExecuteNonQueryAsync(_connectionString, insertDataStatement).Wait();
        }
        
        private void TearDownPostgresSqlTestData()
        {
            
        }

        private const string TestExcelTemplate = @"..\..\..\TestExcelTemplates\CitizensTemplate.xlsx";
        private const string ReportFile = @".\Report.xlsx";
        private const string SqlServerDataExecutionConfig = @"..\..\..\ExampleConfig\sqlServerDataExtractionParams.xml";
        private const string SqLiteDataExecutionConfig = @"..\..\..\ExampleConfig\sqLiteDataExtractionParams.xml";
        private const string MySqlDataExecutionConfig = @"..\..\..\ExampleConfig\mySql_testReport4_StoredProcedure.xml";
        private const string PostgresSqlDataExecutionConfig = @"..\..\..\postgresViewDataExtractionParams.xml";

        private const string TestSqlServerHost = @"(localdb)\mssqllocaldb";
        private const string TestSqlServerDatabasePattern = "ReportGeneratorTestDb";
        private const string TestSqLiteDatabase = "ReportGeneratorTestDb.sqlite";
        private const string TestMySqlHost = "localhost";
        private const string TestMySqlDatabase = "ReportGeneratorTestDb";
        private const string TestPostgresSqlHost = "localhost";
        private const string TestPostgresSqlDatabase = "ReportGeneratorTestDb";

        private const string SqlServerCreateDatabaseScript = @"..\..\..\DbScripts\SqlServerCreateDb.sql";
        private const string SqlServerInsertDataScript = @"..\..\..\DbScripts\SqlServerCreateData.sql";
        private const string SqLiteCreateDatabaseScript = @"..\..\..\DbScripts\SqLiteCreateDb.sql";
        private const string SqLiteInsertDataScript = @"..\..\..\DbScripts\SqLiteCreateData.sql";
        private const string MySqlCreateDatabaseScript = @"..\..\..\DbScripts\MySqlCreateDb.sql";
        private const string MySqlInsertDataScript = @"..\..\..\DbScripts\MySqlCreateData.sql";
        private const string PostgresSqlCreateDatabaseScript = @"..\..\..\DbScripts\PostgresSqlCreateDb.sql";
        private const string PostgresSqlInsertDataScript = @"..\..\..\DbScripts\PostgresSqlCreateData.sql";

        private string _testSqlServerDbName;
        private string _connectionString;
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
        private IDbManager _dbManager;
    }
}
