using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT
{
    public class TokenService : ITokenService
    {
        public string BuilToken(IEnumerable<Claim> claims, JWTOptions options)
        {
            TimeSpan expireDuration = TimeSpan.FromSeconds(options.ExpireSeconds); //过期时间
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key)); // 对称安全秘钥
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature); // 签名证书，安全加密方式
            var tokenDescriptor = new JwtSecurityToken(options.Issuer, options.Audience, claims,
                expires: DateTime.Now.Add(expireDuration), signingCredentials: credentials); // Token描述
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor); // 生成Token
        }
    }
}
