using System.Security.Claims;

namespace JWT
{
    public interface ITokenService
    {
        string BuilToken(IEnumerable<Claim> claims,JWTOptions options);
    }
}
