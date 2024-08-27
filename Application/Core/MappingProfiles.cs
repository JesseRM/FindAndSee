using Application.Finds;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Find, FindBasicDto>()
                .ForMember(dest => dest.FindId, opt => opt.MapFrom(src => src.FindId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image.Url))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated));
            CreateMap<Find, FindDetailsDto>();
        }
    }
}
