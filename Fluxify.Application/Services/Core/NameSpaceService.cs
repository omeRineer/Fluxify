using AutoMapper;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;
using System.Threading.Channels;
using Fluxify.Application.Models.Dto.NameSpace;
using Fluxify.Domain.Entities.Core;
using Fluxify.Application.Services.Interfaces.Core;

namespace Fluxify.Application.Services.Core
{
    public class NameSpaceService : INameSpaceService
    {
        readonly INameSpaceRepository NameSpaceRepository;
        readonly IMapper Mapper;

        public NameSpaceService(INameSpaceRepository nameSpaceRepository, IMapper mapper)
        {
            NameSpaceRepository = nameSpaceRepository;
            Mapper = mapper;
        }

        public async Task<IDataResult<CreateNameSpace.Response>> CreateAsync(CreateNameSpace.Request model)
        {
            var nameSpace = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Signature == model.Signature);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeAvaibleRule(nameSpace, "Namespace is avaible. Please, set a difference signature value")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<CreateNameSpace.Response>(message: businessResult.Message);

            var newNameSpace = Mapper.Map<NameSpace>(model);

            await NameSpaceRepository.AddAsync(newNameSpace);
            await NameSpaceRepository.SaveChangesAsync();

            var result = Mapper.Map<CreateNameSpace.Response>(newNameSpace);

            return new SuccessDataResult<CreateNameSpace.Response>(result, "Namespace is created");
        }

        public async Task<IDataResult<UpdateNameSpace.Response>> UpdateAsync(Guid id, UpdateNameSpace.Request model)
        {
            var nameSpace = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(nameSpace, "Namespace is not found")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<UpdateNameSpace.Response>(message: businessResult.Message);


            nameSpace = Mapper.Map(model, nameSpace);

            await NameSpaceRepository.UpdateAsync(nameSpace);
            await NameSpaceRepository.SaveChangesAsync();

            var result = Mapper.Map<UpdateNameSpace.Response>(nameSpace);

            return new SuccessDataResult<UpdateNameSpace.Response>(result, "Namespace is updated.");
        }

        public async Task<IDataResult<NameSpaceDto.Detail>> GetDetailAsync(Guid id)
        {
            var nameSpace = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(nameSpace, "Namespace is not found")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<NameSpaceDto.Detail>(message: businessResult.Message);

            var result = Mapper.Map<NameSpaceDto.Detail>(nameSpace);

            return new SuccessDataResult<NameSpaceDto.Detail>(result);
        }
    }
}
