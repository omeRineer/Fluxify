using AutoMapper;
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
            CreateMap<Channel, ChannelResponse>();
            CreateMap<Channel, CreateChannelResponse>().IncludeBase<Channel, ChannelResponse>();
            CreateMap<Channel, UpdateChannelResponse>().IncludeBase<Channel, ChannelResponse>();
            CreateMap<Channel, GetChannelDetailResponse>().IncludeBase<Channel, ChannelResponse>();

            CreateMap<CreateChannelRequest, Channel>();
            CreateMap<UpdateChannelRequest, Channel>();
        }
    }
}
