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
        public CsvReportGeneratorManager(ILoggerFactory loggerFactory, DbEngine dbEngine, string connectionString, string separator = DefaultCsvSeparator)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<CsvReportGeneratorManager>();
            _extractor = new SimpleDbExtractor(loggerFactory, dbEngine, connectionString);
            _separator = separator;
        }

        public async Task<int> GenerateAsync(string template, string executionConfigFile, string reportFile, object[] parameters)
        {
            ExecutionConfig config = ExecutionConfigManager.Read(executionConfigFile);
            return await GenerateImplAsync(template, config, reportFile, parameters);
        }

        public async Task<int> GenerateAsync(string template, ExecutionConfig config, string reportFile, object[] parameters)
        {
            return await GenerateImplAsync(template, config, reportFile, parameters);
        }

        private async Task<int> GenerateImplAsync(string template, ExecutionConfig config, string reportFile, object[] parameters)
        {
            try
            {
                _logger.LogInformation("CSV RepGen: Report generation was started");
                _logger.LogInformation("CSV RepGen: Database data extraction was started");
                DbData result = config.DataSource == ReportDataSource.View ? await _extractor.ExtractAsync(config.Name, config.ViewParameters)
                    : await _extractor.ExtractAsync(config.Name, config.StoredProcedureParameters);
                _logger.LogInformation("CSV RepGen: Database data extraction was finished");
                if (result == null)
                {
                    _logger.LogInformation("CSV RepGen: Report generation was terminated (error during data reading).");
                    return -1;
                }

                IReportGenerator generator = new CsvReportGenerator(_loggerFactory, template, _separator, reportFile);
                
                _logger.LogInformation("CSV RepGen: Database data write to csv file was started");
                int rowsWritten = await generator.GenerateAsync(result, parameters);
                _logger.LogInformation($"CSV RepGen: Database data write to csv file was finished, written {rowsWritten} lines");
                _logger.LogInformation("CSV RepGen: Report generation was completed");
                return rowsWritten;
            }
            catch (Exception e)
            {
                _logger.LogError($"CSV RepGen: An error occurred during report generation, exception is: {e}");
                _logger.LogInformation("CSV RepGen: Report generation was terminated (error during data reading).");
                return -1;
            }
        }

        public const string DefaultCsvSeparator = ",";

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<CsvReportGeneratorManager> _logger;
        private readonly IDbExtractor _extractor;
        private readonly string _separator;
    }
}