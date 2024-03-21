
namespace IdentityService.WebAPI.Controllers.Login
{
    public record UserRequest(Guid Id, string? PhoneNumber, DateTime CreationTime);

}
