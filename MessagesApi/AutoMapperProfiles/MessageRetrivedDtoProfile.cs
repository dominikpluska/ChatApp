using AutoMapper;
using MessagesApi.Dto;
using MessagesApi.Models;

namespace MessagesApi.AutoMapperProfiles
{
    public class MessageRetrivedDtoProfile : Profile
    {
        public MessageRetrivedDtoProfile()
        {
            //CreateMap<Message, MessageRetrivedDto>()
            //.ForMember(dest => dest.TextMessage, src => src.MapFrom(x => x.TextMessage))
            //.ForMember(dest => dest.PostedDate, src => src.MapFrom(x => x.PostedDate))
            //.ForMember(dest => dest.User, src => src.MapFrom(x => x.UserId))
            //.ForMember(dest => dest.PostedDate, src => src.MapFrom(x => x.PostedDate));
        }
    }
}
