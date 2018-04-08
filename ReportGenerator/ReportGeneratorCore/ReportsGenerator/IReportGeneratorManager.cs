using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator.Core.ReportsGenerator
{
    public interface IReportGeneratorManager
    {
        Task<bool> Generate(string template, string executionConfigFile, string reportFile, object[] parameters);
    }
}
