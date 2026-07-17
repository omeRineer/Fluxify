using Fluxify.Application.Models.Dto.NameSpace;
using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;


namespace Fluxify.Application.Services.Interfaces.Core
{
    public interface INameSpaceService
    {
        Task<IDataResult<CreateNameSpace.Response>> CreateAsync(CreateNameSpace.Request model);
        Task<IDataResult<UpdateNameSpace.Response>> UpdateAsync(Guid id, UpdateNameSpace.Request model);
        Task<IDataResult<NameSpaceDto.Detail>> GetDetailAsync(Guid id);
    }
}
