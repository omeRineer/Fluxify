using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Core
{
    public class Topic : MongoEntity<string>
    {
        [BsonRepresentation(BsonType.String)]
        public Guid ChannelId { get; set; }
        public string Name { get; set; }
    }
}
