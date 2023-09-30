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

        public async Task<FindDetailsDto> GetFind(Guid id)
        {
            string sql =
                @"SELECT f.find_id, title, date_created, longitude, latitude, description, author_object_id, 
                i.url AS image_url, i.public_id AS image_public_id, u.display_name
                FROM finds f 
                JOIN images i ON f.find_id = i.find_id
                JOIN users u ON u.object_id = author_object_id
                WHERE f.find_id = @FindId";

            var results = await _db.LoadData<FindDetailsDto, dynamic>(sql, new { FindId = id });

            return results.FirstOrDefault();
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

        public Task InsertFind(Find find)
        {
            string sql =
                @"INSERT INTO finds (title, image_url, date_created, longitude, latitude, 
                                     description, author_object_Id, is_approved, rejected)
	            VALUES (@title, @image_url, @date_created, @longitude, @latitude, @description, 
                        @author_object_id, false, false)";

            return _db.SaveData(
                sql,
                new
                {
                    title = find.Title,
                    image_url = find.ImageUrl,
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
