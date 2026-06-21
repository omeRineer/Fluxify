using Fluxify.Application.Models.Rest.UserTopic;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services
{
    public class UserTopicService : IUserTopicService
    {
        readonly IUserTopicRepository UserTopicRepository;
        readonly ITopicRepository TopicRepository;

        public UserTopicService(IUserTopicRepository userTopicRepository, ITopicRepository topicRepository)
        {
            UserTopicRepository = userTopicRepository;
            TopicRepository = topicRepository;
        }

        public async Task<IResult> SetAsync(Guid userId, Guid nameSpaceId, SetUserTopics.Request req)
        {
            var topics = await TopicRepository.GetListAsync(f => req.Topics.Contains(f.Id));
            var userTopics = await UserTopicRepository.GetListAsync(f => f.UserId == userId);

            await UserTopicRepository.DeleteRangeAsync(userTopics.Select(s=> s.Id));
            await UserTopicRepository.AddRangeAsync(topics.Select(t => new Domain.Core.UserTopic
            {
                UserId = userId,
                TopicId = t.Id,
                NameSpaceId = nameSpaceId
            }));

            return new SuccessResult("User topics are updated");
        }
    }
}
