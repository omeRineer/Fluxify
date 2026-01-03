using AutoMapper;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;
using System.Linq;
using System.Threading.Channels;

namespace Fluxify.Application.Services
{
    public class UserService : IUserService
    {
        readonly IUserRepository UserRepository;
        readonly IUserTopicRepository UserTopicRepository;
        readonly INameSpaceRepository NameSpaceRepository;
        readonly IChannelRepository ChannelRepository;
        readonly IMapper Mapper;

        public UserService(IUserRepository userRepository, INameSpaceRepository nameSpaceRepository, IMapper mapper, IChannelRepository channelRepository, IUserTopicRepository userTopicRepository)
        {
            UserRepository = userRepository;
            NameSpaceRepository = nameSpaceRepository;
            Mapper = mapper;
            ChannelRepository = channelRepository;
            UserTopicRepository = userTopicRepository;
        }

        public async Task<IDataResult<CreateUserResponse>> CreateAsync(CreateUserRequest model)
        {
            var nameSpace = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Id == model.NameSpace);
            var avaibleSubscriber = await UserRepository.GetSingleOrDefaultAsync(f => f.Key == model.Key && f.NameSpace == model.NameSpace);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(nameSpace, "Namespace is not found."),
                    new DataMustBeAvaibleRule(avaibleSubscriber, "Subscriber is avaible. Please, set a different key")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<CreateUserResponse>(message: businessResult.Message);

            if (model.Topics != null && model.Topics.Count > 0)
            {
                var topics = (await ChannelRepository.GetListAsync()).SelectMany(s => s.Topics);
                var topicNotAvaible = model.Topics.FirstOrDefault(x => !topics.Contains(x));

                businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeAvaibleRule(topicNotAvaible, $"{topicNotAvaible} is not found")
                );
                if (!businessResult.IsSuccess)
                    return new ErrorDataResult<CreateUserResponse>(message: businessResult.Message);
            }

            string apiKey = CryptoHelper.Encrypt(model.Key, nameSpace.Signature);
            var subscriber = new User
            {
                NameSpace = model.NameSpace,
                Key = model.Key,
                ApiKey = apiKey,
                Email = model.Email,
                Name = model.Name,
                ExpireDate = model.ExpireDate,
            };

            await UserRepository.AddAsync(subscriber);

            var result = Mapper.Map<CreateUserResponse>(subscriber);

            return new SuccessDataResult<CreateUserResponse>(result, "Subscriber is created.");
        }

        public async Task<IDataResult<UpdateUserResponse>> UpdateAsync(UpdateUserRequest model)
        {
            var subscriber = await UserRepository.GetSingleOrDefaultAsync(f => f.Id == model.Id);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(subscriber, "Subscriber is not found.")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<UpdateUserResponse>(message: businessResult.Message);

            if (model.Topics != null && model.Topics.Count > 0)
            {
                var topics = (await ChannelRepository.GetListAsync()).SelectMany(s => s.Topics);
                var isNotAvaibleTopic = model.Topics.FirstOrDefault(x => !topics.Contains(x));

                businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(isNotAvaibleTopic, $"{isNotAvaibleTopic} is not found")
                );
                if (!businessResult.IsSuccess)
                    return new ErrorDataResult<UpdateUserResponse>(message: businessResult.Message);
            }

            subscriber = Mapper.Map(model, subscriber);

            await UserRepository.UpdateAsync(subscriber);

            var result = Mapper.Map<UpdateUserResponse>(subscriber);

            return new SuccessDataResult<UpdateUserResponse>(result, "Subscriber is updated.");
        }

        public async Task<IDataResult<GetUserDetailResponse>> GetDetailAsync(Guid id)
        {
            var subscriber = await UserRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            var businessResult = await BusinessRuleEngine.Run(
                                new DataMustBeFoundRule(subscriber, "Subscriber is not found.")
                        );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<GetUserDetailResponse>(message: businessResult.Message);

            var result = Mapper.Map<GetUserDetailResponse>(subscriber);

            return new SuccessDataResult<GetUserDetailResponse>(data: result);
        }

        public async Task<IDataResult<GetUserDetailResponse?>> GetDetailAsync(string apiKey)
        {
            var subscriber = await UserRepository.GetSingleOrDefaultAsync(f => f.ApiKey == apiKey);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(subscriber, "Subscriber is not found.")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<GetUserDetailResponse?>(message: businessResult.Message);

            var result = Mapper.Map<GetUserDetailResponse>(subscriber);

            return new SuccessDataResult<GetUserDetailResponse?>(result);
        }

        public async Task<IDataResult<List<UserResponse>>> GetListByNameSpaceAsync(Guid nameSpace)
        {
            var subscribers = await UserRepository.GetListAsync(f => f.NameSpace == nameSpace);

            var result = Mapper.Map<List<UserResponse>>(subscribers);

            return new SuccessDataResult<List<UserResponse>>(result);
        }

        public async Task<IDataResult<List<UserResponse>>> GetListByTopicsAsync(Guid nameSpace, params Guid[] topics)
        {
            var userTopics = await UserTopicRepository.GetListAsync(f => topics.Contains(f.TopicId));
            var users = await UserRepository.GetListAsync(f => f.NameSpace == nameSpace && userTopics.Select(ut => ut.UserId).Contains(f.Id));

            var result = Mapper.Map<List<UserResponse>>(users);

            return new SuccessDataResult<List<UserResponse>>(result);
        }
    }
}
