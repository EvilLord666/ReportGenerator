using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Data.Parameters;
using ReportGenerator.Core.Database;
using ReportGenerator.Core.Database.Managers;
using ReportGenerator.Core.Database.Utils;
using ReportGenerator.Core.Extractor;
using Xunit;

namespace ReportGenerator.Core.Tests.Extractor
{
    // todo: umv: check data rows in future
    public class TestSimpleDbExtractor
    {
        public TestSimpleDbExtractor()
        {
            _loggerFactory = new LoggerFactory();
        }

        [Fact]
        public void TestExtractFromStoredProcNoParams()
        {
            SetUpTestData();
            // testing is here
            
            IDbExtractor extractor = new SimpleDbExtractor(_loggerFactory, DbEngine.SqlServer, Server, TestDatabase);
            Task<DbData> result = extractor.ExtractAsync(TestStoredProcedureWithoutParams, new List<StoredProcedureParameter>());
            result.Wait();
            DbData rows = result.Result;
            const int expectedNumberOfRows = 15;
            Assert.Equal(expectedNumberOfRows, rows.Rows.Count);
            TearDownTestData();
        }

        [Theory]
        [InlineData("г. Екатеринбург", 5)]
        [InlineData("г. Нижний Тагил", 3)]
        [InlineData("г. Первоуральск", 3)]
        [InlineData("г. Челябинск", 4)]
        public void TestExtractFromStoredProcWithCityParam(string parameterValue, int expectedNumberOfRows)
        {
            SetUpTestData();
            // testing is here
            IDbExtractor extractor = new SimpleDbExtractor(_loggerFactory, DbEngine.SqlServer, Server, TestDatabase);
            Task<DbData> result = extractor.ExtractAsync(TestStoredProcedureWithCity, 
                                                         new List<StoredProcedureParameter>{ new StoredProcedureParameter(SqlDbType.NVarChar, "City", parameterValue) });
            result.Wait();
            DbData rows = result.Result;
            Assert.Equal(expectedNumberOfRows, rows.Rows.Count);
            TearDownTestData();
        }

        [Theory]
        [InlineData("г. Екатеринбург", 33, 2)]
        [InlineData("г. Нижний Тагил", 40, 1)]
        [InlineData("г. Первоуральск", 31, 1)]
        [InlineData("г. Челябинск", 15, 3)]
        public void TestExtractFromStoredProcWithCityAndAgeParams(string cityParameterValue, int ageParameterValue, int expectedNumberOfRows)
        {
            SetUpTestData();
            // testing is here
            IDbExtractor extractor = new SimpleDbExtractor(_loggerFactory, DbEngine.SqlServer, Server, TestDatabase);
            Task<DbData> result = extractor.ExtractAsync(TestStoredProcedureWithCityAndAge,  new List<StoredProcedureParameter>
            {
                new StoredProcedureParameter(SqlDbType.NVarChar, "City", cityParameterValue),
                new StoredProcedureParameter(SqlDbType.Int, "PersonAge", ageParameterValue)
            });
            result.Wait();
            DbData rows = result.Result;
            Assert.Equal(expectedNumberOfRows, rows.Rows.Count);
            TearDownTestData();
        }

        [Fact]
        public void TestExtractFromView()
        {
            SetUpTestData();
            // testing is here
            IDbExtractor extractor = new SimpleDbExtractor(_loggerFactory, DbEngine.SqlServer, Server, TestDatabase);
            Task<DbData> result = extractor.ExtractAsync(TestView, new ViewParameters());
            result.Wait();
            DbData rows = result.Result;
            int expectedNumberOfRows = 15;
            Assert.Equal(expectedNumberOfRows, rows.Rows.Count);
            TearDownTestData();
        }

        [Theory]
        [InlineData("N'Алексей'", null, 1)]
        [InlineData("N'Алексей'", true, 1)]
        [InlineData("N'Алексей'", false, 0)]
        [InlineData(null, true, 7)]
        [InlineData(null, false, 8)]
        public void TestExtractFromViewWithParams(string name, bool? sex, int expectedNumberOfRows)
        {
            SetUpTestData();
            // testing is here
            ViewParameters parameters = new ViewParameters();
            if (!string.IsNullOrEmpty(name))
            {
                parameters.WhereParameters.Add(new DbQueryParameter(null, "FirstName", "=", name));
            }
            if (sex.HasValue)
            {
                IList<JoinCondition> sexJoin = parameters.WhereParameters.Count > 0 ? new List<JoinCondition>() {JoinCondition.And}  : null;
                parameters.WhereParameters.Add(new DbQueryParameter(sexJoin, "Sex", "=", sex.Value ? "1" : "0"));
            }
            IDbExtractor extractor = new SimpleDbExtractor(_loggerFactory, DbEngine.SqlServer, Server, TestDatabase);
            Task<DbData> result = extractor.ExtractAsync(TestView, parameters);
            result.Wait();
            DbData rows = result.Result;
            Assert.Equal(expectedNumberOfRows, rows.Rows.Count);
            TearDownTestData();
        }

        private void SetUpTestData()
        {
            /*TestSqlServerDatabaseManager.CreateDatabase(Server, TestDatabase, true);
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(CreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(InsertDataScript));
            TestSqlServerDatabaseManager.ExecuteSql(Server, TestDatabase, createDatabaseStatement);
            TestSqlServerDatabaseManager.ExecuteSql(Server, TestDatabase, insertDataStatement);*/
            
            _dbManager = new CommonDbManager(DbEngine.SqlServer, _loggerFactory.CreateLogger<CommonDbManager>());
            IDictionary<string, string> connectionStringParams = new Dictionary<string, string>()
            {
                {DbParametersKeys.HostKey, Server},
                {DbParametersKeys.DatabaseKey, TestDatabase},
                {DbParametersKeys.UseIntegratedSecurityKey, "true"},
                {DbParametersKeys.UseTrustedConnectionKey, "true"}
            };
            _connectionString = ConnectionStringBuilder.Build(DbEngine.SqlServer, connectionStringParams);
            _dbManager.CreateDatabase(_connectionString, true);
            // 
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(CreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(InsertDataScript));
            _dbManager.ExecuteNonQueryAsync(_connectionString, createDatabaseStatement).Wait();
            _dbManager.ExecuteNonQueryAsync(_connectionString, insertDataStatement).Wait();
        }

        private void TearDownTestData()
        {
            _dbManager.DropDatabase(_connectionString);
        }

        private const string Server = @"(localdb)\mssqllocaldb";
        private const string TestDatabase = "ReportGeneratorTestDb";

        private const string CreateDatabaseScript = @"..\..\..\DbScripts\SqlServerCreateDb.sql";
        private const string InsertDataScript = @"..\..\..\DbScripts\SqlServerCreateData.sql";

        private const string TestStoredProcedureWithoutParams = "SelectCitizensWithCities";
        private const string TestStoredProcedureWithCity = "SelectCitizensWithCitiesByCity";
        private const string TestStoredProcedureWithCityAndAge = "SelectCitizensWithCitiesByCityAndAge";

        private const string TestView = "CitizensWithRegion";

        private string _connectionString;
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
        private IDbManager _dbManager;
    }
}