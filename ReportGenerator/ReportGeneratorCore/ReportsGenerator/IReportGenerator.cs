using System;
using System.Threading.Tasks;
using ReportGenerator.Core.Data;

namespace ReportGenerator.Core.ReportsGenerator
{
    public interface IReportGenerator
    {
        Task<int> GenerateAsync(DbData data, object[] parameters);
    }
}
