using System;
using System.Collections.Generic;
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

    public class SqlParameter
    {
        public SqlParameter()
        {
        }

        /// <summary>
        /// Sql parameter constructor
        /// </summary>
        /// <param name="conditions">
        ///     List of conditions, would be applied in order of list item. Could be cases like 'Not In', or join as 'Or Not' e.t.c
        ///     if parameter is first in WHERE statement or in ORDER BY Name ASC, Name2 DESC
        /// </param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        public SqlParameter(IList<JoinCondition> conditions, string parameterName, string parameterValue)
        {
            Conditions = conditions;
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        public IList<JoinCondition> Conditions { get; private set; }
        public string ParameterName { get; private set; }
        public string ParameterValue { get; private set; }
    }
}
