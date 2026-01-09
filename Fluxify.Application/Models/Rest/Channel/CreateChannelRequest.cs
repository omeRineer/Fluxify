using Fluxify.Application.Models.Inputs;
using System.Globalization;

namespace Fluxify.Application.Models.Rest.Channel
{
    public class CreateChannelRequest
    {
        public string Prefix
        {
            get => _Prefix;
            set => _Prefix = value.ToLower(new CultureInfo("en-EN"));
        }
        public string Name { get; set; }
        public List<CreateTopicInput>? Topics { get; set; }



        private string _Prefix;
    }
}
