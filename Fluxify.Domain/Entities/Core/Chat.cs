using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Core
{
    public class Chat : MongoEntity<Guid>
    {
        public Guid NameSpaceId { get; set; }
    }
}
