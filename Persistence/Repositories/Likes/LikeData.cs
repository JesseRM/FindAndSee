using Application.Likes;
using Dapper;
using Domain;
using Persistence.DbAccess;
using System.Data;

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

            using var connection = await _db.GetConnection();

            var result = await connection.QueryAsync(
                sql,
                new { UserObjectId = userObjectId, FindId = findId }
            );

            return result.FirstOrDefault();
        }

        public async Task<int> InsertLike(Guid userObjectId, Guid findId)
        {
            string sql =
                @"INSERT INTO likes (user_object_id, find_id)
                           VALUES(@UserObjectId, @FindId)";

            using var connection = await _db.GetConnection();

            var result = await connection.ExecuteAsync(
                sql,
                new { UserObjectId = userObjectId, FindId = findId },
                commandType: CommandType.Text
            );

            return result;
        }

        public async Task<int> DeleteLike(Guid userObjectId, Guid findId)
        {
            string sql =
                @"DELETE FROM likes
                  WHERE user_object_id = @UserObjectId AND find_id = @FindId";

            using var connection = await _db.GetConnection();

            var result = await connection.ExecuteAsync(
                sql,
                new { UserObjectId = userObjectId, FindId = findId },
                commandType: CommandType.Text
            );

            return result;
        }
    }
}
