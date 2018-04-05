using System.IO;
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
            TearDownTestData();
        }

        [Fact]
        public void TestExctractFromStoredProcWithParams()
        {
            SetUpTestData();
            // testing is here
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
    }
}