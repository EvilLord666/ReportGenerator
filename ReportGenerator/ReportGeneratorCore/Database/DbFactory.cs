using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace ReportGenerator.Core.Database
{
    // todo: impl 4 other DB engine types
    public static class DbFactory
    {
        /*public static DbConnection Create(string connectionString, DatabaseEngine databaseEngine = DatabaseEngine.SqlServer)
        {
            if (databaseEngine == DatabaseEngine.SqlServer)
                return new SqlConnection(connectionString);
            if(databaseEngine == DatabaseEngine.SqLite)
                return new SQLiteConnection(connectionString);          
            return null;
        }

        public static IDbCommand Create(DbConnection connection, string commandText, DatabaseEngine databaseEngine = DatabaseEngine.SqlServer)
        {
            if (databaseEngine == DatabaseEngine.SqlServer)
                return new SqlCommand(commandText, connection as SqlConnection);
            if (databaseEngine == DatabaseEngine.SqLite)
                return new SQLiteCommand(commandText, connection as SQLiteConnection);
            return null;
        }*/

        public static IDbManager Create(DbEngine dbEngine)
        {
            if (dbEngine == DbEngine.SqlServer)
                return new SqlServerDbManager();
            return null;
        }
    }
}