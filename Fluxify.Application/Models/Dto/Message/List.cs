using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Dto.Message
{
    public partial class MessageDto
    {
        public class List
        {
            public Guid ChatId { get; set; }
            public Guid UserId { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
