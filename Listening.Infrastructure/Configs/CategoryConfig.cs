using Listening.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.EFCore;
using Listening.Domain.Entities;


namespace Listening.Infrastructure.Configs
{
    class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("T_Categories");
            builder.HasKey(x => x.Id).IsClustered(false);  // 对于Guid主键，不要建聚集索引，否则插入性能很差
            builder.OwnsOneMultilingualString(e => e.Name);
            builder.Property(e=>e.CoverUrl).IsRequired(false).HasMaxLength(500).IsUnicode();
        }
    }
}
