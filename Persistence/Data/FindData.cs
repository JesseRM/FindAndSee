using Domain;
using Persistence.DbAccess;

namespace Persistence.Data
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
            string sql = "SELECT * FROM get_recent_finds()";

            var results = await _db.LoadData<Find, dynamic>(sql, new { });

            return results;
        }

        public async Task<IEnumerable<Find>> GetFindsWithTerm(string term)
        {
            string sql = "SELECT * FROM get_finds_with_term(@search_term)";

            var results = await _db.LoadData<Find, dynamic>(sql, new { search_term = term });

            return results;
        }

        public async Task<Find> GetFind(Guid id)
        {
            string sql = "SELECT * FROM get_find(@FindId)";

            var results = await _db.LoadData<Find, dynamic>(sql, new { FindId = id });

            return results.FirstOrDefault();
        }

        public async Task<IEnumerable<Find>> GetLikedFinds(Guid userObjectId)
        {
            string sql = "SELECT * FROM get_liked_finds(@UserObjectId)";

            var results = await _db.LoadData<Find, dynamic>(
                sql,
                new { UserObjectId = userObjectId }
            );

            return results;
        }

        public async Task<IEnumerable<Find>> GetUserFinds(Guid userObjectId)
        {
            string sql = "SELECT * FROM get_user_finds(@UserObjectId)";

            var results = await _db.LoadData<Find, dynamic>(
                sql,
                new { UserObjectId = userObjectId }
            );

            return results;
        }

        public Task InsertFind(Find find)
        {
            return _db.SaveData(
                "insert_find",
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
            //TODO: Check if user owns the find and is allowed to perform update
            return _db.SaveData(
                "update_find",
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
            //TODO: Check if user owns the find and is allowed to perform delete
            return _db.SaveData("delete_find", new { fid = findId });
        }
    }
}
