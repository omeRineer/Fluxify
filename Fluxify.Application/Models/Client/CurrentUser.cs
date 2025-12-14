using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Client
{
    public class CurrentUser
    {
        public Guid NameSpace { get; set; }
        public string Key { get; set; }
        public string ApiKey { get; set; }
        public string Name { get; set; }
    }
}
