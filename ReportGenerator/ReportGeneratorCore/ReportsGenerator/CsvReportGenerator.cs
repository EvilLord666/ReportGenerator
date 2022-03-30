using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Data;

namespace ReportGenerator.Core.ReportsGenerator
{
    public class CsvReportGenerator : IReportGenerator
    {
        public CsvReportGenerator(ILoggerFactory loggerFactory, string template, string separator, string reportFile)
        {
            _logger = loggerFactory.CreateLogger<CsvReportGenerator>();
            _template = template;
            _separator = separator;
            _reportFile = reportFile;
        }

        public bool Generate(DbData data, object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        private readonly ILogger<CsvReportGenerator> _logger;
        private readonly string _template;
        private readonly string _separator;
        private readonly string _reportFile;
    }
}