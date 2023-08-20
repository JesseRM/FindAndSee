using Domain;
using Persistence.DbAccess;

namespace Persistence.Data
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<User?> GetUser(string objectId)
        {
            string sql = "SELECT get_user(@Id)";

            var results = await _db.LoadData<User, dynamic>(sql, new { Id = objectId });

            return results.FirstOrDefault();
        }
    }
}
