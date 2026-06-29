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
    public class NotificationWorker : ConsumerWorker<Guid>
    {
        protected override string QueueName => "notification-socket";
        protected override int RetryCount => 3;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<NotificationHub> _hubContext;
        readonly HubConnectionManager<NotificationHub> _connectionManager;

        public NotificationWorker(IChannel channel, IServiceScopeFactory scopeFactory, IHubContext<NotificationHub> hubContext, HubConnectionManager<NotificationHub> connectionManager)
            : base(channel)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _connectionManager = connectionManager;
        }

        protected override async Task HandleAsync(Guid message, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            var notificationResult = await notificationService.GetAsync(message);
            var sender = await userService.GetAsync(notificationResult.Data.Sender);

            

            await _hubContext.Clients.Groups(notificationResult.Data.Topics.Select(s => $"{notificationResult.Data.NameSpace}:{s}"))
                                     .SendAsync(HubEvents.NotificationHub.ReceiveNotification,
                                     new PushToClientNotificationModel
                                     {
                                         Id = notificationResult.Data.Id,
                                         Date = notificationResult.Data.CreateDate,
                                         Properties = notificationResult.Data.Properties,
                                         Level = EnumHelper.ToEnumString<NotificationLevelType>(notificationResult.Data.Level),
                                         Sender = sender.Data.Key,
                                         Title = notificationResult.Data.Title,
                                         Summary = notificationResult.Data.Summary
                                     });
        }
    }
}
