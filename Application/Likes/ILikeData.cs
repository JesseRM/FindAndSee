using Domain;

namespace Application.Likes
{
    public interface ILikeData
    {
        Task<Like> GetLike(Guid userObjectId, Guid findId);
        Task InsertLike(Guid userObjectId, Guid findId);
        Task DeleteLike(Guid userObjectId, Guid findId);
    }
}
