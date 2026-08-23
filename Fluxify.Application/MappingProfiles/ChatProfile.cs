using AutoMapper;
using Fluxify.Application.Models.Dto.Chat;
using Fluxify.Application.Models.Rest.Chat;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Domain.Entities.Core;
using Fluxify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fluxify.Application.Models.Dto.Chat.ChatDto.Detail;

namespace Fluxify.Application.MappingProfiles
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<Chat, ChatDto.List>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(x => EnumHelper.ToEnumString<ChatType>((int)x.Type, true)));

            CreateMap<Chat, ChatDto.Detail>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(x => EnumHelper.ToEnumString<ChatType>((int)x.Type, true)));




            CreateMap<CreateChat.Request, Chat>()
                .ForMember(dest => dest.Users,
                           opt => opt.MapFrom(src => src.UserIds.Select(s => new ChatUser { UserId = s }).ToList()))
                .ForMember(dest => dest.Type,
                           opt => opt.MapFrom(src => (src.UserIds != null && src.UserIds.Length > 1) ? ChatType.Group : ChatType.Direct));

        }
    }
}
