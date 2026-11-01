using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Entities.Core
{
    public class User : CoreEntity<Guid>
    {
        public Guid NameSpaceId { get; set; }
        public string Key { get; set; }
        public string? Title { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public DateTime AccountDate { get; set; }


        public virtual NameSpace NameSpace { get; set; }
        public virtual Credential? Credential { get; set; }
        public virtual List<UserTopic>? Topics { get; set; }
        public virtual List<UserDevice>? Devices { get; set; }
        public virtual List<ChatUser>? Chats { get; set; }
    }
}
