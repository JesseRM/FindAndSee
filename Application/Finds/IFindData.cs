using Domain;

namespace Application.Finds
{
    public interface IFindData
    {
        Task<Find> GetFind(Guid id);
        Task<IEnumerable<Find>> GetFindsWithTerm(string term);
        Task<IEnumerable<Find>> GetLikedFinds(Guid userObjectId);
        Task<IEnumerable<Find>> GetRecentFinds();
        Task<IEnumerable<Find>> GetUserFinds(Guid userObjectId);
        Task<int> DeleteFind(Guid id);
        Task<int> InsertFind(FindCreateDto find);
        Task<int> UpdateFind(FindUpdateDto find);
    }
}
