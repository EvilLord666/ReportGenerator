using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ReportGenerator.Core.Data.Parameters
{
    public class StoredProcedureParameter
    {
        public StoredProcedureParameter()
        {
        }

        public StoredProcedureParameter(SqlDbType parameterType, string parameterName, object parameterValue)
        {
            ParameterType = parameterType;
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        public SqlDbType ParameterType { get; set; }   // todo: make cross db 
        public string ParameterName { get; set; }
        public object ParameterValue { get; set; }
    }
}
