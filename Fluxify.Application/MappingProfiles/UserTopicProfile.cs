using AutoMapper;
using Fluxify.Application.Models.Dto.UserTopic;
using Fluxify.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.MappingProfiles
{
    public class UserTopicProfile : Profile
    {
        public UserTopicProfile()
        {
            CreateMap<UserTopic, UserTopicDto.List>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Topic.Slug))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Topic.Name));
        }
    }
}
