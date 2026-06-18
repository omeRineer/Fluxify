
using Fluxify.Api.Hub.Workers.Main;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fluxify.Api.Hub.Workers
{
    public class NotificationRouterWorker : ConsumerWorker
    {
        public NotificationRouterWorker(IChannel channel, IConnection connection) 
            : base(channel, connection, "Notification_Router_Queue", "Notification_Router_Worker", false)
        {
        }

        protected override Task HandleAsync(object model, BasicDeliverEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
