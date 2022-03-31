using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbTools.Core;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Config;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Extractor;

namespace ReportGenerator.Core.ReportsGenerator
{
    public class CsvReportGeneratorManager : IReportGeneratorManager
    {
        public CsvReportGeneratorManager(ILoggerFactory loggerFactory, DbEngine dbEngine, string connectionString,
                                         string separator/*, IList<string>columns*/)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<CsvReportGeneratorManager>();
            _extractor = new SimpleDbExtractor(loggerFactory, dbEngine, connectionString);
            _separator = separator;
            //_columns = columns;
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
                IReportGenerator generator = new CsvReportGenerator(_loggerFactory, template, _separator, reportFile);
                _logger.LogDebug("Report generation completed");
                return generator.Generate(result, parameters);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred during report generation, exception is: {e}");
                return false;
            }
        }

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<CsvReportGeneratorManager> _logger;
        private readonly IDbExtractor _extractor;
        private readonly string _separator;
        //private readonly IList<string> _columns;
    }
}