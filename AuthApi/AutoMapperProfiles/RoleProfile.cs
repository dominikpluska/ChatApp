using AuthApi.Dto;
using AuthApi.Models;
using AutoMapper;

namespace AuthApi.AutoMapperProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.RoleId, src => src.MapFrom(x => x.RoleId))
                .ForMember(dest => dest.RoleName, src => src.MapFrom(x => x.RoleName));
        }
    }
}
