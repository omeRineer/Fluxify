using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models
{
    public class ConnectionUser
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public Guid NameSpaceId { get; set; }
        public string NameSpace { get; set; }
        public int AvailabilityStatus { get; set; }
        public DateTime ConnectedDate { get; set; }
        public List<string>? Topics { get; set; }

        public HashSet<string> Connections { get; } = new();
    }
}
