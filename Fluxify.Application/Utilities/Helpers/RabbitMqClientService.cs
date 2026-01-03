using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fluxify.Application.Utilities.Helpers
{
    public class RabbitMqClientService
    {
        readonly IChannel Channel;

        public RabbitMqClientService(IChannel channel)
        {
            Channel = channel;
        }


        public async Task PushAsync(string queue, object body, string exchange = "")
        {
            await Channel.QueueDeclareAsync(queue,
                                      durable: true,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);


            var serializedMessage = JsonSerializer.Serialize(body);
            var bytes = Encoding.UTF8.GetBytes(serializedMessage);

            await Channel.BasicPublishAsync(exchange, queue, bytes);
        }
    }
}
