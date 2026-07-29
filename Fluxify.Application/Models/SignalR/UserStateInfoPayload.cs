using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.SignalR
{
    public class UserStateInfoPayload
    {
        public string Key { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
