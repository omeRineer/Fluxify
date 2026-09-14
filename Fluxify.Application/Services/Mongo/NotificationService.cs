using Fluxify.Application.Hubs;
using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Dto.Notification;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Common;
using AutoMapper;
using Fluxify.Application.Infrastructure;
using Fluxify.Domain.Entities.Mongo;
using Fluxify.Application.Services.Interfaces.Mongo;
using Fluxify.Application.Repositories.Base;
using Fluxify.Application.Repositories.Mongo;
using Fluxify.Application.Models.Events;
using MongoDB.Driver;
using System.Text.Json;
using MongoDB.Bson;

namespace Fluxify.Application.Services.Mongo
{

    public class NotificationService : INotificationService
    {
        readonly INotificationRepository NotificationRepository;
        readonly IUserRepository UserRepository;
        readonly ITopicRepository TopicRepository;
        readonly IUserNotificationRepository UserNotificationRepository;
        readonly IUserTopicRepository UserTopicRepository;
        readonly IEventPublisher EventPublisher;
        readonly IMapper _mapper;
        public NotificationService(INotificationRepository notifications, IUserNotificationRepository userNotificationRepository, ITopicRepository topicRepository, IMapper mapper, IUserTopicRepository userTopicRepository, IEventPublisher eventPublisher, IUserRepository userRepository)
        {
            NotificationRepository = notifications;
            UserNotificationRepository = userNotificationRepository;
            TopicRepository = topicRepository;
            _mapper = mapper;
            UserTopicRepository = userTopicRepository;
            EventPublisher = eventPublisher;
            UserRepository = userRepository;
        }

        public async Task<IResult> CreateAsync(Guid nameSpaceId, CreateNotification.Request req)
        {
            var newNotification = _mapper.Map<Notification>(req);
            newNotification.NameSpace = nameSpaceId;

            if (req.To?.Users != null)
            {
                var targetUsers = await UserRepository.GetProjectedListAsync(s => s.Id, f => f.NameSpaceId == nameSpaceId && req.To.Users.Contains(f.Key));
                newNotification.To!.Users = targetUsers;
            }

            await NotificationRepository.InsertAsync(newNotification);
            await EventPublisher.PublishAsync("notification-topic",
                                              new SendNotificationEvent
                                              {
                                                  Id = newNotification.Id,
                                                  NameSpaceId = nameSpaceId,
                                                  Type = newNotification.Type,
                                                  Title = newNotification.Title,
                                                  Summary = newNotification.Summary,
                                                  Date = newNotification.CreateDate,
                                                  Topics = newNotification.To?.Topics,
                                                  Users = newNotification.To?.Users,
                                                  Thumbnail = newNotification.Thumbnail,
                                                  Properties = req.Properties
                                              },
                                              "notification.created");

            return new SuccessResult("Notification is created");
        }
        public async Task<IDataResult<PagedDataResponse<NotificationDto.List>>> GetListAsync(Guid nameSpaceId, Guid userId, NotificationQuery query)
        {
            var userTopics = await UserTopicRepository.GetProjectedListAsync(s => s.Topic.Slug,
                                                                             x => x.UserId == userId);
            var userNotifications = await UserNotificationRepository.GetListAsync(x => x.UserId == userId);
            var readNotificationIds = userNotifications.Select(un => un.NotificationId).ToList();

            var notifications = await NotificationRepository.GetPagedAsync(query.Page,
                                                                           query.Size,
                                                                           x => x.NameSpace == nameSpaceId &&
                                                                                x.CreateDate <= query.LastDate &&
                                                                               (query.IsRead ? readNotificationIds.Contains(x.Id) : !readNotificationIds.Contains(x.Id)) &&
                                                                               (x.To == null || (x.To != null && (x.To.Topics.Any(o => userTopics.Contains(o)) || x.To.Users.Contains(userId)))),
                                                                           Builders<Notification>.Sort.Descending(x => x.CreateDate));

            var mappedList = _mapper.Map<List<NotificationDto.List>>(notifications.Data);
            mappedList.ForEach(x =>
            {
                if (userNotifications.Any(un => un.NotificationId == x.Id))
                    x.IsRead = true;
                else
                    x.IsRead = false;
            });

            var result = new PagedDataResponse<NotificationDto.List>
            {
                Data = mappedList,
                TotalCount = notifications.TotalCount
            };

            return new SuccessDataResult<PagedDataResponse<NotificationDto.List>>(result);
        }
        public async Task<IDataResult<NotificationDto.Detail>> GetDetailAsync(Guid nameSpaceId, Guid userId, Guid id)
        {
            var isRead = await UserNotificationRepository.ExistsAsync(f => f.NotificationId == id &&
                                                                           f.UserId == userId);


            var notification = await NotificationRepository.GetByIdAsync(id);
            var result = new NotificationDto.Detail
            {
                Id = notification.Id,
                Type = EnumHelper.ToEnumString<NotificationType>(notification.Type),
                ContentType = EnumHelper.ToEnumString<NotificationContentType>(notification.ContentType),
                Title = notification.Title,
                Summary = notification.Summary,
                Content = notification.Content,
                IsRead = isRead,
                CreateDate = notification.CreateDate,
                Properties = notification.Properties?.ToDictionary(),
                Thumbnail = notification.Thumbnail
            };

            return new SuccessDataResult<NotificationDto.Detail>(result);
        }
    }
}
