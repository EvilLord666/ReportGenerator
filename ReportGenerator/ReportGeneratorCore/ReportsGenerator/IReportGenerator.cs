using System;
using ReportGenerator.Core.Data;

namespace ReportGenerator.Core.ReportsGenerator
{
    public interface IReportGenerator
    {
        bool Generate(DbData data, object[] parameters);
    }
}
