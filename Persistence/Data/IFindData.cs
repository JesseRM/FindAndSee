using Domain;

namespace Persistence.Data
{
    public interface IFindData
    {
        Task DeleteFind(Guid id);
        Task<Find> GetFind(Guid id);
        Task<IEnumerable<Find>> GetFindsWithTerm(string term);
        Task<IEnumerable<Find>> GetRecentFinds();
        Task InsertFind(Find find);
        Task UpdateFind(Find find);
    }
}
