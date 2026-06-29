using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Configuration.Options
{
    public class RabbitMqOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<ExchangeOptions> Exchanges { get; set; }
        public List<QueueOptions> Queues { get; set; }
        public class ExchangeOptions
        {
            public string Name { get; set; } = string.Empty;
            public string Type { get; set; } = "direct"; // direct, topic, fanout, headers
            public bool Durable { get; set; } = true;
            public bool AutoDelete { get; set; } = false;
        }

        public class QueueOptions
        {
            public string Name { get; set; } = string.Empty;
            public Dictionary<string, string> Bindings { get; set; } = new();
            public bool Durable { get; set; } = true;
            public bool Exclusive { get; set; } = false;
            public bool AutoDelete { get; set; } = false;
            public bool EnableDlq { get; set; } = false;
            public bool EnableRq { get; set; } = false;
        }
    }
}
