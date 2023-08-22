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
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(string sql, U parameters)
        {
            await using var connection = await NpgsqlDataSource
                .Create(_config["ASPNETCORE_DB_CONNECTION_STRING"])
                .OpenConnectionAsync();

            return await connection.QueryAsync<T>(sql, parameters, commandType: CommandType.Text);
        }

        public async Task SaveData<T>(string storedProcedure, T parameters)
        {
            await using var connection = await NpgsqlDataSource
                .Create(_config["ASPNETCORE_DB_CONNECTION_STRING"])
                .OpenConnectionAsync();

            await connection.ExecuteAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
