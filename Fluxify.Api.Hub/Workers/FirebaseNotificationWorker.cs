using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Hubs;
using Fluxify.Application.Services.Interfaces;
using INF = Fluxify.Application.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Fluxify.Application.Repositories;
using Fluxify.Application.Models.Queue;

namespace Fluxify.Api.Hub.Workers
{
    public class FirebaseNotificationWorker : ConsumerWorker<NotificationQueueModel>
    {
        protected override string QueueName => "notification-firebase";
        protected override int RetryCount => 0;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<NotificationHub> _hubContext;
        readonly HubConnectionManager<NotificationHub> _connectionManager;

        public FirebaseNotificationWorker(IChannel channel, IServiceScopeFactory scopeFactory, IHubContext<NotificationHub> hubContext, HubConnectionManager<NotificationHub> connectionManager)
            : base(channel)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _connectionManager = connectionManager;
        }


        protected async override Task HandleAsync(NotificationQueueModel message, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var fireBaseNotificationService = scope.ServiceProvider.GetRequiredService<INF.IPushNotificationService>();
            var userDeviceRepository = scope.ServiceProvider.GetRequiredService<IUserDeviceRepository>();
            var userTopicRepository = scope.ServiceProvider.GetRequiredService<IUserTopicRepository>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var targetUsers = await userTopicRepository.GetListAsync(f => message.Topics.Contains(f.TopicId) && f.NameSpaceId == message.NameSpaceId);
            var targetDevices = await userDeviceRepository.GetListAsync(f => targetUsers.Select(s => s.UserId).Contains(f.UserId));

            if (targetDevices != null && targetDevices.Any())
                await fireBaseNotificationService.SendNotificationAsync(targetDevices.Select(s => s.Token), 
                                                                        message.Title, 
                                                                        message.Summary,
                                                                        new()
                                                                        {
                                                                            { "url", message.Url }
                                                                        });

        }
    }
}
