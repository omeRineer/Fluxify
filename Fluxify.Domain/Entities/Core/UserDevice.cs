using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Entities.Core
{
    public class UserDevice : CoreEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid NameSpaceId { get; set; }
        public string DeviceId { get; set; }
        public int Platform { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastSeen { get; set; }

        public virtual NameSpace NameSpace { get; set; }
        public virtual User User { get; set; }
    }
}
