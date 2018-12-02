using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Data.Parameters;
using ReportGenerator.Core.Database;
using ReportGenerator.Core.Database.Factories;
using ReportGenerator.Core.Database.Utils;
using ReportGenerator.Core.StatementsGenerator;

namespace ReportGenerator.Core.Extractor
{
    //todo: umv: add logging
    public class SimpleDbExtractor : IDbExtractor
    {
        public SimpleDbExtractor(ILogger<SimpleDbExtractor> logger, DbEngine dbEngine, string connectionString)
        {
            _logger = logger;
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError("Connection string is Null.");
                throw new ArgumentNullException(connectionString);
            }

            _connectionString = connectionString;
            _dbEngine = dbEngine;
        }

        public SimpleDbExtractor(ILogger<SimpleDbExtractor> logger, DbEngine dbEngine, string host, string database, 
                                 bool trustedConnection = true, string username = null, string password = null)
        {
            _logger = logger;
            _dbEngine = dbEngine;
            _dbManager = DbManagerFactory.Create(dbEngine);
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(host))
                parameters[DbParametersDefs.HostKey] = host;
            if (!string.IsNullOrEmpty(database))
                parameters[DbParametersDefs.DatabaseKey] = database;
            parameters[DbParametersDefs.UseTrustedConnectionKey] = trustedConnection.ToString();

            if (username == null && password == null)
                parameters[DbParametersDefs.UseIntegratedSecurityKey] = true.ToString();
            else
            {
                parameters[DbParametersDefs.LoginKey] = username;
                parameters[DbParametersDefs.PasswordKey] = password;
                parameters[DbParametersDefs.UseIntegratedSecurityKey] = false.ToString();
            }

            _connectionString = ConnectionStringBuilder.Build(dbEngine, parameters);
        }

        public async Task<DbData> ExtractAsync(string storedPocedureName, IList<StoredProcedureParameter> parameters)
        {
            using (DbConnection connection = DbManagerFactory.Create(_connectionString, databaseEngine: _dbEngine))
            {
                try
                {
                    _logger.LogDebug("Extract db data async via \"Stored procedure\" started");
                    DbData result = null;
                    await connection.OpenAsync().ConfigureAwait(false); ;
                    using (IDbCommand command = DbManagerFactory.Create(connection, storedPocedureName, _dbEngine))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // add parameters
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (StoredProcedureParameter parameter in parameters)
                            {
                                if (string.IsNullOrEmpty(parameter.ParameterName))
                                    throw new InvalidDataException("parameter name can't be null or empty");
                                SqlParameter procedureParameter = new SqlParameter(parameter.ParameterName, parameter.ParameterType);
                                procedureParameter.Value = parameter.ParameterValue;
                                command.Parameters.Add(procedureParameter);
                            }
                        }

                        result = await ReadDataImplAsync((DbCommand)command);
                    }

                    connection.Close();
                    _logger.LogDebug("Extract db data async via \"Stored procedure\" completed");
                    return result;
                }
                catch (Exception e)
                {
                    _logger.LogError($"An error occured during async data extraction via \"Stored procedure\", exception: {e}");
                    return null;
                }
            }
        }

        public async Task<DbData> ExtractAsync(string viewName, ViewParameters parameters)
        {
            using (DbConnection connection = DbManagerFactory.Create(_connectionString, _dbEngine))
            {
                try
                {
                    _logger.LogDebug("Extract db data async via \"View\" started");
                    DbData result = null;
                    await connection.OpenAsync().ConfigureAwait(false); ;
                    string cmdText = SqlStatmentsGenerator.CreateSelectStatement(SqlStatmentsGenerator.SelectAllColumns, viewName, parameters);
                    using (IDbCommand command = DbManagerFactory.Create(connection, cmdText, _dbEngine))
                    {
                        command.CommandType = CommandType.Text;
                        result = await ReadDataImplAsync((DbCommand)command);
                    }

                    connection.Close();
                    _logger.LogDebug("Extract db data async via \"View\" completed");
                    return result;
                }
                catch (Exception e)
                {
                    _logger.LogError($"An error occured during async data extraction via \"View\", exception: {e}");
                    return null;
                }
            }
        }

        private async Task<DbData> ReadDataImplAsync(DbCommand command)
        {
            try
            {
                DbData result = new DbData();
                DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
                //SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    IList<DbValue> dbRow = new List<DbValue>();
                    for (int columnNumber = 0; columnNumber < reader.FieldCount; columnNumber++)
                    {
                        object value = reader.GetValue(columnNumber);
                        string column = reader.GetName(columnNumber);
                        dbRow.Add(new DbValue(column, value));
                    }
                    result.Rows.Add(dbRow);
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured during read data impl async, exception: {e}");
                return null;
            }
        }

        private readonly string _connectionString;
        private readonly ILogger<SimpleDbExtractor> _logger;
        private readonly DbEngine _dbEngine;
        private readonly IDbManager _dbManager;
    }
}
