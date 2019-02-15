using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DbTools.Core;
using DbTools.Core.Managers;
using DbTools.Simple.Managers;
using DbTools.Simple.Utils;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Helpers;
using ReportGenerator.Core.ReportsGenerator;
using Xunit;

namespace ReportGenerator.Core.Tests.ReportsGenerator
{
    // todo: umv: test data execution both with View and StoredProcedure config and check resulting data
    public class TestExcelReportGeneratorManager
    {
        [Fact]
        public void TestGenerateReportSqlServer()
        {
            _testSqlServerDbName = GlobalTestsParams.TestSqlServerDatabasePattern + "_" + DateTime.Now.Millisecond.ToString();
            SetUpSqlServerTestData();
            // executing extraction ...
            object[] parameters = ExcelReportGeneratorHelper.CreateParameters(1, 2, 3);
            ILoggerFactory loggerFactory = new LoggerFactory();
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(loggerFactory, DbEngine.SqlServer, 
                                                                              GlobalTestsParams.TestSqlServerHost, 
                                                                              _testSqlServerDbName);
            Task<bool> result = manager.GenerateAsync(TestExcelTemplate, GlobalTestsParams.SqlServerViewDataExecutionConfig, ReportFile, parameters);
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
            // loggerFactory.AddConsole();
            // loggerFactory.AddDebug();
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(loggerFactory, DbEngine.SqLite, _connectionString);
            Task<bool> result = manager.GenerateAsync(TestExcelTemplate, GlobalTestsParams.SqLiteViewDataExecutionConfig, ReportFile, parameters);
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
            // loggerFactory.AddConsole();
            // loggerFactory.AddDebug();
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(loggerFactory, DbEngine.PostgresSql, _connectionString);
            Task<bool> result = manager.GenerateAsync(TestExcelTemplate, GlobalTestsParams.PostgresSqlViewDataExecutionConfig, ReportFile, parameters);
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
            // loggerFactory.AddConsole();
            // loggerFactory.AddDebug();
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(loggerFactory, DbEngine.MySql, _connectionString);
            Task<bool> result = manager.GenerateAsync(TestExcelTemplate, GlobalTestsParams.MySqlStoredProcedureDataExecutionConfig, ReportFile, parameters);
            result.Wait();
            Assert.True(result.Result);
            TearDownMySqlTestData();
        }

        private void SetUpSqlServerTestData()
        {
            _dbManager = new CommonDbManager(DbEngine.SqlServer, _loggerFactory.CreateLogger<CommonDbManager>());
            IDictionary<string, string> connectionStringParams = new Dictionary<string, string>()
            {
                {DbParametersKeys.HostKey, GlobalTestsParams.TestSqlServerHost},
                {DbParametersKeys.DatabaseKey, _testSqlServerDbName},
                {DbParametersKeys.UseIntegratedSecurityKey, "true"},
                {DbParametersKeys.UseTrustedConnectionKey, "true"}
            };
            _connectionString = ConnectionStringBuilder.Build(DbEngine.SqlServer, connectionStringParams);
            _dbManager.CreateDatabase(_connectionString, true);
            // 
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(GlobalTestsParams.SqlServerCreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(GlobalTestsParams.SqlServerInsertDataScript));
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
                {DbParametersKeys.DatabaseKey, GlobalTestsParams.TestSqLiteDatabase},
                {DbParametersKeys.DatabaseEngineVersion, "3"}
            };
            _connectionString = ConnectionStringBuilder.Build(DbEngine.SqLite, connectionStringParams);
            _dbManager.CreateDatabase(_connectionString, true);
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(GlobalTestsParams.SqLiteCreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(GlobalTestsParams.SqLiteInsertDataScript));
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
                {DbParametersKeys.HostKey, GlobalTestsParams.TestMySqlHost},
                {DbParametersKeys.DatabaseKey, GlobalTestsParams.TestMySqlDatabase},
                // {DbParametersKeys.UseIntegratedSecurityKey, "true"} // is not working ...
                {DbParametersKeys.LoginKey, "root"},
                {DbParametersKeys.PasswordKey, "123"}
            };
            _connectionString = ConnectionStringBuilder.Build(DbEngine.MySql, connectionStringParams);
            _dbManager.CreateDatabase(_connectionString, true);
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(GlobalTestsParams.MySqlCreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(GlobalTestsParams.MySqlInsertDataScript));
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
                {DbParametersKeys.HostKey, GlobalTestsParams.TestPostgresSqlHost},
                {DbParametersKeys.DatabaseKey, GlobalTestsParams.TestPostgresSqlDatabase},
                // {DbParametersKeys.UseIntegratedSecurityKey, "true"} // is not working ...
                {DbParametersKeys.LoginKey, "postgres"},
                {DbParametersKeys.PasswordKey, "123"}
            };
            _connectionString = ConnectionStringBuilder.Build(DbEngine.PostgresSql, connectionStringParams);
            _dbManager.CreateDatabase(_connectionString, true);
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(GlobalTestsParams.PostgresSqlCreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(GlobalTestsParams.PostgresSqlInsertDataScript));
            _dbManager.ExecuteNonQueryAsync(_connectionString, createDatabaseStatement).Wait();
            _dbManager.ExecuteNonQueryAsync(_connectionString, insertDataStatement).Wait();
        }
        
        private void TearDownPostgresSqlTestData()
        {
            _dbManager.DropDatabase(_connectionString);
        }

        private const string TestExcelTemplate = @"..\..\..\TestExcelTemplates\CitizensTemplate.xlsx";
        private const string ReportFile = @".\Report.xlsx";

        private string _testSqlServerDbName;
        private string _connectionString;
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
        private IDbManager _dbManager;
    }
}
