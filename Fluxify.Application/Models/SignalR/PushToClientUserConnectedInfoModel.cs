using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.SignalR
{
    public class PushToClientUserConnectedInfoModel
    {
        public string UserId { get; set; }
        public DateTime ConnectedDate { get; set; }
    }
}
