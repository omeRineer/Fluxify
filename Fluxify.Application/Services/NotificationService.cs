using Fluxify.Application.Hubs;
using Fluxify.Application.Models.Client;
using Fluxify.Application.Models.Enums;
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

namespace Fluxify.Application.Services
{
    public class NotificationService : INotificationService
    {
        readonly INotificationRepository NotificationRepository;
        readonly IUserService UserService;
        readonly IChannelService ChannelService;
        readonly ITopicService TopicService;
        readonly IUserNotificationRepository UserNotificationRepository;
        readonly RabbitMqClientService RabbitMqClient;
        readonly CurrentUser CurrentUser;
        public NotificationService(INotificationRepository notifications, CurrentUser currentUser, RabbitMqClientService rabbitMqClientService, IUserNotificationRepository userNotificationRepository, IUserService userService, IChannelService channelService, ITopicService topicService)
        {
            NotificationRepository = notifications;
            CurrentUser = currentUser;
            CurrentUser = currentUser;
            RabbitMqClient = rabbitMqClientService;
            UserNotificationRepository = userNotificationRepository;
            UserService = userService;
            ChannelService = channelService;
            TopicService = topicService;
        }

        public async Task<IResult> CreateAsync(CreateNotificationRequest req)
        {
            var ticketId = Guid.NewGuid();
            var newNotification = new Notification
            {
                Id = ticketId,
                NameSpace = CurrentUser.NameSpaceId,
                Level = req.Level ?? (int)NotificationLevelType.Info,
                Sender = CurrentUser.Key,
                Title = req.Title,
                Content = req.Content,
                ContentType = req.ContentType ?? (int)NotificationContentType.Text,
                CreateDate = DateTime.Now,
                Properties = req.Properties
            };


            List<UserNotification>? notificationUsers = null;
            if (req.Topics != null && req.Topics.Any())
            {
                var topics = await TopicService.GetListByKey(req.Topics);
                var targetUsers = await UserService.GetListByTopicsAsync(newNotification.NameSpace, topics.Data.Select(s => s.Id).ToArray());

                notificationUsers = targetUsers.Data.Select(s => new UserNotification
                {
                    NotificationId = ticketId,
                    UserId = s.Id,
                    Status = (int)NotificationStatus.Pending,
                    CreateDate = newNotification.CreateDate
                }).ToList();
            }

            await NotificationRepository.AddAsync(newNotification);

            if (notificationUsers != null && notificationUsers.Any())
                await UserNotificationRepository.AddRangeAsync(notificationUsers);

            await RabbitMqClient.PushAsync(Queue.NotificationQueue, ticketId);

            return new SuccessResult("Notification is created");
        }

        public async Task<IDataResult<Notification>> GetAsync(Guid id)
            => new SuccessDataResult<Notification>(await NotificationRepository.GetSingleAsync(f => f.Id == id));

        public async Task<IDataResult<GetNotificationDetailResponse>> GetDetailAsync(Guid id)
        {
            #region Notification
            var notification = await NotificationRepository.GetSingleAsync(f => f.Id == id && f.NameSpace == CurrentUser.NameSpaceId);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(notification, "Notification is not found")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<GetNotificationDetailResponse>(message: businessResult.Message);
            #endregion

            #region User Notification
            var userNotification = await UserNotificationRepository.GetFirstOrDefaultAsync(f => f.NotificationId == id &&
                                                                                                f.UserId == CurrentUser.NameIdentifier &&
                                                                                                (f.Status == (int)NotificationStatus.Delivered || f.Status == (int)NotificationStatus.Read));
            businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(userNotification, "User notification is not found")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<GetNotificationDetailResponse>(message: businessResult.Message);
            #endregion

            var result = new GetNotificationDetailResponse
            {
                Id = notification.Id,
                Level = notification.Level,
                ContentType = notification.ContentType,
                Sender = notification.Sender,
                Title = notification.Title,
                Content = notification.Content,
                Status = userNotification.Status,
                CreateDate = notification.CreateDate,
                Topics = notification.Topics,
                Properties = notification.Properties
            };

            return new SuccessDataResult<GetNotificationDetailResponse>(result);
        }
        public async Task<IDataResult<GetNotificationsResponse>> GetListAsync()
        {
            var userNotifications = await UserNotificationRepository.GetListAsync(f => f.UserId == CurrentUser.NameIdentifier &&
                                                                                       (f.Status == (int)NotificationStatus.Delivered || f.Status == (int)NotificationStatus.Read));
            var notifications = await NotificationRepository.GetListAsync(f => f.NameSpace == CurrentUser.NameSpaceId &&
                                                                               userNotifications.Select(s => s.NotificationId).Contains(f.Id));

            var result = new GetNotificationsResponse
            {
                Notifications = notifications.Select(s => new NotificationResponse
                {
                    Id = s.Id,
                    Level = s.Level,
                    ContentType = s.ContentType,
                    Sender = s.Sender,
                    Title = s.Title,
                    CreateDate = s.CreateDate,
                    Status = userNotifications.First(f => f.NotificationId == s.Id).Status,
                    Properties = s.Properties
                }).ToList()
            };

            return new SuccessDataResult<GetNotificationsResponse>(result);
        }
        public async Task<IDataResult<GetNotificationsResponse>> GetAndMarkPendingAsync(Guid userId, Guid nameSpace)
        {
            var userNotifications = await UserNotificationRepository.GetListAsync(
                f => f.UserId == userId &&
                     f.Status == (int)NotificationStatus.Pending
            );

            if (!userNotifications.Any())
                return new ErrorDataResult<GetNotificationsResponse>();

            var notificationIds = userNotifications
                .Select(s => s.NotificationId)
                .ToList();

            var notifications = await NotificationRepository.GetListAsync(
                f => f.NameSpace == nameSpace &&
                     notificationIds.Contains(f.Id)
            );

            await UserNotificationRepository.MarkAsStatusByNotificationAsync(
                NotificationStatus.Delivered,
                userId,
                notificationIds.ToArray()
            );

            var result = new GetNotificationsResponse
            {
                Notifications = notifications.Select(n =>
                {
                    return new NotificationResponse
                    {
                        Id = n.Id,
                        Level = n.Level,
                        ContentType = n.ContentType,
                        Sender = n.Sender,
                        Title = n.Title,
                        CreateDate = n.CreateDate,
                        Status = (int)NotificationStatus.Delivered,
                        Properties = n.Properties
                    };
                }).ToList()
            };

            return new SuccessDataResult<GetNotificationsResponse>(result);
        }

        public async Task<IDataResult<List<UserNotification>>> GetTargetsAsync(Guid notificationId)
        {
            var userNotifications = await UserNotificationRepository.GetListAsync(f => f.NotificationId == notificationId);

            return new SuccessDataResult<List<UserNotification>>(userNotifications);
        }

        public async Task<IResult> MarkAsStatusAsync(NotificationStatus status, params Guid[] ids)
        {
            await UserNotificationRepository.MarkAsStatusAsync(status, ids);

            return new SuccessResult();
        }

        public async Task<IResult> MarkAsStatusByNotificationAsync(NotificationStatus status, Guid? userId = null, params Guid[] notificationIds)
        {
            await UserNotificationRepository.MarkAsStatusByNotificationAsync(status, userId, notificationIds);

            return new SuccessResult();
        }
    }
}
