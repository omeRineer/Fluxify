using AutoMapper;
using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Domain.Core;

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

        public async Task<ApiDataResponse<CreateNameSpaceResponse>> CreateAsync(CreateNameSpaceRequest model)
        {
            var nameSpace = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Signature == model.Signature);
            if (nameSpace != null)
                return ApiResponseHelper.Error<CreateNameSpaceResponse>(message: "Namespace is avaible. Please, set a difference signature value.");

            var newNameSpace = new NameSpace
            {
                Signature = model.Signature,
                Name = model.Name,
                ExpireDate = model.ExpireDate,
            };

            await NameSpaceRepository.AddAsync(newNameSpace);

            var result = Mapper.Map<CreateNameSpaceResponse>(newNameSpace);

            return ApiResponseHelper.Success(result, "Namespace is created.");
        }

        public async Task<ApiDataResponse<UpdateNameSpaceResponse>> UpdateAsync(UpdateNameSpaceRequest model)
        {
            var nameSpace = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Id == model.Id);
            if (nameSpace == null)
                return ApiResponseHelper.Error<UpdateNameSpaceResponse>(message: "Namespace is not found.");

            nameSpace = Mapper.Map(model, nameSpace);

            await NameSpaceRepository.UpdateAsync(nameSpace);

            var result = Mapper.Map<UpdateNameSpaceResponse>(nameSpace);

            return ApiResponseHelper.Success(result, "Namespace is updated.");
        }

        public async Task<ApiDataResponse<GetNameSpaceDetailResponse>> GetDetailAsync(Guid id)
        {
            var nameSpace = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            if (nameSpace == null)
                return ApiResponseHelper.Error<GetNameSpaceDetailResponse>(message: "Namespace is not found.");

            var result = Mapper.Map<GetNameSpaceDetailResponse>(nameSpace);

            return ApiResponseHelper.Success(data:result);
        }
    }
}
