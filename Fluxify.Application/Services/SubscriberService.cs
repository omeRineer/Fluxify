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
using System.Threading.Channels;

namespace Fluxify.Application.Services
{
    public class SubscriberService : ISubscriberService
    {
        readonly ISubscriberRepository SubscriberRepository;
        readonly INameSpaceRepository NameSpaceRepository;
        readonly IChannelRepository ChannelRepository;
        readonly IMapper Mapper;

        public SubscriberService(ISubscriberRepository subscriberRepository, INameSpaceRepository nameSpaceRepository, IMapper mapper, IChannelRepository channelRepository)
        {
            SubscriberRepository = subscriberRepository;
            NameSpaceRepository = nameSpaceRepository;
            Mapper = mapper;
            ChannelRepository = channelRepository;
        }

        public async Task<IDataResult<CreateSubscriberResponse>> CreateAsync(CreateSubscriberRequest model)
        {
            var nameSpace = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Id == model.NameSpace);
            var avaibleSubscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.Key == model.Key && f.NameSpace == model.NameSpace);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(nameSpace, "Namespace is not found."),
                    new DataMustBeAvaibleRule(avaibleSubscriber, "Subscriber is avaible. Please, set a different key")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<CreateSubscriberResponse>(message: businessResult.Message);

            if (model.Topics != null && model.Topics.Count > 0)
            {
                var topics = (await ChannelRepository.GetListAsync()).SelectMany(s => s.Topics);
                var isNotAvaibleTopic = model.Topics.FirstOrDefault(x => !topics.Contains(x));

                businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeAvaibleRule(isNotAvaibleTopic, $"{isNotAvaibleTopic} is not found")
                );
                if (!businessResult.IsSuccess)
                    return new ErrorDataResult<CreateSubscriberResponse>(message: businessResult.Message);
            }

            string apiKey = CryptoHelper.Encrypt(model.Key, nameSpace.Signature);
            var subscriber = new Subscriber
            {
                NameSpace = model.NameSpace,
                Key = model.Key,
                ApiKey = apiKey,
                Topics = model.Topics,
                ExpireDate = model.ExpireDate,
            };

            await SubscriberRepository.AddAsync(subscriber);

            var result = Mapper.Map<CreateSubscriberResponse>(subscriber);

            return new SuccessDataResult<CreateSubscriberResponse>(result, "Subscriber is created.");
        }

        public async Task<IDataResult<UpdateSubscriberResponse>> UpdateAsync(UpdateSubscriberRequest model)
        {
            var subscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.Id == model.Id);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(subscriber, "Subscriber is not found.")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<UpdateSubscriberResponse>(message: businessResult.Message);

            if (model.Topics != null && model.Topics.Count > 0)
            {
                var topics = (await ChannelRepository.GetListAsync()).SelectMany(s => s.Topics);
                var isNotAvaibleTopic = model.Topics.FirstOrDefault(x => !topics.Contains(x));

                businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(isNotAvaibleTopic, $"{isNotAvaibleTopic} is not found")
                );
                if (!businessResult.IsSuccess)
                    return new ErrorDataResult<UpdateSubscriberResponse>(message: businessResult.Message);
            }

            subscriber = Mapper.Map(model, subscriber);

            await SubscriberRepository.UpdateAsync(subscriber);

            var result = Mapper.Map<UpdateSubscriberResponse>(subscriber);

            return new SuccessDataResult<UpdateSubscriberResponse>(result, "Subscriber is updated.");
        }

        public async Task<IDataResult<GetSubscriberDetailResponse>> GetDetailAsync(Guid id)
        {
            var subscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            var businessResult = await BusinessRuleEngine.Run(
                                new DataMustBeFoundRule(subscriber, "Subscriber is not found.")
                        );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<GetSubscriberDetailResponse>(message: businessResult.Message);

            var result = Mapper.Map<GetSubscriberDetailResponse>(subscriber);

            return new SuccessDataResult<GetSubscriberDetailResponse>(data: result);
        }

        public async Task<IDataResult<GetSubscriberDetailResponse?>> GetDetailAsync(string apiKey)
        {
            var subscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.ApiKey == apiKey);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(subscriber, "Subscriber is not found.")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<GetSubscriberDetailResponse?>(message: businessResult.Message);

            var result = Mapper.Map<GetSubscriberDetailResponse>(subscriber);

            return new SuccessDataResult<GetSubscriberDetailResponse?>(result);
        }

        public async Task<IDataResult<List<SubscriberResponse>>> GetListByNameSpaceAsync(Guid nameSpace)
        {
            var subscribers = await SubscriberRepository.GetListAsync(f => f.NameSpace == nameSpace);

            var result = Mapper.Map<List<SubscriberResponse>>(subscribers);

            return new SuccessDataResult<List<SubscriberResponse>>(result);
        }
    }
}
