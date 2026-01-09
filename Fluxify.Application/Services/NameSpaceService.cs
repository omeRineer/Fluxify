using AutoMapper;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;
using System.Threading.Channels;

namespace Fluxify.Application.Services
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

        public async Task<IDataResult<CreateNameSpaceResponse>> CreateAsync(CreateNameSpaceRequest model)
        {
            var nameSpace = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Signature == model.Signature);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeAvaibleRule(nameSpace, "Namespace is avaible. Please, set a difference signature value")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<CreateNameSpaceResponse>(message: businessResult.Message);

            var newNameSpace = new NameSpace
            {
                Signature = model.Signature,
                Name = model.Name,
                ExpireDate = model.ExpireDate,
            };

            await NameSpaceRepository.AddAsync(newNameSpace);

            var result = Mapper.Map<CreateNameSpaceResponse>(newNameSpace);

            return new SuccessDataResult<CreateNameSpaceResponse>(result, "Namespace is created");
        }

        public async Task<IDataResult<UpdateNameSpaceResponse>> UpdateAsync(UpdateNameSpaceRequest model)
        {
            var nameSpace = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Id == model.Id);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(nameSpace, "Namespace is not found")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<UpdateNameSpaceResponse>(message: businessResult.Message);


            nameSpace = Mapper.Map(model, nameSpace);

            await NameSpaceRepository.UpdateAsync(nameSpace);

            var result = Mapper.Map<UpdateNameSpaceResponse>(nameSpace);

            return new SuccessDataResult<UpdateNameSpaceResponse>(result, "Namespace is updated.");
        }

        public async Task<IDataResult<GetNameSpaceDetailResponse>> GetDetailAsync(Guid id)
        {
            var nameSpace = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(nameSpace, "Namespace is not found")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<GetNameSpaceDetailResponse>(message: businessResult.Message);

            var result = Mapper.Map<GetNameSpaceDetailResponse>(nameSpace);

            return new SuccessDataResult<GetNameSpaceDetailResponse>(result);
        }

        public async Task<IDataResult<NameSpace>> GetAsync(Guid id)
            => new SuccessDataResult<NameSpace>(await NameSpaceRepository.GetSingleAsync(f => f.Id == id));
    }
}
