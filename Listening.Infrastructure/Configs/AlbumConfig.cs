using Listening.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.EFCore;


namespace Listening.Infrastructure.Configs
{
    class AlbumConfig : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("T_Albums");
            builder.HasKey(x => x.Id).IsClustered(false);  // 对于Guid主键，不要建聚集索引，否则插入性能很差
            builder.OwnsOneMultilingualString(e => e.Name);
            builder.HasIndex(e => new { e.CategoryId, e.IsDeleted });
        }
    }
}
