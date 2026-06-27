using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Infrastructure.Services.Hosted
{
    public class RabbitMqInitializer : IHostedService
    {
        readonly IChannel _channel;

        public RabbitMqInitializer(IChannel channel)
        {
            _channel = channel;
        }

        private record Topology(string Exchange, string ExchangeType, string Queue, string RoutingKey);

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Topology[] topologies = new[]
            {
                new Topology("notification-direct", "direct", "notification-socket", "notification.created"),
                new Topology("notification-direct", "direct", "notification-firebase", "notification.created")
            };

            foreach (Topology topology in topologies)
            {
                await _channel.ExchangeDeclareAsync(exchange: topology.Exchange,
                                                    type: topology.ExchangeType,
                                                    durable: true,
                                                    autoDelete: false);

                await _channel.QueueDeclareAsync(queue: topology.Queue,
                                                 durable: true,
                                                 exclusive: false,
                                                 autoDelete: false);

                await _channel.QueueBindAsync(queue: topology.Queue,
                                              exchange: topology.Exchange,
                                              routingKey: topology.RoutingKey);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
