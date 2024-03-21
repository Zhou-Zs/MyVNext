
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IIdRepository
    {
        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User?> FindByIdAsync(Guid userId);

        /// <summary>
        /// 根据手机号获取用户
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        Task<User?> FindByPhoneNumberAsync(string phoneNum);

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<User?> FindByNameAsync(string userName);

        /// <summary>
        /// 为了登录而检查用户名、密码是否正确
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="lockoutOnFailure">如果登录失败，则记录一次登陆失败</param>
        /// <returns></returns>
        public Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure);

        /// <summary>
        /// 获取用户的角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IList<string>> GetRolesAsync(User user);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> ChangePasswordAsync(Guid userId, string password);
    }
}
