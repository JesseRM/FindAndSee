using Application.Finds;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Find, FindBasicDto>();
            CreateMap<Find, FindDetailsDto>();
        }
    }
}
