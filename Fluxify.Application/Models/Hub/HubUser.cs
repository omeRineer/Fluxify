using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Hub
{
    public abstract class HubUser
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public NameSpaceInfo NameSpace { get; init; }
        public Dictionary<string, DeviceInfo> Connections { get; } = new();


        public class DeviceInfo
        {
            public DeviceInfo(string device, string platform)
            {
                Device = device;
                Platform = platform;
                Date = DateTime.UtcNow;
            }
            public string Device { get; }
            public string Platform { get; }
            public DateTime Date { get; }

        }
        public class NameSpaceInfo
        {
            public Guid Id { get; init; }
            public string Signature { get; init; }
        }
    }

}
