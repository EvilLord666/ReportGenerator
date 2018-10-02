using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.ReportsGenerator;

namespace ReportGenerator.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddReportGenerator(this IServiceCollection services, string connectionString)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            services.AddScoped<IReportGeneratorManager>(_ => new ExcelReportGeneratorManager(serviceProvider.GetService<ILoggerFactory>(), connectionString));
        }
        
        public static void AddReportGenerator(this IServiceCollection services, string server, string database, 
                                              bool trustedConnection = true, string userName = null, string password = null)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            services.AddScoped<IReportGeneratorManager>(_ => new ExcelReportGeneratorManager(serviceProvider.GetService<ILoggerFactory>(), server, database,
                                                                                             trustedConnection, userName, password));
        }
    }
}