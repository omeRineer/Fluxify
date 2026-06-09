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
        public string Level { get; set; }
        public string ContentType { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string? Content { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public Dictionary<string, object>? Properties { get; set; }
    }
}
