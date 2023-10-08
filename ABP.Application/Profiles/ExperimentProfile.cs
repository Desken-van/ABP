using ABP.AppCore.Entities;
using ABP.Application.Models;
using ABP.Application.Models.Requests;
using AutoMapper;

namespace ABP.Application.Profiles
{
    public class ExperimentProfile : Profile
    {
        public ExperimentProfile()
        {
            CreateMap<ExperimentEntity, Experiment>().ReverseMap();

            CreateMap<ExperimentRequest, Experiment>().ReverseMap()
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.DeviceId))
                .ForMember(dest => dest.ExperimentType, opt => opt.MapFrom(src => src.ExperimentType))
                .ForMember(dest => dest.KeyValue, opt => opt.MapFrom(src => src.KeyValue))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
        }
    }
}
