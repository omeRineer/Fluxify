using AutoMapper;
using Fluxify.Application.Models.Dto.User;
using Fluxify.Application.Models.Rest.Common;
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

        public async Task<IDataResult<CreateUser.Response>> CreateAsync(Guid nameSpaceId, CreateUser.Request model)
        {
            var nameSpaceE = await NameSpaceRepository.GetSingleAsync(f => f.Id == nameSpaceId);
            var avaibleSubscriber = await UserRepository.GetSingleOrDefaultAsync(f => f.Key == model.Key && f.NameSpace == nameSpaceE.Id);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeAvaibleRule(avaibleSubscriber, "Subscriber is avaible. Please, set a different key")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<CreateUser.Response>(message: businessResult.Message);

            string apiKey = CryptoHelper.Encrypt(model.Key, nameSpaceE.Signature);
            var subscriber = new User
            {
                NameSpace = nameSpaceE.Id,
                Key = model.Key,
                ApiKey = apiKey,
                Email = model.Email,
                Name = model.Name,
                ExpireDate = model.ExpireDate,
            };

            await UserRepository.AddAsync(subscriber);

            var result = Mapper.Map<CreateUser.Response>(subscriber);

            return new SuccessDataResult<CreateUser.Response>(result, "Subscriber is created.");
        }

        public async Task<IDataResult<UpdateUser.Response>> UpdateAsync(Guid id, UpdateUser.Request model)
        {
            var subscriber = await UserRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(subscriber, "Subscriber is not found.")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<UpdateUser.Response>(message: businessResult.Message);

            subscriber = Mapper.Map(model, subscriber);

            await UserRepository.UpdateAsync(subscriber);

            var result = Mapper.Map<UpdateUser.Response>(subscriber);

            return new SuccessDataResult<UpdateUser.Response>(result, "Subscriber is updated.");
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

        public async Task<IDataResult<User>> GetAsync(string apiKey)
        {
            var subscriber = await UserRepository.GetSingleAsync(f => f.ApiKey == apiKey);

            return new SuccessDataResult<User>(subscriber);
        }

        public async Task<IDataResult<DataListResponse<UserDto.List>>> GetListByTopicsAsync(Guid nameSpaceId, params string[] topics)
        {
            var userTopics = await UserTopicRepository.GetListAsync(f => topics.Contains(f.TopicId));
            var users = await UserRepository.GetListAsync(f => f.NameSpace == nameSpaceId && userTopics.Select(ut => ut.UserId).Contains(f.Id));

            var mappedList = Mapper.Map<List<UserDto.List>>(users);

            var result = DataListResponse<UserDto.List>.Load(mappedList);
            return new SuccessDataResult<DataListResponse<UserDto.List>>(result);
        }

        public async Task<IDataResult<DataListResponse<UserDto.List>>> GetListAsync(Guid nameSpaceId)
        {
            var users = await UserRepository.GetListAsync(f => f.NameSpace == nameSpaceId);

            var mappedList = Mapper.Map<List<UserDto.List>>(users);

            var result = DataListResponse<UserDto.List>.Load(mappedList);
            return new SuccessDataResult<DataListResponse<UserDto.List>>(result);

        }

        public async Task<IDataResult<User>> GetAsync(Guid id)
        {
            var subscriber = await UserRepository.GetSingleAsync(f => f.Id == id);

            return new SuccessDataResult<User>(subscriber);
        }
    }
}
