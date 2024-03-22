using IdentityService.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace IdentityService.WebAPI.Controllers.Login
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IdDomainService _idDomainService;
        private readonly IIdRepository _idRepository;
        public LoginController(IdDomainService idDomainService, IIdRepository idRepository)
        {
            _idDomainService = idDomainService;
            _idRepository = idRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserRequest>> GetUserInfo()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _idRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return new UserRequest(user.Id, user.PhoneNumber, user.CreationTime);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<string?>> LoginByPhoneAndPwd(LoginByPhoneAndPwdRequest req)
        {
            //todo：要通过行为验证码、图形验证码等形式来防止暴力破解
            (var checkResult, string? token) = await _idDomainService.LoginByPhoneAndPwdAsync(req.PhoneNum, req.Password);
            if (checkResult.Succeeded)
            {
                return token;
            }
            else if (checkResult.IsLockedOut)
            {
                return StatusCode((int)HttpStatusCode.Locked, "此账号已经锁定");
            }
            else
            {
                string msg = "登录失败";
                return StatusCode((int)HttpStatusCode.BadRequest, msg);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<string?>> LoginByUserNameAndPwd(LoginByUserNameAndPwdRequest req)
        {
            //todo：要通过行为验证码、图形验证码等形式来防止暴力破解
            (var checkResult, string? token) = await _idDomainService.LoginByUserNameAndPwdAsync(req.UserName, req.Password);
            if (checkResult.Succeeded)
            {
                return token;
            }
            else if (checkResult.IsLockedOut)
            {
                return StatusCode((int)HttpStatusCode.Locked, "此账号已经锁定");
            }
            else
            {
                string msg = checkResult.ToString();
                return BadRequest("登录失败" + msg);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangeMyPassword(ChangeMyPasswordRequest req)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var resetPwdResult = await _idRepository.ChangePasswordAsync(userId, req.Password);
            if (resetPwdResult.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(resetPwdResult.Errors.SumErrors());
            }
        }
    }
}
