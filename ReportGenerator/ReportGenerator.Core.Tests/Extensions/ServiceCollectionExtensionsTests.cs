using System;
using Microsoft.Extensions.DependencyInjection;
using ReportGenerator.Core.Extensions;
using ReportGenerator.Core.Tests.TestUtils;
using Xunit;

namespace ReportGenerator.Core.Tests.Extensions
{
    public class ServiceCollectionExtensionsTests : IDisposable
    {
        public ServiceCollectionExtensionsTests()
        {
            _testDbName = TestDatabasePattern + "_" + DateTime.Now.ToString("YYYYMMDDHHmmss");
            _connectionString = TestDatabaseManager.CreateDatabase(Server, _testDbName);
            _services = new ServiceCollection();
            _services.AddReportGenerator(_connectionString);
        }

        public void Dispose()
        {
            TestDatabaseManager.DropDatabase(Server, _testDbName);
        }

        [Fact]
        public void TestServiceInstantiationViaProvider()
        {
            
        }

        private const string Server = @"(localdb)\mssqllocaldb";
        private const string TestDatabasePattern = "ReportGeneratorTestDb";
        
        private readonly IServiceCollection _services;
        private readonly string _testDbName;
        private readonly string _connectionString;
    }
}