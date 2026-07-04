using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.UserDevice
{
    public class SetUserDevice
    {
        public class Request
        {
            public string Platform { get; set; }
            public string DeviceId { get; set; }
            public string Token { get; set; }
        }
    }
}
