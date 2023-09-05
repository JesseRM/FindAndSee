using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace API.Image
{
    public class ImageAccessor : IImageAccessor
    {
        private readonly Cloudinary _cloudinary;

        public ImageAccessor(IConfiguration config)
        {
            _cloudinary = new Cloudinary(config["ASPNETCORE_CLOUDINARY_URL"]);
            _cloudinary.Api.Secure = true;
        }

        public async Task<ImageUploadResult> AddPhoto(IFormFile file)
        {
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                return uploadResult;
            }

            return null;
        }

        public async Task<string> DeletePhoto(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok" ? result.Result : null;
        }
    }
}
