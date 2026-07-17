using Fluxify.Application.Models.Dto.User;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces.Core
{
    public interface IUserService
    {
        Task<IDataResult<CreateUser.Response>> CreateAsync(Guid nameSpaceId, CreateUser.Request model);
        Task<IDataResult<UpdateUser.Response>> UpdateAsync(Guid id, UpdateUser.Request model);
        Task<IDataResult<UserDto.Detail>> GetDetailAsync(Guid id);
        Task<IDataResult<DataListResponse<UserDto.List>>> GetListByTopicsAsync(Guid nameSpace, params Guid[] topics);
        Task<IDataResult<DataListResponse<UserDto.List>>> GetListAsync(Guid nameSpaceId);


        #region Internal
        Task<IDataResult<User>> GetAsync(string apiKey);
        Task<IDataResult<User>> GetAsync(Guid id);
        #endregion

    }
}
