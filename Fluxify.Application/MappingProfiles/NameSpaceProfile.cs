using AutoMapper;
using Fluxify.Application.Models.Dto.NameSpace;
using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Domain.Entities.Core;

namespace Fluxify.Application.MappingProfiles
{
    public class NameSpaceProfile : Profile
    {
        public NameSpaceProfile()
        {
            CreateMap<NameSpace, NameSpaceDto.Detail>();
            CreateMap<NameSpace, CreateNameSpace.Response>().IncludeBase<NameSpace, NameSpaceDto.Detail>();
            CreateMap<NameSpace, UpdateNameSpace.Response>().IncludeBase<NameSpace, NameSpaceDto.Detail>();

            CreateMap<CreateNameSpace.Request, NameSpace>();
            CreateMap<UpdateNameSpace.Request, NameSpace>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
