using Infrastructure;
using Infrastructure.EFCore;
using Listening.Domain.Entities;
using Listening.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace Listening.Infrastructure
{
    public class ListeningDbContext : BaseDbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public ListeningDbContext(DbContextOptions options, IMediator? mediator) : base(options, mediator)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            modelBuilder.EnableSoteDeletionGlobalFilter();
        }
    }
}
