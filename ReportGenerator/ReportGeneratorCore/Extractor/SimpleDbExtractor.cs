using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Data.Parameters;
using ReportGenerator.Core.StatementsGenerator;

namespace ReportGenerator.Core.Extractor
{
    //todo: umv: add logging
    public class SimpleDbExtractor : IDbExtractor
    {
        public SimpleDbExtractor(ILogger<SimpleDbExtractor> logger, string connectionString)
        {
            _logger = logger;
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError("Connection string is Null.");
                throw new ArgumentNullException(connectionString);
            }

            _connectionString = connectionString;
        }

        public SimpleDbExtractor(ILogger<SimpleDbExtractor> logger, string host, string database, 
                                 bool trustedConnection = true, string username = null, string password = null)
        {
            _logger = logger;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = host;
            builder.InitialCatalog = database;
            builder.TrustServerCertificate = trustedConnection;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                builder.UserID = username;
                builder.Password = password;
            }
            _connectionString = builder.ConnectionString;
        }

        public async Task<DbData> ExtractAsync(string storedPocedureName, IList<StoredProcedureParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    _logger.LogDebug("Extract db data async via \"Stored procedure\" started");
                    await connection.OpenAsync().ConfigureAwait(false); ;
                    SqlCommand command = new SqlCommand(storedPocedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    // add parameters
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (StoredProcedureParameter parameter in parameters)
                        {
                            if(string.IsNullOrEmpty(parameter.ParameterName))
                                throw new InvalidDataException("parameter name can't be null or empty");
                            SqlParameter procedureParameter = new SqlParameter(parameter.ParameterName, parameter.ParameterType);
                            procedureParameter.Value = parameter.ParameterValue;
                            command.Parameters.Add(procedureParameter);
                        }
                    }
                    DbData result = await ReadDataImplAsync(command);                                    
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    _logger.LogDebug("Extract db data async via \"View\" started");
                    await connection.OpenAsync().ConfigureAwait(false); ;
                    string cmdText = SqlStatmentsGenerator.CreateSelectStatement(SqlStatmentsGenerator.SelectAllColumns, viewName, parameters);
                    SqlCommand command = new SqlCommand(cmdText, connection);
                    command.CommandType = CommandType.Text;
                    DbData result = await ReadDataImplAsync(command);

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

        private async Task<DbData> ReadDataImplAsync(SqlCommand command)
        {
            try
            {
                DbData result = new DbData();
                SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
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
    }
}
