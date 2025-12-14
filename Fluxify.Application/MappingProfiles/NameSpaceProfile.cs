using AutoMapper;
using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Domain.Core;

namespace Fluxify.Application.MappingProfiles
{
    public class NameSpaceProfile : Profile
    {
        public NameSpaceProfile()
        {
            CreateMap<NameSpace, NameSpaceResponse>();
            CreateMap<NameSpace, CreateNameSpaceResponse>().IncludeBase<NameSpace, NameSpaceResponse>();
            CreateMap<NameSpace, UpdateNameSpaceResponse>().IncludeBase<NameSpace, NameSpaceResponse>();
            CreateMap<NameSpace, GetNameSpaceDetailResponse>().IncludeBase<NameSpace, NameSpaceResponse>();

            CreateMap<CreateNameSpaceRequest, NameSpace>();
            CreateMap<UpdateNameSpaceRequest, NameSpace>();
        }
    }
}
