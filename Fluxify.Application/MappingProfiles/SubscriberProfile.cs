using AutoMapper;
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

            CreateMap<CreateUser.Request, User>();
            CreateMap<UpdateUser, User>();
        }
    }
}
