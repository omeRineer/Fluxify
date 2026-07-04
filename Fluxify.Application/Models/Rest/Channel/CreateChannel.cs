using System.Globalization;
using Fluxify.Application.Models.Dto.Channel;

namespace Fluxify.Application.Models.Rest.Channel
{
    public class CreateChannel
    {
        public class Request
        {
            public string Key
            {
                get => _Key;
                set => _Key = value.ToLower(new CultureInfo("en-EN"));
            }
            public string Name { get; set; }
            public List<Topic>? Topics { get; set; }



            private string _Key;


            public class Topic
            {
                public string Key { get; set; }
                public string Name { get; set; }
            }
        }

        public class Response : ChannelDto.Detail
        {

        }
    }
}
