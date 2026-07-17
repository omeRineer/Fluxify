using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Entities.Core
{
    public class Topic : CoreEntity<Guid>
    {
        public Guid ChannelId { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }

        public virtual Channel Channel { get; set; }
        public virtual List<UserTopic>? Users { get; set; }
    }
}
