namespace ReportGenerator.Core.Tests
{
    public static class GlobalTestsParams
    {
        // Globals database instances options
        public const string TestSqlServerHost = @"(localdb)\mssqllocaldb";
        public const string TestSqlServerDatabasePattern = "ReportGeneratorTestDb";
        public const string TestSqLiteDatabase = "ReportGeneratorTestDb.sqlite";
        public const string TestMySqlHost = "localhost";
        public const string TestMySqlDatabase = "ReportGeneratorTestDb";
        public const string TestPostgresSqlHost = "localhost";
        public const string TestPostgresSqlDatabase = "ReportGeneratorTestDb";
        
        // Common Test Scripts (create structure and init db with data)
        public const string SqlServerCreateDatabaseScript = @"..\..\..\DbScripts\SqlServerCreateDb.sql";
        public const string SqlServerInsertDataScript = @"..\..\..\DbScripts\SqlServerCreateData.sql";
        public const string SqLiteCreateDatabaseScript = @"..\..\..\DbScripts\SqLiteCreateDb.sql";
        public const string SqLiteInsertDataScript = @"..\..\..\DbScripts\SqLiteCreateData.sql";
        public const string MySqlCreateDatabaseScript = @"..\..\..\DbScripts\MySqlCreateDb.sql";
        public const string MySqlInsertDataScript = @"..\..\..\DbScripts\MySqlCreateData.sql";
        public const string PostgresSqlCreateDatabaseScript = @"..\..\..\DbScripts\PostgresSqlCreateDb.sql";
        public const string PostgresSqlInsertDataScript = @"..\..\..\DbScripts\PostgresSqlCreateData.sql";
        
        // Config files
        public const string SqlServerViewDataExecutionConfig = @"..\..\..\ExampleConfig\sqlServerDataExtractionParams.xml";
        public const string SqLiteViewDataExecutionConfig = @"..\..\..\ExampleConfig\sqLiteDataExtractionParams.xml";
        public const string MySqlStoredProcedureDataExecutionConfig = @"..\..\..\ExampleConfig\mySql_testReport4_StoredProcedure.xml";
        public const string MySqlViewDataExecutionConfig = @"..\..\..\ExampleConfig\mySql_testReport4_StoredProcedure.xml";
        public const string PostgresSqlViewDataExecutionConfig = @"..\..\..\ExampleConfig\postgresViewDataExtractionParams.xml";
        public const string PostgresSqlStoredProcedureDataExecutionConfig = @"..\..\..\ExampleConfig\postgresStoredProcDataExtractionParams.xml";
    }
}