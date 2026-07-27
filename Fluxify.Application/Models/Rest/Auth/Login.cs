using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.Auth
{
    public class Login
    {
        public class Request
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class Response
        {

        }
    }
}
