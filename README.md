# ReportGenerator
## 1 Overview
A small tool (library) for generating reports (plain dataset) from ***Stored Procedures*** or ***Views*** following databases:
* MS SQL (SQL Server)
* MySQL
* Postgres

These reports could be generated in ***MS Excel*** or ***CSV*** formats with templates files (typically Headers only).
Logic of data extracting depends on parameters (variables) Stored procedures or Views (**WHERE** parameters, **ORDER BY** and **GROUP BY** parameters). 

Report generator core passes parameters for data filtering in View or Variables for Stored Procedures, parameters are taking from:
* 1) ExecutionConfig (.xml file)
* 2) in runtime (as instance of ExecutionConfig class).

There are two ways for getting data:

- from stored procedure (passing variables)
- from view (passing where clauses, ordering and grouping conditions)

In both 2 ways there are a possibility to manipulate data selection by setting parameters (for stored procedure) and automatic sql generation for filtering 
View data. It should be noted that there is a configuration of View and StoredProcedure in xml.

### 1.1 Configuration
For Stored Procedure Configuration we must set <DataSource>StoredProcedure</DataSource>

Name to stored procedure name

And also if we have aparameters we should specify collection of StoredProcedureParameters like, ParameterType is a integer value which is specific for databaseEngine :

SQL Server - SqlDbType (https://msdn.microsoft.com/ru-ru/library/system.data.sqldbtype(v=vs.110).aspx)

MySQL Server - MySqlDbType (https://dev.mysql.com/doc/dev/connector-net/8.0/html/T_MySql_Data_MySqlClient_MySqlDbType.htm)

Postgres - NpgsqlDbType (https://www.npgsql.org/doc/api/NpgsqlTypes.NpgsqlDbType.html)
`
Example of config for SQL server (examples of config could be found in ReportGenerator.Core.Tests/ExampleConfig)
```xml
<?xml version="1.0"?>
<ExecutionConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <!-- 
       If use SQL Server see for parameters Type : https://docs.microsoft.com/ru-ru/dotnet/api/system.data.sqldbtype 
    -->
    <DataSource>StoredProcedure</DataSource>
    <Name>GetSitizensByCityAndDateOfBirth</Name>
    <StoredProcedureParameters>
        <!-- SQL Server NVarChar enum value is 12-->
        <ParameterType>12</ParameterType> 
        <ParameterName>City</ParameterName>
        <ParameterValue xsi:type="xsd:string">N'Yekaterinburg</ParameterValue>
    </StoredProcedureParameters>
    <StoredProcedureParameters>
        <!-- SQL Server Int enum value is 8-->
        <ParameterType>8</ParameterType>
        <ParameterName>PostalCode</ParameterName>
        <ParameterValue xsi:type="xsd:string">620000</ParameterValue>
    </StoredProcedureParameters>
    <StoredProcedureParameters>
        <!-- SQL Server Date enum value is 4-->
        <ParameterType>4</ParameterType>
        <ParameterName>DateOfBirth</ParameterName>
        <ParameterValue xsi:type="xsd:string">'2018-01-01'</ParameterValue>
    </StoredProcedureParameters>
</ExecutionConfig>
```
For View Configuration is a little similar as DataSource we must note View:

<DataSource>View</DataSource>

Name is a name of View

But here we should also specify parameters for rows selection (WHERE, ORDER BY and GROUP BY parameters). All of them are represented with only one type - DbQueryParameter.

DbQueryParameter consist of following:

-collection of JoinCondition (works only for WHERE parameters) - is way of how parameters joined in WHERE statement by logical AND or logical OR, or maybe AND/OR with inversion - AND NOT Condition ... OR NOT Condition (see example below). Should be ommited for first parameter in WHERE statement.

-parameter Name make sense for every type of parameter (WHERE, ORDER BY and GROUP BY)

-comparison operator (make sense only for WHERE parameters) - is any valid SQL condition operator like >, <, =, IS, IS NOT, IN, BETWEEN, LIKE and so on...

-parameter value works for WHERE (value for comparison) and ORDER BY (ASC or DESC),  THESE are DEFAULT values, they OF COURSE could be set during the runtime

Example:

```xml
<?xml version="1.0"?>
<ExecutionConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <DataSource>View</DataSource>
    <Name>CitizensView</Name>
    <ViewParameters>
        <WhereParameters>
            <DbQueryParameter>
                <ParameterName>FirstName</ParameterName>
                <ParameterValue>N'Michael'</ParameterValue>
                <ComparisonOperator>=</ComparisonOperator>
            </DbQueryParameter>
            <DbQueryParameter>
                <Conditions>
                    <JoinCondition>And</JoinCondition>
                    <JoinCondition>Not</JoinCondition>
                </Conditions>
                <ParameterName>City</ParameterName>
                <ParameterValue>N'Yekaterinburg'</ParameterValue>
                <ComparisonOperator>=</ComparisonOperator>
            </DbQueryParameter>
            <DbQueryParameter>
                <Conditions>
                    <JoinCondition>Between</JoinCondition>
                </Conditions>
                <ParameterName>Age</ParameterName>
                <ParameterValue>18 AND 60</ParameterValue>
            </DbQueryParameter>
            <DbQueryParameter>
                <Conditions>
                    <JoinCondition>In</JoinCondition>
                </Conditions>
                <ParameterName>District</ParameterName>
                <ParameterValue>(N'D1', N'A3', N'A5', N'C7')</ParameterValue>
            </DbQueryParameter>
            <DbQueryParameter>
                <Conditions>
                    <JoinCondition>Or</JoinCondition>
                </Conditions>
                <ParameterName>Region</ParameterName>
                <ParameterValue>N'Sverdlovskaya oblast'</ParameterValue>
                <ComparisonOperator>!=</ComparisonOperator>
            </DbQueryParameter>
        </WhereParameters>
        <OrderByParameters>
            <DbQueryParameter>
                <ParameterName>FirstName</ParameterName>
                <ParameterValue>ASC</ParameterValue>
            </DbQueryParameter>
            <DbQueryParameter>
                <ParameterName>LastName</ParameterName>
                <ParameterValue>DESC</ParameterValue>
            </DbQueryParameter>
        </OrderByParameters>
        <GroupByParameters>
            <DbQueryParameter>
                <ParameterName>District</ParameterName>
            </DbQueryParameter>
        </GroupByParameters>
    </ViewParameters>
</ExecutionConfig>
```

## 2 Example of usage
Top interface is - IReportGeneratorManager.
For all functionality were written Unit tests with xUnit (sucks), full example could be found in unit test project (ReportGenerator.Core.Tests/ReportsGenerator/TestExcelReportGeneratorManager):

```csharp
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
                Task<int> result = manager.GenerateAsync(TestExcelTemplate, SqlServerDataExecutionConfig, ReportFile, parameters);
                result.Wait();
                Assert.True(result.Result > 0);
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
                Task<int> result = manager.GenerateAsync(TestExcelTemplate, SqLiteDataExecutionConfig, ReportFile, parameters);
                result.Wait();
                Assert.True(result.Result > 0);
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
                Task<int> result = manager.GenerateAsync(TestExcelTemplate, PostgresSqlViewDataExecutionConfig, ReportFile, parameters);
                result.Wait();
                Assert.True(result.Result > 0);
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
                Task<int> result = manager.GenerateAsync(TestExcelTemplate, MySqlDataExecutionConfig, ReportFile, parameters);
                result.Wait();
                Assert.True(result.Result > 0);
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
                _dbManager.DropDatabase(_connectionString);
            }

            private const string TestExcelTemplate = @"..\..\..\TestExcelTemplates\CitizensTemplate.xlsx";
            private const string ReportFile = @".\Report.xlsx";
            private const string SqlServerDataExecutionConfig = @"..\..\..\ExampleConfig\sqlServerDataExtractionParams.xml";
            private const string SqLiteDataExecutionConfig = @"..\..\..\ExampleConfig\sqLiteDataExtractionParams.xml";
            private const string MySqlDataExecutionConfig = @"..\..\..\ExampleConfig\mySql_testReport4_StoredProcedure.xml";
            private const string PostgresSqlViewDataExecutionConfig = @"..\..\..\ExampleConfig\postgresViewDataExtractionParams.xml";
            private const string PostgresSqlStoredProcedureDataExecutionConfig = @"..\..\..\ExampleConfig\postgresStoredProcDataExtractionParams.xml";

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
  `

## 3 Service for Dependency injection
  
Here is example of how to use ReportGenerator with dependency injection:
`csharp
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ReportGenerator.Core.Database;
    using ReportGenerator.Core.Database.Managers;
    using ReportGenerator.Core.Database.Utils;
    using ReportGenerator.Core.Extensions;
    using ReportGenerator.Core.ReportsGenerator;
    using Xunit;

    namespace ReportGenerator.Core.Tests.Extensions
    {
        public class ServiceCollectionExtensionsTests : IDisposable
        {
            public ServiceCollectionExtensionsTests()
            {
                _testDbName = TestDatabasePattern + "_" + DateTime.Now.ToString("YYYYMMDDHHmmss");
                _dbManager = new CommonDbManager(DbEngine.SqlServer, _loggerFactory.CreateLogger<CommonDbManager>());
                IDictionary<string, string> connectionStringParams = new Dictionary<string, string>()
                {
                    {DbParametersKeys.HostKey, Server},
                    {DbParametersKeys.DatabaseKey, _testDbName},
                    {DbParametersKeys.UseIntegratedSecurityKey, "true"},
                    {DbParametersKeys.UseTrustedConnectionKey, "true"}
                };
                _connectionString = ConnectionStringBuilder.Build(DbEngine.SqlServer, connectionStringParams);
                _dbManager.CreateDatabase(_connectionString, true);
                _services = new ServiceCollection();
                _services.AddScoped<ILoggerFactory>(_ => new LoggerFactory());
                _services.AddReportGenerator(DbEngine.SqlServer, _connectionString);
            }

            public void Dispose()
            {
                _dbManager.DropDatabase(_connectionString);
            }

            [Fact]
            public void TestServiceInstantiationViaProvider()
            {
                IServiceProvider serviceProvider = _services.BuildServiceProvider();
                IReportGeneratorManager reportGenerator = serviceProvider.GetService<IReportGeneratorManager>();
                
                Assert.NotNull(reportGenerator);
            }

            private const string Server = @"(localdb)\mssqllocaldb";
            private const string TestDatabasePattern = "ReportGeneratorTestDb";

            private readonly IServiceCollection _services;
            private readonly string _testDbName;
            private readonly string _connectionString;
            private IDbManager _dbManager;
            private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
        }
    }
```
## 4 GUI
  
A Full Web application (NET Core) with usage Report Generator as DI service will be implemented in this project: 
https://github.com/Wissance/ReportGeneratorWebGui it is also allow to use Web API for ReportsGeneration 
  
# 5 Nuget Package
Nuget package is available on nuget.org : https://www.nuget.org/packages/ReportsGenerator/
There are some strange troubles with import of version 1.1.0 and 1.1.1 they are **Broken and unlisted**. **ACTUAL NUGET PACKAGE IS 1.1.2**
