using AutoMapper;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.ApiResponse;
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

        public async Task<ApiDataResponse<CreateChannelResponse>> CreateAsync(CreateChannelRequest model)
        {
            var isAvaible = await channelRepository.GetSingleOrDefaultAsync(f => f.Prefix == model.Prefix);
            if (isAvaible != null)
                return ApiResponseHelper.Error<CreateChannelResponse>(message: "Channel is avaible. Please, set a difference name value.");

            var channel = Mapper.Map<Channel>(model);
            channel.Topics = channel.Topics?.Select(s => $"{model.Prefix}_{s}").ToList();

            await channelRepository.AddAsync(channel);

            var result = Mapper.Map<CreateChannelResponse>(channel);

            return ApiResponseHelper.Success(result, "Channel is created.");
        }

        public async Task<ApiDataResponse<UpdateChannelResponse>> UpdateAsync(UpdateChannelRequest model)
        {
            var channel = await channelRepository.GetSingleOrDefaultAsync(f => f.Id == model.Id);
            if (channel == null)
                return ApiResponseHelper.Error<UpdateChannelResponse>(message: "Channel is not found.");

            channel = Mapper.Map(model, channel);
            channel.Topics = channel.Topics?.Select(s => $"{channel.Prefix}{s}").ToList();

            await channelRepository.UpdateAsync(channel);

            var result = Mapper.Map<UpdateChannelResponse>(channel);

            return ApiResponseHelper.Success(result, "Channel is updated.");
        }

        public async Task<ApiDataResponse<GetChannelDetailResponse>> GetDetailAsync(Guid id)
        {
            var channel = await channelRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            if (channel == null)
                return ApiResponseHelper.Error<GetChannelDetailResponse>(message: "Channel is not found.");

            var result = Mapper.Map<GetChannelDetailResponse>(channel);

            return ApiResponseHelper.Success(result);
        }

    }
}
