using System;
using System.Collections.Generic;
using System.Text;

namespace ReportGenerator.Core.Data.Parameters
{
    public enum SqlParameterType
    {
        WhereParameter,
        OrderByParameter,
        GroupByParameter
    }

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

        public SqlParameter(SqlParameterType parameterType, IList<JoinCondition> conditions, string parameterName, string parameterValue)
        {
            ParameterType = parameterType;
            Conditions = conditions;
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        public SqlParameterType ParameterType { get; private set; }
        public IList<JoinCondition> Conditions { get; private set; }
        public string ParameterName { get; private set; }
        public string ParameterValue { get; private set; }
    }
}
