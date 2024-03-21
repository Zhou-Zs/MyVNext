using JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IdentityService.Domain
{
    /// <summary>
    /// 领域服务
    /// </summary>
    public class IdDomainService
    {
        private readonly IIdRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IOptions<JWTOptions> _optJWT;
        public IdDomainService(IIdRepository repository, ITokenService tokenService, IOptions<JWTOptions> optJWT)
        {
            _repository = repository;
            _tokenService = tokenService;
            _optJWT = optJWT;
        }

        /// <summary>
        /// 检查手机和密码是否匹配
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<SignInResult> CkeckPhoneNumAndPwdAsync(string phoneNum, string password)
        {
            var user = await _repository.FindByPhoneNumberAsync(phoneNum);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            var result = await _repository.CheckForSignInAsync(user, password, true);
            return result;
        }

        /// <summary>
        /// 检查用户名和密码是否匹配
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<SignInResult> CkeckUserNameNumAndPwdAsync(string userName, string password)
        {
            var user = await _repository.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            var result = await _repository.CheckForSignInAsync(user, password, true);
            return result;
        }

        /// <summary>
        /// 绑定Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<string> BuildTokenAsync(User user)
        {
            var roles = await _repository.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return _tokenService.BuilToken(claims, _optJWT.Value);
        }

        /// <summary>
        /// 根据手机号码个密码登录
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(SignInResult Result, string? ToKen)> LoginByPhoneAndPwdAsync(string phoneNum, string password)
        {
            var ckeckResult = await CkeckPhoneNumAndPwdAsync(phoneNum, password);
            if (ckeckResult.Succeeded)
            {
                var user = await _repository.FindByPhoneNumberAsync(phoneNum);
                string token = await BuildTokenAsync(user);
                return (SignInResult.Success, token);
            }
            else
            {
                return (ckeckResult, null);
            }
        }

        /// <summary>
        /// 根据手机号码个密码登录
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(SignInResult Result, string? ToKen)> LoginByUserNameAndPwdAsync(string userName, string password)
        {
            var ckeckResult = await CkeckUserNameNumAndPwdAsync(userName, password);
            if (ckeckResult.Succeeded)
            {
                var user = await _repository.FindByPhoneNumberAsync(userName);
                string token = await BuildTokenAsync(user);
                return (SignInResult.Success, token);
            }
            else
            {
                return (ckeckResult, null);
            }
        }

    }
}
