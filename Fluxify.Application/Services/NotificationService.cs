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

namespace Fluxify.Application.Services
{

    public class NotificationService : INotificationService
    {
        readonly INotificationRepository NotificationRepository;
        readonly IUserRepository UserRepository;
        readonly ITopicRepository TopicRepository;
        readonly IUserNotificationRepository UserNotificationRepository;
        readonly IUserTopicRepository UserTopicRepository;
        readonly RabbitMqClientService RabbitMqClient;
        readonly IMapper _mapper;
        public NotificationService(INotificationRepository notifications, RabbitMqClientService rabbitMqClientService, IUserNotificationRepository userNotificationRepository, ITopicRepository topicRepository, IMapper mapper, IUserTopicRepository userTopicRepository)
        {
            NotificationRepository = notifications;
            RabbitMqClient = rabbitMqClientService;
            UserNotificationRepository = userNotificationRepository;
            TopicRepository = topicRepository;
            _mapper = mapper;
            UserTopicRepository = userTopicRepository;
        }

        public async Task<IResult> CreateAsync(Guid nameSpaceId, Guid userId, CreateNotification.Request req)
        {
            var ticketId = Guid.NewGuid();
            var newNotification = new Notification
            {
                Id = ticketId,
                NameSpace = nameSpaceId,
                Sender = userId,
                Level = (int)EnumHelper.Parse<NotificationLevelType>(req.Level),
                Title = req.Title,
                Summary = req.Summary,
                Content = req.Content,
                ContentType = (int)EnumHelper.Parse<NotificationContentType>(req.ContentType),
                CreateDate = DateTime.Now,
                Properties = req.Properties
            };

            if (req.Topics != null && req.Topics.Any())
            {
                var topics = await TopicRepository.GetListAsync(req.Topics);
                newNotification.Topics = topics.Select(x => x.Id)
                                               .Where(f=> req.Topics.Contains(f))
                                               .ToList();
            }

            await NotificationRepository.AddAsync(newNotification);
            await RabbitMqClient.PushAsync(Queues.FirebaseNotificationQueue, ticketId);

            return new SuccessResult("Notification is created");
        }
        public async Task<IDataResult<DataListResponse<NotificationDto.List>>> GetListAsync(Guid userId)
        {
            var userTopics = await UserTopicRepository.GetListAsync(x => x.UserId == userId);
            var topics = await TopicRepository.GetListAsync(x => userTopics.Select(ut => ut.TopicId).Contains(x.Id));
            var userNotifications = await UserNotificationRepository.GetListAsync(x => x.UserId == userId);

            if (userNotifications.Count == 0)
                return new SuccessDataResult<DataListResponse<NotificationDto.List>>(
                    DataListResponse<NotificationDto.List>.Load([]));

            var notificationIds = userNotifications
                .Select(x => x.NotificationId)
                .Distinct()
                .ToList();

            var notifications = await NotificationRepository.GetListAsync(x => notificationIds.Contains(x.Id) ||
                                                                               x.Topics.Any(o=> topics.Select(s=> s.Id).Contains(o)));

            var mappedList = _mapper.Map<List<NotificationDto.List>>(notifications);
            mappedList.ForEach(x =>
            {
                if (userNotifications.Any(un => un.NotificationId == x.Id))
                    x.IsRead = true;
                else
                    x.IsRead = false;
            });

            var result = DataListResponse<NotificationDto.List>.Load(mappedList);

            return new SuccessDataResult<DataListResponse<NotificationDto.List>>(result);
        }
        public async Task<IDataResult<NotificationDto.Detail>> GetDetailAsync(Guid id, Guid userId)
        {
            var isRead = await UserNotificationRepository.IsExistAsync(f => f.NotificationId == id &&
                                                                                      f.UserId == userId);


            var notification = await NotificationRepository.GetSingleAsync(f => f.Id == id);
            var user = await UserRepository.GetSingleAsync(f=> f.Id == notification.Sender);
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
    }
}
