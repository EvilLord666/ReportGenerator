using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace ReportGenerator.Core.Database.Utils
{
    public static class ConnectionStringBuilder
    {
        public static string Build(DbEngine dbEngine, IDictionary<string, string> parameters)
        {
            if (dbEngine == DbEngine.SqlServer)
                return BuildSqlServerConnectionString(parameters);
            if (dbEngine == DbEngine.SqLite)
                return BuildSqLiteConnectionString(parameters);
            throw new NotImplementedException("Other db engine were not implemented yet");

        }
        
        private static string BuildSqlServerConnectionString(IDictionary<string, string> parameters)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            if (parameters.ContainsKey(DbParametersKeys.HostKey))
                builder.DataSource = parameters[DbParametersKeys.HostKey];
            if (parameters.ContainsKey(DbParametersKeys.DatabaseKey))
                builder.InitialCatalog = parameters[DbParametersKeys.DatabaseKey];
            if (parameters.ContainsKey(DbParametersKeys.LoginKey))
                builder.UserID = parameters[DbParametersKeys.LoginKey];
            if (parameters.ContainsKey(DbParametersKeys.PasswordKey))
                builder.Password = parameters[DbParametersKeys.PasswordKey];
            if (parameters.ContainsKey(DbParametersKeys.UseIntegratedSecurityKey))
                builder.IntegratedSecurity = Convert.ToBoolean(parameters[DbParametersKeys.UseIntegratedSecurityKey]);
            if (parameters.ContainsKey(DbParametersKeys.UseTrustedConnectionKey))
                builder.TrustServerCertificate = Convert.ToBoolean(parameters[DbParametersKeys.UseTrustedConnectionKey]);
            return builder.ConnectionString;
        }

        private static string BuildSqLiteConnectionString(IDictionary<string, string> parameters)
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = parameters[DbParametersKeys.DatabaseKey];
            builder.Version = Convert.ToInt32(parameters[DbParametersKeys.DatabaseEngineVersion]);
            return builder.ConnectionString;
        }
    }
}