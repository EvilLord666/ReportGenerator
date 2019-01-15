using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using Npgsql;

namespace ReportGenerator.Core.Database.Factories
{
    public static class DbConnectionFactory
    {
        public static DbConnection Create(DbEngine dbEngine, string connectionString)
        {
            if (dbEngine == DbEngine.SqlServer)
                return new SqlConnection(connectionString);
            if (dbEngine == DbEngine.SqLite)
                return new SQLiteConnection(connectionString);
            if (dbEngine == DbEngine.MySql)
                return new MySqlConnection(connectionString);
            if (dbEngine == DbEngine.PostgresSql)
                return new NpgsqlConnection(connectionString);
            throw new NotImplementedException("Other db engine are not supported yet, please add a github issue https://github.com/EvilLord666/ReportGenerator");
        }
    }
}