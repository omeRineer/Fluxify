using AutoMapper;
using Fluxify.Application.Models.Dto.NameSpace;
using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Application.Utilities.Helpers;
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

            CreateMap<CreateNameSpace.Request, NameSpace>()
                .ForMember(dest => dest.ApiKey, opt => opt.MapFrom(src => CryptoHelper.Encrypt(DateTime.UtcNow.ToString(), src.Signature)));
            CreateMap<UpdateNameSpace.Request, NameSpace>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
