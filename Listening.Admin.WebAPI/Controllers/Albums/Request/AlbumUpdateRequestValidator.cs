using DomainCommons;
using FluentValidation;
using Listening.Domain.Entities;
using Listening.Infrastructure;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
namespace Listening.Admin.WebAPI.Controllers.Albums.Request
{
    public record AlbumUpdateRequest(MultilingualString Name, Guid CategoryId);

    //把校验规则写到单独的文件，也是DDD的一种原则
    public class AlbumUpdateRequestValidator : AbstractValidator<AlbumUpdateRequest>
    {
        public AlbumUpdateRequestValidator(ListeningDbContext dbCtx)
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name.Chinese).NotNull().Length(1, 200);
            RuleFor(x => x.Name.English).NotNull().Length(1, 200);
        }
    }

}
