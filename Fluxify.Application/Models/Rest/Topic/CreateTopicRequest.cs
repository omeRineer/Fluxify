using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.Topic
{
    public class CreateTopicRequest
    {
        public Guid ChannelId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
