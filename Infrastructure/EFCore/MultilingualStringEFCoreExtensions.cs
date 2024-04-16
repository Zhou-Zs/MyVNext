using DomainCommons;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Infrastructure.EFCore
{
    public static class MultilingualStringEFCoreExtensions
    {
        public static EntityTypeBuilder<IEntity> OwnsOneMultilingualString<IEntity>(this EntityTypeBuilder<IEntity> entityTypeBuilder,
            Expression<Func<IEntity, MultilingualString>> navigationExpression, bool required = true, int maxLength = 200) where IEntity : class
        {
            entityTypeBuilder.OwnsOne(navigationExpression, dp =>
            {
                dp.Property(c => c.Chinese).IsRequired(required).HasMaxLength(maxLength).IsUnicode();
                dp.Property(c => c.English).IsRequired(required).HasMaxLength(maxLength).IsUnicode();
            });

            entityTypeBuilder.Navigation(navigationExpression).IsRequired(required);

            return entityTypeBuilder;
        }
    }
}
