using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.NameSpace
{
    public class NameSpaceResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}
