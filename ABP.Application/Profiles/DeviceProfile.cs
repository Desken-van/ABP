using AutoMapper;
using ABP.AppCore.Entities;
using ABP.Application.Models;
using ABP.Application.Models.Requests;
using ABP_Server.Models;

namespace ABP.Application.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<DeviceEntity, Device>().ReverseMap();

            CreateMap<DeviceTokenEntity, DeviceToken>().ReverseMap();

            CreateMap<DeviceRequest, Device>().ReverseMap()
                .ForMember(dest => dest.DeviceName, opt => opt.MapFrom(src => src.DeviceName));

            CreateMap<DeviceTokenRequest, DeviceToken>().ReverseMap()
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.DeviceId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
        }
    }
}
