using AutoMapper;
using Fluxify.Application.Models.Dto.UserDevice;
using Fluxify.Application.Models.Rest.Common;
using Fluxify.Application.Models.Rest.UserDevice;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Entities.Core;
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

        public async Task<IDataResult<DataListResponse<UserDeviceDto.List>>> GetListAsync(Guid userId)
        {
            var devices = await UserDeviceRepository.GetListAsync(x => x.UserId == userId);

            var mappedList = _mapper.Map<List<UserDeviceDto.List>>(devices);

            var result = DataListResponse<UserDeviceDto.List>.Load(mappedList);

            return new SuccessDataResult<DataListResponse<UserDeviceDto.List>>(result);
        }

        public async Task<IResult> SetAsync(Guid userId, Guid nameSpaceId, SetUserDevice.Request req)
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
                userDevice.NameSpaceId = nameSpaceId;

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
