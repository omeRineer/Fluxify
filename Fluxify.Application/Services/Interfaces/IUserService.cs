using Fluxify.Application.Models.Dto.User;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.Common;
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
        Task<IDataResult<CreateUser.Response>> CreateAsync(Guid nameSpace, CreateUser.Request model);
        Task<IDataResult<UpdateUser.Response>> UpdateAsync(Guid id, UpdateUser.Request model);
        Task<IDataResult<UserDto.Detail>> GetDetailAsync(Guid id);
        Task<IDataResult<UserDto.Detail>> GetDetailAsync(string apiKey);
        Task<IDataResult<DataListResponse<UserDto.Detail>>> GetListByTopicsAsync(Guid nameSpace, params Guid[] topics);

    }
}
