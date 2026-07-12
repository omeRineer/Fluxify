using AutoMapper;
using Fluxify.Application.Models.Dto.Topic;
using Fluxify.Application.Models.Rest.Topic;
using Fluxify.Domain.Entities.Core;
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
            CreateMap<Topic, TopicDto.List>();

            CreateMap<CreateTopic.Request, Topic>();
        }
    }
}
