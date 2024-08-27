using Npgsql;

namespace Persistence.DbAccess
{
    public interface ISqlDataAccess
    {
        Task<NpgsqlConnection> GetConnection();
    }
}
