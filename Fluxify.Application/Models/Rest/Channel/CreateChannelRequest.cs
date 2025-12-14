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
        public List<string>? Topics { get; set; }



        private string _Prefix;
    }
}
