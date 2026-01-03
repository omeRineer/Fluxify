using AutoMapper;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Domain.Core;

namespace Fluxify.Application.MappingProfiles
{
    public class SubscriberProfile : Profile
    {
        public SubscriberProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<User, CreateUserResponse>().IncludeBase<User, UserResponse>();
            CreateMap<User, UpdateUserResponse>().IncludeBase<User, UserResponse>();
            CreateMap<User, GetUserDetailResponse>().IncludeBase<User, UserResponse>();

            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
        }
    }
}
