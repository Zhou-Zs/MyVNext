using DomainCommons;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class EFCoreExtensions
    {
        public static void EnableSoteDeletionGlobalFilter(this ModelBuilder modelBuilder)
        {
           var entityTypesHasSoftDeletion =  modelBuilder.Model.GetEntityTypes().Where(e => e.ClrType.IsAssignableTo(typeof(ISoftDelete)));
        }
    }
}
