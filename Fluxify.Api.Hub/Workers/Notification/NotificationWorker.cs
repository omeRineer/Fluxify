using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Hubs;
using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Events;
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

namespace Fluxify.Api.Hub.Workers.Notification
{
    public class NotificationWorker : ConsumerWorker<SendNotificationEvent>
    {
        protected override string QueueName => "notification-socket";
        protected override int RetryCount => 3;

        private readonly IHubContext<PresenceHub> _hubContext;
        readonly HubConnectionManager<PresenceHub> _connectionManager;

        public NotificationWorker(IChannel channel, IHubContext<PresenceHub> hubContext, HubConnectionManager<PresenceHub> connectionManager)
            : base(channel)
        {
            _hubContext = hubContext;
            _connectionManager = connectionManager;
        }

        protected override async Task HandleAsync(SendNotificationEvent message, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.Groups(message.Topics.Select(s => $"{message.NameSpaceId}:{s}"))
                                     .SendAsync(HubEvents.NotificationHub.ReceiveNotification,
                                     new PushToClientNotificationModel
                                     {
                                         Id = message.Id,
                                         Date = message.Date,
                                         Level = EnumHelper.ToEnumString<NotificationLevelType>(message.Level),
                                         Title = message.Title,
                                         Summary = message.Summary,
                                         Url = message.Url,
                                         Thumbnail = message.Thumbnail
                                     });
        }
    }
}
