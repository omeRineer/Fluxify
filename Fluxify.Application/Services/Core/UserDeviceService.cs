using AutoMapper;
using Fluxify.Application.Models.Dto.UserDevice;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.UserDevice;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fluxify.Application.Models.Dto.Chat.ChatDto.Detail;

namespace Fluxify.Application.Services.Core
{
    public class UserDeviceService : IUserDeviceService
    {
        readonly IUserDeviceRepository UserDeviceRepository;
        readonly IUserRepository _userRepository;
        readonly IMapper _mapper;

        public UserDeviceService(IUserDeviceRepository userDeviceRepository, IMapper mapper, IUserRepository userRepository)
        {
            UserDeviceRepository = userDeviceRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<IDataResult<List<UserDeviceDto.List>>> GetListAsync(Guid userId)
        {
            var devices = await UserDeviceRepository.GetListAsync(x => x.UserId == userId);

            var mappedList = _mapper.Map<List<UserDeviceDto.List>>(devices);

            return new SuccessDataResult<List<UserDeviceDto.List>>(mappedList);
        }

        public async Task<IResult> RegisterAsync(Guid userId, Guid nameSpaceId, SetUserDevice model)
        {
            var userDevice = await UserDeviceRepository.GetSingleOrDefaultAsync(
                x => x.UserId == userId &&
                     x.DeviceId == model.DeviceId);

            var duplicatedDevices = await UserDeviceRepository.GetListAsync(x =>
                    x.Token == model.Token &&
                    x.IsActive);

            foreach (var device in duplicatedDevices)
                device.IsActive = false;

            if (userDevice == null)
            {
                userDevice = _mapper.Map<UserDevice>(model);
                userDevice.UserId = userId;
                userDevice.NameSpaceId = nameSpaceId;
                userDevice.IsActive = true;

                await UserDeviceRepository.AddAsync(userDevice);
            }
            else
            {
                userDevice.Token = model.Token;
                userDevice.NameSpaceId = nameSpaceId;
                userDevice.IsActive = true;
                userDevice.LastSeen = DateTime.UtcNow;
            }

            await UserDeviceRepository.SaveChangesAsync();

            return new SuccessResult("User device has been registered.");
        }

        public async Task<IResult> RegisterAsync(string key, Guid nameSpaceId, SetUserDevice model)
        {
            var user = await _userRepository.GetProjectedSingleOrDefaultAsync(s => s.Id, f => f.Key == key);
            await BusinessRuleEngine.RunAndThrow(
                    new MustExistRule<Guid>(user, "User not found!")
            );

            var userDevice = await UserDeviceRepository.GetSingleOrDefaultAsync(
                x => x.User.Key == key &&
                     x.NameSpaceId == nameSpaceId &&
                     x.DeviceId == model.DeviceId,
                i => i.Include(x => x.User));

            var duplicatedDevices = await UserDeviceRepository.GetListAsync(x =>
                    x.Token == model.Token &&
                    x.IsActive);

            foreach (var device in duplicatedDevices)
                device.IsActive = false;

            if (userDevice == null)
            {
                userDevice = _mapper.Map<UserDevice>(model);
                userDevice.UserId = user;
                userDevice.NameSpaceId = nameSpaceId;
                userDevice.IsActive = true;

                await UserDeviceRepository.AddAsync(userDevice);
            }
            else
            {
                userDevice.Token = model.Token;
                userDevice.NameSpaceId = nameSpaceId;
                userDevice.IsActive = true;
                userDevice.LastSeen = DateTime.UtcNow;
            }

            await UserDeviceRepository.SaveChangesAsync();

            return new SuccessResult("User device has been registered.");
        }
    }
}
