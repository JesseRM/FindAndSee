using Application.Finds;
using Domain;

namespace Application.Core
{
    public interface IMapper
    {
        IEnumerable<FindBasicDto> FindToFindBasicDto(IEnumerable<Find> finds);
    }
}
