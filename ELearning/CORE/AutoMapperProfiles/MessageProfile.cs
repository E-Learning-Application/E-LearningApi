using AutoMapper;
using CORE.DTOs.Message;
using DATA.Models;

namespace CORE.AutoMapperProfiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageAddRequest, Message>()
                .ReverseMap();

            CreateMap<Message, MessageResponse>()
                .ForMember(dest => dest.SenderUserName, opt => opt.MapFrom(src => src.Sender.UserName))
                .ForMember(dest => dest.ReceiverUserName, opt => opt.MapFrom(dest => dest.Receiver.UserName))
                .ForMember(dest => dest.SenderImagePath, opt => opt.MapFrom(src => src.Sender.ImagePath))
                .ForMember(dest => dest.ReceiverImagePath, opt => opt.MapFrom(dest => dest.Receiver.ImagePath))
                .ReverseMap();



        }
    }
}
