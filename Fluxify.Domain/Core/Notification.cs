using Fluxify.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Core
{
    public class Notification : MongoEntity<Guid>
    {
        [BsonRepresentation(BsonType.String)]
        public Guid NameSpace { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid Sender { get; set; }

        public int Level { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int ContentType { get; set; }
        public string? Content { get; set; }
        public string? Thumbnail { get; set; }
        public List<string> Topics { get; set; }

        public Dictionary<string, object>? Properties { get; set; }

    }
}
