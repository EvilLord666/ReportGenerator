using System;
using System.Collections.Generic;
using System.Text;
using ReportGenerator.Core.Data;

namespace ReportGenerator.Core.Extractor
{
    public interface IDbExtractor
    {
        DbData Extract(string storedPocedureName);
    }
}
