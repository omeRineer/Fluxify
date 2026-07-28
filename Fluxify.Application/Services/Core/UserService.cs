using AutoMapper;
using Fluxify.Application.Models.Dto.Auth;
using Fluxify.Application.Models.Dto.User;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Application.Utilities.Result;
using Fluxify.Application.Utilities.Token;
using Fluxify.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Channels;
using System.Xml.Linq;

namespace Fluxify.Application.Services.Core
{
    public class UserService : IUserService
    {
        readonly IUserRepository UserRepository;
        readonly IUserTopicRepository UserTopicRepository;
        readonly INameSpaceRepository NameSpaceRepository;
        readonly IChannelRepository ChannelRepository;
        readonly ITopicRepository TopicRepository;
        readonly ITokenService _tokenService;
        readonly IMapper Mapper;

        public UserService(IUserRepository userRepository, INameSpaceRepository nameSpaceRepository, IMapper mapper, IChannelRepository channelRepository, IUserTopicRepository userTopicRepository, ITopicRepository topicRepository, ITokenService tokenService)
        {
            UserRepository = userRepository;
            NameSpaceRepository = nameSpaceRepository;
            Mapper = mapper;
            ChannelRepository = channelRepository;
            UserTopicRepository = userTopicRepository;
            TopicRepository = topicRepository;
            _tokenService = tokenService;
        }

        public async Task<IDataResult<CreateUser.Response>> CreateAsync(Guid nameSpaceId, CreateUser.Request model)
        {
            var nameSpaceE = await NameSpaceRepository.GetSingleAsync(f => f.Id == nameSpaceId);
            var avaibleSubscriber = await UserRepository.GetSingleOrDefaultAsync(f => f.Key == model.Key && f.NameSpaceId == nameSpaceE.Id);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeAvaibleRule(avaibleSubscriber, "Subscriber is avaible. Please, set a different key")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<CreateUser.Response>(message: businessResult.Message);

            var subscriber = new User
            {
                NameSpaceId = nameSpaceE.Id,
                Key = model.Key,
                Email = model.Email,
                Name = model.Name
            };

            if (model.Topics != null && model.Topics.Count > 0)
                subscriber.Topics = await TopicRepository.GetProjectedListAsync(s => new UserTopic { TopicId = s.Id },
                                                                                f => model.Topics.Contains(f.Slug));

            await UserRepository.AddAsync(subscriber);
            await UserRepository.SaveChangesAsync();

            var result = Mapper.Map<CreateUser.Response>(subscriber);
            subscriber.NameSpace = nameSpaceE;
            result.Token = _tokenService.GenerateToken(subscriber).Token;


            return new SuccessDataResult<CreateUser.Response>(result, "Subscriber is created.");
        }


        public async Task<IDataResult<UserDto.Detail>> GetDetailAsync(Guid id)
        {
            var subscriber = await UserRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            var businessResult = await BusinessRuleEngine.Run(
                                new DataMustBeFoundRule(subscriber, "Subscriber is not found.")
                        );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<UserDto.Detail>(message: businessResult.Message);

            var result = Mapper.Map<UserDto.Detail>(subscriber);

            return new SuccessDataResult<UserDto.Detail>(data: result);
        }

        public async Task<IDataResult<DataListResponse<UserDto.List>>> GetListByTopicsAsync(Guid nameSpaceId, params Guid[] topics)
        {
            var userTopics = await UserTopicRepository.GetListAsync(f => topics.Contains(f.TopicId));
            var users = await UserRepository.GetListAsync(f => f.NameSpaceId == nameSpaceId && userTopics.Select(ut => ut.UserId).Contains(f.Id));

            var mappedList = Mapper.Map<List<UserDto.List>>(users);

            var result = DataListResponse<UserDto.List>.Load(mappedList);
            return new SuccessDataResult<DataListResponse<UserDto.List>>(result);
        }

        public async Task<IDataResult<DataListResponse<UserDto.List>>> GetListAsync(Guid nameSpaceId)
        {
            var users = await UserRepository.GetListAsync(f => f.NameSpaceId == nameSpaceId);

            var mappedList = Mapper.Map<List<UserDto.List>>(users);

            var result = DataListResponse<UserDto.List>.Load(mappedList);
            return new SuccessDataResult<DataListResponse<UserDto.List>>(result);

        }

        public async Task<IDataResult<UpdateUser.Response>> UpdateAsync(Guid nameSpaceId, string key, UpdateUser.Request model)
        {
            var subscriber = await UserRepository.GetSingleOrDefaultAsync(f => f.Key == key && f.NameSpaceId == nameSpaceId);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(subscriber, "Subscriber is not found.")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<UpdateUser.Response>(message: businessResult.Message);

            subscriber = Mapper.Map(model, subscriber);

            await UserRepository.UpdateAsync(subscriber);
            await UserRepository.SaveChangesAsync();

            var result = Mapper.Map<UpdateUser.Response>(subscriber);

            return new SuccessDataResult<UpdateUser.Response>(result, "Subscriber is updated.");
        }

        public async Task<IDataResult<UserDto.Detail>> GetDetailAsync(Guid nameSpaceId, string key)
        {
            var subscriber = await UserRepository.GetSingleOrDefaultAsync(f => f.Key == key && f.NameSpaceId == nameSpaceId);
            var businessResult = await BusinessRuleEngine.Run(
                                new DataMustBeFoundRule(subscriber, "Subscriber is not found.")
                        );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<UserDto.Detail>(message: businessResult.Message);

            var result = Mapper.Map<UserDto.Detail>(subscriber);

            return new SuccessDataResult<UserDto.Detail>(data: result);
        }

        public async Task<IDataResult<AccessToken>> GetTokenAsync(Guid nameSpaceId, string key)
        {
            var user = await UserRepository.GetSingleOrDefaultAsync(f => f.NameSpaceId == nameSpaceId && f.Key == key,
                                                                    i => i.Include(x => x.NameSpace));

            if (user == null)
                return new ErrorDataResult<AccessToken>("User not found!");

            AccessToken token = _tokenService.GenerateToken(user);

            return new SuccessDataResult<AccessToken>(token);
        }
    }
}
