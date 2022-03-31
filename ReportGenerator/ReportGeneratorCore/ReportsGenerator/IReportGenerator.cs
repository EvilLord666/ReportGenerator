using System;
using System.Threading.Tasks;
using ReportGenerator.Core.Data;

namespace ReportGenerator.Core.ReportsGenerator
{
    public interface IReportGenerator
    {
        Task<bool> GenerateAsync(DbData data, object[] parameters);
    }
}
