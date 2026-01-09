using AutoMapper;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services
{
    public class ChannelService : IChannelService
    {
        readonly IChannelRepository channelRepository;
        readonly IMapper Mapper;

        public ChannelService(IChannelRepository channelRepository, IMapper mapper)
        {
            this.channelRepository = channelRepository;
            Mapper = mapper;
        }

        public async Task<IDataResult<CreateChannelResponse>> CreateAsync(CreateChannelRequest model)
        {
            var channel = await channelRepository.GetSingleOrDefaultAsync(f => f.Prefix == model.Prefix);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeAvaibleRule(channel, "Channel is avaible")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<CreateChannelResponse>(message: businessResult.Message);

            var newChannel = Mapper.Map<Channel>(model);

            await channelRepository.AddAsync(newChannel);

            var result = Mapper.Map<CreateChannelResponse>(newChannel);

            return new SuccessDataResult<CreateChannelResponse>(result, "Channel is created");
        }
        public async Task<IDataResult<UpdateChannelResponse>> UpdateAsync(UpdateChannelRequest model)
        {
            var channel = await channelRepository.GetSingleOrDefaultAsync(f => f.Id == model.Id);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(channel, "Channel is not found")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<UpdateChannelResponse>(message: businessResult.Message);

            channel = Mapper.Map(model, channel);

            await channelRepository.UpdateAsync(channel);

            var result = Mapper.Map<UpdateChannelResponse>(channel);

            return new SuccessDataResult<UpdateChannelResponse>(result, "Channel is updated.");
        }
        public async Task<IDataResult<GetChannelDetailResponse>> GetDetailAsync(Guid id)
        {
            var channel = await channelRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            var businessResult = await BusinessRuleEngine.Run(
                    new DataMustBeFoundRule(channel, "Channel is not found")
            );
            if (!businessResult.IsSuccess)
                return new ErrorDataResult<GetChannelDetailResponse>(message: businessResult.Message);

            var result = Mapper.Map<GetChannelDetailResponse>(channel);

            return new SuccessDataResult<GetChannelDetailResponse>(result);
        }

        public async Task<IDataResult<Channel>> GetAsync(Guid id)
            => new SuccessDataResult<Channel>(await channelRepository.GetSingleAsync(f => f.Id == id));
    }
}
