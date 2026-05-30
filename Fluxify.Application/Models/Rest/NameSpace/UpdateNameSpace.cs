
using Fluxify.Application.Models.Dto.Channel;
using Fluxify.Application.Models.Dto.NameSpace;
namespace Fluxify.Application.Models.Rest.NameSpace
{
    public class UpdateNameSpace
    {
        public class Request
        {
            public string Name { get; set; }
            public DateTime? ExpireDate { get; set; }
        }

        public class Response:NameSpaceDto.Detail
        {

        }
    }
}
