using AutoMapper;
using Fluxify.Application.Models.Dto.UserDevice;
using Fluxify.Application.Models.Rest.UserDevice;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Domain.Entities.Core;
using Fluxify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.MappingProfiles
{
    public class UserDeviceProfile : Profile
    {
        public UserDeviceProfile()
        {
            CreateMap<UserDevice, UserDeviceDto.List>()
                .ForMember(dest => dest.Platform, opt => opt.MapFrom(src => EnumHelper.ToEnumString<DeviceType>(src.Platform, true)));

            CreateMap<SetUserDevice, UserDevice>()
                .ForMember(dest => dest.Platform, opt => opt.MapFrom(src => EnumHelper.Parse<DeviceType>(src.Platform, DeviceType.Android)))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        }
    }
}
