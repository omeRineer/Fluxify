using Fluxify.Application.Models.Dto.Channel;

namespace Fluxify.Application.Models.Rest.Channel
{
    public class UpdateChannel
    {
        public class Request
        {
            public string Name { get; set; }
        }

        public class Response : ChannelDto.Detail
        {

        }
    }
}
