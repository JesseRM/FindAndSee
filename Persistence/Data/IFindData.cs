using Domain;

namespace Persistence.Data
{
    public interface IFindData
    {
        Task DeleteFind(Guid id);
        Task<Find> GetFind(Guid id);
        Task<IEnumerable<Find>> GetFindsWithTerm(string term);
        Task<IEnumerable<Find>> GetLikedFinds(Guid userObjectId);
        Task<IEnumerable<Find>> GetRecentFinds();
        Task<IEnumerable<Find>> GetUserFinds(Guid userObjectId);
        Task InsertFind(Find find);
        Task UpdateFind(Find find);
    }
}
