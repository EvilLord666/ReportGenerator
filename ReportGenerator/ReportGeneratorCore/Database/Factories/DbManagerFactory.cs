using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using ReportGenerator.Core.Database.Managers;

namespace ReportGenerator.Core.Database.Factories
{
    // todo: impl 4 other DB engine types
    public static class DbManagerFactory
    {
        public static IDbManager Create(DbEngine dbEngine)
        {
            if (dbEngine == DbEngine.SqlServer)
                return new SqlServerDbManager();
            return null;
        }
    }
}