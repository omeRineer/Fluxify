using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Hubs;
using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Queue;
using Fluxify.Application.Models.SignalR;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Helpers;
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
            var notificationId = GetMessage<Guid>(args);

            var notificationResult = await notificationService.GetAsync(notificationId);
            var nameSpaceUsers = _connectionManager.GetAllConnections(notificationResult.Data.NameSpace);

            if (nameSpaceUsers != null && nameSpaceUsers.Count > 0)
            {
                var destinationUsers = nameSpaceUsers.Where(f => notificationResult.Data.Topics.Any(item => f.Topics.Contains(item)));

                await _hubContext.Clients.Clients(destinationUsers.SelectMany(s => s.Connections).Select(s => s.Key)).SendAsync(HubEvents.NotificationHub.ReceiveNotification, new PushToClientNotificationModel
                {
                    Id = notificationResult.Data.Id,
                    ContentType = EnumHelper.ToEnumString<NotificationContentType>(notificationResult.Data.ContentType),
                    CreateDate = notificationResult.Data.CreateDate,
                    Properties = notificationResult.Data.Properties,
                    Level = EnumHelper.ToEnumString<NotificationLevelType>(notificationResult.Data.Level),
                    Status = EnumHelper.ToEnumString<NotificationStatus>((int)NotificationStatus.Delivered),
                    Sender = nameSpaceUsers.Single(f => f.Id == notificationResult.Data.Sender).Key,
                    Title = notificationResult.Data.Title,
                    Summary = notificationResult.Data.Summary
                });

            }
        }
    }
}
