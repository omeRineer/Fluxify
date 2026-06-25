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
        public NameSpaceInfo NameSpace { get; init; }
        public StatusInfo Status { get; set; }
        public DateTime ConnectedDate { get; set; }
        public List<string> Topics { get; set; } = new();

        public Dictionary<string, DeviceInfo> Connections { get; } = new();


        public class DeviceInfo
        {
            public DeviceInfo(string device, string platform)
            {
                Device = device;
                Platform = platform;
            }
            public string Device { get; }
            public string Platform { get; }

        }

        public class NameSpaceInfo
        {
            public Guid Id { get; init; }
            public string Signature { get; init; }
        }

        public class StatusInfo
        {
            public int Id { get; set; }
            public string Name
                => ((UserAvailabilityStatus)Id).ToString();
        }
    }

}
