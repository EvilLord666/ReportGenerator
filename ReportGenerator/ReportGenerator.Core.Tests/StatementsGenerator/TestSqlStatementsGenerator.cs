using System;
using System.Collections.Generic;
using System.Text;
using ReportGenerator.Core.Data.Parameters;
using ReportGenerator.Core.StatementsGenerator;
using Xunit;

namespace ReportGenerator.Core.Tests.StatementsGenerator
{
    public class TestSqlStatementsGenerator
    {
        [Theory]
        [InlineData(true, "Citizen", "SELECT * FROM Citizen WHERE City  IN (Yekaterinburg, Moscow, KazanZ)  OR Age > 18  AND District BETWEEN(Northern AND Southerly)")]
        public void TestCreateSelectStatementWithWhereParametersOnly(bool wherePresent, string tableName, string expectedSelectStatement)
        {
            ViewParameters viewParams = GetTestParameters(wherePresent);
            string actualSelectStatement = SqlStatmentsGenerator.CreateSelectStatement(null, tableName, viewParams);
            Assert.Equal(expectedSelectStatement, actualSelectStatement);
        }

        private ViewParameters GetTestParameters(bool useWhereParameters)
        {
            ViewParameters parameters = new ViewParameters();
            if (useWhereParameters)
            {
                IList<DbQueryParameter> whereParams = new List<DbQueryParameter>();
                whereParams.Add(new DbQueryParameter(new[] { JoinCondition.In }, "City", string.Empty, "Yekaterinburg, Moscow, Kazan"));
                whereParams.Add(new DbQueryParameter(new[] { JoinCondition.Or }, "Age", ">", "18"));
                whereParams.Add(new DbQueryParameter(new[] { JoinCondition.And, JoinCondition.Between }, "District", null, "Northern AND Southerly"));
            }
        }
    }
}
