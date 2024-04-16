using Listening.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.EFCore;
using Listening.Domain.Entities;


namespace Listening.Infrastructure.Configs
{
    class EpisodeConfig : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.ToTable("T_Episodes");
            builder.HasKey(x => x.Id).IsClustered(false);  // 对于Guid主键，不要建聚集索引，否则插入性能很差
            builder.HasIndex(e => new { e.AlbumId, e.IsDeleted });
            builder.OwnsOneMultilingualString(e => e.Name);

            builder.Property(e=>e.AudioUrl).HasMaxLength(500).IsUnicode().IsRequired();
            builder.Property(e=>e.Subtitle).HasMaxLength(int.MaxValue).IsUnicode().IsRequired();
            builder.Property(e=>e.SubtitleType).HasMaxLength(10).IsUnicode().IsRequired();  
        }
    }
}
