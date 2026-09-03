using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.Notification
{
    public class CreateNotification
    {
        public class Request
        {
            public string? Type { get; set; }
            public string Title { get; set; }
            public string Summary { get; set; }
            public string? ContentType { get; set; }
            public string? Content { get; set; }
            public string? Thumbnail { get; set; }
            public string? Url { get; set; }
            public Target? To { get; set; }
            

            public Dictionary<string, object>? Properties { get; set; }


            public class Target
            {
                public List<string> Topics { get; set; } = new();
                public List<string> Users { get; set; } = new();
            }
        }
    }
}
