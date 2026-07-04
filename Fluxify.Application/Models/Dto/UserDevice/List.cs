using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Dto.UserDevice
{
    public partial class UserDeviceDto
    {
        public class List
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public Guid NameSpaceId { get; set; }
            public string DeviceId { get; set; }
            public string Platform { get; set; }
            public string Token { get; set; }
            public bool IsActive { get; set; }
            public DateTime LastSeen { get; set; }
        }
    }
}
