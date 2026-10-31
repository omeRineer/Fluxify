using Fluxify.Application.Models.Dto.User;
using Fluxify.Application.Models.Rest.Profile;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces.Core
{
    public interface IProfileService
    {
        Task<IDataResult<UserDto.Profile>> GetDetailAsync(Guid id);
        Task<IDataResult<UpdateProfile.Response>> UpdateAsync(Guid id, UpdateProfile.Request req);
    }
}
