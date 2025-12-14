using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Application.Utilities.ApiResponse;

namespace Fluxify.Application.Services.Interfaces
{
    public interface INameSpaceService
    {
        Task<ApiDataResponse<CreateNameSpaceResponse>> CreateAsync(CreateNameSpaceRequest model);
        Task<ApiDataResponse<UpdateNameSpaceResponse>> UpdateAsync(UpdateNameSpaceRequest model);
        Task<ApiDataResponse<GetNameSpaceDetailResponse>> GetDetailAsync(Guid id);
    }
}
