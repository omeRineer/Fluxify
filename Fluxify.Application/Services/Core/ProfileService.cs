using AutoMapper;
using Fluxify.Application.Models.Dto.User;
using Fluxify.Application.Repositories;
using Fluxify.Application.Rules.Common;
using Fluxify.Application.Rules;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fluxify.Domain.Entities.Core;
using Fluxify.Application.Models.Rest.Profile;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Fluxify.Application.Services.Core
{
    public class ProfileService : IProfileService
    {
        readonly IUserRepository UserRepository;
        private readonly IMapper Mapper;

        public ProfileService(IUserRepository userRepository, IMapper mapper)
        {
            UserRepository = userRepository;
            Mapper = mapper;
        }

        public async Task<IDataResult<UserDto.Profile>> GetDetailAsync(Guid id)
        {
            var user = await UserRepository.GetSingleOrDefaultAsync(f => f.Id == id, i => i.Include(x => x.Credential));
            await BusinessRuleEngine.RunAndThrow(
                                new MustExistRule<User>(user)
                        );

            var result = Mapper.Map<UserDto.Profile>(user);

            return new SuccessDataResult<UserDto.Profile>(data: result);
        }

        public async Task<IDataResult<UpdateProfile.Response>> UpdateAsync(Guid id, UpdateProfile.Request req)
        {
            var user = await UserRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            await BusinessRuleEngine.RunAndThrow(
                    new MustExistRule<User>(user)
            );

            user = Mapper.Map(req, user);

            await UserRepository.UpdateAsync(user);
            await UserRepository.SaveChangesAsync();

            var result = Mapper.Map<UpdateProfile.Response>(user);

            return new SuccessDataResult<UpdateProfile.Response>(data: result);
        }
    }
}
