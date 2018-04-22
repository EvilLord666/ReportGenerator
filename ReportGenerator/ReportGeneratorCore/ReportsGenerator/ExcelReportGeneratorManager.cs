﻿using System;
using System.Threading.Tasks;
using ReportGenerator.Core.Config;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.Extractor;

namespace ReportGenerator.Core.ReportsGenerator
{
    public class ExcelReportGeneratorManager : IReportGeneratorManager
    {
        public ExcelReportGeneratorManager(string server, string database,  bool trustedconnection = true, 
                                           string userName = null, string password = null)
        {
            _extractor = new SimpleDbExtractor(server, database, trustedconnection, userName, password);
        }

        public ExcelReportGeneratorManager(string connectionString)
        {
            _extractor = new SimpleDbExtractor(connectionString);
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

        public async Task<bool> GenerateImplAsync(string template, ExecutionConfig config, string reportFile, object[] parameters)
        {
            try
            {
                DbData result = config.DataSource == ReportDataSource.View ? await _extractor.ExtractAsync(config.Name, config.ViewParameters)
                    : await _extractor.ExtractAsync(config.Name, config.StoredProcedureParameters);
                if (result == null)
                    return false;
                IReportGenerator generator = new ExcelReportGenerator(template, reportFile);
                return generator.Generate(result, parameters);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private readonly IDbExtractor _extractor;
    }
}