using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReportGenerator.Core.Tests.TestUtils
{
    public static class TestDatabaseManager
    {
        public static void CreateDatabase(string serverInstance, string database, bool drop = false)
        {
            if (drop)
            {
                try
                {
                    DropDatabase(serverInstance, database);
                }
                catch (Exception e) { }
            }
            ExecuteStatement(GetConnectionString(serverInstance, MasterDatabase), string.Format(CreateDatabaseStatementTemplate, database));
        }

        public static void DropDatabase(string serverInstance, string database)
        {
            ExecuteStatement(GetConnectionString(serverInstance, MasterDatabase), string.Format(DropDatabaseStatementTemplate, database));
        }

        public static void ExecuteSql(string serverInstance, string database, string sql)
        {
            ExecuteStatement(GetConnectionString(serverInstance, database), sql);
        }

        private static void ExecuteStatement(string connectionString, string statement)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand createCommand = new SqlCommand(statement, connection);
                createCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static  string GetConnectionString(string serverInstance, string database)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = serverInstance;
            builder.InitialCatalog = database;
            builder.TrustServerCertificate = true;
            return builder.ConnectionString;
        }

        private const string MasterDatabase = "master";
        private const string CreateDatabaseStatementTemplate = "CREATE DATABASE {0};";
        private const string DropDatabaseStatementTemplate = "ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;  DROP DATABASE [{0}];";
    }
}
