using Domain;

namespace Application.Finds
{
    public interface IFindData
    {
        Task DeleteFind(Guid id);
        Task<FindDetailsDto> GetFind(Guid id);
        Task<IEnumerable<FindBasicDto>> GetFindsWithTerm(string term);
        Task<IEnumerable<FindBasicDto>> GetLikedFinds(Guid userObjectId);
        Task<IEnumerable<FindBasicDto>> GetRecentFinds();
        Task<IEnumerable<FindBasicDto>> GetUserFinds(Guid userObjectId);
        Task InsertFind(Find find);
        Task UpdateFind(Find find);
    }
}
