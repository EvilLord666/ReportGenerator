using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Database.Factories;
using ReportGenerator.Core.Database.Utils;

namespace ReportGenerator.Core.Database.Managers
{
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
                string dbName = ConnectionStringHelper.GetDatabaseName(connectionString, _dbEngine);
                if (_dbEngine == DbEngine.SqLite)
                {
                    SQLiteConnection.CreateFile(dbName);
                    return true;
                }
                string createDbStatement = string.Format(CommonServerCreateDatabaseStatementTemplate, dbName);
                if (_dbEngine == DbEngine.SqlServer)
                    connectionString = ConnectionStringHelper.GetSqlServerMasterConnectionString(connectionString);
                if (_dbEngine == DbEngine.MySql)
                    connectionString = ConnectionStringHelper.GetMySqlDbNameLessConnectionString(connectionString);

                return ExecuteStatement(connectionString, createDbStatement);
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
                string dbName = ConnectionStringHelper.GetDatabaseName(connectionString, _dbEngine);
                if (_dbEngine == DbEngine.SqLite)
                {
                    if (File.Exists(dbName))
                        File.Delete(dbName);
                    return true;
                }
                string dropSqlStatement = GetDropDatabaseStatement(dbName);
                if (_dbEngine == DbEngine.SqlServer)
                    connectionString = ConnectionStringHelper.GetSqlServerMasterConnectionString(connectionString);
                

                return ExecuteStatement(connectionString, dropSqlStatement);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured during database drop, exception {e}");
                return false;
            }
        }

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
            using (DbConnection connection = DbConnectionFactory.Create(_dbEngine, connectionString))
            {
                IDbCommand command = DbCommandFactory.Create(_dbEngine, connection, cmdText);
                await connection.OpenAsync();
                bool result = await ExecuteNonQueryAsync(command as DbCommand);
                connection.Close();
                return result;
            }
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
                _logger.LogError($"An error occurred during Db Reader Execution: {e}");
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
                _logger.LogError($"An error occurred during Async Db Reader Execution: {e}");
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
            bool result = true;
            using (IDbConnection connection = DbConnectionFactory.Create(_dbEngine, connectionString))
            {
                connection.Open();
                IDbCommand command = DbCommandFactory.Create(_dbEngine, connection, statement);
                result = ExecuteNonQuery(command);
                connection.Close();
                return result;
            }
        }

        private string GetDropDatabaseStatement(string dbName)
        {
            if (_dbEngine == DbEngine.SqlServer)
                return string.Format(SqlServerDropDatabaseStatementTemplate, dbName);
            if (_dbEngine == DbEngine.SqLite)
                return string.Format(SqLiteDropDatabaseStatementTemplate, dbName);
            if (_dbEngine == DbEngine.MySql)
                return string.Format(MySqlDropDatabaseStatementTemplate, dbName);
            throw new NotImplementedException("Other db engine were not implemented yet");
        }


        // create database statements
        private const string CommonServerCreateDatabaseStatementTemplate = "CREATE DATABASE {0};";
        //private const string MySqlCreateDatabaseStatementTemplate = "CREATE DATABASE {0} IF EXISTS;";
        // drop database statements
        private const string SqlServerDropDatabaseStatementTemplate = "ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{0}];";
        private const string MySqlDropDatabaseStatementTemplate = "DROP DATABASE {0} IF EXISTS;";
        private const string SqLiteDropDatabaseStatementTemplate = "DETACH DATABASE {0};";

        private readonly DbEngine _dbEngine;
        private readonly ILogger<CommonDbManager> _logger;
    }
}