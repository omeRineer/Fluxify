using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Hub
{
    public class NotificationHubUser : HubUser
    {
        public List<string>? Topics { get; set; }
    }
}
