using CloudinaryDotNet.Actions;

namespace API.Image
{
    public interface IImageAccessor
    {
        Task<ImageUploadResult> AddPhoto(IFormFile file);
        Task<string> DeletePhoto(string publicId);
    }
}