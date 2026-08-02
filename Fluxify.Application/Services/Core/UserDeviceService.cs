using AutoMapper;
using Fluxify.Application.Models.Dto.UserDevice;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.UserDevice;
using Fluxify.Application.Repositories;
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
        readonly IMapper _mapper;

        public UserDeviceService(IUserDeviceRepository userDeviceRepository, IMapper mapper)
        {
            UserDeviceRepository = userDeviceRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<DataListResponse<UserDeviceDto.List>>> GetListAsync(Guid userId)
        {
            var devices = await UserDeviceRepository.GetListAsync(x => x.UserId == userId);

            var mappedList = _mapper.Map<List<UserDeviceDto.List>>(devices);

            var result = DataListResponse<UserDeviceDto.List>.Load(mappedList);

            return new SuccessDataResult<DataListResponse<UserDeviceDto.List>>(result);
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
                userDevice.UserId = userDevice.UserId;
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
