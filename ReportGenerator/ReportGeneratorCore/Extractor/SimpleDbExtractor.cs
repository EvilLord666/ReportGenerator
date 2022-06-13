using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using DbTools.Core;
using DbTools.Core.Managers;
using DbTools.Simple.Factories;
using DbTools.Simple.Utils;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Data.Parameters;
using ReportGenerator.Core.StatementsGenerator;

namespace ReportGenerator.Core.Extractor
{
    public class SimpleDbExtractor : IDbExtractor, IDisposable
    {
        public SimpleDbExtractor(ILoggerFactory loggerFactory, DbEngine dbEngine, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(connectionString);
            }

            Init(loggerFactory, dbEngine, connectionString);
        }

        public SimpleDbExtractor(ILoggerFactory loggerFactory, DbEngine dbEngine, string host, string database, 
                                 bool trustedConnection = true, string username = null, string password = null)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(host))
                parameters[DbParametersKeys.HostKey] = host;
            if (!string.IsNullOrEmpty(database))
                parameters[DbParametersKeys.DatabaseKey] = database;
            parameters[DbParametersKeys.UseTrustedConnectionKey] = trustedConnection.ToString();

            if (username == null && password == null)
                parameters[DbParametersKeys.UseIntegratedSecurityKey] = true.ToString();
            else
            {
                parameters[DbParametersKeys.LoginKey] = username;
                parameters[DbParametersKeys.PasswordKey] = password;
                parameters[DbParametersKeys.UseIntegratedSecurityKey] = false.ToString();
            }
            string connectionString = ConnectionStringBuilder.Build(dbEngine, parameters);

            Init(loggerFactory, dbEngine, connectionString);
        }

        public void Dispose()
        {
        }

        public async Task<DbData> ExtractAsync(string storedProcedureName, IList<StoredProcedureParameter> parameters)
        {
            using (DbConnection connection = DbConnectionFactory.Create(_dbEngine, _connectionString))
            {
                try
                {
                    _logger.LogDebug("Extract db data async via \"Stored procedure\" started");
                    DbData result = null;
                    await connection.OpenAsync();
                    using (IDbCommand command = DbCommandFactory.Create(_dbEngine, connection, storedProcedureName))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // add parameters
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (StoredProcedureParameter parameter in parameters)
                            {
                                if (string.IsNullOrEmpty(parameter.ParameterName))
                                    throw new InvalidDataException("parameter name can't be null or empty");
                                DbParameter procedureParameter = DbParameterFactory.Create(_dbEngine, parameter.ParameterName, parameter.ParameterType, 
                                                                                           parameter.ParameterValue);
                                command.Parameters.Add(procedureParameter);
                            }
                        }

                        result = await ReadDataImplAsync((DbCommand)command);
                    }
                    _logger.LogDebug("Extract db data async via \"Stored procedure\" completed");
                    return result;
                }
                catch (Exception e)
                {
                    _logger.LogError($"An error occurred during async data extraction via \"Stored procedure\", exception: {e}");
                    return null;
                }
            }
        }

        public async Task<DbData> ExtractAsync(string viewName, ViewParameters parameters)
        {
            using (DbConnection connection = DbConnectionFactory.Create(_dbEngine, _connectionString))
            {
                try
                {
                    _logger.LogDebug("Extract db data async via \"View\" started");
                    DbData result = null;
                    await connection.OpenAsync();
                    string cmdText = SqlStatmentsGenerator.CreateSelectStatement(SqlStatmentsGenerator.SelectAllColumns, viewName, parameters);
                    using (IDbCommand command = DbCommandFactory.Create(_dbEngine, connection, cmdText))
                    {
                        command.CommandType = CommandType.Text;
                        result = await ReadDataImplAsync((DbCommand)command);
                    }

                    _logger.LogDebug("Extract db data async via \"View\" completed");
                    return result;
                }
                catch (Exception e)
                {
                    _logger.LogError($"An error occurred during async data extraction via \"View\", exception: {e}");
                    return null;
                }
            }
        }

        private void Init(ILoggerFactory loggerFactory, DbEngine dbEngine, string connectionString)
        {
            _logger = loggerFactory.CreateLogger<SimpleDbExtractor>();
            _connectionString = connectionString;
            _dbEngine = dbEngine;
            _dbManager = DbManagerFactory.Create(dbEngine, loggerFactory);
        }

        private async Task<DbData> ReadDataImplAsync(DbCommand command)
        {
            try
            {
                DbData result = new DbData();
                using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
                {
                    while (await reader.ReadAsync())
                    {
                        IList<DbValue> dbRow = new List<DbValue>();
                        for (int columnNumber = 0; columnNumber < reader.FieldCount; columnNumber++)
                        {
                            // reader.GetFieldValueAsync<>()
                            object value = reader.GetValue(columnNumber);
                            string column = reader.GetName(columnNumber);
                            dbRow.Add(new DbValue(column, value));
                        }
                        result.Rows.Add(dbRow);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred during read data impl async, exception: {e}");
                return null;
            }
        }

        private string _connectionString;
        private ILogger<SimpleDbExtractor> _logger;
        private DbEngine _dbEngine;
        private IDbManager _dbManager;
    }
}
