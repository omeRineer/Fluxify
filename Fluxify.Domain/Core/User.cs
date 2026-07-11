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
    public class User : MongoEntity<Guid>
    {
        [BsonRepresentation(BsonType.String)]
        public Guid NameSpace { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Key { get; set; }
        public string ApiKey { get; set; }
        public string? Image { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
