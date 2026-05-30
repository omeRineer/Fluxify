using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.SignalR
{
    public class PushToClientNotificationModel
    {
        public Guid Id { get; set; }
        public string Sender { get; set; }
        public int Level { get; set; }
        public int ContentType { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public Dictionary<string, object>? Properties { get; set; }
    }
}
