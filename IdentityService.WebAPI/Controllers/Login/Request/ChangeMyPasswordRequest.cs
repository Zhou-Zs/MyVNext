using FluentValidation;
namespace IdentityService.WebAPI.Controllers.Login
{
    public record ChangeMyPasswordRequest(string Password, string Password2);
    public class ChangeMyPasswordRequestValidator : AbstractValidator<ChangeMyPasswordRequest>
    {
        public ChangeMyPasswordRequestValidator()
        { 
            RuleFor(e=>e.Password).NotNull().NotEmpty().Equal(c=>c.Password2);
            RuleFor(e=>e.Password2).NotNull().NotEmpty();
        }
    }
}
