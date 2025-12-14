using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.Subscriber
{
    public class CreateSubscriberRequest
    {
        public Guid NameSpace { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Key { get; set; }
        public List<string>? Topics { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
