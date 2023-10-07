using Dapper;
using Domain;
using Persistence.DbAccess;

namespace Application.Finds
{
    public class FindData : IFindData
    {
        private readonly ISqlDataAccess _db;

        public FindData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<IEnumerable<FindBasicDto>> GetRecentFinds()
        {
            string sql =
                @"SELECT f.find_id, f.title, f.date_created, i.url AS image_url
                FROM finds f 
                JOIN images i ON f.find_id = i.find_id 
                ORDER BY date_created DESC 
                LIMIT 10";

            var results = await _db.LoadData<FindBasicDto, dynamic>(sql, new { });

            return results;
        }

        public async Task<IEnumerable<FindBasicDto>> GetFindsWithTerm(string term)
        {
            string sql =
                @"SELECT f.find_id, f.title, f.date_created, i.url AS image_url
                FROM finds f 
                JOIN images i ON f.find_id = i.find_id 
                WHERE LOWER(f.title) LIKE '%' || LOWER(@SearchTerm) || '%'";

            var results = await _db.LoadData<FindBasicDto, dynamic>(sql, new { SearchTerm = term });

            return results;
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

        public async Task<IEnumerable<FindBasicDto>> GetLikedFinds(Guid userObjectId)
        {
            string sql =
                @"SELECT f.find_id, f.title, f.date_created, i.url AS image_url
                FROM finds f 
                JOIN images i ON f.find_id = i.find_id
                JOIN likes l ON f.author_object_id = l.user_object_id
                WHERE l.user_object_id = @UserObjectId";

            var results = await _db.LoadData<FindBasicDto, dynamic>(
                sql,
                new { UserObjectId = userObjectId }
            );

            return results;
        }

        public async Task<IEnumerable<FindBasicDto>> GetUserFinds(Guid userObjectId)
        {
            string sql =
                @"SELECT f.find_id, f.title, f.date_created, i.url AS image_url
                FROM finds f 
                JOIN images i ON f.find_id = i.find_id
                WHERE f.author_object_id = @UserObjectId";

            var results = await _db.LoadData<FindBasicDto, dynamic>(
                sql,
                new { UserObjectId = userObjectId }
            );

            return results;
        }

        public Task InsertFind(FindCreateDto find)
        {
            string sql =
                @"INSERT INTO finds (title, date_created, longitude, latitude,
                                     description, author_object_Id, is_approved, rejected)
                VALUES (@title, @date_created, @longitude, @latitude, @description,
                        @author_object_id, false, false)";

            return _db.SaveData(
                sql,
                new
                {
                    title = find.Title,
                    date_created = find.DateCreated,
                    longitude = find.Longitude,
                    latitude = find.Latitude,
                    description = find.Description,
                    author_object_id = find.AuthorObjectId,
                }
            );
        }

        public Task UpdateFind(Find find)
        {
            string sql =
                @"UPDATE finds
	            SET
		            longitude = @new_longitude,
		            latitude = @new_latitude,
		            description = @new_description
	            WHERE find_id = fid";

            return _db.SaveData(
                sql,
                new
                {
                    fid = find.FindId,
                    new_longitude = find.Longitude,
                    new_latitude = find.Latitude,
                    new_description = find.Description,
                }
            );
        }

        public Task DeleteFind(Guid findId)
        {
            string sql =
                @"DELETE FROM finds
	            WHERE find_id = @fid";

            return _db.SaveData(sql, new { fid = findId });
        }
    }
}
