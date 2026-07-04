using AutoMapper;
using Fluxify.Application.Models.Dto.Channel;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.MappingProfiles
{
    public class ChannelProfile : Profile
    {
        public ChannelProfile()
        {
            CreateMap<Channel, ChannelDto.Detail>();
            CreateMap<Channel, CreateChannel.Response>().IncludeBase<Channel, ChannelDto.Detail>();
            CreateMap<Channel, UpdateChannel.Response>().IncludeBase<Channel, ChannelDto.Detail>();

            CreateMap<CreateChannel.Request, Channel>();
            CreateMap<UpdateChannel.Request, Channel>();
        }
    }
}
