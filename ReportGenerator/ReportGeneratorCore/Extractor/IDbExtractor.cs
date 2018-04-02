using System;
using System.Collections.Generic;
using System.Text;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Data.Parameters;

namespace ReportGenerator.Core.Extractor
{
    public interface IDbExtractor
    {
        DbData Extract(string storedPocedureName, IList<StoredProcedureParameter> parameters);

        DbData Extract(string viewName, IList<SqlParameter> whereParameters, IList<SqlParameter> orderByParameters,
                       IList<SqlParameter> groupByParameters);
    }
}
