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

        public virtual List<ChatUser>? Users { get; set; }
    }
}
