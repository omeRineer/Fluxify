using FirebaseAdmin;
using Fluxify.Application.Infrastructure;
using Fluxify.Configuration;
using Fluxify.Infrastructure.Services;
using Fluxify.Infrastructure.Services.EventPublishers;
using Fluxify.Infrastructure.Services.Hosted;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IPushNotificationService, FirebaseNotificationService>();

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile($"{AppDomain.CurrentDomain.BaseDirectory}/External/fluxify-adminsdk.json")
            });
        }


        public static async Task<IServiceCollection> AddRabbitMqAsync(this IServiceCollection services)
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = AppConfiguration.RabbitMqOptions.Host;
            connectionFactory.Port = AppConfiguration.RabbitMqOptions.Port;
            connectionFactory.VirtualHost = AppConfiguration.RabbitMqOptions.VirtualHost;
            connectionFactory.UserName = AppConfiguration.RabbitMqOptions.UserName;
            connectionFactory.Password = AppConfiguration.RabbitMqOptions.Password;

            IConnection? connection = null;
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(1000);
                connection = await connectionFactory.CreateConnectionAsync();

                if (connection != null && connection.IsOpen)
                    break;
            }
            var channel = await connection.CreateChannelAsync();

            services.AddSingleton<IConnection>(connection);
            services.AddSingleton<IChannel>(channel);
            services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
            services.AddHostedService<RabbitMqInitializer>();

            return services;
        }
    }
}
