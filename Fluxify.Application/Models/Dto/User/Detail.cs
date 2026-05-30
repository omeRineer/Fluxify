using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Dto.User
{
    public partial class UserDto
    {
        public class Detail
        {
            public Guid Id { get; set; }
            public Guid NameSpace { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Key { get; set; }
            public string ApiKey { get; set; }
            public List<string>? Topics { get; set; }
            public DateTime? ExpireDate { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
        }
    }
}
