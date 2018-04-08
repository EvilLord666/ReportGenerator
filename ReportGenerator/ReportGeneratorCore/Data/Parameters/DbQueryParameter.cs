using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportGenerator.Core.Data.Parameters
{

    /// <summary>
    /// Condition for parameters Join 
    ///     Or Name = Value
    ///     And Name = Value
    ///     Not Name = Value, if this parameter is not first it shoul follow the Or or And
    ///     Name In Value, Value is: (v1, v2, ....v3)
    ///     Name Between Value, Value is: v1 and v2
    /// </summary>
    public enum JoinCondition
    {
        Or,                  // Or Name = Value
        And,                 // And Name = Value
        Not,                 // Not Name = Value, if this parameter is not first it shoul follow the Or or And
        In,                  // Name In Value, Value is: (v1, v2, ....v3)
        Between              // Name Between Value, Value is: v1 and v2
    }

    public class DbQueryParameter
    {
        public DbQueryParameter()
        {
        }

        /// <summary>
        /// Sql parameter constructor
        /// </summary>
        /// <param name="conditions">
        ///     List of conditions, would be applied in order of list item. Could be cases like 'Not In', or join as 'Or Not' e.t.c
        ///     if parameter is first in WHERE statement or in ORDER BY Name ASC, Name2 DESC
        /// </param>
        /// <param name="parameterName"> 
        ///     Name of column 
        /// </param>
        /// <param name="comparisonOperator">
        ///     >, >=, !=, =, IS, e.t.c
        /// </param>
        /// <param name="parameterValue">
        ///    a) for where parameter is actual value but for IN should be string 'v1, v2, v3' for BETWEEN - string 'v1 AND v2'
        ///    b) for order by parameters ASC or DESC
        /// </param>
        public DbQueryParameter(IList<JoinCondition> conditions, string parameterName, string comparisonOperator, string parameterValue)
        {
            Conditions = conditions?.ToList();
            ParameterName = parameterName;
            ComparisonOperator = comparisonOperator;
            ParameterValue = parameterValue;
        }

        public List<JoinCondition> Conditions { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string ComparisonOperator { get; set; }
    }
}
