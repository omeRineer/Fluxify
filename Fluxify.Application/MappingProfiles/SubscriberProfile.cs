using AutoMapper;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Domain.Core;

namespace Fluxify.Application.MappingProfiles
{
    public class SubscriberProfile : Profile
    {
        public SubscriberProfile()
        {
            CreateMap<Subscriber, SubscriberResponse>();
            CreateMap<Subscriber, CreateSubscriberResponse>().IncludeBase<Subscriber, SubscriberResponse>();
            CreateMap<Subscriber, UpdateSubscriberResponse>().IncludeBase<Subscriber, SubscriberResponse>();
            CreateMap<Subscriber, GetSubscriberDetailResponse>().IncludeBase<Subscriber, SubscriberResponse>();

            CreateMap<CreateSubscriberRequest, Subscriber>();
            CreateMap<UpdateSubscriberRequest, Subscriber>();
        }
    }
}
