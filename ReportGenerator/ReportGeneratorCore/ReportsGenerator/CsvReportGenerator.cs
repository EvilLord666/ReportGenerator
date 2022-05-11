using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<int> GenerateAsync(DbData data, object[] parameters)
        {
            try
            {
                // 1. Drop reportFile if exists & create
                if (File.Exists(_reportFile))
                    File.Delete(_reportFile);
                // we assume that valid file was passed here (at least have .csv extension)
                File.Create(_reportFile).Close();
                // 2. Write CSV header to reportFile
                string[] headers = File.ReadAllLines(_template);
                File.WriteAllLines(_reportFile, headers);
                // 3. Get Batch from dbData
                int batchCounter = 0;
                while (true)
                {
                    IList<IList<DbValue>> rowsBatch = data.Rows.Skip(batchCounter * BatchSize).Take(BatchSize).ToList();
                    batchCounter++;
                    // 4. Append every batch to the end of file
                    if (rowsBatch.Any())
                    {
                        IList<string> linesToAdd = rowsBatch.Select(r => CreateCsvRow(r)).ToList();
                        File.AppendAllLines(_reportFile, linesToAdd);
                    }
                    // 5. Stop if number of rows < BatchSize
                    if (rowsBatch.Count < BatchSize)
                    {
                        break;
                    }
                }

                return data.Rows.Count;
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred during csv report generation: {e.Message}");
                return -1;
            }
        }

        private string CreateCsvRow(IList<DbValue> columns)
        {
            StringBuilder builder = new StringBuilder();
            foreach (DbValue column in columns)
            {
                if (builder.Length > 0)
                    builder.Append(_separator);
                builder.Append(column.Value);
            }

            // builder.Append(Environment.NewLine);
            return builder.ToString();
        }

        private const int BatchSize = 50000;

        private readonly ILogger<CsvReportGenerator> _logger;
        private readonly string _template;
        private readonly string _separator;
        private readonly string _reportFile;
    }
}