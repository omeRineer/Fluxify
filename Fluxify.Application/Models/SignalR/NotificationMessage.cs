using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.SignalR
{
    public class NotificationMessage
    {
        public Guid? Id { get; set; }
        public string Level { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string? Url { get; set; }
        public string? Thumbnail { get; set; }
        public DateTime Date { get; set; }
    }
}
