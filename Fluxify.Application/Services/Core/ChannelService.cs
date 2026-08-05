using AutoMapper;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fluxify.Application.Models.Dto.Channel;
using Fluxify.Domain.Entities.Core;
using Fluxify.Application.Services.Interfaces.Core;

namespace Fluxify.Application.Services.Core
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

        public async Task<IDataResult<CreateChannel.Response>> CreateAsync(CreateChannel.Request model)
        {
            var channel = await channelRepository.GetSingleOrDefaultAsync(f => f.Slug == model.Slug);
            await BusinessRuleEngine.RunAndThrow(
                    new MustNotExistRule<Channel>(channel, "Channel is avaible")
            );

            var newChannel = Mapper.Map<Channel>(model);

            await channelRepository.AddAsync(newChannel);
            await channelRepository.SaveChangesAsync();

            var result = Mapper.Map<CreateChannel.Response>(newChannel);

            return new SuccessDataResult<CreateChannel.Response>(result, "Channel is created");
        }
        public async Task<IDataResult<UpdateChannel.Response>> UpdateAsync(Guid id, UpdateChannel.Request model)
        {
            var channel = await channelRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            await BusinessRuleEngine.RunAndThrow(
                    new MustExistRule<Channel>(channel, "Channel is not found")
            );

            channel = Mapper.Map(model, channel);

            await channelRepository.UpdateAsync(channel);
            await channelRepository.SaveChangesAsync();

            var result = Mapper.Map<UpdateChannel.Response>(channel);

            return new SuccessDataResult<UpdateChannel.Response>(result, "Channel is updated.");
        }
        public async Task<IDataResult<ChannelDto.Detail>> GetDetailAsync(Guid id)
        {
            var channel = await channelRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            await BusinessRuleEngine.RunAndThrow(
                    new MustExistRule<Channel>(channel, "Channel is not found")
            );

            var result = Mapper.Map<ChannelDto.Detail>(channel);

            return new SuccessDataResult<ChannelDto.Detail>(result);
        }

    }
}
