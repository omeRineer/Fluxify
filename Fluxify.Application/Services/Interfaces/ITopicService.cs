using Fluxify.Application.Models.Dto.Topic;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.Topic;
using Fluxify.Application.Utilities.Result;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces
{
    public interface ITopicService
    {
        Task<IResult> CreateAsync(CreateTopic.Request req);
        Task<IDataResult<DataListResponse<TopicDto.List>>> GetListAsync();

    }
}
