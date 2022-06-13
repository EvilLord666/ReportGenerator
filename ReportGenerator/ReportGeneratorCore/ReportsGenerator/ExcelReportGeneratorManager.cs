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
        public ExcelReportGeneratorManager(ILoggerFactory loggerFactory, DbEngine dbEngine,
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

        public async Task<int> GenerateAsync(string template, string executionConfigFile, string reportFile, object[] parameters)
        {
            ExecutionConfig config = ExecutionConfigManager.Read(executionConfigFile);
            return await GenerateImplAsync(template, config, reportFile, parameters);
        }

        public async Task<int> GenerateAsync(string template, ExecutionConfig config, string reportFile, object[] parameters)
        {
            return await GenerateImplAsync(template, config, reportFile, parameters);
        }

        //todo: umv: almost same as csv ...
        private async Task<int> GenerateImplAsync(string template, ExecutionConfig config, string reportFile, object[] parameters)
        {
            try
            {
                _logger.LogInformation("Excel RepGen: Report generation was started");
                _logger.LogInformation("Excel RepGen: Database data extraction was started");
                DbData result = config.DataSource == ReportDataSource.View ? await _extractor.ExtractAsync(config.Name, config.ViewParameters)
                    : await _extractor.ExtractAsync(config.Name, config.StoredProcedureParameters);
                if (result == null)
                {
                    _logger.LogInformation("Excel RepGen: Report generation was terminated (error during data reading).");
                    return -1;
                }
                _logger.LogInformation("Excel RepGen: Database data extraction was finished");
                _logger.LogInformation("Excel RepGen: Database data write to excel file was started");
                IReportGenerator generator = new ExcelReportGenerator(_loggerFactory.CreateLogger<ExcelReportGenerator>(), template, reportFile);
                int rowsWritten = await generator.GenerateAsync(result, parameters);
                _logger.LogInformation($"Excel RepGen: Database data write to excel file was finished, written {rowsWritten} lines");
                _logger.LogInformation("Excel RepGen: Report generation was completed");
                return rowsWritten;
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred during report generation, exception is: {e}");
                return -1;
            }
        }

        private readonly IDbExtractor _extractor;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<ExcelReportGeneratorManager> _logger;
    }
}
