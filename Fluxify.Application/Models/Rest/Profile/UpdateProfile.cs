using Fluxify.Application.Models.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Rest.Profile
{
    public class UpdateProfile
    {
        public class Request
        {
            public string? Title { get; set; }
            public string Name { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public DateTime? BirthdayDate { get; set; }
        }

        public class Response : UserDto.Profile
        {

        }
    }
}
