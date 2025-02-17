using AuthApi.Dto;
using AuthApi.Models;
using AutoMapper;

namespace AuthApi.AutoMapperProfiles
{
    public class UserAccountLightDtoProfile : Profile
    {
        public UserAccountLightDtoProfile()
        {
            CreateMap<UserAccount, UserAccountLightDto>()
                .ForMember(dest => dest.UserAccountId, src => src.MapFrom(x => x.UserAccountId))
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.UserName));
        }
    }
}
