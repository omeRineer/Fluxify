using Fluxify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Entities.Core
{
    public class Chat : CoreEntity<Guid>
    {
        public Guid NameSpaceId { get; set; }
        public Guid CreatedUserId { get; set; }
        public string? Title { get; set; }
        public ChatType Type { get; set; }


        public virtual ICollection<ChatUser> Users { get; set; } = new List<ChatUser>();
        public virtual User CreatedUser { get; set; }
    }
}
