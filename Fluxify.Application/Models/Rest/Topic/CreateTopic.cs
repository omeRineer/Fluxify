using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.Topic
{
    public class CreateTopic
    {
        public class Request
        {
            public string Channel { get; set; }
            public string Key { get; set; }
            public string Name { get; set; }
        }
        
        public class Response
        {

        }
    }
}
