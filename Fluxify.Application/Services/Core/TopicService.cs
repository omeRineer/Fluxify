using AutoMapper;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Models.Rest.Topic;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
using Fluxify.Application.Utilities.Result;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Dto.Topic;
using Fluxify.Domain.Entities.Core;
using Fluxify.Application.Services.Interfaces.Core;

namespace Fluxify.Application.Services.Core
{
    public class TopicService : ITopicService
    {
        readonly IChannelRepository channelRepository;
        readonly ITopicRepository topicRepository;
        readonly IMapper _mapper;

        public TopicService(ITopicRepository topicRepository, IMapper mapper, IChannelRepository channelRepository)
        {
            this.topicRepository = topicRepository;
            _mapper = mapper;
            this.channelRepository = channelRepository;
        }


        public async Task<IDataResult<PagedDataResponse<TopicDto.List>>> GetListAsync(TopicQuery query)
        {
            var pagedData = await topicRepository.GetPagedListAsync(query.Page, query.Size);

            var mappedList = _mapper.Map<List<TopicDto.List>>(pagedData.Data);

            var pagedResponse = PagedDataResponse<TopicDto.List>.Load(mappedList, pagedData.TotalCount);

            return new SuccessDataResult<PagedDataResponse<TopicDto.List>>(pagedResponse);
        }
    }
}
