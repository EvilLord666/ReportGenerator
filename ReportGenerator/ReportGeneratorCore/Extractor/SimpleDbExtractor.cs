using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Data.Parameters;

namespace ReportGenerator.Core.Extractor
{
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

        public DbData Extract(string storedPocedureName, IList<StoredProcedureParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                DbData result = new DbData();
                return result;
            }
        }

        public DbData Extract(string viewName, ViewParameters parameters)
        {
            throw new NotImplementedException();
        }

        private readonly string _connectionString;
    }
}
