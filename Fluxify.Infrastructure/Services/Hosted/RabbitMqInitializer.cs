using Fluxify.Configuration;
using Fluxify.Configuration.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
        private readonly RabbitMqOptions _options;

        public RabbitMqInitializer(IChannel channel)
        {
            _options = AppConfiguration.RabbitMqOptions;
            _channel = channel;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var exchange in _options.Exchanges)
            {
                await _channel.ExchangeDeclareAsync(
                    exchange: exchange.Name,
                    type: exchange.Type,
                    durable: exchange.Durable,
                    autoDelete: exchange.AutoDelete,
                    arguments: null,
                    cancellationToken: cancellationToken);
            }

            foreach (var queue in _options.Queues)
            {
                var exchange = $"{queue.Name}.main-dx";
                var routingKey = $"{queue.Name}.main-rk";
                Dictionary<string, object> queueArguments = new();


                if (queue.EnableRq)
                {
                    var retryExchange = $"{queue.Name}.retry-dx";
                    var retryQueue = $"{queue.Name}.retry";
                    var retryRoutingKey = $"{queue.Name}.retry-rk";

                    await _channel.ExchangeDeclareAsync(retryExchange, ExchangeType.Direct, true, false, cancellationToken: cancellationToken);

                    var retryArguments = new Dictionary<string, object>
                    {
                        { "x-dead-letter-exchange", exchange },      // Bekleme süresi bitince ana exchange'e geri dön
                        { "x-dead-letter-routing-key", routingKey }, // Ana routing key ile geri dön
                        { "x-message-ttl", 5000 }                          // Yeniden denemeler arası bekleme süresi (5 saniye - milisaniye cinsinden)
                    };

                    await _channel.QueueDeclareAsync(retryQueue, true, false, false, retryArguments, cancellationToken: cancellationToken);
                    await _channel.QueueBindAsync(retryQueue, retryExchange, retryRoutingKey, null, cancellationToken: cancellationToken);

                    queueArguments["x-dead-letter-exchange"] = retryExchange;
                    queueArguments["x-dead-letter-routing-key"] = retryRoutingKey;
                }

                await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct, true, false, cancellationToken: cancellationToken);
                await _channel.QueueDeclareAsync(
                    queue: queue.Name,
                    durable: queue.Durable,
                    exclusive: queue.Exclusive,
                    autoDelete: queue.AutoDelete,
                    arguments: queueArguments,
                    cancellationToken: cancellationToken);
                await _channel.QueueBindAsync(queue.Name, exchange, routingKey, null, cancellationToken: cancellationToken);

                if (queue.Bindings.Count > 0)
                    foreach(var binding in queue.Bindings)
                        await _channel.QueueBindAsync(queue.Name, binding.Value, binding.Key, null, cancellationToken: cancellationToken);

            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
