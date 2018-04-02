using System;
using System.Collections.Generic;
using System.Text;

namespace ReportGenerator.Core.Data.Parameters
{
    public class ViewParameters
    {

        public ViewParameters()
        {
        }

        public ViewParameters(IList<SqlParameter> whereParameters, IList<SqlParameter> orderByParameters,
                              IList<SqlParameter> groupByParameters)
        {
            WhereParameters = whereParameters;
            OrderByParameters = orderByParameters;
            GroupByParameters = groupByParameters;
        }

        public IList<SqlParameter> WhereParameters { get; set; }
        public IList<SqlParameter> OrderByParameters { get; set; }
        public IList<SqlParameter> GroupByParameters { get; set; }
    }
}
