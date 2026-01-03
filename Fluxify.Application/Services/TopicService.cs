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
        readonly IChannelService channelService;
        readonly ITopicRepository topicRepository;
        readonly IMapper _mapper;

        public TopicService(ITopicRepository topicRepository, IMapper mapper, IChannelService channelService)
        {
            this.topicRepository = topicRepository;
            _mapper = mapper;
            this.channelService = channelService;
        }

        public async Task<IResult> CreateAsync(CreateTopicRequest req)
        {
            var entity = _mapper.Map<Topic>(req);

            await topicRepository.AddAsync(entity);

            return new SuccessResult("Topic is created");
        }

        public async Task<IDataResult<List<Topic>>> GetListByChannelAsync(Guid channelId)
        {
            var topics = await topicRepository.GetListAsync(f => f.ChannelId == channelId);

            return new SuccessDataResult<List<Topic>>(topics);
        }

        public async Task<IDataResult<List<Topic>>> GetListByKey(params string[] topics)
        {
            var topicList = await topicRepository.GetListAsync(f => topics.Contains(f.Key));

            return new SuccessDataResult<List<Topic>>(topicList);
        }
    }
}
