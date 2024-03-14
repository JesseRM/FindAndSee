using Npgsql;

namespace Persistence.DbAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string sql, U parameters);
        Task<int> SaveData<T>(string storedProcedure, T parameters);
        Task<NpgsqlConnection> GetConnection();
    }
}
