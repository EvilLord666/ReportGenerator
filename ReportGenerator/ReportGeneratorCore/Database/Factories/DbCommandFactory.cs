using System;
using System.Data;
using System.Data.SqlClient;

namespace ReportGenerator.Core.Database.Factories
{
    public static class DbCommandFactory
    {
        public static IDbCommand Create(DbEngine dbEngine, IDbConnection connection, string cmdText)
        {
            if (dbEngine == DbEngine.MySql)
                return new SqlCommand(cmdText, connection as SqlConnection);
            throw new NotImplementedException("other db engines are not ready yet!");
            return null;
        }
    }
}