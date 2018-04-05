using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Data.Parameters;
using ReportGenerator.Core.Extractor;
using ReportGenerator.Core.Tests.TestUtils;
using Xunit;

namespace ReportGenerator.Core.Tests.Extractor
{
    public class TestSimpleDbExtractor
    {
        [Fact]
        public void TestExctractFromStoredProcNoParams()
        {
            SetUpTestData();
            // testing is here
            IDbExtractor extractor = new SimpleDbExtractor(Server, TestDatabase);
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
        public void TestExctractFromStoredProcWithCityParam(string parameterValue, int expectedNumberOfRows)
        {
            SetUpTestData();
            // testing is here
            IDbExtractor extractor = new SimpleDbExtractor(Server, TestDatabase);
            Task<DbData> result = extractor.ExtractAsync(TestStoredProcedureWithCity, 
                                                         new List<StoredProcedureParameter>{ new StoredProcedureParameter(SqlDbType.NVarChar, "City", parameterValue) });
            result.Wait();
            DbData rows = result.Result;
            Assert.Equal(expectedNumberOfRows, rows.Rows.Count);
            TearDownTestData();
        }

        private void SetUpTestData()
        {
            TestDatabaseManager.CreateDatabase(Server, TestDatabase, true);
            string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(CreateDatabaseScript));
            string insertDataStatement = File.ReadAllText(Path.GetFullPath(InsertDataScript));
            TestDatabaseManager.ExecuteSql(Server, TestDatabase, createDatabaseStatement);
            TestDatabaseManager.ExecuteSql(Server, TestDatabase, insertDataStatement);
        }

        private void TearDownTestData()
        {
            TestDatabaseManager.DropDatabase(Server, TestDatabase);
        }

        private const string Server = @"(localdb)\mssqllocaldb";
        private const string TestDatabase = "ReportGeneratorTestDb";

        private const string CreateDatabaseScript = @"..\..\..\DbScripts\CreateDb.sql";
        private const string InsertDataScript = @"..\..\..\DbScripts\CreateData.sql";

        private const string TestStoredProcedureWithoutParams = "SelectCitizensWithCities";
        private const string TestStoredProcedureWithCity = "SelectCitizensWithCitiesByCity";
    }
}