using Fluxify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Dto.Notification
{
    public partial class NotificationDto
    {
        public class Detail
        {
            public Guid Id { get; set; }
            public string Type { get; set; }
            public string ContentType { get; set; }
            public string? Content { get; set; }
            public string Title { get; set; }
            public string Summary { get; set; }
            public string? Thumbnail { get; set; }
            public DateTime CreateDate { get; set; }
            public Dictionary<string, object>? Properties { get; set; }

            public bool IsRead { get; set; }

        }

        public class List
        {
            public Guid Id { get; set; }
            public string Type { get; set; }
            public string Title { get; set; }
            public string Summary { get; set; }
            public string? Thumbnail { get; set; }
            public DateTime CreateDate { get; set; }
            public Dictionary<string, object>? Properties { get; set; }

            public bool IsRead { get; set; }

        }
    }
}
