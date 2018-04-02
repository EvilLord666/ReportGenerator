using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ReportGenerator.Core.Data.Parameters
{
    public class StoredProcedureParameter
    {
        public StoredProcedureParameter(SqlDbType parameterType, string parameterName, object parameterValue)
        {
            ParameterType = parameterType;
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        public SqlDbType ParameterType { get; private set; }
        public string ParameterName { get; private set; }
        public object ParameterValue { get; private set; }
    }
}
