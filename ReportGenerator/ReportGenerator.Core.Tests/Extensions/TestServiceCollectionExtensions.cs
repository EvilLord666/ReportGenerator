using System;
using System.Collections.Generic;
using DbTools.Core;
using DbTools.Core.Managers;
using DbTools.Simple.Managers;
using DbTools.Simple.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Extensions;
using ReportGenerator.Core.ReportsGenerator;
using Xunit;

namespace ReportGenerator.Core.Tests.Extensions
{
    public class TestServiceCollectionExtensions : IDisposable
    {
        public TestServiceCollectionExtensions()
        {
            string testDbName = GlobalTestsParams.TestSqlServerDatabasePattern + "_" + DateTime.Now.ToString("YYYYMMDDHHmmss");
            _dbManager = new CommonDbManager(DbEngine.SqlServer, _loggerFactory.CreateLogger<CommonDbManager>());
            IDictionary<string, string> connectionStringParams = new Dictionary<string, string>()
            {
                {DbParametersKeys.HostKey, GlobalTestsParams.TestSqlServerHost},
                {DbParametersKeys.DatabaseKey, testDbName},
                {DbParametersKeys.UseIntegratedSecurityKey, "true"},
                {DbParametersKeys.UseTrustedConnectionKey, "true"}
            };
            _connectionString = ConnectionStringBuilder.Build(DbEngine.SqlServer, connectionStringParams);
            _dbManager.CreateDatabase(_connectionString, true);
            _services = new ServiceCollection();
            _services.AddScoped<ILoggerFactory>(_ => new LoggerFactory());
            _services.AddReportGenerator(DbEngine.SqlServer, _connectionString);
        }

        public void Dispose()
        {
            _dbManager.DropDatabase(_connectionString);
        }

        [Fact]
        public void TestServiceInstantiationViaProvider()
        {
            IServiceProvider serviceProvider = _services.BuildServiceProvider();
            IReportGeneratorManager reportGenerator = serviceProvider.GetService<IReportGeneratorManager>();
            
            Assert.NotNull(reportGenerator);
        }
        
        private readonly IServiceCollection _services;
        private readonly string _connectionString;
        private readonly IDbManager _dbManager;
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
    }
}