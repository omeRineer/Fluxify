using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.Chat
{
    public class CreateChat
    {
        public class Request
        {
            public string Title { get; set; }
            public Guid[] UserIds { get; set; }
        }
    }

}
