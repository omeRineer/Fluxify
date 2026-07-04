using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Infrastructure
{
    public interface IEventPublisher
    {
        public Task PublishAsync(string exchange,
                                 object body,
                                 string routingKey = "");
    }
}
