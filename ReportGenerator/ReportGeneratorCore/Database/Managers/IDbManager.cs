using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace ReportGenerator.Core.Database.Managers
{
    public interface IDbManager
    {
        bool CreateDatabase(string connectionString, bool dropIfExists);
        bool DropDatabase(string connectionString);
        bool ExecuteNonQuery(IDbCommand command);
        bool ExecuteNonQuery(string connectionString, string cmdText); 
        Task<bool> ExecuteNonQueryAsync(DbCommand command);
        Task<bool> ExecuteNonQueryAsync(string connectionString, string cmdText);
        IDataReader ExecuteDbReader(IDbCommand command);
        Task<DbDataReader> ExecuteDbReaderAsync(DbCommand command);
        Task<DbDataReader> ExecuteDbReaderAsync(string connectionString, string cmdText);
    }
}