using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Dto.UserTopic
{
    public partial class UserTopicDto
    {
        public class List
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public string TopicId { get; set; }
        }
    }
}
