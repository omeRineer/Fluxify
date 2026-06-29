using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Fluxify.Api.Hub.Workers.Main;

public abstract class ConsumerWorker<TMessage> : BackgroundService
{
    protected readonly IChannel Channel;

    protected abstract string QueueName { get; }
    protected abstract int RetryCount { get; }

    protected ConsumerWorker(IChannel channel)
    {
        Channel = channel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(Channel);

        consumer.ReceivedAsync += async (_, args) =>
        {
            try
            {
                var message = Deserialize(args);

                if (message is null)
                {
                    await Channel.BasicNackAsync(args.DeliveryTag, false, false);
                    return;
                }

                await HandleAsync(message, stoppingToken);

                await Channel.BasicAckAsync(args.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                await OnErrorAsync(ex, args);

                var currentRetryCount = GetRetryCount(args);

                if (currentRetryCount < RetryCount)
                    await Channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: false);
                else
                {
                    await OnFailAsync(ex, args);

                    await Channel.BasicAckAsync(args.DeliveryTag, multiple: false);
                }
            }
        };

        await Channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);
    }

    protected virtual Task OnErrorAsync(
        Exception exception,
        BasicDeliverEventArgs args)
        => Task.CompletedTask;

    protected virtual Task OnFailAsync(
        Exception exception,
        BasicDeliverEventArgs args)
        => Task.CompletedTask;

    protected abstract Task HandleAsync(
        TMessage message,
        CancellationToken cancellationToken);

    private static TMessage? Deserialize(BasicDeliverEventArgs args)
    {
        var json = Encoding.UTF8.GetString(args.Body.ToArray());
        return JsonSerializer.Deserialize<TMessage>(json);
    }

    private static long GetRetryCount(BasicDeliverEventArgs args)
    {
        if (args.BasicProperties.Headers == null || !args.BasicProperties.Headers.ContainsKey("x-death"))
            return 0;

        if (args.BasicProperties.Headers["x-death"] is List<object> deathList && deathList.Count > 0)
        {
            if (deathList[0] is Dictionary<string, object> deathProperties && deathProperties.ContainsKey("count"))
            {
                return Convert.ToInt64(deathProperties["count"]);
            }
        }

        return 0;
    }
}