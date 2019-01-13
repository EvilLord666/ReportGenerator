using System;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace ReportGenerator.Core.Database.Factories
{
    public static class DbParameterFactory
    {
        public static DbParameter Create(DbEngine dbEngine, string parameterName, int parameterType)
        {
            if (dbEngine == DbEngine.SqlServer)
                return new SqlParameter(parameterName, parameterType);
            if (dbEngine == DbEngine.MySql)
                return new MySqlParameter(parameterName, parameterType);
            throw new NotImplementedException("Other db engine are not supported yet, please add a github issue https://github.com/EvilLord666/ReportGenerator");
        }
    }
}