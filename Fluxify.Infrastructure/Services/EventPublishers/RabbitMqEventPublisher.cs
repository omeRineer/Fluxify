using System.Text;
using System.Text.Json;
using Fluxify.Application.Infrastructure;
using RabbitMQ.Client;

namespace Fluxify.Infrastructure.Services.EventPublishers
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly IChannel _channel;

        public RabbitMqEventPublisher(
            IChannel channel)
        {
            _channel = channel;
        }

        public async Task PublishAsync(
            string exchange,
            object body,
            string routingKey = "")
        {
            var bytes = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(body));

            await _channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                body: bytes);
        }
    }
}