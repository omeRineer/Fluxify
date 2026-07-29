using AutoMapper;
using Fluxify.Application.Models.Dto.Chat;
using Fluxify.Application.Models.Dto.User;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Domain.Entities.Core;

namespace Fluxify.Application.MappingProfiles
{
    public class SubscriberProfile : Profile
    {
        public SubscriberProfile()
        {
            CreateMap<User, UserDto.List>();
            CreateMap<User, UserDto.Detail>();
            CreateMap<User, CreateUser.Response>().IncludeBase<User, UserDto.Detail>();
            CreateMap<User, UpdateUser.Response>().IncludeBase<User, UserDto.Detail>();
            CreateMap<User, ChatDto.Detail.User>();

            CreateMap<CreateUser.Request, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Name) ? $"USR:{src.Key}" : src.Name));
            CreateMap<UpdateUser.Request, User>()
                .AfterMap((src, dest) =>
                {
                    dest.Name = string.IsNullOrEmpty(src.Name) ? $"USR:{dest.Key}" : src.Name;
                })
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
