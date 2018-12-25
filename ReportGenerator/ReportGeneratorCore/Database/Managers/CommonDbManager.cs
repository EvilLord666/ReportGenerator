using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Database.Factories;

namespace ReportGenerator.Core.Database.Managers
{
    // todo: umv: pass logger here
    public class CommonDbManager : IDbManager
    {
        public CommonDbManager(DbEngine dbEngine, ILogger<CommonDbManager> logger)
        {
            _dbEngine = dbEngine;
            _logger = logger;
        }

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
                _logger.LogError($"An error occured during database creation, exception {e}");
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
                _logger.LogError($"An error occured during database drop, exception {e}");
                return false;
            }
        }

        // todo: umv: implement following methods
        public bool ExecuteNonQuery(IDbCommand command)
        {
            bool result = true;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured during execute non query, exception {e}");
                result = false;
            }
            finally
            {
                command.Dispose();
            }

            return result;
        }

        public async Task<bool> ExecuteNonQueryAsync(DbCommand command)
        {
            bool result = true;
            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured during execute non query async, exception {e}");
                result = false;
            }
            finally
            {
                command.Dispose();
            }

            return result;
        }

        public async Task<bool> ExecuteNonQueryAsync(string connectionString, string cmdText)
        {
            IDbConnection connection = DbConnectionFactory.Create(_dbEngine, connectionString);
            IDbCommand command = DbCommandFactory.Create(_dbEngine, connection, cmdText);
            return await ExecuteNonQueryAsync(command as DbCommand);
        }

        public IDataReader ExecuteDbReader(IDbCommand command)
        {
            IDataReader result = null;
            try
            {
                result = command.ExecuteReader();
            }
            catch (Exception e)
            {
                // todo: umv: log an Error
                result = null;
            }
            finally
            {
                command.Dispose();
            }

            return result;
        }

        public async Task<DbDataReader> ExecuteDbReaderAsync(DbCommand command)
        {
            DbDataReader result = null;
            try
            {
                result = await command.ExecuteReaderAsync();
            }
            catch (Exception e)
            {
                // todo: umv: log an Error
                result = null;
            }
            finally
            {
                command.Dispose();
            }

            return result;
        }

        public async Task<DbDataReader> ExecuteDbReaderAsync(string connectionString, string cmdText)
        {
            IDbConnection connection = DbConnectionFactory.Create(_dbEngine, connectionString);
            IDbCommand command = DbCommandFactory.Create(_dbEngine, connection, cmdText);
            return await ExecuteDbReaderAsync(command as DbCommand);
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

        private readonly DbEngine _dbEngine;
        private readonly ILogger<CommonDbManager> _logger;
    }
}