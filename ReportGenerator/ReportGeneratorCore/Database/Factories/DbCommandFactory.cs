using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

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
            throw new NotImplementedException("Other db engine were not implemented yet");
        }
    }
}