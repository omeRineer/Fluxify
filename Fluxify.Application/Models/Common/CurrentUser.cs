using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Common
{
    public class CurrentUser
    {
        public Guid NameIdentifier { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        public NameSpaceInfo NameSpace { get; set; }

        public class NameSpaceInfo
        {
            public NameSpaceInfo(Guid id, string name)
            {
                Id = id;
                Name = name;
            }
            public Guid Id { get; }
            public string Name { get; }
        }
    }
}
