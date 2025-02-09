using AuthApi.Dto;
using AuthApi.Models;
using AutoMapper;

namespace AuthApi.AutoMapperProfiles
{
    public class UserRegistrationProfile : Profile
    {
        public UserRegistrationProfile()
        {
            CreateMap<UserRegistration, UserAccount>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.UserName))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.PasswordHash, src => src.MapFrom(x => x.PasswordHash))
                .ForMember(dest => dest.RoleId, src => src.MapFrom(x => x.RoleId))
                .ForMember(dest => dest.PicturePath, src => src.MapFrom(x => x.ProfilePicturePath));
        }
    }
}
