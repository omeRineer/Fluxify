using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Fluxify.Domain.Entities.Mongo
{
    public class Message : MongoEntity<Guid>
    {
        [BsonRepresentation(BsonType.String)]
        public Guid ChatId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }
        public string Text { get; set; }
    }
}
