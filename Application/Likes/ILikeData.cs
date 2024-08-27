using Domain;

namespace Application.Likes
{
    public interface ILikeData
    {
        Task<Like> GetLike(Guid userObjectId, Guid findId);
        Task<int> InsertLike(Guid userObjectId, Guid findId);
        Task<int> DeleteLike(Guid userObjectId, Guid findId);
    }
}
