using Fluxify.Application.Hubs;
using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Dto.Notification;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;
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
using Fluxify.Application.Models.Queue;

namespace Fluxify.Application.Services
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
        public NotificationService(INotificationRepository notifications, IUserNotificationRepository userNotificationRepository, ITopicRepository topicRepository, IMapper mapper, IUserTopicRepository userTopicRepository, IEventPublisher eventPublisher)
        {
            NotificationRepository = notifications;
            UserNotificationRepository = userNotificationRepository;
            TopicRepository = topicRepository;
            _mapper = mapper;
            UserTopicRepository = userTopicRepository;
            EventPublisher = eventPublisher;
        }

        public async Task<IResult> CreateAsync(Guid nameSpaceId, Guid userId, CreateNotification.Request req)
        {
            var newNotification = _mapper.Map<Notification>(req);
            newNotification.NameSpace = nameSpaceId;
            newNotification.Sender = userId;

            var topics = await TopicRepository.GetListAsync(req.Topics);
            newNotification.Topics = topics?.Select(s => s.Id)?.ToList() ?? new();

            await NotificationRepository.AddAsync(newNotification);
            await EventPublisher.PublishAsync("notification-topic",
                                              new NotificationQueueModel
                                              {
                                                  Id = newNotification.Id,
                                                  NameSpaceId = nameSpaceId,
                                                  Level = newNotification.Level,
                                                  Title = newNotification.Title,
                                                  Summary = newNotification.Summary,
                                                  Date = newNotification.CreateDate,
                                                  Topics = newNotification.Topics,
                                                  Url = req.Url
                                              },
                                              "notification.created");

            return new SuccessResult("Notification is created");
        }
        public async Task<IDataResult<DataListResponse<NotificationDto.List>>> GetListAsync(Guid userId)
        {
            var userTopics = await UserTopicRepository.GetListAsync(x => x.UserId == userId);
            var userNotifications = await UserNotificationRepository.GetListAsync(x => x.UserId == userId);

            var notifications = await NotificationRepository.GetListAsync(x => x.Topics.Any(o => userTopics.Select(s => s.TopicId).Contains(o)));

            var mappedList = _mapper.Map<List<NotificationDto.List>>(notifications);
            mappedList.ForEach(x =>
            {
                if (userNotifications.Any(un => un.NotificationId == x.Id))
                    x.IsRead = true;
                else
                    x.IsRead = false;
            });

            var result = DataListResponse<NotificationDto.List>.Load(mappedList.OrderBy(o => o.IsRead).ToList());

            return new SuccessDataResult<DataListResponse<NotificationDto.List>>(result);
        }
        public async Task<IDataResult<NotificationDto.Detail>> GetDetailAsync(Guid id, Guid userId)
        {
            var isRead = await UserNotificationRepository.IsExistAsync(f => f.NotificationId == id &&
                                                                                      f.UserId == userId);


            var notification = await NotificationRepository.GetSingleAsync(f => f.Id == id);
            var user = await UserRepository.GetSingleAsync(f => f.Id == notification.Sender);
            var result = new NotificationDto.Detail
            {
                Id = notification.Id,
                Level = EnumHelper.ToEnumString<NotificationLevelType>(notification.Level),
                ContentType = EnumHelper.ToEnumString<NotificationContentType>(notification.ContentType),
                Sender = user.Key,
                Title = notification.Title,
                Summary = notification.Summary,
                Content = notification.Content,
                IsRead = isRead,
                CreateDate = notification.CreateDate,
                Topics = notification.Topics,
                Properties = notification.Properties
            };

            return new SuccessDataResult<NotificationDto.Detail>(result);
        }

        public async Task<IDataResult<Notification>> GetAsync(Guid id)
        {
            var notification = await NotificationRepository.GetSingleAsync(f => f.Id == id);

            return new SuccessDataResult<Notification>(notification);
        }

        public async Task<IResult> FireAsync(Guid nameSpaceId, FireNotification.Request req)
        {
            await EventPublisher.PublishAsync("notification-topic",
                                              new NotificationQueueModel
                                              {
                                                  NameSpaceId = nameSpaceId,
                                                  Level = (int)EnumHelper.Parse(req.Level, NotificationLevelType.Info),
                                                  Title = req.Title,
                                                  Summary = req.Summary,
                                                  Date = DateTime.UtcNow,
                                                  Topics = req.Topics,
                                                  ImageUrl = req.ImageUrl,
                                                  Url = req.Url
                                              },
                                              "notification.created");

            return new SuccessResult("Notification is fired");
        }
    }
}
