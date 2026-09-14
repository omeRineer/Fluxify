using AutoMapper;
using Fluxify.Application.Models.Dto.Notification;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Domain.Entities.Mongo;
using Fluxify.Domain.Enums;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fluxify.Application.MappingProfiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto.Detail>()
                .ForMember(des => des.ContentType, opt => opt.MapFrom(src => ((NotificationContentType)src.ContentType).ToString()))
                .ForMember(des => des.Type, opt => opt.MapFrom(src => EnumHelper.ToEnumString<NotificationType>(src.Type, true)))
                .ForMember(des => des.Properties, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (src.Properties != null)
                        dest.Properties = src.Properties.ToDictionary();
                });
            CreateMap<Notification, NotificationDto.List>()
                .ForMember(des => des.Type, opt => opt.MapFrom(src => EnumHelper.ToEnumString<NotificationType>(src.Type, true)))
                .ForMember(des => des.Properties, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (src.Properties != null)
                        dest.Properties = src.Properties.ToDictionary();
                });

            CreateMap<CreateNotification.Request, Notification>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(des => des.CreateDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.Type, opt => opt.MapFrom(src => (int)EnumHelper.Parse(src.Type, NotificationType.Info)))
                .ForMember(des => des.ContentType, opt => opt.MapFrom(src => (int)EnumHelper.Parse(src.ContentType, NotificationContentType.Text)))
                .ForMember(dest => dest.Properties, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (src.Properties != null)
                        dest.Properties = BsonDocument.Parse(JsonSerializer.Serialize(src.Properties));
                });
            CreateMap<CreateNotification.Request.Target, Notification.Target>()
                .ForMember(dest => dest.Users, opt => opt.Ignore());
        }
    }
}
