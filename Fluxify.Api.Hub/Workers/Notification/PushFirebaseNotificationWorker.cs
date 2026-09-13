using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Hubs;
using Fluxify.Application.Services.Interfaces;
using INF = Fluxify.Application.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Fluxify.Application.Repositories;
using Fluxify.Application.Models.Events;
using Fluxify.Application.Models.Hub;
using System.Linq;

namespace Fluxify.Api.Hub.Workers.Notification
{
    public class PushFirebaseNotificationWorker : ConsumerWorker<SendNotificationEvent>
    {
        protected override string QueueName => "notification-firebase";
        protected override int RetryCount => 0;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<PresenceHub> _hubContext;
        readonly HubConnectionManager<PresenceHubUser> _connectionManager;

        public PushFirebaseNotificationWorker(IChannel channel, IServiceScopeFactory scopeFactory, IHubContext<PresenceHub> hubContext, HubConnectionManager<PresenceHubUser> connectionManager)
            : base(channel)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _connectionManager = connectionManager;
        }


        protected async override Task HandleAsync(SendNotificationEvent message, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var fireBaseNotificationService = scope.ServiceProvider.GetRequiredService<INF.IPushNotificationService>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var targets = await userRepository.GetProjectedListAsync(
                s => new { s.Id, s.Devices },
                f => f.NameSpaceId == message.NameSpaceId &&
                     (
                         (f.Topics != null && message.Topics != null && f.Topics.Any(topic => message.Topics.Contains(topic.Topic.Slug))) ||
                         (message.Users != null && message.Users.Contains(f.Id))
                     )
            );
            var devices = targets.SelectMany(s => s.Devices);

            if (devices != null && devices.Any())
                await fireBaseNotificationService.SendNotificationAsync(devices?.Select(s=> s.Token),
                                                                        message.Title,
                                                                        message.Summary,
                                                                        message.Thumbnail,
                                                                        message.Properties?.ToDictionary(k=> k.Key?.ToString(), v=> v.Value?.ToString()));

        }
    }
}
