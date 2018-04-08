using System.Collections.Generic;
using ReportGenerator.Core.Data.Parameters;
using ReportGenerator.Core.StatementsGenerator;
using Xunit;

namespace ReportGenerator.Core.Tests.StatementsGenerator
{
    public class TestSqlStatementsGenerator
    {
        [Theory]
        [InlineData(true, false, false, "Citizen", "SELECT * FROM Citizen  WHERE City IN (Yekaterinburg, Moscow, Kazan) OR Age > 18 AND  District BETWEEN Northern AND Southerly")]
        [InlineData(true, true, false, "Citizen", "SELECT * FROM Citizen  WHERE City IN (Yekaterinburg, Moscow, Kazan) OR Age > 18 AND  District BETWEEN Northern AND Southerly ORDER BY City ASC, Name DESC")]
        [InlineData(true, true, true, "Citizen", "SELECT * FROM Citizen  WHERE City IN (Yekaterinburg, Moscow, Kazan) OR Age > 18 AND  District BETWEEN Northern AND Southerly GROUP BY Age, Name ORDER BY City ASC, Name DESC")]
        public void TestCreateSelectStatementSuccessfully(bool wherePresent, bool orderByPresent, bool groupByPresent, string tableName, string expectedSelectStatement)
        {
            ViewParameters viewParams = GetTestParameters(wherePresent, orderByPresent, groupByPresent);
            string actualSelectStatement = SqlStatmentsGenerator.CreateSelectStatement(null, tableName, viewParams);
            Assert.Equal(expectedSelectStatement, actualSelectStatement);
        }

        [Fact]
        public void TestCreateSelectStatementMultiple()
        {
            ViewParameters parameters = new ViewParameters();
            List<DbQueryParameter> whereParams = new List<DbQueryParameter>();
            whereParams.Add(new DbQueryParameter(null, "Age", ">", "18"));
            whereParams.Add(new DbQueryParameter(new[] { JoinCondition.And, JoinCondition.Not }, "FirstName", "IS NOT", "'John'"));
            whereParams.Add(new DbQueryParameter(new[] { JoinCondition.Or }, "LastName", "=", "'Smith'"));
            parameters.WhereParameters = whereParams;
            string expectedSelectStatement = "SELECT * FROM Person  WHERE Age > 18 AND  NOT FirstName IS NOT 'John' OR LastName = 'Smith'";
            string actualSelectStatement = SqlStatmentsGenerator.CreateSelectStatement(null, "Person", parameters);
            Assert.Equal(expectedSelectStatement, actualSelectStatement);
        }

        private ViewParameters GetTestParameters(bool useWhereParameters, bool useOrderByParams, bool useGroupByParams)
        {
            ViewParameters parameters = new ViewParameters();
            if (useWhereParameters)
            {
                List<DbQueryParameter> whereParams = new List<DbQueryParameter>();
                whereParams.Add(new DbQueryParameter(new[] { JoinCondition.In }, "City", string.Empty, "Yekaterinburg, Moscow, Kazan"));
                whereParams.Add(new DbQueryParameter(new[] { JoinCondition.Or }, "Age", ">", "18"));
                whereParams.Add(new DbQueryParameter(new[] { JoinCondition.And, JoinCondition.Between }, "District", null, "Northern AND Southerly"));
                parameters.WhereParameters = whereParams;
            }

            if (useOrderByParams)
            {
                List<DbQueryParameter> orderByParams = new List<DbQueryParameter>();
                orderByParams.Add(new DbQueryParameter(null, "City", null, "ASC"));
                orderByParams.Add(new DbQueryParameter(null, "Name", null, "DESC"));
                parameters.OrderByParameters = orderByParams;
            }

            if (useGroupByParams)
            {
                List<DbQueryParameter> groupByParams = new List<DbQueryParameter>();
                groupByParams.Add(new DbQueryParameter(null, "Age", null, null));
                groupByParams.Add(new DbQueryParameter(null, "Name", null, null));
                parameters.GroupByParameters = groupByParams;
            }
            return parameters;
        }
    }
}
