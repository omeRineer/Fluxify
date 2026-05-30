using Fluxify.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Domain.Core
{
    public class Channel : MongoEntity<Guid>
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
