using Fluxify.Application.Hubs;
using Fluxify.Application.Models.Client;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services
{
    public class NotificationService : INotificationService
    {
        readonly INotificationRepository NotificationRepository;
        readonly ISubscriberRepository SubscriberRepository;
        readonly RabbitMqClientService RabbitMqClient;
        readonly CurrentUser CurrentUser;
        public NotificationService(INotificationRepository notifications, CurrentUser currentUser, ISubscriberRepository subscriberRepository, RabbitMqClientService rabbitMqClientService)
        {
            NotificationRepository = notifications;
            CurrentUser = currentUser;
            CurrentUser = currentUser;
            SubscriberRepository = subscriberRepository;
            RabbitMqClient = rabbitMqClientService;
        }

        public async Task<IResult> CreateAsync(CreateNotificationRequest req)
        {
            var ticketId = Guid.NewGuid();
            var newNotification = new Notification
            {
                Id = ticketId,
                NameSpace = CurrentUser.NameSpace,
                Level = req.Level.ToLower(new CultureInfo("en-US")),
                Sender = req.Sender ?? CurrentUser.Key,
                Title = req.Title,
                Content = req.Content,
                ContentType = req.ContentType.ToLower(new CultureInfo("en-US")),
                Targets = req.Targets,
                Topics = req.Topics,
                CreateDate = DateTime.Now,
                Custom = req.Custom
            };
            await NotificationRepository.AddAsync(newNotification);

            await RabbitMqClient.PushAsync("Notification_Queue", ticketId);

            return new SuccessResult("Notification creating is successful");
        }

        public async Task<IDataResult<GetNotificationDetailResponse>> GetDetailAsync(Guid id)
        {
            var notification = await NotificationRepository.GetSingleAsync(f => f.Id == id);

            if (notification.ReadBy == null)
            {
                notification.ReadBy = new List<string> { CurrentUser.Key };
                await NotificationRepository.UpdateAsync(notification);
            }
            else if (!notification.ReadBy.Contains(CurrentUser.Key))
            {
                notification.ReadBy.Add(CurrentUser.Key);
                await NotificationRepository.UpdateAsync(notification);
            }

            var result = new GetNotificationDetailResponse
            {
                Id = notification.Id,
                Level = notification.Level,
                ContentType = notification.ContentType,
                Sender = notification.Sender,
                Title = notification.Title,
                Content = notification.Content,
                IsRead = notification.ReadBy.Contains(CurrentUser.Key),
                CreateDate = notification.CreateDate,
                Topics = notification.Topics,
                Custom = notification.Custom
            };

            return new SuccessDataResult<GetNotificationDetailResponse>(result);
        }

        public async Task<IDataResult<GetNotificationsResponse>> GetListAsync()
        {
            var notifications = await NotificationRepository.GetListAsync(f => (f.NameSpace == CurrentUser.NameSpace) &&
                                                                               (f.Sender == CurrentUser.Key ||
                                                                               (f.Targets == null || f.Targets.Contains(CurrentUser.Key))));

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
                    IsRead = s.ReadBy != null ? s.ReadBy.Contains(CurrentUser.Key)
                                              : false,
                    Custom = s.Custom
                }).ToList()
            };

            return new SuccessDataResult<GetNotificationsResponse>(result);
        }
    }
}
