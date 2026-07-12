using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Entities.Core
{

    public class UserTopic : CoreEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid NameSpaceId { get; set; }
        public Guid TopicId { get; set; }
    }


}
