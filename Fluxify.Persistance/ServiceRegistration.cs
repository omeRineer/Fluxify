using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Mongo;
using Fluxify.Configuration;
using Fluxify.Domain.Entities.Mongo;
using Fluxify.Persistance.Context;
using Fluxify.Persistance.Repositories.Base;
using Fluxify.Persistance.Repositories.Core;
using Fluxify.Persistance.Repositories.Mongo;
using Microsoft.EntityFrameworkCore;
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
        public static void AddMongoContext(this IServiceCollection services)
        {
            //services.AddScoped<IDbSession, MongoDbSession>();
            services.AddScoped<MongoContext>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();

            services.AddSingleton<IMongoClient>(_ =>
                new MongoClient(AppConfiguration.MongoDbOptions.ConnectionString));

        }

        public static void AddCoreContext(this IServiceCollection services)
        {
            //services.AddScoped<IDbSession, CoreDbSession<CoreContext>>();

            services.AddDbContext<CoreContext>(opt =>
            {
                opt.UseNpgsql(AppConfiguration.ConnectionStrings.DefaultConnection);
            });

            services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();
            services.AddScoped<INameSpaceRepository, NameSpaceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChannelRepository, ChannelRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IUserTopicRepository, UserTopicRepository>();

        }
    }
}
