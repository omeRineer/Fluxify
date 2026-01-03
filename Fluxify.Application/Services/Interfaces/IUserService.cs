using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IDataResult<CreateUserResponse>> CreateAsync(CreateUserRequest model);
        Task<IDataResult<UpdateUserResponse>> UpdateAsync(UpdateUserRequest model);
        Task<IDataResult<GetUserDetailResponse>> GetDetailAsync(Guid id);
        Task<IDataResult<GetUserDetailResponse?>> GetDetailAsync(string apiKey);
        Task<IDataResult<List<UserResponse>>> GetListByNameSpaceAsync(Guid nameSpace);
        Task<IDataResult<List<UserResponse>>> GetListByTopicsAsync(Guid nameSpace, params Guid[] topics);

    }
}
