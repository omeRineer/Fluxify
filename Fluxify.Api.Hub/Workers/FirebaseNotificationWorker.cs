using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Hubs;
using Fluxify.Application.Services.Interfaces;
using INF =Fluxify.Application.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fluxify.Api.Hub.Workers
{
    public class FirebaseNotificationWorker : ConsumerWorker
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<NotificationHub> _hubContext;
        readonly HubConnectionManager<NotificationHub> _connectionManager;

        public FirebaseNotificationWorker(IChannel channel, IConnection connection, IServiceScopeFactory scopeFactory, IHubContext<NotificationHub> hubContext, HubConnectionManager<NotificationHub> connectionManager)
            : base(channel, connection, "Firebase_Notification_Queue", "Firebase_Notification_Worker", false)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _connectionManager = connectionManager;
        }

        protected async override Task HandleAsync(object model, BasicDeliverEventArgs args)
        {
            using var scope = _scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var fireBaseNotificationService = scope.ServiceProvider.GetRequiredService<INF.INotificationService>();
            var userNotificationService = scope.ServiceProvider.GetRequiredService<IUserNotificationService>();
            var userDeviceService = scope.ServiceProvider.GetRequiredService<IUserDeviceService>();
            var notificationId = GetMessage<Guid>(args);

            var notificationResult = await notificationService.GetAsync(notificationId);
            var nameSpaceUsers = _connectionManager.GetAllConnections(notificationResult.Data.NameSpace);
            var userNotificationsResult = await userNotificationService.GetTargetsAsync(notificationId);
            var userDevices = await userDeviceService.GetListAsync(userNotificationsResult.Data.Select(s => s.UserId).ToArray());
            var targetDevices = userDevices.Data.Where(f => !nameSpaceUsers.SelectMany(s => s.Connections).Select(s => s.Value).Select(s => s.Device).Contains(f.DeviceId));

            await fireBaseNotificationService.SendNotificationAsync(targetDevices.Select(s => s.DeviceId), notificationResult.Data.Title, notificationResult.Data.Summary);
        }
    }
}
