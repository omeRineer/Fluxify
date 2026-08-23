using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Dto.Chat
{
    public partial class ChatDto
    {
        public class Detail
        {
            public Guid Id { get; set; }
            public Guid NameSpaceId { get; set; }
            public User CreatedUser { get; set; }
            public string? Title { get; set; }
            public string Type { get; set; }
            public DateTime CreateDate { get; set; }

            public class User
            {
                public Guid Id { get; set; }
                public string Key { get; set; }
                public string Name { get; set; }
            }
        }
    }
}
