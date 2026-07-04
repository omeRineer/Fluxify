using Fluxify.Configuration.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;

namespace Fluxify.Configuration
{
    public static class AppConfiguration
    {
        readonly static IConfigurationRoot ConfigurationRoot;
        static AppConfiguration()
        {
            ConfigurationRoot = new ConfigurationBuilder()
                                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                                .Build();
        }

        public static string[] AllowOrigins { get => ConfigurationRoot.GetSection("AllowOrigins").Get<string[]>(); }
        public static MongoDbOptions MongoDbOptions { get => ConfigurationRoot.GetSection("MongoDbOptions").Get<MongoDbOptions>(); }
        public static RabbitMqOptions RabbitMqOptions { get => ConfigurationRoot.GetSection("RabbitMqOptions").Get<RabbitMqOptions>(); }

    }
}
