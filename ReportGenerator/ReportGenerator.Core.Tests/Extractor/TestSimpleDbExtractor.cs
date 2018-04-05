using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using ReportGenerator.Core.StatementsGenerator;
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
            string masterDbConnString = GetConnectionString(MasterDatabase);
            using (SqlConnection connection = new SqlConnection(masterDbConnString))
            {
                connection.Open();
                SqlCommand createCommand = new SqlCommand(string.Format(CreateDatabaseStatementTemplate, TestDatabase), connection);
                createCommand.ExecuteNonQuery();
                connection.Close();
            }

            string testDbConnString = GetConnectionString(TestDatabase);
            using (SqlConnection connection = new SqlConnection(testDbConnString))
            {
                connection.Open();
                // read statements from script and execute ...
                connection.Close();
            }
        }

        private void TearDownTestData()
        {
            string masterDbConnString = GetConnectionString(MasterDatabase);
            using (SqlConnection connection = new SqlConnection(masterDbConnString))
            {
                connection.Open();
                SqlCommand createCommand = new SqlCommand(string.Format(DropDatabaseStatementTemplate, TestDatabase), connection);
                createCommand.ExecuteNonQuery();
                connection.Close();
            }
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
        private string DropDatabaseStatementTemplate = "DROP DATABASE {0};";
    }
}