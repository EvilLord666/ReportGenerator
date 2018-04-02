using System;
using System.Collections.Generic;
using System.Text;

namespace ReportGenerator.Core.Data.Parameters
{
    public class ViewParameters
    {
        public enum SqlParameterType
        {
            WhereParameter,
            OrderByParameter,
            GroupByParameter
        }

        public ViewParameters()
        {
        }

        public ViewParameters(IList<SqlParameter> whereParameters, IList<SqlParameter> orderByParameters,
                              IList<SqlParameter> groupByParameters)
        {
        }

        // public IList<SqlParameter> WhereParameters

        
    }
}
