using Fluxify.Application.Models.Dto.NameSpace;
using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;

namespace Fluxify.Application.Services.Interfaces
{
    public interface INameSpaceService
    {
        Task<IDataResult<CreateNameSpace.Response>> CreateAsync(CreateNameSpace.Request model);
        Task<IDataResult<UpdateNameSpace.Response>> UpdateAsync(Guid id, UpdateNameSpace.Request model);
        Task<IDataResult<NameSpaceDto.Detail>> GetDetailAsync(Guid id);
    }
}
