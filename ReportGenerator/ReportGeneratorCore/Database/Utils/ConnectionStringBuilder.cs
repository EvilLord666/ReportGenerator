using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ReportGenerator.Core.Database.Utils
{
    public static class ConnectionStringBuilder
    {
        public static string Build(DbEngine dbEngine, IDictionary<string, string> parameters)
        {
            if (dbEngine == DbEngine.SqlServer)
                return BuildSqlServerConnectionString(parameters);
            throw new NotImplementedException("Unsupported Db Engine type, please write an issue () iw you would like me to support other Db engine");

        }

        private static string BuildSqlServerConnectionString(IDictionary<string, string> parameters)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            if (parameters.ContainsKey(DbParametersDefs.HostKey))
                builder.DataSource = parameters[DbParametersDefs.HostKey];
            if (parameters.ContainsKey(DbParametersDefs.DatabaseKey))
                builder.InitialCatalog = parameters[DbParametersDefs.DatabaseKey];
            if (parameters.ContainsKey(DbParametersDefs.LoginKey))
                builder.UserID = parameters[DbParametersDefs.LoginKey];
            if (parameters.ContainsKey(DbParametersDefs.PasswordKey))
                builder.Password = parameters[DbParametersDefs.PasswordKey];
            if (parameters.ContainsKey(DbParametersDefs.UseIntegratedSecurityKey))
                builder.IntegratedSecurity = Convert.ToBoolean(parameters[DbParametersDefs.UseIntegratedSecurityKey]);
            if (parameters.ContainsKey(DbParametersDefs.UseTrustedConnectionKey))
                builder.TrustServerCertificate = Convert.ToBoolean(parameters[DbParametersDefs.UseTrustedConnectionKey]);
            return builder.ConnectionString;
        }
    }
}