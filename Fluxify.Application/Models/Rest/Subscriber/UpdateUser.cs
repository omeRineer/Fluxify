using Fluxify.Application.Models.Dto.User;

namespace Fluxify.Application.Models.Rest.Subscriber
{
    public class UpdateUser
    {
        public class Request
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime? ExpireDate { get; set; }
        }

        public class Response:UserDto.Detail
        {
            public string Token { get; set; }
        }
    }
}
