using System;
using System.Threading.Tasks;
using DbTools.Core;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Config;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Extractor;

namespace ReportGenerator.Core.ReportsGenerator
{
    public class ExcelReportGeneratorManager : IReportGeneratorManager
    {
        public ExcelReportGeneratorManager(ILoggerFactory loggerFactory,
                                           DbEngine dbEngine,
                                           string server, string database, bool trustedConnection = true, 
                                           string userName = null, string password = null)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<ExcelReportGeneratorManager>();
            _extractor = new SimpleDbExtractor(loggerFactory, dbEngine, server, database, 
                                               trustedConnection, userName, password);
        }

        public ExcelReportGeneratorManager(ILoggerFactory loggerFactory, DbEngine dbEngine, string connectionString)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<ExcelReportGeneratorManager>();
            _extractor = new SimpleDbExtractor(_loggerFactory, dbEngine, connectionString);
        }

        public async Task<bool> GenerateAsync(string template, string executionConfigFile, string reportFile, object[] parameters)
        {
            ExecutionConfig config = ExecutionConfigManager.Read(executionConfigFile);
            return await GenerateImplAsync(template, config, reportFile, parameters);
        }

        public async Task<bool> GenerateAsync(string template, ExecutionConfig config, string reportFile, object[] parameters)
        {
            return await GenerateImplAsync(template, config, reportFile, parameters);
        }

        private async Task<bool> GenerateImplAsync(string template, ExecutionConfig config, string reportFile, object[] parameters)
        {
            try
            {
                _logger.LogDebug("Report generation started");
                DbData result = config.DataSource == ReportDataSource.View ? await _extractor.ExtractAsync(config.Name, config.ViewParameters)
                    : await _extractor.ExtractAsync(config.Name, config.StoredProcedureParameters);
                if (result == null)
                    return false;
                IReportGenerator generator = new ExcelReportGenerator(_loggerFactory.CreateLogger<ExcelReportGenerator>(), template, reportFile);
                _logger.LogDebug("Report generation completed");
                return generator.Generate(result, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred during report generation, exception is: {e}");
                return false;
            }
        }

        private readonly IDbExtractor _extractor;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<ExcelReportGeneratorManager> _logger;
    }
}
