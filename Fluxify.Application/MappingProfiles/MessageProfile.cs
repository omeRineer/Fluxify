using AutoMapper;
using Fluxify.Application.Models.Dto.Message;
using Fluxify.Domain.Entities.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.MappingProfiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageDto.List>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreateDate));
        }
    }
}
