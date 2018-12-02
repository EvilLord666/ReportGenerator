using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ReportGenerator.Core.Database.Managers
{
    // todo: umv: pass logger here
    public class SqlServerDbManager : IDbManager
    {
        public bool CreateDatabase(string connectionString, bool dropIfExists)
        {
            try
            {
                if (dropIfExists)
                    DropDatabase(connectionString);
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                string createDbStatement = string.Format(CreateDatabaseStatementTemplate, builder.InitialCatalog);
                builder.InitialCatalog = MasterDatabase;
                return ExecuteStatement(builder.ConnectionString, createDbStatement);
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public bool DropDatabase(string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                string dropDbStatement = string.Format(DropDatabaseStatementTemplate, builder.InitialCatalog);
                builder.InitialCatalog = MasterDatabase;
                return ExecuteStatement(builder.ConnectionString, dropDbStatement);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool ExecuteNonQuery(IDbCommand command)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ExecuteNonQueryAsync(IDbCommand command)
        {
            throw new System.NotImplementedException();
        }

        public DbDataReader ExecuteDbReader(IDbCommand command)
        {
            throw new System.NotImplementedException();
        }

        public async Task<DbDataReader> ExecuteDbReaderAsync(IDbCommand command)
        {
            throw new System.NotImplementedException();
        }

        private bool ExecuteStatement(string connectionString, string statement)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(statement, connection);
                return ExecuteNonQuery(command);
            }
        }

        private const string MasterDatabase = "master";
        private const string CreateDatabaseStatementTemplate = "CREATE DATABASE {0}";
        private const string DropDatabaseStatementTemplate = "ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{0}];";
    }
}