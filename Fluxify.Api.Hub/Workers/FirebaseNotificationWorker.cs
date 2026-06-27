using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Hubs;
using Fluxify.Application.Services.Interfaces;
using INF = Fluxify.Application.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Fluxify.Application.Repositories;

namespace Fluxify.Api.Hub.Workers
{
    public class FirebaseNotificationWorker : ConsumerWorker<Guid>
    {
        protected override string QueueName => "notification-firebase";

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


        protected async override Task HandleAsync(Guid message, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
            var fireBaseNotificationService = scope.ServiceProvider.GetRequiredService<INF.IPushNotificationService>();
            var userDeviceRepository = scope.ServiceProvider.GetRequiredService<IUserDeviceRepository>();
            var userTopicRepository = scope.ServiceProvider.GetRequiredService<IUserTopicRepository>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var notification = await notificationRepository.GetSingleAsync(f => f.Id == message);
            var connectedUsers = _connectionManager.GetAllConnections(notification.NameSpace);
            var targetUsers = await userTopicRepository.GetListAsync(f => notification.Topics.Contains(f.TopicId) && f.NameSpaceId == notification.NameSpace);
            var userDevices = await userDeviceRepository.GetListAsync(f => targetUsers.Select(s => s.UserId).Contains(f.UserId));
            var targetDevices = userDevices.Where(f => !connectedUsers.SelectMany(s => s.Connections).Select(s => s.Value).Select(s => s.Device).Contains(f.DeviceId));

            if (targetDevices != null && targetDevices.Any())
                await fireBaseNotificationService.SendNotificationAsync(targetDevices.Select(s => s.Token), notification.Title, notification.Summary);

        }
    }
}
