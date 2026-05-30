using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Hubs;
using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Queue;
using Fluxify.Application.Models.SignalR;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Fluxify.Api.Hub.Workers
{
    public class NotificationWorker : ConsumerWorker
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<NotificationHub> _hubContext;
        readonly HubConnectionManager<NotificationHub> _connectionManager;

        public NotificationWorker(IChannel channel, IConnection connection, IServiceScopeFactory scopeFactory, IHubContext<NotificationHub> hubContext, HubConnectionManager<NotificationHub> connectionManager)
            : base(channel, connection, "Notification_Queue", "Notification_Worker", false)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _connectionManager = connectionManager;
        }

        protected override async Task HandleAsync(object model, BasicDeliverEventArgs args)
        {
            using var scope = _scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var userNotificationService = scope.ServiceProvider.GetRequiredService<IUserNotificationService>();
            var notificationId = GetMessage<Guid>(args);

            var notificationResult = await notificationService.GetAsync(notificationId);
            var nameSpaceUsers = _connectionManager.GetAllConnections(notificationResult.Data.NameSpace);

            if (nameSpaceUsers != null && nameSpaceUsers.Count > 0)
            {
                var userNotificationsResult = await userNotificationService.GetTargetsAsync(notificationId);
                var destinationUsers = nameSpaceUsers.Where(f => userNotificationsResult.Data.Any(x => x.UserId == f.Id));

                await _hubContext.Clients.Clients(destinationUsers.SelectMany(s => s.Connections)).SendAsync(HubEvents.NotificationHub.ReceiveNotification, new PushToClientNotificationModel
                {
                    Id = notificationResult.Data.Id,
                    ContentType = notificationResult.Data.ContentType,
                    CreateDate = notificationResult.Data.CreateDate,
                    Properties = notificationResult.Data.Properties,
                    Level = notificationResult.Data.Level,
                    Status = (int)NotificationStatus.Delivered,
                    Sender = notificationResult.Data.Sender,
                    Title = notificationResult.Data.Title
                });

                var destinationNotifications = userNotificationsResult.Data.Where(f => destinationUsers.Any(x => x.Id == f.UserId));
                await userNotificationService.MarkAsStatusAsync(NotificationStatus.Delivered, destinationNotifications.Select(s => s.NotificationId).ToArray());
            }
        }
    }
}
