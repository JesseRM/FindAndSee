using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;

namespace Persistence.DbAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public async Task<NpgsqlConnection> GetConnection()
        {
            return await NpgsqlDataSource
                .Create(_config["ASPNETCORE_DB_CONNECTION_STRING"])
                .OpenConnectionAsync();
        }
    }
}
