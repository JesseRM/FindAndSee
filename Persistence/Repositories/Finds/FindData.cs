using Dapper;
using Domain;
using Persistence.DbAccess;
using System.Data;

namespace Application.Finds
{
    public class FindData : IFindData
    {
        private readonly ISqlDataAccess _db;

        public FindData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Find>> GetRecentFinds()
        {
            string sql =
                @"SELECT f.*, i.*, u.*
                FROM finds f 
                JOIN images i ON f.find_id = i.find_id
                JOIN users u ON u.object_id = author_object_id
                ORDER BY date_created DESC  
                LIMIT 10";

            using var connection = await _db.GetConnection();

            var result = await connection.QueryAsync<Find, Image, User, Find>(
                sql,
                (find, image, user) =>
                {
                    find.Image = image;
                    find.User = user;

                    return find;
                },
                splitOn: "image_id, object_id"
            );

            return result;
        }

        public async Task<IEnumerable<Find>> GetFindsWithTerm(string term)
        {
            string sql =
                @"SELECT f.*, i.*, u.*
                FROM finds f 
                JOIN images i ON f.find_id = i.find_id
                JOIN users u ON u.object_id = author_object_id
                WHERE LOWER(f.title) LIKE '%' || LOWER(@SearchTerm) || '%'";

            using var connection = await _db.GetConnection();

            var result = await connection.QueryAsync<Find, Image, User, Find>(
                sql,
                (find, image, user) =>
                {
                    find.Image = image;
                    find.User = user;

                    return find;
                },
                new { SearchTerm = term },
                splitOn: "image_id, object_id"
            );

            return result;
        }

        public async Task<Find> GetFind(Guid id)
        {
            string sql =
                @"SELECT f.*, i.*, u.*
                FROM finds f 
                JOIN images i ON f.find_id = i.find_id
                JOIN users u ON u.object_id = author_object_id
                WHERE f.find_id = @FindId";

            using var connection = await _db.GetConnection();

            var result = await connection.QueryAsync<Find, Image, User, Find>(
                sql,
                (find, image, user) =>
                {
                    find.Image = image;
                    find.User = user;

                    return find;
                },
                new { FindId = id },
                splitOn: "image_id, object_id"
            );

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Find>> GetLikedFinds(Guid userObjectId)
        {
            string sql =
                @"SELECT f.*, i.*, u.*
                FROM finds f 
                JOIN images i ON f.find_id = i.find_id
                JOIN users u ON u.object_id = author_object_id
                JOIN likes l ON f.find_id = l.find_id
                WHERE l.user_object_id = @UserObjectId";

            using var connection = await _db.GetConnection();

            var result = await connection.QueryAsync<Find, Image, User, Find>(
                sql,
                (find, image, user) =>
                {
                    find.Image = image;
                    find.User = user;

                    return find;
                },
                new { UserObjectId = userObjectId },
                splitOn: "image_id, object_id"
            );

            return result;
        }

        public async Task<IEnumerable<Find>> GetUserFinds(Guid userObjectId)
        {
            string sql =
                @"SELECT f.*, i.*, u.*
                FROM finds f 
                JOIN images i ON f.find_id = i.find_id
                JOIN users u ON u.object_id = author_object_id
                WHERE f.author_object_id = @UserObjectId";

            using var connection = await _db.GetConnection();

            var result = await connection.QueryAsync<Find, Image, User, Find>(
                sql,
                (find, image, user) =>
                {
                    find.Image = image;
                    find.User = user;

                    return find;
                },
                new { UserObjectId = userObjectId },
                splitOn: "image_id, object_id"
            );

            return result;
        }

        public async Task<int> InsertFind(FindCreateDto find)
        {
            string sql =
                @"INSERT INTO finds (find_id, title, date_created, longitude, latitude,
                                     description, author_object_Id, is_approved, is_rejected)
                VALUES (@FindId, @Title, @DateCreated, @Longitude, @Latitude, @Description,
                        @AuthorObjectId, false, false)";

            using var connection = await _db.GetConnection();

            return await connection.ExecuteAsync(
                sql,
                new
                {
                    find.FindId,
                    find.Title,
                    find.DateCreated,
                    find.Longitude,
                    find.Latitude,
                    find.Description,
                    find.AuthorObjectId,
                },
                commandType: CommandType.Text
            );
        }

        public async Task<int> UpdateFind(FindUpdateDto find)
        {
            string sql =
                @"UPDATE finds
	            SET
                    title = @Title,
		            longitude = @Longitude,
		            latitude = @Latitude,
		            description = @Description
	            WHERE find_id = @FindId AND author_object_id = @AuthorObjectId";

            using var connection = await _db.GetConnection();

            return await connection.ExecuteAsync(
                sql,
                new
                {
                    find.FindId,
                    find.Title,
                    find.Longitude,
                    find.Latitude,
                    find.Description,
                    find.AuthorObjectId
                }
            );
        }

        public async Task<int> DeleteFind(Guid findId)
        {
            string sql =
                @"DELETE FROM finds
	            WHERE find_id = @fid";

            using var connection = await _db.GetConnection();

            return await connection.ExecuteAsync(sql, new { fid = findId });
        }
    }
}
