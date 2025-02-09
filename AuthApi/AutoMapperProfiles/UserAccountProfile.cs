using AuthApi.Dto;
using AuthApi.Models;
using AutoMapper;

namespace AuthApi.AutoMapperProfiles
{
    public class UserAccountProfile : Profile
    {
        public UserAccountProfile()
        {
            CreateMap<UserAccount, UserAccountDto>()
                .ForMember(dest => dest.UserAccountId, src => src.MapFrom(x => x.UserAccountId))
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.UserName))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
                .ForMember(dest => dest.RoleName, src => src.MapFrom(x => x.Role!.RoleName));
        }
    }
}
