using System.Collections.Generic;
using System.Linq;

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
            WhereParameters = whereParameters.ToList();
            OrderByParameters = orderByParameters.ToList();
            GroupByParameters = groupByParameters.ToList();
        }

        public List<DbQueryParameter> WhereParameters { get; set; }
        public List<DbQueryParameter> OrderByParameters { get; set; }
        public List<DbQueryParameter> GroupByParameters { get; set; }
    }
}
