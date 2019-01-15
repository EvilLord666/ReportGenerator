using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using NpgsqlTypes;

namespace ReportGenerator.Core.Database.Factories
{
    public static class DbParameterFactory
    {
        public static DbParameter Create(DbEngine dbEngine, string parameterName, int parameterType, 
                                         ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            if (dbEngine == DbEngine.SqlServer)
                return new SqlParameter(parameterName, (SqlDbType)parameterType)
                {
                    Direction = parameterDirection
                };
            if (dbEngine == DbEngine.MySql)
                return new MySqlParameter(parameterName, (MySqlDbType)parameterType)
                {
                    Direction = parameterDirection
                };
            if (dbEngine == DbEngine.PostgresSql)
                return new NpgsqlParameter(parameterName, (NpgsqlDbType) parameterType)
                {
                    Direction = parameterDirection
                };
            throw new NotImplementedException("Other db engine are not supported yet, please add a github issue https://github.com/EvilLord666/ReportGenerator");
        }
    }
}