using System.Threading.Tasks;
using ReportGenerator.Core.Config;

namespace ReportGenerator.Core.ReportsGenerator
{
    public interface IReportGeneratorManager
    {
        Task<int> GenerateAsync(string template, string executionConfigFile, string reportFile, object[] parameters);
        Task<int> GenerateAsync(string template, ExecutionConfig config, string reportFile, object[] parameters);
    }
}
