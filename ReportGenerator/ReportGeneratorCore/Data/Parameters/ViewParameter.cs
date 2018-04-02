using System;
using System.Collections.Generic;
using System.Text;

namespace ReportGenerator.Core.Data.Parameters
{
    public class ViewParameter
    {
        public ViewParameter()
        {
        }

        public ViewParameter(string parameterName, string parameterValue)
        {
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        public string ParameterName { get; private set; }
        public string ParameterValue { get; private set; }
    }
}
