using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using MySql.Data.MySqlClient;

namespace ReportGenerator.Core.Database.Factories
{
    public static class DbCommandFactory
    {
        public static IDbCommand Create(DbEngine dbEngine, IDbConnection connection, string cmdText)
        {
            if (dbEngine == DbEngine.SqlServer)
                return new SqlCommand(cmdText, connection as SqlConnection);
            if (dbEngine == DbEngine.SqLite)
                return new SQLiteCommand(cmdText, connection as SQLiteConnection);
            if (dbEngine == DbEngine.MySql)
                return new MySqlCommand(cmdText, connection as MySqlConnection);
            throw new NotImplementedException("Other db engine are not supported yet, please add a github issue https://github.com/EvilLord666/ReportGenerator");
        }
    }
}