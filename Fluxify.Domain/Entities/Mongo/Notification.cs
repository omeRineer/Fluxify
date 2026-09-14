using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Entities.Mongo
{
    public class Notification : MongoEntity<Guid>
    {
        [BsonRepresentation(BsonType.String)]
        public Guid NameSpace { get; set; }

        public int Type { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int ContentType { get; set; }
        public string? Content { get; set; }
        public string? Thumbnail { get; set; }
        public Target? To { get; set; }
        

        public BsonDocument? Properties { get; set; }



        public class Target
        {
            public List<string> Topics { get; set; } = new();

            [BsonRepresentation(BsonType.String)]
            public List<Guid> Users { get; set; } = new();
        }

    }
}
