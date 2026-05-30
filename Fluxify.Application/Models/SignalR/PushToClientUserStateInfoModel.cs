using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.SignalR
{
    public class PushToClientUserStateInfoModel
    {
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
    }
}
