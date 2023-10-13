using Application.Users;
using Domain;
using Persistence.DbAccess;

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

            var results = await _db.LoadData<User, dynamic>(sql, new { DisplayName = displayName });

            return results.FirstOrDefault();
        }

        public async Task CreateUser(UserCreate user)
        {
            string sql =
                @"INSERT INTO users (object_id, display_name)
	              VALUES (@ObjectId, @DisplayName)";

            await _db.SaveData<dynamic>(sql, new { user.ObjectId, user.DisplayName });
        }
    }
}
