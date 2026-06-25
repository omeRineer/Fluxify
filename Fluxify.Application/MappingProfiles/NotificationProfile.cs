using AutoMapper;
using Fluxify.Application.Models.Dto.Notification;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Domain.Core;
using Fluxify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.MappingProfiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto.Detail>()
                .ForMember(des => des.ContentType, opt => opt.MapFrom(src => ((NotificationContentType)src.ContentType).ToString()))
                .ForMember(des => des.Level, opt => opt.MapFrom(src => ((NotificationLevelType)src.Level).ToString()));
            CreateMap<Notification, NotificationDto.List>()
                .ForMember(des => des.ContentType, opt => opt.MapFrom(src => ((NotificationContentType)src.ContentType).ToString()))
                .ForMember(des => des.Level, opt => opt.MapFrom(src => ((NotificationLevelType)src.Level).ToString()));

            CreateMap<CreateNotification.Request, Notification>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(des => des.CreateDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.Level, opt => opt.MapFrom(src => (int)EnumHelper.Parse(src.Level, NotificationLevelType.Info)))
                .ForMember(des => des.ContentType, opt => opt.MapFrom(src => (int)EnumHelper.Parse(src.ContentType, NotificationContentType.Text)));
        }
    }
}
