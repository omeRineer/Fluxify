using Fluxify.Application.Models.Dto.Auth;
using Fluxify.Application.Models.Rest.Auth;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Utilities.Result;
using Fluxify.Application.Utilities.Token;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Core
{
    public interface IAuthService
    {
        Task<IDataResult<AccessToken>> LoginAsync(Login.Request req);
    }
    public class AuthService : IAuthService
    {
        readonly ITokenService _tokenService;
        readonly IUserRepository _userRepository;
        readonly ICredentialRepository _credentialRepository;

        public AuthService(ITokenService tokenService, IUserRepository userRepository, ICredentialRepository credentialRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _credentialRepository = credentialRepository;
        }

        public async Task<IDataResult<AccessToken>> LoginAsync(Login.Request req)
        {
            var credentialUser = await _credentialRepository.GetSingleOrDefaultAsync(f => f.UserName == req.UserName &&
                                                                                            f.Password == req.Password,
                                                                           i => i.Include(x => x.User)
                                                                                 .ThenInclude(x => x.NameSpace));

            if (credentialUser == null)
                return new ErrorDataResult<AccessToken>("User not found!");

            AccessToken token = _tokenService.GenerateToken(credentialUser.User, credentialUser.User.NameSpace);

            return new SuccessDataResult<AccessToken>(token);

        }
    }
}
