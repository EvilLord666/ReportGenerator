using System.Collections.Generic;

namespace ReportGenerator.Core.Data.Parameters
{
    public class ViewParameters
    {

        public ViewParameters()
        {
            WhereParameters = new List<DbQueryParameter>();
            OrderByParameters = new List<DbQueryParameter>();
            GroupByParameters = new List<DbQueryParameter>();
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
