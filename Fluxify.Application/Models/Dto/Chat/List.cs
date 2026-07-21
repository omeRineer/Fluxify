using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Dto.Chat
{
    public partial class ChatDto
    {
        public class List
        {
            public Guid Id { get; set; }
            public Guid NameSpaceId { get; set; }
            public Guid CreatedUserId { get; set; }
            public string? Title { get; set; }
            public string Type { get; set; }
        }
    }
}
