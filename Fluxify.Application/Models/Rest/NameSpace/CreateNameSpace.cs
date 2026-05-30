using Fluxify.Application.Models.Dto.NameSpace;

namespace Fluxify.Application.Models.Rest.NameSpace
{
    public class CreateNameSpace
    {
        public class Request
        {
            public string Signature { get; set; }
            public string Name { get; set; }
            public DateTime? ExpireDate { get; set; }
        }


        public class Response : NameSpaceDto.Detail
        {

        }
    }
}
