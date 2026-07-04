using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Core
{
    public class UserNotification : MongoEntity<Guid>
    {
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid NotificationId { get; set; }
    }
}
