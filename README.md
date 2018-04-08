# ReportGenerator
# 1 Overview
A small tool 4 generating excel reports based (MS SQL, other db were not tested) on data from db and excel template
There are two ways for getting data:

from stored procedure

from view

In both 2 ways there are a possibility to manipulate data selection by setting parameters (for stored procedure) and automatic sql generation for filtering View data.
It should be noted that there is a configuration of View and StoredProcedure in xml.

# 2 Example of usage
Top interface is - IReportGeneratorManager.
For all functionality were written Unit tests with xUnit (sucks), full example could be found in unit test project (ReportGenerator.Core.Tests/ReportsGenerator/TestExcelReportGeneratorManager):

`

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using ReportGenerator.Core.Helpers;
    using ReportGenerator.Core.ReportsGenerator;
    using ReportGenerator.Core.Tests.TestUtils;
    using Xunit;

    namespace ReportGenerator.Core.Tests.ReportsGenerator
    {
        public class TestExcelReportGeneratorManager
        {
            [Fact]
            public void TestGenerateReport()
            {
                SetUpTestData();
                // executing extraction ...
                object[] parameters = ExcelReportGeneratorHelper.CreateParameters(1, 2, 3);
                IReportGeneratorManager manager = new ExcelReportGeneratorManager(Server, TestDatabase);
                Task<bool> result = manager.Generate(TestExcelTemplate, DataExecutionConfig, ReportFile, parameters);
                result.Wait();
                Assert.True(result.Result);
                TearDownTestData();
            }

            private void SetUpTestData()
            {
                TestDatabaseManager.CreateDatabase(Server, TestDatabase, true);
                string createDatabaseStatement = File.ReadAllText(Path.GetFullPath(CreateDatabaseScript));
                string insertDataStatement = File.ReadAllText(Path.GetFullPath(InsertDataScript));
                TestDatabaseManager.ExecuteSql(Server, TestDatabase, createDatabaseStatement);
                TestDatabaseManager.ExecuteSql(Server, TestDatabase, insertDataStatement);
            }

            private void TearDownTestData()
            {
                TestDatabaseManager.DropDatabase(Server, TestDatabase);
            }

            private const string TestExcelTemplate = @"..\..\..\TestExcelTemplates\CitizensTemplate.xlsx";
            private const string ReportFile = @".\Report.xlsx";
            private const string DataExecutionConfig = @"..\..\..\ExampleConfig\dataExtractionParams.xml";

            private const string Server = @"(localdb)\mssqllocaldb";
            private const string TestDatabase = "ReportGeneratorTestDb";

            private const string CreateDatabaseScript = @"..\..\..\DbScripts\CreateDb.sql";
            private const string InsertDataScript = @"..\..\..\DbScripts\CreateData.sql";
        }
    }
  `
