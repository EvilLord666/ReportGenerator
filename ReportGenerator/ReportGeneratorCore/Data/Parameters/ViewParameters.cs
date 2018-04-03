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

        public ViewParameters(IList<DbQueryParameter> whereParameters, IList<DbQueryParameter> orderByParameters,
                              IList<DbQueryParameter> groupByParameters)
        {
            WhereParameters = whereParameters;
            OrderByParameters = orderByParameters;
            GroupByParameters = groupByParameters;
        }

        public IList<DbQueryParameter> WhereParameters { get; set; }
        public IList<DbQueryParameter> OrderByParameters { get; set; }
        public IList<DbQueryParameter> GroupByParameters { get; set; }
    }
}
