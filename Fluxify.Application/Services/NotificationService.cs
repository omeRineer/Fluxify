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
        readonly IUserService UserService;
        readonly ITopicRepository TopicRepository;
        readonly IUserNotificationRepository UserNotificationRepository;
        readonly RabbitMqClientService RabbitMqClient;
        readonly IMapper _mapper;
        public NotificationService(INotificationRepository notifications, RabbitMqClientService rabbitMqClientService, IUserNotificationRepository userNotificationRepository, IUserService userService, ITopicRepository topicRepository, IMapper mapper)
        {
            NotificationRepository = notifications;
            RabbitMqClient = rabbitMqClientService;
            UserNotificationRepository = userNotificationRepository;
            UserService = userService;
            TopicRepository = topicRepository;
            _mapper = mapper;
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


            List<UserNotification>? notificationUsers = null;
            if (req.Topics != null && req.Topics.Any())
            {
                var topics = await TopicRepository.GetListAsync(req.Topics);
                var targetUsers = await UserService.GetListByTopicsAsync(newNotification.NameSpace, topics.Select(s => s.Id).ToArray());

                notificationUsers = targetUsers.Data.Items.Select(s => new UserNotification
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

            await RabbitMqClient.PushAsync(Queues.NotificationQueue, ticketId);

            return new SuccessResult("Notification is created");
        }
        public async Task<IDataResult<DataListResponse<NotificationDto.List>>> GetListAsync(Guid userId, NotificationStatus[]? statuses = null)
        {
            var statusValues = statuses?
                .Select(x => (int)x)
                .ToArray();

            var userNotifications = await UserNotificationRepository.GetListAsync(x =>
                x.UserId == userId &&
                (
                    statusValues == null ||
                    statusValues.Contains(x.Status)
                ));

            if (userNotifications.Count == 0)
            {
                return new SuccessDataResult<DataListResponse<NotificationDto.List>>(
                    DataListResponse<NotificationDto.List>.Load([]));
            }

            var notificationIds = userNotifications
                .Select(x => x.NotificationId)
                .Distinct()
                .ToList();

            var notifications = await NotificationRepository.GetListAsync(x =>
                notificationIds.Contains(x.Id));

            var statusLookup = userNotifications.ToDictionary(
                x => x.NotificationId,
                x => x.Status);

            var mappedList = _mapper.Map<List<NotificationDto.List>>(notifications);
            mappedList.ForEach(x =>
            {
                if (statusLookup.TryGetValue(x.Id, out var status))
                    x.Status = EnumHelper.ToEnumString<NotificationStatus>(status);
            });

            var result = DataListResponse<NotificationDto.List>.Load(mappedList);

            return new SuccessDataResult<DataListResponse<NotificationDto.List>>(result);
        }
        public async Task<IDataResult<NotificationDto.Detail>> GetDetailAsync(Guid id, Guid userId)
        {
            #region User Notification
            var userNotification = await UserNotificationRepository.GetFirstOrDefaultAsync(f => f.NotificationId == id &&
                                                                                                f.UserId == userId &&
                                                                                                (f.Status == (int)NotificationStatus.Delivered || f.Status == (int)NotificationStatus.Read));
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(userNotification, "Notification is not found")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<NotificationDto.Detail>(message: businessResult.Message);
            #endregion

            var notification = await NotificationRepository.GetSingleAsync(f => f.Id == id);
            var user = await UserService.GetAsync(notification.Sender);
            var result = new NotificationDto.Detail
            {
                Id = notification.Id,
                Level = EnumHelper.ToEnumString<NotificationLevelType>(notification.Level),
                ContentType = EnumHelper.ToEnumString<NotificationContentType>(notification.ContentType),
                Sender = user.Data.Key,
                Title = notification.Title,
                Summary = notification.Summary,
                Content = notification.Content,
                Status = EnumHelper.ToEnumString<NotificationStatus>(userNotification.Status),
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
