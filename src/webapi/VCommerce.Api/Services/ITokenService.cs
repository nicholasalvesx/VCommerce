using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace VCommerce.Api.Services;

public interface ITokenService
{
    JwtSecurityToken GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration configuration);
    
    string GenerateRefreshToken();
    
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration configuration);
}