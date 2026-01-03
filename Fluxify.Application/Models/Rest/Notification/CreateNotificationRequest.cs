using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.Notification
{
    public class CreateNotificationRequest
    {
        public int? Level { get; set; }
        public string Title { get; set; }
        public int? ContentType { get; set; }
        public string? Content { get; set; }
        public string[]? Topics { get; set; }

        public Dictionary<string, object>? Properties { get; set; }
    }
}
