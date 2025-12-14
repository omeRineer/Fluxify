using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.Notification
{
    public class CreateNotificationRequest
    {
        public string? Sender { get; set; }
        public string Level { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; } = "text";
        public string? Content { get; set; }
        public List<string>? Targets { get; set; }
        public List<string>? Topics { get; set; }

        public Dictionary<string, object>? Custom { get; set; }
    }
}
