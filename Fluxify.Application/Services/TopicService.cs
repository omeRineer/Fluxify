using AutoMapper;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Models.Rest.Topic;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services
{
    public class TopicService : ITopicService
    {
        readonly IChannelRepository channelRepository;
        readonly ITopicRepository topicRepository;
        readonly IUserTopicRepository userTopicRepository;
        readonly IMapper _mapper;

        public TopicService(ITopicRepository topicRepository, IMapper mapper, IChannelRepository channelRepository, IUserTopicRepository userTopicRepository)
        {
            this.topicRepository = topicRepository;
            _mapper = mapper;
            this.channelRepository = channelRepository;
            this.userTopicRepository = userTopicRepository;
        }

        public async Task<IResult> CreateAsync(CreateTopic.Request req)
        {
            var channel = await channelRepository.GetSingleAsync(f => f.Key == req.Channel);

            var newTopic = new Topic
            {
                ChannelId = channel.Id,
                Key = $"{channel.Key}:{req.Key}",
                Name = req.Name
            };

            await topicRepository.AddAsync(newTopic);

            return new SuccessResult("Topic is created");
        }

        public async Task<IDataResult<List<Topic>>> GetListAsync(Guid userId)
        {
            var userTopics = await userTopicRepository.GetListAsync(f => f.UserId == userId);
            var topicList = await topicRepository.GetListAsync(f => userTopics.Select(ut => ut.TopicId).Contains(f.Id));

            return new SuccessDataResult<List<Topic>>(topicList);
        }
    }
}
