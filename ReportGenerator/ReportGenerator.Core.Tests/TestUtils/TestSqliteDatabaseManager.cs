using System;
using System.Data.SQLite;
using System.IO;

namespace ReportGenerator.Core.Tests.TestUtils
{
    public static class TestSqLiteDatabaseManager
    {
        public static void CreateDatabase(string database, bool drop = false)
        {
            SQLiteConnection.CreateFile(database);
        }

        public static void DropDatabase(string database)
        {
            try
            {
                SQLiteConnection.ClearAllPools();
            }
            catch (Exception e)
            {
            }

            if (File.Exists(database))
                File.Delete(database);
        }

        public static void ExecuteSql(string database, string sql)
        {
            using (SQLiteConnection connection = new SQLiteConnection(string.Format(SqLiteConnectionStringTemplate, 
                                                                                    database, 3)))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        
        private const string SqLiteConnectionStringTemplate = "Data Source={0};Version={1}";
    }
}