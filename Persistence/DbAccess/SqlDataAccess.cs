using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;
using System.Data;

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

        public async Task<IEnumerable<T>> LoadData<T, U>(string sql, U parameters)
        {
            await using var connection = await NpgsqlDataSource
                .Create(_config["ASPNETCORE_DB_CONNECTION_STRING"])
                .OpenConnectionAsync();

            return await connection.QueryAsync<T>(sql, parameters, commandType: CommandType.Text);
        }

        public async Task<int> SaveData<T>(string sql, T parameters)
        {
            await using var connection = await NpgsqlDataSource
                .Create(_config["ASPNETCORE_DB_CONNECTION_STRING"])
                .OpenConnectionAsync();

            return await connection.ExecuteAsync(sql, parameters, commandType: CommandType.Text);
        }

        public async Task<NpgsqlConnection> GetConnection()
        {
            return await NpgsqlDataSource
                .Create(_config["ASPNETCORE_DB_CONNECTION_STRING"])
                .OpenConnectionAsync();
        }
    }
}
