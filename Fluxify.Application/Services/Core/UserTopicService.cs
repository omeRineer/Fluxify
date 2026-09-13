using AutoMapper;
using Fluxify.Application.Models.Dto.UserTopic;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.UserTopic;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fluxify.Application.Models.Rest.Channel.CreateChannel.Request;

namespace Fluxify.Application.Services.Core
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

        public async Task<IDataResult<List<UserTopicDto.List>>> GetListAsync(Guid userId)
        {
            var userTopics = await UserTopicRepository.GetListAsync(filter: f => f.UserId == userId, 
                                                                    i => i.Include(x => x.Topic));

            var mappedList = _mapper.Map<List<UserTopicDto.List>>(userTopics.Select(s=> s.Topic));

            return new SuccessDataResult<List<UserTopicDto.List>>(mappedList);
        }

        public async Task<List<string>> GetTopicsByUser(Guid userId)
        {
            var topics = await UserTopicRepository.GetProjectedListAsync(s => s.Topic.Slug, f => f.UserId == userId);

            return topics;
        }

        public async Task<IResult> SetAsync(Guid userId, Guid nameSpaceId, SetUserTopics.Request req)
        {
            if (!req.Removes.Any() && !req.News.Any())
                return new SuccessResult("User topics are updated");

            var existingUserTopics = await UserTopicRepository.GetProjectedListAsync(
                s => new { s.Id, s.Topic.Slug, s.TopicId },
                f => f.UserId == userId && f.NameSpaceId == nameSpaceId
            );

            var topicIdsToDelete = existingUserTopics
                .Where(f => req.Removes.Contains(f.Slug))
                .Select(s => s.Id)
                .ToList();

            if (topicIdsToDelete.Any())
                await UserTopicRepository.DeleteRangeAsync(topicIdsToDelete);

            var existingTopicSlugs = existingUserTopics.Select(x => x.Slug).ToHashSet();
            var newSlugsToAdd = req.News.Where(slug => !existingTopicSlugs.Contains(slug)).ToList();

            if (newSlugsToAdd.Any())
            {
                var topicIdsToAdd = await TopicRepository.GetProjectedListAsync(
                    s => s.Id,
                    f => newSlugsToAdd.Contains(f.Slug)
                );

                if (topicIdsToAdd.Any())
                {
                    await UserTopicRepository.AddRangeAsync(topicIdsToAdd.Select(topicId => new UserTopic
                    {
                        UserId = userId,
                        TopicId = topicId,
                        NameSpaceId = nameSpaceId
                    }));
                }
            }

            await UserTopicRepository.SaveChangesAsync();

            return new SuccessResult("User topics are updated");
        }
    }
}
