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
        public string Prefix { get; set; }
        public string Name { get; set; }
        public List<string>? Topics { get; set; }
    }
}
