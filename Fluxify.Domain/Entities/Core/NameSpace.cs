using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Entities.Core
{
    public class NameSpace : CoreEntity<Guid>
    {
        public string Signature { get; set; }
        public string ApiKey { get; set; }
        public string Name { get; set; }
        public DateTime? ExpireDate { get; set; }

        public virtual List<User>? Users { get; set; }
    }
}
