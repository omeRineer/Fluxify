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
            var avaibleUser = await UserRepository.GetSingleOrDefaultAsync(f => f.Key == model.Key && f.NameSpaceId == nameSpaceE.Id);
            await BusinessRuleEngine.RunAndThrow(
                    new MustNotExistRule<User>(avaibleUser, "User already exists. Please, set a different key")
            );

            var user = Mapper.Map<User>(model);
            user.NameSpaceId = nameSpaceId;

            if (model.Topics != null && model.Topics.Any())
                user.Topics = await TopicRepository.GetProjectedListAsync(s => new UserTopic { TopicId = s.Id },
                                                                                f => model.Topics.Contains(f.Slug));

            await UserRepository.AddAsync(user);
            await UserRepository.SaveChangesAsync();

            var result = Mapper.Map<CreateUser.Response>(user);
            result.Token = _tokenService.GenerateToken(user, nameSpaceE).Token;


            return new SuccessDataResult<CreateUser.Response>(result, "User is created.");
        }


        public async Task<IDataResult<UserDto.Detail>> GetDetailAsync(Guid id)
        {
            var user = await UserRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            await BusinessRuleEngine.RunAndThrow(
                                new MustExistRule<User>(user)
                        );

            var result = Mapper.Map<UserDto.Detail>(user);

            return new SuccessDataResult<UserDto.Detail>(data: result);
        }

        public async Task<IDataResult<PagedDataResponse<UserDto.List>>> GetListAsync(Guid nameSpaceId, UserQuery query)
        {
            var pagedData = await UserRepository.GetPagedListAsync(query.Page, 
                                                               query.Size,
                                                               f => f.NameSpaceId == nameSpaceId);

            var mappedList = Mapper.Map<List<UserDto.List>>(pagedData.Data);

            var result = PagedDataResponse<UserDto.List>.Load(mappedList, pagedData.TotalCount);
            return new SuccessDataResult<PagedDataResponse<UserDto.List>>(result);

        }

        public async Task<IDataResult<UpdateUser.Response>> UpdateAsync(Guid nameSpaceId, string key, UpdateUser.Request model)
        {
            var user = await UserRepository.GetSingleOrDefaultAsync(f => f.Key == key && f.NameSpaceId == nameSpaceId,
                                                                    i => i.Include(x => x.NameSpace));
            await BusinessRuleEngine.RunAndThrow(
                    new MustExistRule<User>(user)
            );

            user = Mapper.Map(model, user);

            await UserRepository.UpdateAsync(user);
            await UserRepository.SaveChangesAsync();

            var result = Mapper.Map<UpdateUser.Response>(user);
            result.Token = _tokenService.GenerateToken(user, user.NameSpace).Token;

            return new SuccessDataResult<UpdateUser.Response>(result, "User is updated.");
        }

        public async Task<IDataResult<UserDto.Detail>> GetDetailAsync(Guid nameSpaceId, string key)
        {
            var user = await UserRepository.GetSingleOrDefaultAsync(f => f.Key == key && f.NameSpaceId == nameSpaceId);
            await BusinessRuleEngine.Run(
                                new MustExistRule<User>(user)
                        );

            var result = Mapper.Map<UserDto.Detail>(user);

            return new SuccessDataResult<UserDto.Detail>(data: result);
        }

        public async Task<IDataResult<AccessToken>> GetTokenAsync(Guid nameSpaceId, string key)
        {
            var user = await UserRepository.GetSingleOrDefaultAsync(f => f.NameSpaceId == nameSpaceId && f.Key == key,
                                                                    i => i.Include(x => x.NameSpace));

            await BusinessRuleEngine.RunAndThrow(
                    new MustExistRule<User>(user)
            );

            AccessToken token = _tokenService.GenerateToken(user, user.NameSpace);

            return new SuccessDataResult<AccessToken>(token);
        }
    }
}
