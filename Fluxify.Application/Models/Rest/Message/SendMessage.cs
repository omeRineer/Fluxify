using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.Message
{
    public class SendMessage
    {
        public class Request
        {
            public string Text { get; set; }
        }
    }
}
