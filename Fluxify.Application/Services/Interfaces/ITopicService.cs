using Fluxify.Application.Models.Rest.Topic;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces
{
    public interface ITopicService
    {
        Task<IDataResult<List<Topic>>> GetListByChannelAsync(Guid channelId);
        Task<IDataResult<List<Topic>>> GetListByKey(params string[] topics);

        Task<IResult> CreateAsync(CreateTopicRequest req);
    }
}
