using AutoMapper;
using UserSettingsApi.Dto;
using UserSettingsApi.Models;

namespace UserSettingsApi.AutoMapperProfiles
{
    public class RequestListDtoProfile : Profile
    {
        public RequestListDtoProfile()
        {
            CreateMap<Request, RequestDto>()
            .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.RequestorId))
            .ForMember(dest => dest.RequestType, src => src.MapFrom(x => x.RequestType))
            .ForMember(dest => dest.RequestId, src => src.MapFrom(x => x.RequestId))
            .ForMember(dest => dest.IsAccepted, src => src.MapFrom(x => x.IsAccepted));
        }
    }
}
