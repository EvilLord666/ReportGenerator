using System.Data;

namespace ReportGenerator.Core.Database.Factories
{
    public static class DbCommandFactory
    {
        public static IDbCommand Create(DbEngine dbEngine, IDbConnection connection, string cmdText)
        {
            return null;
        }
    }
}