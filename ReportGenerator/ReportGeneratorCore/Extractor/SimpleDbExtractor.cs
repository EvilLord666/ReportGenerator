using System;
using System.Collections.Generic;
using System.Text;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Data.Parameters;

namespace ReportGenerator.Core.Extractor
{
    public class SimpleDbExtractor : IDbExtractor
    {
        public DbData Extract(string storedPocedureName, IList<StoredProcedureParameter> parameters)
        {
            throw new NotImplementedException();
        }

        public DbData Extract(string viewName, ViewParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
