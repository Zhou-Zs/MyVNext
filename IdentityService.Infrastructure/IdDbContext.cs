using IdentityService.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

namespace IdentityService.Infrastructure
{
    public class IdDbContext : IdentityDbContext<User, Role, Guid>
    {
        public IdDbContext(DbContextOptions<IdDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            builder.EnableSoteDeletionGlobalFilter(); // 全局过滤未软删除的数据
        }
    }
}
