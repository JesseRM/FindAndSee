using Domain;
using Persistence.DbAccess;

namespace Application.Users
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
            string sql = "SELECT * FROM get_user_with_displayname(@DisplayName)";

            var results = await _db.LoadData<User, dynamic>(sql, new { DisplayName = displayName });

            return results.FirstOrDefault();
        }

        public Task CreateUser(UserCreate user)
        {
            return _db.SaveData(
                "insert_user",
                new { objectid = user.ObjectId, displayname = user.DisplayName, }
            );
        }
    }
}
