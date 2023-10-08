using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ABP.AppCore.Entities;
using ABP.Application.Models;
using ABP.Application.Models.Requests;
using ABP_Server.Models;

namespace ABP.Tests.Settings
{
    public static class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DeviceEntity, Device>().ReverseMap();

                cfg.CreateMap<DeviceTokenEntity, DeviceToken>().ReverseMap();

                cfg.CreateMap<DeviceRequest, Device>().ReverseMap()
                .ForMember(dest => dest.DeviceName, opt => opt.MapFrom(src => src.DeviceName));

                cfg.CreateMap<DeviceTokenRequest, DeviceToken>().ReverseMap()
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.DeviceId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));

                cfg.CreateMap<ExperimentEntity, Experiment>().ReverseMap();

                cfg.CreateMap<ExperimentRequest, Experiment>().ReverseMap()
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.DeviceId))
                .ForMember(dest => dest.ExperimentType, opt => opt.MapFrom(src => src.ExperimentType))
                .ForMember(dest => dest.KeyValue, opt => opt.MapFrom(src => src.KeyValue))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
