using Fluxify.Application.Models.Dto.Auth;
using Fluxify.Domain.Entities.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Application.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using Fluxify.Configuration.Options;
using Fluxify.Configuration;

namespace Fluxify.Application.Utilities.Token
{
    public interface ITokenService
    {
        AccessToken GenerateToken(User user, NameSpace nameSpace);
    }
    public class JwtService : ITokenService
    {
        // TODO : Add tenantInfo
        private readonly TokenOptions TokenOptions;
        public JwtService()
        {
            TokenOptions = AppConfiguration.TokenOptions;
        }

        public AccessToken GenerateToken(User user, NameSpace nameSpace)
        {
            var symetricSecurityKey = SecurityKeyHelper.GetSecurityKey(TokenOptions.SecurityKey);
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var claims = GetClaims(user, nameSpace);

            var securityToken = new JwtSecurityToken
            (
                issuer: TokenOptions.Issuer,
                audience: TokenOptions.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials

            );
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new AccessToken
            {
                Token = token
            };
        }

        private List<Claim> GetClaims(User user, NameSpace nameSpace)
        {
            var claims = new List<Claim>
            {
                new Claim(FluxifyClaims.Key, user.Key),
                new Claim(FluxifyClaims.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(FluxifyClaims.NameSpaceId, nameSpace.Id.ToString()),
                new Claim(FluxifyClaims.NameSpace, nameSpace.Signature),
                new Claim(FluxifyClaims.Name, user.Name)
            };

            return claims;
        }
    }
}
