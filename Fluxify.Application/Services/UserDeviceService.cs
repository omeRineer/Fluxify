using AutoMapper;
using Fluxify.Application.Models.Rest.UserDevice;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services
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

        public async Task<IDataResult<List<UserDevice>>> GetListAsync(Guid[] userIds)
        {
            var devices = await UserDeviceRepository.GetListAsync(x => userIds.Contains(x.UserId) && x.IsActive);

            return new SuccessDataResult<List<UserDevice>>(devices);
        }

        public async Task<IResult> SetAsync(Guid userId, SetUserDevice.Request req)
        {
            var userDevice = await UserDeviceRepository.GetSingleOrDefaultAsync(x => x.UserId == userId && x.DeviceId == req.DeviceId);

            var devicesWithSameToken = await UserDeviceRepository.GetListAsync(f => f.Token == req.Token &&
                                                                                  (f.UserId != userId || f.DeviceId != req.DeviceId));
            if (devicesWithSameToken.Any())
            {
                devicesWithSameToken.ForEach(device => device.IsActive = false);

                await UserDeviceRepository.UpdateRangeAsync(devicesWithSameToken);
            }

            if (userDevice == null)
            {
                userDevice = _mapper.Map<UserDevice>(req);
                userDevice.UserId = userId;

                await UserDeviceRepository.AddAsync(userDevice);
            }
            else
            {
                userDevice.Token = req.Token;
                userDevice.IsActive = true;

                await UserDeviceRepository.UpdateAsync(userDevice);
            }

            return new SuccessResult("User device is settled");
        }
    }
}
