using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Dto.Channel
{
    public partial class ChannelDto
    {
        public class Detail
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime EditDate { get; set; }
        }
    }
}
