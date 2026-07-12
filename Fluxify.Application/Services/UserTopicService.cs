using AutoMapper;
using Fluxify.Application.Models.Dto.UserTopic;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.UserTopic;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Entities.Core;
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
        readonly IMapper _mapper;

        public UserTopicService(IUserTopicRepository userTopicRepository, ITopicRepository topicRepository, IMapper mapper)
        {
            UserTopicRepository = userTopicRepository;
            TopicRepository = topicRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<DataListResponse<UserTopicDto.List>>> GetListAsync(Guid userId)
        {
            var userTopics = await UserTopicRepository.GetListAsync(f => f.UserId == userId);

            var mappedList = _mapper.Map<List<UserTopicDto.List>>(userTopics);

            var result = DataListResponse<UserTopicDto.List>.Load(mappedList);

            return new SuccessDataResult<DataListResponse<UserTopicDto.List>>(result);
        }

        public async Task<IResult> SetAsync(Guid userId, Guid nameSpaceId, SetUserTopics.Request req)
        {
            var topics = await TopicRepository.GetListAsync(f => req.Topics.Contains(f.Id));
            var userTopics = await UserTopicRepository.GetListAsync(f => f.UserId == userId);

            await UserTopicRepository.DeleteRangeAsync(userTopics.Select(s=> s.Id));
            await UserTopicRepository.AddRangeAsync(topics.Select(t => new UserTopic
            {
                UserId = userId,
                TopicId = t.Id,
                NameSpaceId = nameSpaceId
            }));

            return new SuccessResult("User topics are updated");
        }
    }
}
