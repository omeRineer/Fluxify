using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.UserTopic
{
    public class SetUserTopics
    {
        public class Request
        {
            public List<string> Topics { get; set; }
        }

        public class Response
        {

        }
    }
}
