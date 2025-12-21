using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;

namespace Fluxify.Application.Services.Interfaces
{
    public interface INameSpaceService
    {
        Task<IDataResult<CreateNameSpaceResponse>> CreateAsync(CreateNameSpaceRequest model);
        Task<IDataResult<UpdateNameSpaceResponse>> UpdateAsync(UpdateNameSpaceRequest model);
        Task<IDataResult<GetNameSpaceDetailResponse>> GetDetailAsync(Guid id);
    }
}
