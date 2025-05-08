using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace VCommerce.Modules.Core.Application.Interfaces;

public interface ITokenService
{
    JwtSecurityToken GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration configuration);
    
    string GenerateRefreshToken();
    
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration configuration);
}