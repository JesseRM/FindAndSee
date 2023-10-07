using Domain;

namespace Application.Images
{
    public interface IImageData
    {
        Task<Image> GetImage(Guid findId);
        Task InsertImage(Image image);
    }
}
