using AutoMapper;
using Fluxify.Application.Models.Dto.Chat;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Domain.Entities.Core;
using Fluxify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.MappingProfiles
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<Chat, ChatDto.List>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(x => EnumHelper.ToEnumString<ChatType>((int)x.Type, true)));
        }
    }
}
