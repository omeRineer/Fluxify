using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Dto.User
{
    public partial class UserDto
    {
        public class List
        {
            public Guid Id { get; set; }
            public Guid NameSpace { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Key { get; set; }
        }
    }
}
