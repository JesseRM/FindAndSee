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

        public async Task<IEnumerable<T>> LoadData<T, U>(
            string sql,
            U parameters,
            string connectionId = "Default"
        )
        {
            await using var connection = await NpgsqlDataSource
                .Create(_config.GetConnectionString(connectionId))
                .OpenConnectionAsync();

            return await connection.QueryAsync<T>(sql, parameters, commandType: CommandType.Text);
        }

        public async Task SaveData<T>(
            string storedProcedure,
            T parameters,
            string connectionId = "Default"
        )
        {
            await using var connection = await NpgsqlDataSource
                .Create(_config.GetConnectionString(connectionId))
                .OpenConnectionAsync();

            await connection.ExecuteAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
