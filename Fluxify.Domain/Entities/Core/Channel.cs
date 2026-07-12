using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Entities.Core
{
    public class Channel : CoreEntity<Guid>
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
