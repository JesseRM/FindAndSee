using Application.Images;
using Domain;
using Persistence.DbAccess;

namespace Persistence.Repositories.Images
{
    public class ImageData : IImageData
    {
        private readonly ISqlDataAccess _db;

        public ImageData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Image> GetImage(Guid findId)
        {
            string sql =
                @"SELECT *
                           FROM images
                           WHERE find_id = @FindId";

            var results = await _db.LoadData<Image, dynamic>(sql, new { FindId = findId });

            return results.FirstOrDefault();
        }

        public Task InsertImage(Image image)
        {
            string sql =
                @"INSERT INTO images (find_id, public_id, url)
                           VALUES (@FindId, @PublicId, @Url)";

            return _db.SaveData(
                sql,
                new
                {
                    FindId = image.FindId,
                    PublicId = image.PublicId,
                    Url = image.Url
                }
            );
        }

        public Task DeleteImage(Guid findId)
        {
            string sql =
                @"DELETE FROM images
	            WHERE find_id = @FindId";

            return _db.SaveData(sql, new { FindId = findId });
        }
    }
}
