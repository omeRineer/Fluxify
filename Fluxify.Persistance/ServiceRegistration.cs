using Fluxify.Application.Repositories;
using Fluxify.Configuration;
using Fluxify.Persistance.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistance(this IServiceCollection services)
        {
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INameSpaceRepository, NameSpaceRepository>();
            services.AddScoped<ISubscriberRepository, SubscriberRepository>();
            services.AddScoped<IChannelRepository, ChannelRepository>();

            services.AddSingleton<IMongoClient>(opt => new MongoClient(AppConfiguration.MongoDbOptions.ConnectionString));
            services.AddScoped(opt =>
            {
                var client = opt.GetService<IMongoClient>();
                return client.GetDatabase(AppConfiguration.MongoDbOptions.DataBase);
            });
        }
    }
}
