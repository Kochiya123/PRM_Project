using AutoMapper;
using PRM_Project.Models;

namespace PRM_Project.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<StoreLocationDTO, StoreLocation>().ForMember(dest => dest.LocationId, opt => opt.Ignore());

        }
    }
}
