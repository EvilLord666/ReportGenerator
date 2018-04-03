using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Data.Parameters;

namespace ReportGenerator.Core.Extractor
{
    public interface IDbExtractor
    {
        Task<DbData> Extract(string storedPocedureName, IList<StoredProcedureParameter> parameters);

        Task<DbData> Extract(string viewName, ViewParameters parameters);
    }
}
