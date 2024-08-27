using Application.Users;
using Dapper;
using Domain;
using Persistence.DbAccess;
using System.Data;

namespace Persistance.Repositories.Users
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<User> GetUserWithDisplayName(string displayName)
        {
            string sql = @"SELECT * FROM users WHERE LOWER(display_name) = LOWER(@DisplayName)";

            using var connection = await _db.GetConnection();

            var result = await connection.QueryAsync(sql, new { DisplayName = displayName });

            return result.FirstOrDefault();
        }

        public async Task<int> CreateUser(User user)
        {
            string sql =
                @"INSERT INTO users (object_id, display_name)
	              VALUES (@ObjectId, @DisplayName)";

            using var connection = await _db.GetConnection();

            var result = await connection.ExecuteAsync(
                sql,
                new { user.ObjectId, user.DisplayName },
                commandType: CommandType.Text
            );

            return result;
        }
    }
}
