using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Hubs;
using Fluxify.Application.Models.Queue;
using Fluxify.Application.Models.SignalR;
using Fluxify.Application.Services.Interfaces;
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

        public NotificationWorker(IChannel channel, IConnection connection, IServiceScopeFactory scopeFactory, IHubContext<NotificationHub> hubContext)
            : base(channel, connection, "Notification_Queue", "Notification_Worker", false)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
        }

        protected override async Task HandleAsync(object model, BasicDeliverEventArgs args)
        {
            using var scope = _scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var ticketId = GetMessage<Guid>(args);

            var notificationResult = await notificationService.GetDetailAsync(ticketId);

            foreach (var topic in notificationResult?.Data?.Topics)
                _hubContext.Clients.Group(topic)?.SendAsync("ReceiveNotification", new PushToClientNotificationModel
                {
                    Id = notificationResult.Data.Id,
                    ContentType = notificationResult.Data.ContentType,
                    CreateDate = notificationResult.Data.CreateDate,
                    Custom = notificationResult.Data.Custom,
                    IsRead = false,
                    Level = notificationResult.Data.Level,
                    Sender = notificationResult.Data.Sender,
                    Title = notificationResult.Data.Title
                });
        }
    }
}
