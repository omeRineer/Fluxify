using AutoMapper;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Domain.Core;

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

        public async Task<ApiDataResponse<CreateSubscriberResponse>> CreateAsync(CreateSubscriberRequest model)
        {
            var client = await NameSpaceRepository.GetSingleOrDefaultAsync(f => f.Id == model.NameSpace);
            if (client == null)
                return ApiResponseHelper.Error<CreateSubscriberResponse>(message: "Namespace is not avaible.");

            var avaibleSubscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.Key == model.Key && f.NameSpace == model.NameSpace);
            if (avaibleSubscriber != null)
                return ApiResponseHelper.Error<CreateSubscriberResponse>(message: "Subscriber is avaible. Please, set a different key");

            if (model.Topics != null && model.Topics.Count > 0)
            {
                var channel = await ChannelRepository.GetSingleAsync(f => f.Name == "Notification");
                if (channel.Topics == null)
                    return ApiResponseHelper.Error<CreateSubscriberResponse>(message: "There is not any topics in the channel.");

                var isNotAvaibleTopic = model.Topics.FirstOrDefault(x => !channel.Topics.Contains(x));
                if (isNotAvaibleTopic != null)
                    return ApiResponseHelper.Error<CreateSubscriberResponse>(message: $"{isNotAvaibleTopic} is not avaible in the {channel.Name} channel.");
            }

            string apiKey = CryptoHelper.Encrypt(model.Key, client.Signature);
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

            return ApiResponseHelper.Success<CreateSubscriberResponse>(result, "Subscriber is created.");
        }

        public async Task<ApiDataResponse<UpdateSubscriberResponse>> UpdateAsync(UpdateSubscriberRequest model)
        {
            var subscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.Id == model.Id);
            if (subscriber == null)
                return ApiResponseHelper.Error<UpdateSubscriberResponse>(message: "Subscriber is not found.");

            if (model.Topics != null && model.Topics.Count > 0)
            {
                var channel = await ChannelRepository.GetSingleAsync(f => f.Name == "Notification");
                if (channel.Topics == null)
                    return ApiResponseHelper.Error<UpdateSubscriberResponse>(message: $"There is not any topics in the channel.");

                var isNotAvaibleTopic = model.Topics.FirstOrDefault(x => !channel.Topics.Contains(x));
                if (isNotAvaibleTopic != null)
                    return ApiResponseHelper.Error<UpdateSubscriberResponse>(message: $"{isNotAvaibleTopic} is not avaible in the {channel.Name} channel.");
            }

            subscriber = Mapper.Map(model, subscriber);

            await SubscriberRepository.UpdateAsync(subscriber);

            var result = Mapper.Map<UpdateSubscriberResponse>(subscriber);

            return ApiResponseHelper.Success<UpdateSubscriberResponse>(result, "Subscriber is updated.");
        }

        public async Task<ApiDataResponse<GetSubscriberDetailResponse>> GetDetailAsync(Guid id)
        {
            var subscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.Id == id);

            if (subscriber == null)
                return ApiResponseHelper.Error<GetSubscriberDetailResponse>(message: "Subscriber is not avaible.");

            var result = Mapper.Map<GetSubscriberDetailResponse>(subscriber);

            return ApiResponseHelper.Success<GetSubscriberDetailResponse>(data: result);
        }

        public async Task<ApiDataResponse<GetSubscriberDetailResponse?>> GetDetailAsync(string apiKey)
        {
            var subscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.ApiKey == apiKey);

            if (subscriber == null)
                return ApiResponseHelper.Error<GetSubscriberDetailResponse?>(message: "Subscriber is not avaible.");

            var result = Mapper.Map<GetSubscriberDetailResponse>(subscriber);

            return ApiResponseHelper.Success<GetSubscriberDetailResponse?>(result);
        }

        public async Task<ApiDataResponse<GetSubscriberListResponse>> GetSubscribersByNameSpaceAsync(Guid nameSpace)
        {
            var subscribers = await SubscriberRepository.GetListAsync(f => f.NameSpace == nameSpace);

            var result = new GetSubscriberListResponse
            {
                Subscribers = Mapper.Map<List<SubscriberResponse>>(subscribers)
            };

            return ApiResponseHelper.Success<GetSubscriberListResponse>(result);
        }
    }
}
