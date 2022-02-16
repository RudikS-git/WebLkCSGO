using System.Collections.Generic;
using System.Security.Claims;

namespace WebAPI.Utils.ServiceToken
{
    public interface ITokenBuilder : IService
    {
        public string BuildToken(IEnumerable<Claim> claims);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
