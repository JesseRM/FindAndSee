using Application.Likes;
using Domain;
using Persistence.DbAccess;

namespace Persistence.Repositories.Likes
{
    public class LikeData : ILikeData
    {
        private readonly ISqlDataAccess _db;

        public LikeData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Like> GetLike(Guid userObjectId, Guid findId)
        {
            string sql =
                @"SELECT * 
                  FROM likes
                  WHERE user_object_id = @UserObjectId AND find_id = @FindId";

            var results = await _db.LoadData<Like, dynamic>(
                sql,
                new { UserObjectId = userObjectId, FindId = findId }
            );

            return results.FirstOrDefault();
        }

        public Task InsertLike(Guid userObjectId, Guid findId)
        {
            string sql =
                @"INSERT INTO likes (user_object_id, find_id)
                           VALUES(@UserObjectId, @FindId)";

            return _db.SaveData(sql, new { UserObjectId = userObjectId, FindId = findId });
        }

        public Task DeleteLike(Guid userObjectId, Guid findId)
        {
            string sql =
                @"DELETE FROM likes
                  WHERE user_object_id = @UserObjectId AND find_id = @FindId";

            return _db.SaveData(sql, new { UserObjectId = userObjectId, FindId = findId });
        }
    }
}
