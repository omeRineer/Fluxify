using AutoMapper;
using Fluxify.Application.Models.Rest.Topic;
using Fluxify.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.MappingProfiles
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<CreateTopicRequest, Topic>();
        }
    }
}
