using Fluxify.Application.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Hub
{
    public class ConnectionUser
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public ConnectionUserNameSpace NameSpace { get; init; }
        public ConnectionUserStatus Status { get; set; }
        public DateTime ConnectedDate { get; set; }
        public List<string>? Topics { get; set; }

        public HashSet<string> Connections { get; } = new();
    }

    public class ConnectionUserNameSpace
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
    }

    public class ConnectionUserStatus
    {
        public int Id { get; set; }
        public string Name
            => ((UserAvailabilityStatus)Id).ToString();
    }
}
