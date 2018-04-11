using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Data.Parameters;
using ReportGenerator.Core.StatementsGenerator;

namespace ReportGenerator.Core.Extractor
{
    //todo: umv: add logging
    public class SimpleDbExtractor : IDbExtractor
    {
        public SimpleDbExtractor(string connectionString)
        {
            if(string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(connectionString);
            _connectionString = connectionString;
        }

        public SimpleDbExtractor(string host, string database, bool trustedConnection = true, string username = null, string password = null)
        {
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
                    await connection.OpenAsync();
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
                    DbData result = await ReadDataImpl(command);                                    
                    connection.Close();
                    return result;
                }
                catch (Exception e)
                {
                    // todo: add log
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
                    await connection.OpenAsync();
                    string cmdText = SqlStatmentsGenerator.CreateSelectStatement(SqlStatmentsGenerator.SelectAllColumns, viewName, parameters);
                    SqlCommand command = new SqlCommand(cmdText, connection);
                    command.CommandType = CommandType.Text;
                    DbData result = await ReadDataImpl(command);

                    connection.Close();
                    return result;
                }
                catch (Exception e)
                {
                    // todo: add log
                    return null;
                }
            }
        }

        private async Task<DbData> ReadDataImpl(SqlCommand command)
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
                return null;
            }
        }

        private readonly string _connectionString;
    }
}
