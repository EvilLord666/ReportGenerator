using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Extensions;
using ReportGenerator.Core.ReportsGenerator;
using ReportGenerator.Core.Tests.TestUtils;
using Xunit;

namespace ReportGenerator.Core.Tests.Extensions
{
    public class ServiceCollectionExtensionsTests : IDisposable
    {
        public ServiceCollectionExtensionsTests()
        {
            _testDbName = TestDatabasePattern + "_" + DateTime.Now.ToString("YYYYMMDDHHmmss");
            _connectionString = TestSqlServerDatabaseManager.CreateDatabase(Server, _testDbName);
            _services = new ServiceCollection();
            _services.AddScoped<ILoggerFactory>(_ => new LoggerFactory());
            _services.AddReportGenerator(_connectionString);
        }

        public void Dispose()
        {
            TestSqlServerDatabaseManager.DropDatabase(Server, _testDbName);
        }

        [Fact]
        public void TestServiceInstantiationViaProvider()
        {
            IServiceProvider serviceProvider = _services.BuildServiceProvider();
            IReportGeneratorManager reportGenerator = serviceProvider.GetService<IReportGeneratorManager>();
            
            Assert.NotNull(reportGenerator);
        }

        private const string Server = @"(localdb)\mssqllocaldb";
        private const string TestDatabasePattern = "ReportGeneratorTestDb";
        
        private readonly IServiceCollection _services;
        private readonly string _testDbName;
        private readonly string _connectionString;
    }
}