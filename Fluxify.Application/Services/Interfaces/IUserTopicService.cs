using Fluxify.Application.Models.Rest.UserTopic;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Interfaces
{
    public interface IUserTopicService
    {
        Task<IResult> SetAsync(Guid userId, SetUserTopics.Request req);
    }
}
