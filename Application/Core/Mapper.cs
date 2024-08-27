using Application.Finds;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class Mapper : IMapper
    {
        private readonly AutoMapper.IMapper _mapper;

        public Mapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = config.CreateMapper();
        }

        public IEnumerable<FindBasicDto> FindToFindBasicDto(IEnumerable<Find> finds)
        {
            var findBasicDtos = _mapper.Map<IEnumerable<FindBasicDto>>(finds);

            return findBasicDtos;
        }
    }
}
