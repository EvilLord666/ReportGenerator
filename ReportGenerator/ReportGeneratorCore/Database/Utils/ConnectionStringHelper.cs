using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using MySql.Data.MySqlClient;

namespace ReportGenerator.Core.Database.Utils
{
    public static class ConnectionStringHelper
    {
        public static string GetDatabaseName(string connectionString, DbEngine dbEngine)
        {
            if (dbEngine == DbEngine.SqlServer)
            {
                SqlConnectionStringBuilder sqlConnStringBuilder = new SqlConnectionStringBuilder(connectionString);
                return sqlConnStringBuilder.InitialCatalog;
            }

            if (dbEngine == DbEngine.SqLite)
            {
                SQLiteConnectionStringBuilder sqLiteConnStringBuilder = new SQLiteConnectionStringBuilder(connectionString);
                return sqLiteConnStringBuilder.DataSource;
            }

            if (dbEngine == DbEngine.MySql)
            {
                MySqlConnectionStringBuilder mySqlConnStringBuilder = new MySqlConnectionStringBuilder(connectionString);
                return mySqlConnStringBuilder.Database;
            }

            throw new NotImplementedException("Other db engine were not implemented yet");
        }

        public static string GetSqlServerMasterConnectionString(string connectionString)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = "master";
            return builder.ConnectionString;
        }

        public static string GetMySqlDbNameLessConnectionString(string connectionString)
        {
            return null;
        }

        private const string SqlServerMasterDatabase = "master";
    }
}