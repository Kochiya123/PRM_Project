using AutoMapper;
using PRM_Project.Models;

namespace PRM_Project.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<StoreLocationDTO, StoreLocation>()
                .ForMember(dest => dest.LocationId, opt => opt.Ignore());

            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role ?? "Customer"));

            CreateMap<AddChatMessageDTO, ChatMessage>()
                .ForMember(dest => dest.ChatMessageId, opt => opt.Ignore());
                
        }
    }
}
