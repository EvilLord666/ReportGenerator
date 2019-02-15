using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DbTools.Core;
using DbTools.Core.Managers;
using DbTools.Simple.Extensions;
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

        [Theory]
        [InlineData(DbEngine.SqlServer, GlobalTestsParams.TestSqlServerHost, GlobalTestsParams.TestSqlServerDatabasePattern, true, "", "", 
                    GlobalTestsParams.SqlServerCreateDatabaseScript, GlobalTestsParams.SqlServerInsertDataScript, 
                    GlobalTestsParams.SqlServerViewDataExecutionConfig)]
        [InlineData(DbEngine.SqlServer, GlobalTestsParams.TestSqlServerHost, GlobalTestsParams.TestSqlServerDatabasePattern, true, "", "", 
                    GlobalTestsParams.SqlServerCreateDatabaseScript, GlobalTestsParams.SqlServerInsertDataScript, 
                    GlobalTestsParams.SqlServerStoredProcedureDataExecutionConfig)]
        [InlineData(DbEngine.SqLite, "", GlobalTestsParams.TestSqLiteDatabase, true, "", "", 
                    GlobalTestsParams.SqLiteCreateDatabaseScript, GlobalTestsParams.SqLiteInsertDataScript, 
                    GlobalTestsParams.SqLiteViewDataExecutionConfig)]
        [InlineData(DbEngine.MySql, GlobalTestsParams.TestMySqlHost, GlobalTestsParams.TestMySqlDatabase, false, "root", "123", 
                    GlobalTestsParams.MySqlCreateDatabaseScript, GlobalTestsParams.MySqlInsertDataScript, 
                    GlobalTestsParams.MySqlStoredProcedureDataExecutionConfig)]
        [InlineData(DbEngine.PostgresSql, GlobalTestsParams.TestPostgresSqlHost, GlobalTestsParams.TestPostgresSqlDatabase, false, "postgres", "123", 
                    GlobalTestsParams.PostgresSqlCreateDatabaseScript, GlobalTestsParams.PostgresSqlInsertDataScript, 
                    GlobalTestsParams.PostgresSqlViewDataExecutionConfig)]
        public void TestGenerateReport(DbEngine dbEngine, string host, string database, bool useIntegratedSecurity,
                                       string userName, string password, string dbCreateScriptFile, string insertDataScriptFile,
                                       string executionConfigFile)
        {
            database = dbEngine == DbEngine.SqlServer ? database + "_" + DateTime.Now.Millisecond : database;
            IList<string> scripts = new List<string>() {dbCreateScriptFile, insertDataScriptFile};
            object[] executionParameters = ExcelReportGeneratorHelper.CreateParameters(1, 2, 3);
            TestGenerateReportImplAndCheck(dbEngine, host, database, useIntegratedSecurity, userName, password, scripts,
                                           TestExcelTemplate, executionConfigFile, ReportFile, executionParameters);
        }

        private void TestGenerateReportImplAndCheck(DbEngine dbEngine, string host, string database, 
                                                    bool integratedSecurity, string userName, string password,
                                                    IList<string> scripts, 
                                                    string excelTemplateFile, string executionConfigFile, string outputReportFile, 
                                                    object[] executionParameters)
        {
            _dbManager = new CommonDbManager(dbEngine, _loggerFactory.CreateLogger<CommonDbManager>());
            _connectionString = _dbManager.Create(dbEngine, host, database, integratedSecurity, userName, password, scripts);
            ILoggerFactory loggerFactory = new LoggerFactory();
            IReportGeneratorManager manager = new ExcelReportGeneratorManager(loggerFactory, dbEngine, _connectionString);
            Task<bool> result = manager.GenerateAsync(excelTemplateFile, executionConfigFile, outputReportFile, executionParameters);
            result.Wait();
            Assert.True(result.Result);
            _dbManager.DropDatabase(_connectionString);
        }

        private const string TestExcelTemplate = @"..\..\..\TestExcelTemplates\CitizensTemplate.xlsx";
        private const string ReportFile = @".\Report.xlsx";

        private string _connectionString;
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
        private IDbManager _dbManager;
    }
}
