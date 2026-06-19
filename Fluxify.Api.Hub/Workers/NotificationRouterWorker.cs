
using Fluxify.Api.Hub.Workers.Main;
using Fluxify.Application.Infrastructure;
using Fluxify.Application.Utilities.Helpers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fluxify.Api.Hub.Workers
{
    public class NotificationRouterWorker : ConsumerWorker
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public NotificationRouterWorker(IChannel channel, IConnection connection, IServiceScopeFactory scopeFactory)
            : base(channel, connection, "Notification_Router_Queue", "Notification_Router_Worker", false)
        {
            _scopeFactory = scopeFactory;
        }

        protected async override Task HandleAsync(object model, BasicDeliverEventArgs args)
        {
            using var scope = _scopeFactory.CreateScope();
            var rabbitMqClient = scope.ServiceProvider.GetRequiredService<RabbitMqClientService>();

            var notificationId = GetMessage<Guid>(args);

            await rabbitMqClient.PushAsync("Notification_Queue", notificationId);
            await rabbitMqClient.PushAsync("Firebase_Notification_Queue", notificationId);
        }
    }
}
