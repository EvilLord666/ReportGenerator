# ReportGenerator
# 1 Overview
A small tool 4 generating excel reports based (MS SQL, other db were not tested) on data from db and excel template
There are two ways for getting data:

from stored procedure

from view

In both 2 ways there are a possibility to manipulate data selection by setting parameters (for stored procedure) and automatic sql generation for filtering View data.
It should be noted that there is a configuration of View and StoredProcedure in xml.

# 1.1 Configuration
For Stored Procedure Configuration we must set <DataSource>StoredProcedure</DataSource>

Name to stored procedure name

And also if we have aparameters we should specify collection of StoredProcedureParameters like :
`

    <StoredProcedureParameters>
        <ParameterType>NVarChar</ParameterType>
        <ParameterName>City</ParameterName>
        <ParameterValue xsi:type="xsd:string">N'Yekaterinburg</ParameterValue>
    </StoredProcedureParameters>
    <StoredProcedureParameters>
        <ParameterType>Int</ParameterType>
        <ParameterName>PostalCode</ParameterName>
        <ParameterValue xsi:type="xsd:string">620000</ParameterValue>
    </StoredProcedureParameters>
    <StoredProcedureParameters>
        <ParameterType>DateTime</ParameterType>
        <ParameterName>DateOfBirth</ParameterName>
        <ParameterValue xsi:type="xsd:string">'2018-01-01'</ParameterValue>
    </StoredProcedureParameters>
  `
ParameterType is a enumeration see SqlDbType (https://msdn.microsoft.com/ru-ru/library/system.data.sqldbtype(v=vs.110).aspx)

For View Configuration is a little similar as DataSource we must note View:

<DataSource>View</DataSource>

Name is a name of View

But here we should also specify parameters for rows selection (WHERE, ORDER BY and GROUP BY parameters). All of them are represented with only one type - DbQueryParameter.

DbQueryParameter consist of following:

-collection of JoinCondition (works only for WHERE parameters) - is way of how parameters joined in WHERE statement by logical AND or logical OR, or maybe AND/OR with inversion - AND NOT Condition ... OR NOT Condition (see example below). Should be ommited for first parameter in WHERE statement.

-parameter Name make sense for every type of parameter (WHERE, ORDER BY and GROUP BY)

-comparison operator (make sense only for WHERE parameters) - is any valid SQL condition operator like >, <, =, IS, IS NOT, IN, BETWEEN, LIKE and so on...

-parameter value works for WHERE (value for comparison) and ORDER BY (ASC or DESC)

Example:
`

    <?xml version="1.0"?>
        <ExecutionConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
        <DataSource>View</DataSource>
        <Name>CitizensView</Name>
        <ViewParameters>
            <WhereParameters>
                <DbQueryParameter>
                    <ParameterName>FirstName</ParameterName>
                    <ParameterValue>N'Michael'</ParameterValue>
                    <ComparisonOperator>=</ComparisonOperator>
                </DbQueryParameter>
                <DbQueryParameter>
                    <Conditions>
                        <JoinCondition>And</JoinCondition>
                        <JoinCondition>Not</JoinCondition>
                    </Conditions>
                    <ParameterName>City</ParameterName>
                    <ParameterValue>N'Yekaterinburg'</ParameterValue>
                    <ComparisonOperator>=</ComparisonOperator>
                </DbQueryParameter>
                <DbQueryParameter>
                    <Conditions>
                       <JoinCondition>Between</JoinCondition>
                    </Conditions>
                    <ParameterName>Age</ParameterName>
                    <ParameterValue>18 AND 60</ParameterValue>
                </DbQueryParameter>
                <DbQueryParameter>
                    <Conditions>
                        <JoinCondition>In</JoinCondition>
                    </Conditions>
                    <ParameterName>District</ParameterName>
                    <ParameterValue>(N'D1', N'A3', N'A5', N'C7')</ParameterValue>
                </DbQueryParameter>
                <DbQueryParameter>
                    <Conditions>
                        <JoinCondition>Or</JoinCondition>
                    </Conditions>
                    <ParameterName>Region</ParameterName>
                    <ParameterValue>N'Sverdlovskaya oblast'</ParameterValue>
                    <ComparisonOperator>!=</ComparisonOperator>
                </DbQueryParameter>
            </WhereParameters>
            <OrderByParameters>
                <DbQueryParameter>
                    <ParameterName>FirstName</ParameterName>
                    <ParameterValue>ASC</ParameterValue>
                </DbQueryParameter>
                <DbQueryParameter>
                    <ParameterName>LastName</ParameterName>
                    <ParameterValue>DESC</ParameterValue>
                </DbQueryParameter>
            </OrderByParameters>
            <GroupByParameters>
                <DbQueryParameter>
                    <ParameterName>District</ParameterName>
                </DbQueryParameter>
            </GroupByParameters>
        </ViewParameters>
    </ExecutionConfig>
`

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
                Task<bool> result = manager.GenerateAsync(TestExcelTemplate, DataExecutionConfig, ReportFile, parameters);
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
  !!!! THERE IS ONE MORE option : to pass ExecutionConfig like a valiable instead of path to file (see IReportGeneratorManager interface)
  
  # 3 Nuget Package
  Nuget package is available on nuget.org : https://www.nuget.org/packages/ReportsGenerator/
