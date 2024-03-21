using IdentityService.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure
{
    public class IdDbContext : IdentityDbContext<User, Role, Guid>
    {
        public IdDbContext(DbContextOptions<IdDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            builder.Enab
        }
    }
}
