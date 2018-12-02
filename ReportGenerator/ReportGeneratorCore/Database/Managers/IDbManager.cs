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
        Task<bool> ExecuteNonQueryAsync(IDbCommand command);
        DbDataReader ExecuteDbReader(IDbCommand command);
        Task<DbDataReader> ExecuteDbReaderAsync(IDbCommand command);
    }
}