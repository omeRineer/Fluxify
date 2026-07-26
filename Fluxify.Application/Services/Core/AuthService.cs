using Fluxify.Application.Models.Dto.Auth;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Utilities.Result;
using Fluxify.Application.Utilities.Token;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Services.Core
{
    public interface IAuthService
    {
        Task<IDataResult<AccessToken>> GetTokenByApiKeyAsync(string apiKey);
    }
    public class AuthService : IAuthService
    {
        readonly ITokenService _tokenService;
        readonly IUserRepository _userRepository;

        public AuthService(ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<IDataResult<AccessToken>> GetTokenByApiKeyAsync(string apiKey)
        {
            var user = await _userRepository.GetSingleOrDefaultAsync(f => f.ApiKey == apiKey, i => i.Include(x => x.NameSpace));

            if (user == null)
                return new ErrorDataResult<AccessToken>("User not found!");

            AccessToken token = _tokenService.GenerateToken(user);

            return new SuccessDataResult<AccessToken>(token);
        }
    }
}
