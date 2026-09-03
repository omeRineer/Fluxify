using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Hubs;
using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Events;
using Fluxify.Application.Models.Hub;
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
    public class PushSocketNotificationWorker : ConsumerWorker<SendNotificationEvent>
    {
        protected override string QueueName => "notification-socket";
        protected override int RetryCount => 3;

        private readonly IHubContext<PresenceHub> _hubContext;
        readonly HubConnectionManager<PresenceHubUser> _connectionManager;

        public PushSocketNotificationWorker(IChannel channel, IHubContext<PresenceHub> hubContext, HubConnectionManager<PresenceHubUser> connectionManager)
            : base(channel)
        {
            _hubContext = hubContext;
            _connectionManager = connectionManager;
        }

        protected override async Task HandleAsync(SendNotificationEvent message, CancellationToken cancellationToken)
        {
            var targets = _connectionManager.GetAllConnections(message.NameSpaceId).AsEnumerable();

            if (message.Topics != null || message.Users != null)
                targets = targets.Where(f =>
                (
                     (f.Topics != null && message.Topics != null && f.Topics.Any(topic => message.Topics.Contains(topic))) ||
                     (message.Users != null && message.Users.Contains(f.Id))
                ));

            var targetConnections = targets.SelectMany(s => s.Connections).Select(s => s.Key);

            await _hubContext.Clients.Clients(targetConnections)
                                     .SendAsync(HubEvents.PresenceHub.ReceiveNotification,
                                     new NotificationPayload
                                     {
                                         Id = message.Id,
                                         Date = message.Date,
                                         Type = EnumHelper.ToEnumString<NotificationType>(message.Type),
                                         Title = message.Title,
                                         Summary = message.Summary,
                                         Url = message.Url,
                                         Thumbnail = message.Thumbnail
                                     });

            //await _hubContext.Clients.Groups(message.Topics.Select(s => $"{message.NameSpaceId}:{s}"))
            //                         .SendAsync(HubEvents.PresenceHub.ReceiveNotification,
            //                         new NotificationPayload
            //                         {
            //                             Id = message.Id,
            //                             Date = message.Date,
            //                             Level = EnumHelper.ToEnumString<NotificationLevelType>(message.Level),
            //                             Title = message.Title,
            //                             Summary = message.Summary,
            //                             Url = message.Url,
            //                             Thumbnail = message.Thumbnail
            //                         });
        }
    }
}
