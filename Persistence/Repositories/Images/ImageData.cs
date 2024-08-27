using Application.Images;
using Dapper;
using Domain;
using Persistence.DbAccess;
using System.Data;

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

            using var connection = await _db.GetConnection();

            var result = await connection.QueryAsync<Image>(sql, new { FindId = findId });

            return result.FirstOrDefault();
        }

        public async Task<int> InsertImage(Image image)
        {
            string sql =
                @"INSERT INTO images (find_id, public_id, url)
                           VALUES (@FindId, @PublicId, @Url)";

            using var connection = await _db.GetConnection();

            var result = await connection.ExecuteAsync(
                sql,
                new
                {
                    image.FindId,
                    image.PublicId,
                    image.Url
                },
                commandType: CommandType.Text
            );

            return result;
        }

        public async Task<int> DeleteImage(Guid findId)
        {
            string sql =
                @"DELETE FROM images
	            WHERE find_id = @FindId";

            using var connection = await _db.GetConnection();

            var result = await connection.ExecuteAsync(
                sql,
                new { FindId = findId },
                commandType: CommandType.Text
            );

            return result;
        }
    }
}
