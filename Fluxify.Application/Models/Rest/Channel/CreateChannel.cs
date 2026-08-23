using System.Globalization;
using Fluxify.Application.Models.Dto.Channel;

namespace Fluxify.Application.Models.Rest.Channel
{
    public class CreateChannel
    {
        public class Request
        {
            public string Slug
            {
                get => _Slug;
                set => _Slug = value.ToLower(new CultureInfo("en-EN"));
            }
            public string Name { get; set; }
            public List<Topic>? Topics { get; set; }



            private string _Slug;


            public class Topic
            {
                public string Slug { get; set; }
                public string Name { get; set; }
            }
        }

        public class Response : ChannelDto.Detail
        {

        }
    }
}
