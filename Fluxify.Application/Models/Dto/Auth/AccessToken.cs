using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Dto.Auth
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
