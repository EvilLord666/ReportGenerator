using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using ReportGenerator.Core.StatementsGenerator;
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
            /*string masterDbConnString = GetConnectionString(MasterDatabase);
            using (SqlConnection connection = new SqlConnection(masterDbConnString))
            {
                connection.Open();
                SqlCommand createCommand = new SqlCommand(string.Format(CreateDatabaseStatementTemplate, TestDatabase), connection);
                createCommand.ExecuteNonQuery();
                connection.Close();
            }*/

            TestDatabaseManager.CreateDatabase(Server, TestDatabase, true);

            string testDbConnString = GetConnectionString(TestDatabase);
            using (SqlConnection connection = new SqlConnection(testDbConnString))
            {
                connection.Open();
                // read statements from script and execute ...
                string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(CreateDatabaseScript));
                string insertDataStatement = File.ReadAllText(Path.GetFullPath(InsertDataScript));
                SqlCommand command = new SqlCommand(createDatabaseStatement, connection);
                command.ExecuteNonQuery();
                command = new SqlCommand(insertDataStatement, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void TearDownTestData()
        {
            TestDatabaseManager.DropDatabase(Server, TestDatabase);
            /*string masterDbConnString = GetConnectionString(MasterDatabase);
            using (SqlConnection connection = new SqlConnection(masterDbConnString))
            {
                connection.Open();
                SqlCommand createCommand = new SqlCommand(string.Format(DropDatabaseStatementTemplate, TestDatabase), connection);
                createCommand.ExecuteNonQuery();
                connection.Close();
            }*/
        }

        private string GetConnectionString(string database)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = Server;
            builder.InitialCatalog = database;
            builder.TrustServerCertificate = true;
            return builder.ConnectionString;
        }

        private const string Server = @"(localdb)\mssqllocaldb";
        private const string TestDatabase = "ReportGeneratorTestDb";
        private const string MasterDatabase = "master";

        private string CreateDatabaseStatementTemplate = "CREATE DATABASE {0};";
        //private string DropDatabaseStatementTemplate = "ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;  DROP DATABASE [{0}];";

        private const string CreateDatabaseScript = @"..\..\..\DbScripts\CreateDb.sql";
        private const string InsertDataScript = @"..\..\..\DbScripts\CreateData.sql";
    }
}