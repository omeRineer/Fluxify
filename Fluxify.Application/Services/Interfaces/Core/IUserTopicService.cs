using Fluxify.Application.Models.Dto.UserTopic;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.UserTopic;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces.Core
{
    public interface IUserTopicService
    {
        Task<IResult> SetAsync(Guid userId, Guid nameSpaceId, SetUserTopics.Request req);
        Task<IDataResult<DataListResponse<UserTopicDto.List>>> GetListAsync(Guid userId);


        Task<List<string>> GetTopicsByUser(Guid userId);
    }
}
