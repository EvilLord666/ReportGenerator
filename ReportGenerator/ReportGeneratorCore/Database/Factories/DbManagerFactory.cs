using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using Microsoft.Extensions.Logging;
using ReportGenerator.Core.Database.Managers;

namespace ReportGenerator.Core.Database.Factories
{
    public static class DbManagerFactory
    {
        public static IDbManager Create(DbEngine dbEngine, ILoggerFactory loggerFactory)
        {
            return new CommonDbManager(dbEngine, loggerFactory.CreateLogger<CommonDbManager>());
        }
    }
}