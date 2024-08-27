using Domain;

namespace Application.Images
{
    public interface IImageData
    {
        Task<Image> GetImage(Guid findId);
        Task<int> InsertImage(Image image);
    }
}
